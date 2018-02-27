using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DataFramework.Models.Die;

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

			foreach (var resultSymbol in PoolResultSymbols)
			{
				this.PoolResultSymbols.Add(resultSymbol);
			}
		}

		public long PoolResultId { get; set; }

		public long PoolId { get; set; }

		public long Quantity { get; set; }

		[JsonIgnore]
		public virtual Pool Pool { get; set; }

		public virtual ICollection<PoolResultSymbol> PoolResultSymbols { get; set; }

		/// <summary>
		/// Returns a sum of the Symbols in the map
		/// </summary>
		/// <param name="map"></param>
		/// <param name="keys"></param>
		/// <returns></returns>
		public int CountMatchingKeys(Symbol key)
		{
			return PoolResultSymbols.Where(a => a.Symbol == key).Sum(s => s.Quantity);
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
	}
}
