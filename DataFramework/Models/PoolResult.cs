using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataFramework.Models
{
	public class PoolResult
	{
		public PoolResult()
		{
			//something like 8 million records
			PoolResultSymbols = new HashSet<PoolResultSymbol>();
		}

		public PoolResult(List<PoolResultSymbol> PoolResultSymbols)
		{
			//something like 8 million records
			this.PoolResultSymbols = new HashSet<PoolResultSymbol>();

			foreach(var resultSymbol in PoolResultSymbols)
			{
				this.PoolResultSymbols.Add(resultSymbol);
			}
		}

		public override int GetHashCode()
		{
			//gets a unique has by summing the hash of the string and a hash of the value
			return ToString().GetHashCode();
		}

		public override string ToString()
		{
			var ordered = PoolResultSymbols.OrderBy(x => x.Symbol.ToString());

			return string.Join("", ordered.Select(x => x.ToString()));
		}

		public long PoolResultId { get; set; }

		public long PoolId { get; set; }

		public long Quantity { get; set; }

		public virtual Pool Pool { get; set; }

		public virtual ICollection<PoolResultSymbol> PoolResultSymbols { get; set; }

	}
}
