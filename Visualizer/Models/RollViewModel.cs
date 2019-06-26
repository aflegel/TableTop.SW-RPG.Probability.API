using DataFramework.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Visualizer.Models
{
	public class RollViewModel
	{
		public IEnumerable<RollResultViewModel> PositiveResults { get; set; }

		public IEnumerable<RollResultViewModel> NegativeResults { get; set; }

		public IEnumerable<DieViewModel> Dice { get; set; }

		public RollViewModel()
		{
			PositiveResults = new Collection<RollResultViewModel>();
			NegativeResults = new Collection<RollResultViewModel>();
			Dice = new Collection<DieViewModel>();
		}

		public RollViewModel(Pool positivePool, Pool negativePool)
		{
			PositiveResults = SetPool(positivePool);
			NegativeResults = SetPool(negativePool);

			Dice = positivePool.PoolDice.Select(die => new DieViewModel
			{
				DieType = die.Die.Name,
				Quantity = die.Quantity
			});
		}

		private IEnumerable<RollResultViewModel> SetPool(Pool results)
		{
			return results.PoolResults.Select(stat =>
			new RollResultViewModel
			{
				RollSymbols = stat.PoolResultSymbols.Select(s => new RollSymbolViewModel { Symbol = s.Symbol.ToString(), Quantity = s.Quantity }),
				Frequency = stat.Frequency,
			});
		}
	}
}
