using DataFramework.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Visualizer.Models
{
	public class SearchViewModel
	{
		public ICollection<PoolStatisticViewModel> PoolStatistics { get; set; }

		public ICollection<DieViewModel> Dice { get; set; }

		public SearchViewModel()
		{
			PoolStatistics = new Collection<PoolStatisticViewModel>();
			Dice = new Collection<DieViewModel>();
		}

		public SearchViewModel(PoolCombination searchPool)
		{
			PoolStatistics = new Collection<PoolStatisticViewModel>();
			Dice = new Collection<DieViewModel>();

			foreach (var stat in searchPool.PoolCombinationStatistics)
			{
				PoolStatistics.Add(new PoolStatisticViewModel()
				{
					Symbol = stat.Symbol.ToString(),
					Quantity = stat.Quantity,
					Frequency = stat.Frequency,
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
	}
}
