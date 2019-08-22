using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

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

		public string PoolText => string.Join(", ", PoolDice.Select(group => $"{group.Die.Name} {group.Quantity}").ToList());

		public decimal RollEstimation => PoolDice.Aggregate((decimal)1, (x, y) => x * Convert.ToDecimal(Math.Pow(y.Die.DieFaces.Count, y.Quantity)));
	}

	public static class PoolExtensions
	{
		public static ICollection<PoolDie> CopyPoolDice(this Pool pool) => pool.PoolDice.Select(poolDie => new PoolDie(poolDie.Die, poolDie.Quantity)).ToList();
	}
}
