using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using static DataFramework.Models.Die;

namespace DataFramework.Models
{
	public class PoolCombinationStatistic : IEquatable<PoolCombinationStatistic>
	{
		public int PositivePoolId { get; set; }

		public int NegativePoolId { get; set; }

		public Symbol Symbol { get; set; }

		public int Quantity { get; set; }

		public decimal Frequency { get; set; }

		public decimal AlternateTotal { get; set; }

		[JsonIgnore]
		public PoolCombination PoolCombination { get; set; }

		public bool Equals(PoolCombinationStatistic other) => Symbol != other.Symbol ? false : Quantity == other.Quantity;
	}

	public class PoolCombinationStatisticEqualityComparer : IEqualityComparer<PoolCombinationStatistic>
	{
		public bool Equals(PoolCombinationStatistic x, PoolCombinationStatistic y) => x.Equals(y);

		public int GetHashCode(PoolCombinationStatistic obj) => obj == null ? 0 : unchecked(obj.Symbol.ToString().GetHashCode() + obj.Quantity);
	}
}
