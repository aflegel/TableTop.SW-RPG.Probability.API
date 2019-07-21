using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

		protected int PoolDiceCount => PoolDice.Sum(die => die.Quantity);

		public string PoolText => string.Join(", ", PoolDice.Select(group => $"{group.Die.Name} {group.Quantity}").ToList());

		public decimal RollEstimation => PoolDice.Aggregate((decimal)1, (x, y) => x * Convert.ToDecimal(Math.Pow(y.Die.DieFaces.Count, y.Quantity)));
	}
}
