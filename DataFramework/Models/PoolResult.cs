using Newtonsoft.Json;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using static DataFramework.Models.Die;

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

		public override int GetHashCode() => string.Join("", PoolResultSymbols.OrderBy(o => o.Symbol).Select(s => $"{s.Symbol}{s.Quantity}").ToList()).GetHashCode();
	}
}
