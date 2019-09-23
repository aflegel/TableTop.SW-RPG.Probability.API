using System.Collections.Generic;
using System.Linq;
using DataFramework.Models;

namespace Visualizer.Models
{
	public class SearchRollViewModel
	{
		public IEnumerable<RollResultViewModel> PositiveResults { get; set; }
		public IEnumerable<RollResultViewModel> NegativeResults { get; set; }

		public SearchRollViewModel()
		{
			PositiveResults = new List<RollResultViewModel>();
			NegativeResults = new List<RollResultViewModel>();
		}

		public SearchRollViewModel(IEnumerable<PoolResult> positiveResults, IEnumerable<PoolResult> negativeResults)
		{
			PositiveResults = positiveResults.ToResults();
			NegativeResults = negativeResults.ToResults();
		}
	}
}
