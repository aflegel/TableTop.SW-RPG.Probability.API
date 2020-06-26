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

		public SearchRollViewModel(IEnumerable<PoolResult> positiveResults, IEnumerable<PoolResult> negativeResults)
		{
			PositiveResults = positiveResults.ToResults();
			NegativeResults = negativeResults.ToResults();
		}
	}

	public static class SearchRollExtensions
	{
		public static async Task<SearchRollViewModel> ToSearchRoll(this (int positiveId, int negativeId) poolIds, ProbabilityContext context) =>
			new SearchRollViewModel(await context.GetPoolResults(poolIds.positiveId), await context.GetPoolResults(poolIds.negativeId));
	}
}
