using DataFramework.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Visualizer.Models
{
	public class RollViewModel
	{
		public IEnumerable<RollResultViewModel> Results { get; set; }

		public IEnumerable<DieViewModel> Dice { get; set; }

		public RollViewModel()
		{
			Results = new Collection<RollResultViewModel>();
			Dice = new Collection<DieViewModel>();
		}

		public RollViewModel(Pool pool)
		{
			Results = pool.PoolResults.Select(stat => new RollResultViewModel
			{
				Symbols = stat.PoolResultSymbols.Select(s => new RollSymbolViewModel { Symbol = s.Symbol.ToString(), Quantity = s.Quantity }),
				Frequency = stat.Frequency,
			});

			Dice = pool.PoolDice.Select(die => new DieViewModel
			{
				DieType = die.Die.Name,
				Quantity = die.Quantity
			});
		}
	}
}
