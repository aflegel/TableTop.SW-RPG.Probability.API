using DataFramework.Models;
using System.Collections.Generic;
using System.Linq;

namespace Visualizer.Models
{
	public class RollViewModel
	{
		public IEnumerable<RollResultViewModel> Results { get; set; }

		public IEnumerable<DieViewModel> Dice { get; set; }

		public RollViewModel()
		{
			Results = new List<RollResultViewModel>();
			Dice = new List<DieViewModel>();
		}

		public RollViewModel(IEnumerable<PoolResult> poolResults, IEnumerable<PoolDie> poolDice)
		{
			Results = poolResults.Select(stat => new RollResultViewModel
			{
				Symbols = stat.PoolResultSymbols.Select(s => new RollSymbolViewModel { Symbol = s.Symbol.ToString(), Quantity = s.Quantity }),
				Frequency = stat.Frequency,
			});

			Dice = poolDice.Select(die => new DieViewModel
			{
				DieType = die.Die.Name,
				Quantity = die.Quantity
			});
		}
	}
}
