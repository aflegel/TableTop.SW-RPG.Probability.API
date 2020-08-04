using System.Collections.Generic;
using System.Threading.Tasks;
using DataFramework.Context;
using Microsoft.AspNetCore.Mvc;
using Visualizer.Models;

namespace Visualizer.Controllers
{
	[Produces("application/json")]
	[Route("[controller]")]
	[ApiController]
	public class SearchController : ControllerBase
	{
		private readonly ProbabilityContext context;

		public SearchController(ProbabilityContext context) => this.context = context;

		/// <summary>
		/// Returns the corresponding cached statistics for a given pool of dice
		/// </summary>
		/// <param name="dice"></param>
		/// <returns></returns>
		[HttpPost]
		public async Task<ActionResult<SearchViewModel>> Get([FromBody] List<DieViewModel> dice)
		{
			if (dice == null)
				return BadRequest();

			var poolIds = await context.GetPoolIds(dice.ToPool());

			return poolIds.HasValue ? await poolIds.Value.ToSearchView(context)
				: (ActionResult<SearchViewModel>)NotFound();
		}
	}
}
