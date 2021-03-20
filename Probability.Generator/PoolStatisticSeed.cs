using System.Collections.Generic;
using System.Linq;
using Probability.Service.Models;

namespace Probability.Generator
{
	public static class PoolStatisticSeed
	{
		/// <summary>
		/// Takes the cross product of the pools and seeds their statistics
		/// </summary>
		/// <param name="positivePools"></param>
		/// <param name="negativePools"></param>
		/// <returns></returns>
		public static IEnumerable<PoolCombination> SeedCombinationStatistics(this (IEnumerable<Pool>, IEnumerable<Pool>) pools) =>
			pools.Item1.SelectMany(positivePool => pools.Item2, (positivePool, negativePool) =>
			{
				//ConsoleLogger.LogLine($"{positivePool.Name}, {negativePool.Name}");

				return new PoolCombination()
				{
					PositivePool = positivePool,
					NegativePool = negativePool,
					PoolCombinationStatistics = (positivePool.PoolResults, negativePool.PoolResults).ResultCrossProduct().ToList()
				};
			});


		/// <summary>
		/// Takes the cross product of the result lists and sums their frequency and totals
		/// </summary>
		/// <param name="results"></param>
		/// <returns></returns>
		private static IEnumerable<PoolCombinationStatistic> ResultCrossProduct(this (IEnumerable<PoolResult>, IEnumerable<PoolResult>) results) =>
			results.Item1.SelectMany(positive => results.Item2, (positive, negative) => (positive, negative))
				.SeedStatistics()
				.SummarizeStatistics();

		/// <summary>
		/// Creates a list of 4 statistics: Success, Advantage, Triumph, and Despair
		/// </summary>
		/// <param name="poolResults"></param>
		/// <returns></returns>
		private static IEnumerable<PoolCombinationStatistic> SeedStatistics(this IEnumerable<(PoolResult positive, PoolResult negative)> poolResults)
			 => poolResults.Select(poolResult =>
			 {
				 var frequency = poolResult.positive.Frequency * poolResult.negative.Frequency;

				 var triumphNetQuantity = poolResult.positive.CountMatchingKeys(Symbol.Triumph);
				 var despairNetQuantity = poolResult.negative.CountMatchingKeys(Symbol.Despair);

				 //triumphs count as successes but advantages do not and despairs count as failures but threats do not
				 var successNetQuantity = poolResult.positive.CountMatchingKeys(Symbol.Success) + triumphNetQuantity - (poolResult.negative.CountMatchingKeys(Symbol.Failure) + despairNetQuantity);
				 var advantageNetQuantity = poolResult.positive.CountMatchingKeys(Symbol.Advantage) - poolResult.negative.CountMatchingKeys(Symbol.Threat);

				 return new List<PoolCombinationStatistic>
					{
						new PoolCombinationStatistic
						{
							Symbol = Symbol.Success,
							Quantity = successNetQuantity,
							Frequency = frequency,
							AlternateTotal = advantageNetQuantity
						},
						new PoolCombinationStatistic
						{
							Symbol = Symbol.Advantage,
							Quantity = advantageNetQuantity,
							Frequency = frequency,
							AlternateTotal = successNetQuantity
						},
						new PoolCombinationStatistic
						{
							Symbol = Symbol.Triumph,
							Quantity = triumphNetQuantity,
							Frequency = frequency,
							AlternateTotal = despairNetQuantity
						},
						new PoolCombinationStatistic
						{
							Symbol = Symbol.Despair,
							Quantity = despairNetQuantity,
							Frequency = frequency,
							AlternateTotal = triumphNetQuantity
						}
					};
			 }).SelectMany(s => s);

		private static IEnumerable<PoolCombinationStatistic> SummarizeStatistics(this IEnumerable<PoolCombinationStatistic> poolStatistics) =>
			poolStatistics
				.GroupBy(g => g.GetHashCode())
				.Select(s => new PoolCombinationStatistic
				{
					Symbol = s.First().Symbol,
					Quantity = s.First().Quantity,
					AlternateTotal = s.Sum(sum => sum.AlternateTotal * sum.Frequency),
					Frequency = s.Sum(sum => sum.Frequency)
				});
	}
}
