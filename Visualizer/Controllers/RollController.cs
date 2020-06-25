using DataFramework.Context;
using DataFramework.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Visualizer.Models;

namespace Visualizer.Controllers
{
	[Produces("application/json")]
	[Route("[controller]")]
	[ApiController]
	public class RollController : ControllerBase
	{
		private readonly ProbabilityContext context;

		public RollController(ProbabilityContext context) => this.context = context;

		/// <summary>
		/// Returns the corresponding cached statistics for a given pool of dice
		/// </summary>
		/// <param name="dice"></param>
		/// <returns></returns>
		[HttpPost]
		public async Task<ActionResult<SearchRollViewModel>> Get([FromBody] List<DieViewModel> dice)
		{
			if (dice == null)
				return BadRequest();
			
			var poolIds = await context.TryGetPoolIds(dice.ToPool());

			return poolIds.HasValue 
				? new SearchRollViewModel(await context.GetPoolResults(poolIds.Value.positiveId), await context.GetPoolResults(poolIds.Value.negativeId))
				: (ActionResult<SearchRollViewModel>)NotFound();
		}
	}
}
