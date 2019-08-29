using Newtonsoft.Json;
using System;
using static DataFramework.Models.Die;

namespace DataFramework.Models
{
	public class PoolResultSymbol : IEquatable<PoolResultSymbol>
	{
		public PoolResultSymbol(Symbol symbol, int quantity)
		{
			Symbol = symbol;
			Quantity = quantity;
		}

		public int PoolResultId { get; set; }

		public Symbol Symbol { get; set; }

		public int Quantity { get; set; }

		[JsonIgnore]
		public PoolResult PoolResult { get; set; }

		public bool Equals(PoolResultSymbol other) => Symbol != other.Symbol ? false : Quantity == other.Quantity;
	}
}
