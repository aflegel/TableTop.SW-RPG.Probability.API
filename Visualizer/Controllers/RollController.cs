using System.Collections.Generic;
using System.Threading.Tasks;
using DataFramework.Models;
using DataFramework.Services;
using Microsoft.AspNetCore.Mvc;
using Visualizer.Models;

namespace Visualizer.Controllers
{
	[Produces("application/json")]
	[Route("[controller]")]
	[ApiController]
	public class RollController : ControllerBase
	{
		private readonly DataService context;

		public RollController(DataService context) => this.context = context;

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

			var poolIds = await context.GetPoolIds(dice.ToPool());

			return poolIds.HasValue ? new SearchRollViewModel(await context.ToSearchRoll(poolIds.Value))
				: (ActionResult<SearchRollViewModel>)NotFound();
		}
	}
}
