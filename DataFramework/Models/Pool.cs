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

		public long TotalOutcomes { get; set; }
		public long UniqueOutcomes { get; set; }

		public virtual ICollection<PoolDie> PoolDice { get; set; }
		public virtual ICollection<PoolResult> PoolResults { get; set; }

		public virtual ICollection<PoolCombination> PositivePoolCombinations { get; set; }
		public virtual ICollection<PoolCombination> NegativePoolCombinations { get; set; }

		protected int GetPoolDiceCount()
		{
			return PoolDice.Sum(die => die.Quantity);
		}

		public string GetPoolText()
		{
			return string.Join(", ", PoolDice.Select(group => string.Format("{0} {1}", group.Die.Name, group.Quantity)).ToList());
		}

		public ulong GetRollEstimation()
		{
			return PoolDice.Aggregate((ulong)1, (x, y) => x * Convert.ToUInt64(Math.Pow(y.Die.DieFaces.Count, y.Quantity)));
		}

	}
}
