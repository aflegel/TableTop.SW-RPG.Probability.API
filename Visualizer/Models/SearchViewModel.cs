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
			Baseline = new PoolCombinationViewModel()
			{
				PositivePoolId = searchPool.PositivePoolId,
				NegativePoolId = searchPool.NegativePoolId,
				PoolCombinationStatistics = new Collection<PoolCombinationStatisticViewModel>()
			};

			foreach (var stat in searchPool.PoolCombinationStatistics)
			{
				Baseline.PoolCombinationStatistics.Add(new PoolCombinationStatisticViewModel()
				{
					Symbol = stat.Symbol,
					Quantity = stat.Quantity,
					Frequency = (ulong)stat.Frequency,
					AlternateTotal = stat.AlternateTotal
				});
			}

			BaseDice = searchPool.PositivePool.PoolDice.Union(searchPool.NegativePool.PoolDice);
		}

		public PoolCombinationViewModel Baseline { get; set; }
		public IEnumerable<PoolDie> BaseDice { get; set; }
	}

	public class PoolCombinationViewModel
	{
		public int PositivePoolId { get; set; }
		public int NegativePoolId { get; set; }

		public virtual ICollection<PoolCombinationStatisticViewModel> PoolCombinationStatistics { get; set; }
	}

	public class PoolCombinationStatisticViewModel
	{
		public Symbol Symbol { get; set; }
		public int Quantity { get; set; }
		public ulong Frequency { get; set; }
		public long AlternateTotal { get; set; }
	}
}