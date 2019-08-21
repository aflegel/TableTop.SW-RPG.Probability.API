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

		public PoolResult(List<PoolResultSymbol> poolResultSymbols)
		{
			PoolResultSymbols = new HashSet<PoolResultSymbol>(poolResultSymbols);
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

	public static class PoolResultExtension
	{
		private static readonly PoolResultSymbolEqualityComparer comparer = new PoolResultSymbolEqualityComparer();

		public static PoolResult GetMatch(this Collection<PoolResult> result, PoolResult mergedPool)
			=> result.FirstOrDefault(existing => existing.PoolResultSymbols.Count == mergedPool.PoolResultSymbols.Count
				&& !existing.PoolResultSymbols.Except(mergedPool.PoolResultSymbols, comparer).Any()
				&& !mergedPool.PoolResultSymbols.Except(existing.PoolResultSymbols, comparer).Any());
	}
}
