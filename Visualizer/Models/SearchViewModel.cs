using DataFramework.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Visualizer.Models
{
	public class SearchViewModel
	{
		public SearchViewModel()
		{

		}

		public SearchViewModel(PoolCombination searchPool)
		{
			PoolStatistics = new Collection<PoolCombinationStatisticViewModel>();
			Dice = new Collection<DieViewModel>();

			foreach (var stat in searchPool.PoolCombinationStatistics)
			{
				PoolStatistics.Add(new PoolCombinationStatisticViewModel()
				{
					Symbol = stat.Symbol.ToString(),
					Quantity = stat.Quantity,
					Frequency = (ulong)stat.Frequency,
					AlternateTotal = stat.AlternateTotal
				});
			}

			foreach (var die in searchPool.PositivePool.PoolDice.Union(searchPool.NegativePool.PoolDice))
			{
				Dice.Add(new DieViewModel()
				{
					DieType = die.Die.Name,
					Quantity = die.Quantity
				});
			}
		}

		public ICollection<PoolCombinationStatisticViewModel> PoolStatistics { get; set; }

		public ICollection<DieViewModel> Dice { get; set; }
	}

	public class PoolCombinationStatisticViewModel
	{
		public string Symbol { get; set; }
		public int Quantity { get; set; }
		public ulong Frequency { get; set; }
		public long AlternateTotal { get; set; }
	}

	public class DieViewModel
	{
		public string DieType { get; set; }
		public int Quantity { get; set; }
	}
}
