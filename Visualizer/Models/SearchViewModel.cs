using DataFramework.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using static DataFramework.Models.Die;

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

			foreach (var stat in searchPool.PoolCombinationStatistics)
			{
				PoolStatistics.Add(new PoolCombinationStatisticViewModel()
				{
					Symbol = stat.Symbol,
					Quantity = stat.Quantity,
					Frequency = (ulong)stat.Frequency,
					AlternateTotal = stat.AlternateTotal
				});
			}

			Dice = searchPool.PositivePool.PoolDice.Union(searchPool.NegativePool.PoolDice);
		}

		public ICollection<PoolCombinationStatisticViewModel> PoolStatistics { get; set; }

		public IEnumerable<PoolDie> Dice { get; set; }
	}

	public class PoolCombinationStatisticViewModel
	{
		public Symbol Symbol { get; set; }
		public int Quantity { get; set; }
		public ulong Frequency { get; set; }
		public long AlternateTotal { get; set; }
	}
}
