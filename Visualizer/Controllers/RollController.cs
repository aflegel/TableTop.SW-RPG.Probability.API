using DataFramework.Context;
using DataFramework.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Visualizer.Models;

namespace Visualizer.Controllers
{
	[Produces("application/json")]
	[Route("[controller]")]
	[ApiController]
	public class RollController : ControllerBase
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
			context.TryGetPoolIds(dice.ToPool(), out var poolIds)
				? new SearchRollViewModel(context.GetPoolResults(poolIds.positiveId), context.GetPoolResults(poolIds.negativeId))
				: new SearchRollViewModel();
	}
}
