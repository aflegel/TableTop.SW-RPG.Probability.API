using System;
using System.Collections.Generic;
using System.Linq;
using DataFramework.Models;
using Microsoft.EntityFrameworkCore;
using static DataFramework.Models.DieExtensions;

namespace DataFramework.Context
{
	public static class ProbabilityContextAccessors
	{
		/// <summary>
		/// Returns a Die with it's faces and face symbols
		/// </summary>
		/// <param name="context"></param>
		/// <param name="die"></param>
		/// <returns></returns>
		public static Die GetDie(this ProbabilityContext context, DieNames die) => context.GetDie(die.ToString());

		public static Die GetDie(this ProbabilityContext context, string die) => context.Dice.Where(w => w.Name == die.ToString())
			.Include(i => i.DieFaces)
				.ThenInclude(t => t.DieFaceSymbols)
			.FirstOrDefault();

		/// <summary>
		/// Returns the id for the pool with the matching dice
		/// </summary>
		/// <param name="searchForPool"></param>
		/// <returns></returns>
		public static int? GetPoolId(ProbabilityContext context, List<PoolDie> searchForPool) =>
			searchForPool.Select(die => context.PoolDice.Where(w => w.DieId == die.DieId && w.Quantity == die.Quantity && w.Pool.PoolDice.Count == searchForPool.Count())
			.Select(s => s.PoolId)).Aggregate((result, next) => result.Intersect(next)).FirstOrDefault();

		public static Pool GetPoolByName(this ProbabilityContext context, string poolName) => context.Pools.FirstOrDefault(w => w.Name == poolName);

		public static Pool GetPool(this ProbabilityContext context, long poolId) => context.Pools.Where(w => w.PoolId == poolId)
			.Include(i => i.PoolResults)
				.ThenInclude(i => i.PoolResultSymbols)
			.Include(i => i.PoolDice)
			.FirstOrDefault();

		public static bool TrySplitPool(this ProbabilityContext context, Pool pool, out Tuple<int, int> poolIds)
		{
			poolIds = new Tuple<int, int>(
				context.GetPoolByName(pool.FilterDice(PositiveDice).ToString())?.PoolId ?? 0,
				context.GetPoolByName(pool.FilterDice(NegativeDice).ToString())?.PoolId ?? 0);

			return poolIds.Item1 > 0 && poolIds.Item2 > 0;
		}

		public static PoolCombination GetPoolCombination(this ProbabilityContext context, Tuple<int, int> poolIds) => context.PoolCombinations.Where(w => w.PositivePoolId == poolIds.Item1 && w.NegativePoolId == poolIds.Item2)
			.Include(i => i.PoolCombinationStatistics)
			.Include(i => i.PositivePool.PoolDice)
			.Include(i => i.NegativePool.PoolDice)
			.FirstOrDefault();
	}
}
