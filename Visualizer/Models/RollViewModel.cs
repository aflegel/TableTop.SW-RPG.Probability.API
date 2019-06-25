using DataFramework.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Visualizer.Models
{
	public class RollViewModel
	{
		public IEnumerable<RollResultViewModel> PoolResults { get; set; }

		public IEnumerable<DieViewModel> Dice { get; set; }

		public RollViewModel()
		{
			PoolResults = new Collection<RollResultViewModel>();
			Dice = new Collection<DieViewModel>();
		}

		public RollViewModel(Pool searchPool)
		{
			PoolResults = searchPool.PoolResults.Select(stat =>
			new RollResultViewModel
			{
				Symbols = stat.PoolResultSymbols.Select(s => new RollSymbolViewModel { Symbol = s.Symbol.ToString(), Quantity = s.Quantity }),
				Frequency = stat.Frequency,
			});

			Dice = searchPool.PoolDice.Select(die => new DieViewModel
			{
				DieType = die.Die.Name,
				Quantity = die.Quantity
			});
		}
	}
}
