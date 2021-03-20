using System.Collections.Generic;

namespace Probability.Service.Models
{
	public class PoolCombination
	{
		public PoolCombination() => PoolCombinationStatistics = new HashSet<PoolCombinationStatistic>();

		public int PositivePoolId { get; set; }

		public int NegativePoolId { get; set; }

		public Pool PositivePool { get; set; }

		public Pool NegativePool { get; set; }

		public ICollection<PoolCombinationStatistic> PoolCombinationStatistics { get; set; }
	}
}
