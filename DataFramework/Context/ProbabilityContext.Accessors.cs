using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using DataFramework.Models;
using Microsoft.EntityFrameworkCore;

namespace DataFramework.Context
{
	public static class ProbabilityContextAccessors
	{
		/// <summary>
		/// Returns the id for the pool with the matching dice, this is the hard way
		/// </summary>
		/// <param name="searchForPool"></param>
		/// <returns></returns>
		public static int? GetPoolId(ProbabilityContext context, List<PoolDie> searchForPool) =>
			searchForPool.Select(die => context.PoolDice.Where(w => w.DieId == die.DieId && w.Quantity == die.Quantity && w.Pool.PoolDice.Count == searchForPool.Count())
			.Select(s => s.PoolId)).Aggregate((result, next) => result.Intersect(next)).FirstOrDefault();

		/// <summary>
		/// Tries to find the pool id from a given name
		/// </summary>
		/// <param name="context"></param>
		/// <param name="poolName"></param>
		/// <returns></returns>
		public static int? GetPoolIdByName(this ProbabilityContext context, string poolName) => context.Pools.Where(w => w.Name == poolName).Select(s => s.PoolId).FirstOrDefault();

		/// <summary>
		/// Tries to get the pool ids split by positive and negative dice
		/// </summary>
		/// <param name="context"></param>
		/// <param name="pool"></param>
		/// <param name="poolIds"></param>
		/// <returns></returns>
		public static bool TryGetPoolIds(this ProbabilityContext context, Pool pool, out (int positiveId, int negativeId) poolIds)
		{
			poolIds = (
				context.GetPoolIdByName(pool.GetFilteredPoolName(DieExtensions.PositiveDice)) ?? 0,
				context.GetPoolIdByName(pool.GetFilteredPoolName(DieExtensions.NegativeDice)) ?? 0
				);

			return poolIds.positiveId > 0 && poolIds.negativeId > 0;
		}

		/// <summary>
		/// Removes either the positive or negative dice from the full pool to find the pool half
		/// </summary>
		/// <param name="context"></param>
		/// <param name="dice"></param>
		/// <param name="filters"></param>
		/// <returns></returns>
		public static string GetFilteredPoolName(this Pool pool, ImmutableList<DieNames> filters) => new Pool { PoolDice = pool.PoolDice.Where(w => filters.Contains(w.Die.Name.GetName())).ToList() }.ToString();

		/// <summary>
		/// Returns a Pool with it's results and dice
		/// </summary>
		/// <param name="context"></param>
		/// <param name="poolId"></param>
		/// <returns></returns>
		public static Pool GetPool(this ProbabilityContext context, long poolId) => context.Pools.Where(w => w.PoolId == poolId)
			.Include(i => i.PoolResults)
				.ThenInclude(i => i.PoolResultSymbols)
			.Include(i => i.PoolDice)
				.ThenInclude(i => i.Die)
			.FirstOrDefault();

		/// <summary>
		/// Gets a Pool Combination including the statistics and dice
		/// </summary>
		/// <param name="context"></param>
		/// <param name="poolIds"></param>
		/// <returns></returns>
		public static IEnumerable<PoolCombinationStatistic> GetPoolStatistics(this ProbabilityContext context, (int positiveId, int negativeId) poolIds) =>
			context.PoolCombinationStatistics.Where(w => w.PositivePoolId == poolIds.positiveId && w.NegativePoolId == poolIds.negativeId);

		/// <summary>
		/// Gets a Pool Combination including the statistics and dice
		/// </summary>
		/// <param name="context"></param>
		/// <param name="poolIds"></param>
		/// <returns></returns>
		public static IEnumerable<PoolDie> GetPoolDice(this ProbabilityContext context, (int positiveId, int negativeId) poolIds) =>
			context.PoolDice.Where(w => w.PoolId == poolIds.positiveId || w.PoolId == poolIds.negativeId)
			.Include(i => i.Die);
	}
}
