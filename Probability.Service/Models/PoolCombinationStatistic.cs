﻿namespace Probability.Service.Models
{
	public class PoolCombinationStatistic
	{
		public int PositivePoolId { get; set; }

		public int NegativePoolId { get; set; }

		public Symbol Symbol { get; set; }

		public int Quantity { get; set; }

		public decimal Frequency { get; set; }

		public decimal AlternateTotal { get; set; }

		public PoolCombination PoolCombination { get; set; }

		public override int GetHashCode() => (Symbol, Quantity).GetHashCode();
	}
}
