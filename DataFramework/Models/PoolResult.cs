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
	}
}
