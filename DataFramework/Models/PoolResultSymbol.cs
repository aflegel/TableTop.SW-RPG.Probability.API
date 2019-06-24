using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DataFramework.Models.Die;

namespace DataFramework.Models
{
	public class PoolResultSymbol : IEquatable<PoolResultSymbol>
	{
		public PoolResultSymbol() { }

		public PoolResultSymbol(Symbol Symbol, int Quantity)
		{
			this.Symbol = Symbol;
			this.Quantity = Quantity;
		}

		public int PoolResultId { get; set; }

		public Symbol Symbol { get; set; }

		public int Quantity { get; set; }

		[JsonIgnore]
		public PoolResult PoolResult { get; set; }

		public bool Equals(PoolResultSymbol other)
		{
			if (Symbol != other.Symbol)
				return false;
			if (Quantity != other.Quantity)
				return false;

			return true;
		}
	}

	public class PoolResultSymbolEqualityComparer : IEqualityComparer<PoolResultSymbol>
	{
		public bool Equals(PoolResultSymbol x, PoolResultSymbol y)
		{
			return x.Equals(y);
		}

		public int GetHashCode(PoolResultSymbol obj)
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
