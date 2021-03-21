using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Probability.Service;
using Visualizer.Models;

namespace Visualizer.Controllers
{
	[Produces("application/json")]
	[Route("[controller]")]
	[ApiController]
	public class SearchController : ControllerBase
	{
		private DataService Context { get; }
		private ILogger<SearchController> Logger { get; }

		public SearchController(DataService context, ILogger<SearchController> logger)
		{
			Context = context;
			Logger = logger;
		}

		/// <summary>
		/// Returns the corresponding cached statistics for a given pool of dice
		/// </summary>
		/// <param name="dice"></param>
		/// <returns></returns>
		[HttpPost]
		public async Task<ActionResult<SearchViewModel>> Get([FromBody] List<DieViewModel> dice)
		{
			if (dice == null)
			{
				Logger.LogWarning("Empty request");
				return BadRequest();
			}
			var poolIds = await Context.GetPoolIds(dice.ToPool());

			if (!poolIds.HasValue)
			{
				Logger.LogWarning("No results found for request", dice);
				return NotFound();
			}

			return new SearchViewModel(await Context.ToSearchView(poolIds.Value));
		}
	}
}
