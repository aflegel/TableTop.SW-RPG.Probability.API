using System.Collections.Immutable;
using System.Linq;
using DataFramework.Models;

namespace DataFramework.Context
{
	public static class ProbabilityContextAccessors
	{
		/// <summary>
		/// Removes either the positive or negative dice from the full pool to find the pool half
		/// </summary>
		/// <param name="context"></param>
		/// <param name="dice"></param>
		/// <param name="filters"></param>
		/// <returns></returns>
		public static string GetFilteredPoolName(this Pool pool, ImmutableList<string> filters) => new Pool { PoolDice = pool.PoolDice.Where(w => filters.Contains(w.Die.Name)).ToList() }.ToString();
	}
}
