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

		public int PositivePoolId { get; set; }
		public int NegativePoolId { get; set; }

		[JsonIgnore]
		public virtual Pool PositivePool { get; set; }
		[JsonIgnore]
		public virtual Pool NegativePool { get; set; }

		public virtual ICollection<PoolCombinationStatistic> PoolCombinationStatistics { get; set; }

		/// <summary>
		/// Adds or Merges a new CombinationStatistic
		/// </summary>
		/// <param name="poolCombinationStatistic"></param>
		public void AddPoolCombinationStatistic(PoolCombinationStatistic poolCombinationStatistic)
		{
			foreach (var stat in PoolCombinationStatistics)
			{
				if (poolCombinationStatistic.Symbol == stat.Symbol && poolCombinationStatistic.Quantity == stat.Quantity)
				{
					//calculate the new frequency
					var updatedFrequency = (ulong)stat.Frequency + (ulong)poolCombinationStatistic.Frequency;

					//update the running average.  A running total will result in numbers too large for Int64
					stat.OffSymbolAverage = ((stat.OffSymbolAverage * stat.Frequency) + (poolCombinationStatistic.OffSymbolAverage * poolCombinationStatistic.Frequency)) / updatedFrequency;

					stat.Frequency = (long)updatedFrequency;

					return;
				}
			}
			PoolCombinationStatistics.Add(poolCombinationStatistic);
		}
	}
}
