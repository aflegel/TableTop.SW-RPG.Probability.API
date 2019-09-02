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
		/// Returns the id for the pool with the matching dice, this is the hard way
		/// </summary>
		/// <param name="searchForPool"></param>
		/// <returns></returns>
		public static int? GetPoolId(ProbabilityContext context, List<PoolDie> searchForPool) =>
			searchForPool.Select(die => context.PoolDice.Where(w => w.DieId == die.DieId && w.Quantity == die.Quantity && w.Pool.PoolDice.Count == searchForPool.Count())
			.Select(s => s.PoolId)).Aggregate((result, next) => result.Intersect(next)).FirstOrDefault();

		/// <summary>
		/// Trys to find the pool id from a given name
		/// </summary>
		/// <param name="context"></param>
		/// <param name="poolName"></param>
		/// <returns></returns>
		public static int? GetPoolIdByName(this ProbabilityContext context, string poolName) => context.Pools.Where(w => w.Name == poolName).Select(s => s.PoolId).FirstOrDefault();

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
			.FirstOrDefault();

		/// <summary>
		/// Returns all pools containing positive dice
		/// </summary>
		/// <param name="context"></param>
		/// <returns></returns>
		public static IEnumerable<Pool> GetPositivePools(this ProbabilityContext context) => context.GetPools(PositiveDice);

		/// <summary>
		/// Returns all pools containing negative dice
		/// </summary>
		/// <param name="context"></param>
		/// <returns></returns>
		public static IEnumerable<Pool> GetNegativePools(this ProbabilityContext context) => context.GetPools(NegativeDice);

		/// <summary>
		/// Returns all pools containing dice in the filter with the Statistics and Results
		/// </summary>
		/// <param name="context"></param>
		/// <param name="filters"></param>
		/// <returns></returns>
		public static IEnumerable<Pool> GetPools(this ProbabilityContext context, List<DieNames> filters) => context.Pools.Where(pool => pool.PoolDice.Any(die => filters.Contains(die.Die.Name.GetName())))
			.Include(i => i.PositivePoolCombinations)
					.ThenInclude(tti => tti.PoolCombinationStatistics)
			.Include(i => i.PoolResults)
					.ThenInclude(tti => tti.PoolResultSymbols);

		/// <summary>
		/// Trys to get the pool ids split by positive and negative dice
		/// </summary>
		/// <param name="context"></param>
		/// <param name="pool"></param>
		/// <param name="poolIds"></param>
		/// <returns></returns>
		public static bool TryGetPoolIds(this ProbabilityContext context, Pool pool, out (int positiveId, int negativeId) poolIds)
		{
			poolIds = (
				context.GetPoolIdByName(pool.FilterDice(PositiveDice).ToString()) ?? 0,
				context.GetPoolIdByName(pool.FilterDice(NegativeDice).ToString()) ?? 0
				);

			return poolIds.positiveId > 0 && poolIds.negativeId > 0;
		}

		/// <summary>
		/// Gets a Pool Combination including the statistics and dice
		/// </summary>
		/// <param name="context"></param>
		/// <param name="poolIds"></param>
		/// <returns></returns>
		public static PoolCombination GetPoolCombination(this ProbabilityContext context, (int positiveId, int negativeId) poolIds) => context.PoolCombinations.Where(w => w.PositivePoolId == poolIds.positiveId && w.NegativePoolId == poolIds.negativeId)
			.Include(i => i.PoolCombinationStatistics)
			.Include(i => i.PositivePool.PoolDice)
			.Include(i => i.NegativePool.PoolDice)
			.FirstOrDefault();
	}
}
