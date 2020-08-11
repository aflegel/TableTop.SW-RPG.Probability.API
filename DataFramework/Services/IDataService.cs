using System.Collections.Generic;
using System.Threading.Tasks;
using DataFramework.Models;

namespace DataFramework.Services
{
	public interface IDataService
	{
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
