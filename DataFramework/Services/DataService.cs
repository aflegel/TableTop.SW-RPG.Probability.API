using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataFramework.Context;
using DataFramework.Models;
using Microsoft.EntityFrameworkCore;

namespace DataFramework.Services
{
	public class DataService
	{
		public ProbabilityContext Context { get; }

		public DataService(ProbabilityContext context) => Context = context;

		/// <summary>
		/// Tries to find the pool id from a given name
		/// </summary>
		/// <param name="poolName"></param>
		/// <returns></returns>
		public async Task<int?> GetPoolIdByName(string poolName) => await Context.Pools.AsQueryable().Where(w => w.Name == poolName).Select(s => s.PoolId).FirstOrDefaultAsync();

		/// <summary>
		/// Tries to get the pool ids split by positive and negative dice
		/// </summary>
		/// <param name="pool"></param>
		/// <param name="poolIds"></param>
		/// <returns></returns>
		public async Task<(int positiveId, int negativeId)?> GetPoolIds(Pool pool)
		{
			var poolIds = (
				positiveId: await GetPoolIdByName(pool.GetFilteredPoolName(DieExtensions.PositiveDice)) ?? 0,
				negativeId: await GetPoolIdByName(pool.GetFilteredPoolName(DieExtensions.NegativeDice)) ?? 0
				);

			return poolIds.positiveId > 0 && poolIds.negativeId > 0 ? ((int, int)?)poolIds : null;
		}

		/// <summary>
		/// Returns a set of PoolResults for a given pool id
		/// </summary>
		/// <param name="poolId"></param>
		/// <returns></returns>
		public Task<List<PoolResult>> GetPoolResults(int poolId) => Context.PoolResults.AsQueryable().Where(w => w.PoolId == poolId).Include(i => i.PoolResultSymbols).ToListAsync();

		/// <summary>
		/// Gets a list of PoolCombinationStatistics for a set of pool ids
		/// </summary>
		/// <param name="poolIds"></param>
		/// <returns></returns>
		public Task<List<PoolCombinationStatistic>> GetPoolStatistics((int positiveId, int negativeId) poolIds) =>
			Context.PoolCombinationStatistics.AsQueryable().Where(w => w.PositivePoolId == poolIds.positiveId && w.NegativePoolId == poolIds.negativeId).ToListAsync();

		/// <summary>
		/// Gets the dice and quantity for a given set of pool ids
		/// </summary>
		/// <param name="poolIds"></param>
		/// <returns></returns>
		public Task<List<PoolDie>> GetPoolDice((int positiveId, int negativeId) poolIds) =>
			Context.PoolDice.AsQueryable().Where(w => w.PoolId == poolIds.positiveId || w.PoolId == poolIds.negativeId)
			.Include(i => i.Die).ToListAsync();

		/// <summary>
		/// Gets the dice and quantity for a given pool id
		/// </summary>
		/// <param name="poolId"></param>
		/// <returns></returns>
		public Task<List<PoolDie>> GetPoolDice(int poolId) => GetPoolDice((poolId, -1));

		public async Task<(List<PoolCombinationStatistic>, List<PoolDie>)> ToSearchView((int positiveId, int negativeId) poolIds) => (await GetPoolStatistics(poolIds), await GetPoolDice(poolIds));

		public async Task<(List<PoolResult>, List<PoolResult>)> ToSearchRoll((int positiveId, int negativeId) poolIds) =>
			(await GetPoolResults(poolIds.positiveId), await GetPoolResults(poolIds.negativeId));
	}
}
