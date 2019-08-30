using System.Collections.Generic;

namespace DataFramework.Models
{
	public class PoolCombination
	{
		public PoolCombination()
		{
			PoolCombinationStatistics = new HashSet<PoolCombinationStatistic>();
		}

		public PoolCombination(Pool positivePool, Pool negativePool)
		{
			PoolCombinationStatistics = new HashSet<PoolCombinationStatistic>();

			PositivePool = positivePool;
			NegativePool = negativePool;

			positivePool.PositivePoolCombinations.Add(this);
			negativePool.NegativePoolCombinations.Add(this);
		}

		public int PositivePoolId { get; set; }

		public int NegativePoolId { get; set; }

		public Pool PositivePool { get; set; }

		public Pool NegativePool { get; set; }

		public ICollection<PoolCombinationStatistic> PoolCombinationStatistics { get; set; }
	}
}
