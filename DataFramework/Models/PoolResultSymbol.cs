using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DataFramework.Models.Die;

namespace DataFramework.Models
{
	public class PoolResultSymbol
	{
		public PoolResultSymbol()
		{
		}

		public PoolResultSymbol(Symbol Symbol, int Quantity)
		{
			this.Symbol = Symbol;
			this.Quantity = Quantity;
		}

		public override int GetHashCode()
		{
			//gets a unique has by summing the hash of the string and a hash of the value
			return ToString().GetHashCode();
		}

		public override string ToString()
		{
			return string.Format("{0}{1}", Symbol.ToString(), Quantity);
		}

		public long PoolResultId { get; set; }

		public Symbol Symbol { get; set; }

		public int Quantity { get; set; }

		[JsonIgnore]
		public virtual PoolResult PoolResult { get; set; }
	}
}
