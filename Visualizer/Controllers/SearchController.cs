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
		public SearchViewModel Get([FromBody]List<DieViewModel> dice)
		{
			if (dice == null)
			{
				return new SearchViewModel();
			}

			var combinedPool = context.SplitPoolByDice(dice.ToPool());

			return (combinedPool.Item1 != null && combinedPool.Item2 != null)
				? new SearchViewModel(context.GetPoolCombination(combinedPool.Item1, combinedPool.Item2))
				: new SearchViewModel();
		}
	}
}
