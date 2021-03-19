using System.Collections.Generic;
using System.Linq;
using Probability.Service.Models;

namespace Visualizer.Models
{
	public class SearchViewModel
	{
		public IEnumerable<PoolStatisticViewModel> Statistics { get; set; }

		public IEnumerable<DieViewModel> Dice { get; set; }

		public SearchViewModel((IEnumerable<PoolCombinationStatistic> statistics, IEnumerable<PoolDie> dice) data)
		{
			Statistics = data.statistics.Select(stat => new PoolStatisticViewModel
			{
				Symbol = stat.Symbol.ToString(),
				Quantity = stat.Quantity,
				Frequency = stat.Frequency,
				AlternateTotal = stat.AlternateTotal
			});


			Dice = data.dice.Select(die => new DieViewModel
			{
				DieType = die.Die.Name,
				Quantity = die.Quantity
			});
		}
	}
}
