using DataFramework.Models;
using System.Linq;
using System.Collections.Generic;

namespace DataFramework.Context.Seed
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
			pools.Item1.SelectMany(positivePool => pools.Item2, (positivePool, negativePool) => (positivePool, negativePool).ToPoolCombination());

		/// <summary>
		/// Compares the outcome of each pool's combined rolls
		/// </summary>
		private static PoolCombination ToPoolCombination(this (Pool positivePool, Pool negativePool) poolPair)
		{
			var poolCombination = new PoolCombination()
			{
				PositivePool = poolPair.positivePool,
				NegativePool = poolPair.negativePool,
				PoolCombinationStatistics = (poolPair.positivePool.PoolResults, poolPair.negativePool.PoolResults).ResultCrossProduct().ToList()
			};

			ConsoleLogger.LogRoll($"{poolCombination.PositivePool.Name}, {poolCombination.NegativePool.Name}", 
				poolCombination.PositivePool.TotalOutcomes * poolCombination.NegativePool.TotalOutcomes, 
				poolCombination.PositivePool.UniqueOutcomes * poolCombination.NegativePool.UniqueOutcomes);

			return poolCombination;
		}

		/// <summary>
		/// Takes the cross product of the result lists and sums their frequency and totals
		/// </summary>
		/// <param name="results"></param>
		/// <returns></returns>
		private static IEnumerable<PoolCombinationStatistic> ResultCrossProduct(this (IEnumerable<PoolResult>, IEnumerable<PoolResult>) results) =>
			results.Item1.SelectMany(positive => results.Item2, (positive, negative) => (positive, negative).SeedStatistics()).SelectMany(s => s)
				.GroupBy(g => g.GetHashCode()).Select(s => new PoolCombinationStatistic
				{
					Symbol = s.First().Symbol,
					Quantity = s.First().Quantity,
					AlternateTotal = s.Sum(sum => sum.AlternateTotal * sum.Frequency),
					Frequency = s.Sum(sum => sum.Frequency)
				});

		/// <summary>
		/// Creates a list of 4 statistics: Success, Advantage, Triumph, and Despair
		/// </summary>
		/// <param name="poolResults"></param>
		/// <returns></returns>
		private static IEnumerable<PoolCombinationStatistic> SeedStatistics(this (PoolResult positive, PoolResult negative) poolResults)
		{
			var frequency = poolResults.positive.Frequency * poolResults.negative.Frequency;

			var triumphNetQuantity = poolResults.positive.CountMatchingKeys(Symbol.Triumph);
			var despairNetQuantity = poolResults.negative.CountMatchingKeys(Symbol.Despair);

			//triumphs count as successes but advantages do not and despairs count as failures but threats do not
			var successNetQuantity = poolResults.positive.CountMatchingKeys(Symbol.Success) + triumphNetQuantity - (poolResults.negative.CountMatchingKeys(Symbol.Failure) + despairNetQuantity);
			var advantageNetQuantity = poolResults.positive.CountMatchingKeys(Symbol.Advantage) - poolResults.negative.CountMatchingKeys(Symbol.Threat);

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
		}
	}
}
