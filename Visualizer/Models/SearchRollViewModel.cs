using System.Collections.Generic;
using System.Threading.Tasks;
using DataFramework.Context;
using DataFramework.Models;

namespace Visualizer.Models
{
	public class SearchRollViewModel
	{
		public IEnumerable<RollResultViewModel> PositiveResults { get; set; }
		public IEnumerable<RollResultViewModel> NegativeResults { get; set; }

		public SearchRollViewModel((IEnumerable<PoolResult> positiveResults, IEnumerable<PoolResult> negativeResults) data)
		{
			PositiveResults = data.positiveResults.ToResults();
			NegativeResults = data.negativeResults.ToResults();
		}
	}
}
