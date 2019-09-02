using System.Collections.Generic;
using System.Linq;

namespace DataFramework.Models
{
	public static class PoolExtensions
	{
		/// <summary>
		/// Clones a copy of the the dice from a pool
		/// </summary>
		/// <param name="pool"></param>
		/// <returns></returns>
		public static IEnumerable<PoolDie> CopyPoolDice(this Pool pool) => pool.PoolDice.Select(poolDie => new PoolDie(poolDie.Die, poolDie.Quantity));

		/// <summary>
		/// Removes either the positive or negative dice from the full pool to find the pool half
		/// </summary>
		/// <param name="context"></param>
		/// <param name="dice"></param>
		/// <param name="filters"></param>
		/// <returns></returns>
		public static Pool FilterDice(this Pool pool, List<DieNames> filters) => new Pool { PoolDice = pool.PoolDice.Where(w => filters.Contains(w.Die.Name.GetName())).ToList() };
	}
}
