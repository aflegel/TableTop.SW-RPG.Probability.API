using DataFramework.Context;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Visualizer.Models;

namespace Visualizer.Controllers
{
	[Produces("application/json")]
	[Route("[controller]")]
	public class SearchController : Controller
	{
		private readonly ProbabilityContext context;

		public SearchController(ProbabilityContext context)
		{
			this.context = context;
		}

		/// <summary>
		/// Returns the corresponding cached statistics for a given pool of dice
		/// </summary>
		/// <param name="dice"></param>
		/// <returns></returns>
		[HttpPost]
		public SearchViewModel Get([FromBody]List<DieViewModel> dice) => dice != null &&
			context.TryGetPoolIds(dice.ToPool(), out var poolIds)
				? new SearchViewModel(context.GetPoolCombination(poolIds))
				: new SearchViewModel();
	}
}
