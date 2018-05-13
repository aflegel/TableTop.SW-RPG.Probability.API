using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DataFramework.Models.Die;

namespace DataFramework.Models
{
	public class PoolCombinationStatistic : IEquatable<PoolCombinationStatistic>
	{
		public PoolCombinationStatistic()
		{
		}

		public int PositivePoolId { get; set; }
		public int NegativePoolId { get; set; }
		public Symbol Symbol { get; set; }
		public int Quantity { get; set; }
		public long Frequency { get; set; }
		public decimal OffSymbolAverage { get; set; }

		[JsonIgnore]
		public virtual PoolCombination PoolCombination { get; set; }

		public bool Equals(PoolCombinationStatistic other)
		{
			if (Symbol != other.Symbol)
				return false;
			if (Quantity != other.Quantity)
				return false;

			return true;
		}
	}

	public class PoolCombinationStatisticEqualityComparer : IEqualityComparer<PoolCombinationStatistic>
	{
		public bool Equals(PoolCombinationStatistic x, PoolCombinationStatistic y)
		{
			return x.Equals(y);
		}

		public int GetHashCode(PoolCombinationStatistic obj)
		{
			unchecked
			{
				if (obj == null)
					return 0;

				return obj.Symbol.ToString().GetHashCode() + obj.Quantity;
			}
		}
	}
}
