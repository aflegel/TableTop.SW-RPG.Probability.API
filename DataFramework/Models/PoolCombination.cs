using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataFramework.Models
{
	public class PoolCombination
	{
		public PoolCombination()
		{
			PoolCombinationStatistics = new HashSet<PoolCombinationStatistic>();
		}

		public long PositivePoolId { get; set; }
		public long NegativePoolId { get; set; }

		[JsonIgnore]
		public virtual Pool PositivePool { get; set; }
		[JsonIgnore]
		public virtual Pool NegativePool { get; set; }

		public virtual ICollection<PoolCombinationStatistic> PoolCombinationStatistics { get; set; }

		public void AddPoolCombinationStatistic(PoolCombinationStatistic poolCombinationStatistic)
		{
			foreach (var stat in PoolCombinationStatistics)
			{
				if (poolCombinationStatistic.Symbol  == stat.Symbol && poolCombinationStatistic.Quantity == stat.Quantity)
				{
					stat.Frequency += poolCombinationStatistic.Frequency;
					return;
				}
			}
			PoolCombinationStatistics.Add(poolCombinationStatistic);
		}
	}
}
