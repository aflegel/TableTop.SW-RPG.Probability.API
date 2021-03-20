using System.Collections.Generic;
using System.Linq;
using Probability.Service.Models;

namespace Visualizer.Models
{
	public static class SearchRollViewModelExtensions
	{
		public static IEnumerable<RollResultViewModel> ToResults(this IEnumerable<PoolResult> results)
			=> results.Select(stat => new RollResultViewModel
			{
				Symbols = stat.PoolResultSymbols.Select(s => new RollSymbolViewModel { Symbol = s.Symbol.ToString(), Quantity = s.Quantity }),
				Frequency = stat.Frequency,
			});
	}
}
