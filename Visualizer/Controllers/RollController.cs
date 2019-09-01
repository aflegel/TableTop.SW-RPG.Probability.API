using DataFramework.Context;
using DataFramework.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Visualizer.Models;

namespace Visualizer.Controllers
{
	[Produces("application/json")]
	[Route("[controller]")]
	public class RollController : Controller
	{
		private readonly ProbabilityContext context;

		public RollController(ProbabilityContext context)
		{
			this.context = context;
		}

		/// <summary>
		/// Returns the corresponding cached statistics for a given pool of dice
		/// </summary>
		/// <param name="dice"></param>
		/// <returns></returns>
		[HttpPost]
		public SearchRollViewModel Get([FromBody]List<DieViewModel> dice) => dice != null &&
			context.TrySplitPool(dice.ToPool(), out var poolIds)
				? new SearchRollViewModel(context.GetPool(poolIds.Item1), context.GetPool(poolIds.Item2))
				: new SearchRollViewModel();
	}
}
