using DataFramework.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Visualizer.Models
{
	public class SearchViewModel
	{
		public IEnumerable<PoolStatisticViewModel> Statistics { get; set; }

		public IEnumerable<DieViewModel> Dice { get; set; }

		public SearchViewModel()
		{
			Statistics = new Collection<PoolStatisticViewModel>();
			Dice = new Collection<DieViewModel>();
		}

		public SearchViewModel(IEnumerable<PoolCombinationStatistic> statistics, IEnumerable<PoolDie> dice)
		{
			Statistics = statistics.Select(stat => new PoolStatisticViewModel
			{
				Symbol = stat.Symbol.ToString(),
				Quantity = stat.Quantity,
				Frequency = stat.Frequency,
				AlternateTotal = stat.AlternateTotal
			});


			Dice = dice.Select(die => new DieViewModel
			{
				DieType = die.Die.Name,
				Quantity = die.Quantity
			});
		}
	}
}
