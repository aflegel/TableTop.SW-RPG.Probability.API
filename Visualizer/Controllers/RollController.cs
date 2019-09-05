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
			context.TryGetPoolIds(dice.ToPool(), out var poolIds)
				? new SearchRollViewModel()
				{
					PositiveRolls = new RollViewModel(context.GetPoolResults(poolIds.positiveId), context.GetPoolDice(poolIds.positiveId)),
					NegativeRolls = new RollViewModel(context.GetPoolResults(poolIds.negativeId), context.GetPoolDice(poolIds.negativeId))
				}
				: new SearchRollViewModel();
	}
}
