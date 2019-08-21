using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace DataFramework.Models
{
	public class PoolCombination
	{
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

		[JsonIgnore]
		public Pool PositivePool { get; set; }

		[JsonIgnore]
		public Pool NegativePool { get; set; }

		public ICollection<PoolCombinationStatistic> PoolCombinationStatistics { get; set; }

		/// <summary>
		/// Adds or Merges a new CombinationStatistic
		/// </summary>
		/// <param name="poolCombinationStatistic"></param>
		public void AddPoolCombinationStatistic(PoolCombinationStatistic poolCombinationStatistic)
		{
			var match = PoolCombinationStatistics.FirstOrDefault(stat => stat.Symbol == poolCombinationStatistic.Symbol && poolCombinationStatistic.Quantity == stat.Quantity);

			if (match != null)
			{
				match.AlternateTotal += poolCombinationStatistic.AlternateTotal * poolCombinationStatistic.Frequency;
				match.Frequency += poolCombinationStatistic.Frequency;
			}
			else
			{
				PoolCombinationStatistics.Add(poolCombinationStatistic);
			}
		}
	}
}
