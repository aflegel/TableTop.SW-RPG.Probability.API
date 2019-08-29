using System;
using System.Collections.Generic;
using System.Linq;
using DataFramework.Context;
using static DataFramework.Models.Die;

namespace DataFramework.Models
{
	public class Pool
	{
		public Pool()
		{
			PoolDice = new HashSet<PoolDie>();
			PoolResults = new HashSet<PoolResult>();
			PositivePoolCombinations = new HashSet<PoolCombination>();
			NegativePoolCombinations = new HashSet<PoolCombination>();
		}

		public int PoolId { get; set; }

		public string Name { get; set; }

		public decimal TotalOutcomes { get; set; }

		public decimal UniqueOutcomes { get; set; }

		public ICollection<PoolDie> PoolDice { get; set; }

		public ICollection<PoolResult> PoolResults { get; set; }

		public ICollection<PoolCombination> PositivePoolCombinations { get; set; }

		public ICollection<PoolCombination> NegativePoolCombinations { get; set; }

		protected int PoolDiceCount => PoolDice.SumQuantity();

		public string PoolText => string.Join(", ", PoolDice.OrderBy(o => o.DieId).Select(group => $"{group.Die.Name} {group.Quantity}").ToList());

		public decimal RollEstimation => PoolDice.Aggregate((decimal)1, (x, y) => x * Convert.ToDecimal(Math.Pow(y.Die.DieFaces.Count, y.Quantity)));
	}

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
		private static Pool FilterDice(this Pool pool, List<DieNames> filters)
			=> new Pool { PoolDice = pool.PoolDice.Where(w => filters.Contains(w.Die.Name.GetName())).ToList() };

		public static Pool GetPoolByName(this ProbabilityContext context, string poolName) => context.Pools.FirstOrDefault(w => w.Name == poolName);

		public static Tuple<Pool,Pool> SplitPoolByDice(this Pool pool, ProbabilityContext context)
			=> new Tuple<Pool,Pool>(context.GetPoolByName(pool.FilterDice(PositiveDice).PoolText), context.GetPoolByName(pool.FilterDice(NegativeDice).PoolText));
	}
}
