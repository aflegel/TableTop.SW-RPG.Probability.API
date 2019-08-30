using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace DataFramework.Models
{
	public class PoolResult
	{
		public PoolResult()
		{
			PoolResultSymbols = new HashSet<PoolResultSymbol>();
		}

		public int PoolResultId { get; set; }

		public int PoolId { get; set; }

		public decimal Frequency { get; set; }

		[JsonIgnore]
		public Pool Pool { get; set; }

		public ICollection<PoolResultSymbol> PoolResultSymbols { get; set; }

		/// <summary>
		/// Returns a sum of the Symbols in the map
		/// </summary>
		/// <param name="key"></param>
		/// <returns></returns>
		public int CountMatchingKeys(Symbol key) => PoolResultSymbols.Where(a => a.Symbol == key).Sum(s => s.Quantity);

		public override int GetHashCode() => PoolResultSymbols.Select(s => s.GetHashCode()).Aggregate(1, (a, b) => unchecked(a * b));
	}
}
