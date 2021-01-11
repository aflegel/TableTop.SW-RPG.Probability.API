using System.Collections.Generic;
using System.Threading.Tasks;
using DataFramework.Models;
using Functional;

namespace DataFramework.Services
{
	public interface IDataService
	{
		Task<Option<int>> GetPoolIdByNameF(string poolName);

		Task<Option<(int positiveId, int negativeId)>> GetPoolIdsF(Pool pool);

		Task<int?> GetPoolIdByName(string poolName);

		Task<(int positiveId, int negativeId)?> GetPoolIds(Pool pool);

		Task<List<PoolResult>> GetPoolResults(int poolId);

		Task<List<PoolCombinationStatistic>> GetPoolStatistics((int positiveId, int negativeId) poolIds);

		Task<List<PoolDie>> GetPoolDice((int positiveId, int negativeId) poolIds);

		Task<List<PoolDie>> GetPoolDice(int poolId);

		Task<(List<PoolCombinationStatistic>, List<PoolDie>)> ToSearchView((int positiveId, int negativeId) poolIds);

		Task<(List<PoolResult>, List<PoolResult>)> ToSearchRoll((int positiveId, int negativeId) poolIds);
	}
}
