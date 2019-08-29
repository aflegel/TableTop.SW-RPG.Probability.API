using Newtonsoft.Json;
using System;
using static DataFramework.Models.Die;

namespace DataFramework.Models
{
	public class PoolCombinationStatistic
	{
		public int PositivePoolId { get; set; }

		public int NegativePoolId { get; set; }

		public Symbol Symbol { get; set; }

		public int Quantity { get; set; }

		public decimal Frequency { get; set; }

		public decimal AlternateTotal { get; set; }

		[JsonIgnore]
		public PoolCombination PoolCombination { get; set; }

		public override int GetHashCode() => unchecked(Symbol.ToString().GetHashCode() + Quantity);
	}
}
