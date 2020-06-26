using DataFramework.Context;
using DataFramework.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Visualizer.Models
{
	public class SearchViewModel
	{
		public IEnumerable<PoolStatisticViewModel> Statistics { get; set; }

		public IEnumerable<DieViewModel> Dice { get; set; }

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

	public static class SearchViewExtensions
	{
		public static async Task<SearchViewModel> ToSearchView(this (int positiveId, int negativeId) poolIds, ProbabilityContext context) =>
			new SearchViewModel(await context.GetPoolStatistics(poolIds), await context.GetPoolDice(poolIds));
	}
}
