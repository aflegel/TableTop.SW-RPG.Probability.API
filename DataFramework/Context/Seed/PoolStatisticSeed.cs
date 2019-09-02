using System;
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
		public static IEnumerable<PoolCombination> CrossProduct(this (IEnumerable<Pool>, IEnumerable<Pool>) pools) =>
			pools.Item1.SelectMany(positivePool => pools.Item2, (positivePool, negativePool) => new PoolCombination(positivePool, negativePool).SeedStatistics());

		/// <summary>
		/// Compares the outcome of each pool's combined rolls
		/// </summary>
		public static PoolCombination SeedStatistics(this PoolCombination poolCombination)
		{
			PrintConsoleLog(poolCombination);

			poolCombination.PoolCombinationStatistics = (poolCombination.PositivePool.PoolResults, poolCombination.NegativePool.PoolResults).ResultCrossProduct().ToList();

			return poolCombination;
		}

		/// <summary>
		/// Takes the cross product of the result lists and sums their frequency and totals
		/// </summary>
		/// <param name="results"></param>
		/// <returns></returns>
		private static IEnumerable<PoolCombinationStatistic> ResultCrossProduct(this (IEnumerable<PoolResult>, IEnumerable<PoolResult>) results) =>
			results.Item1.SelectMany(positive => results.Item2, (positive, negative) => new OutcomeAnalysis(positive, negative)).SelectMany(f => f.ToStatistics())
				.GroupBy(g => g.GetHashCode()).Select(s => new PoolCombinationStatistic
				{
					Symbol = s.First().Symbol,
					Quantity = s.First().Quantity,
					AlternateTotal = s.Sum(sum => sum.AlternateTotal * sum.Frequency),
					Frequency = s.Sum(sum => sum.Frequency)
				});

		/// <summary>
		/// Displays the Unique List of rolls
		/// </summary>
		/// <param name="poolCombination"></param>
		private static void PrintConsoleLog(PoolCombination poolCombination)
		{
			PrintStartLog($"{poolCombination.PositivePool.Name}, {poolCombination.NegativePool.Name}", poolCombination.PositivePool.TotalOutcomes * poolCombination.NegativePool.TotalOutcomes);
			PrintFinishLog(poolCombination.PositivePool.UniqueOutcomes * poolCombination.NegativePool.UniqueOutcomes);
		}

		public static void PrintStartLog(string poolText, decimal rollEstimation) => Console.Write($"{poolText,-80}|{rollEstimation,29:n0}");

		public static void PrintFinishLog(decimal rollEstimation) => Console.Write($"  |{rollEstimation,12:n0}\n");
	}
}
