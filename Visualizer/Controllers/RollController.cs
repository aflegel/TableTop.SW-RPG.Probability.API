using DataFramework.Context;
using DataFramework.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
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
		public SearchRollViewModel Get([FromBody]List<DieViewModel> dice)
		{
			if (dice == null)
			{
				return new SearchRollViewModel();
			}

			var combinedPool = dice.ToPool(context);

			return (combinedPool.Item1 != null && combinedPool.Item2 != null)
				? new SearchRollViewModel(GetPool(combinedPool.Item1.PoolId), GetPool(combinedPool.Item2.PoolId))
				: new SearchRollViewModel();
		}

		private Pool GetPool(long poolId) => context.Pools.Where(w => w.PoolId == poolId)
			.Include(i => i.PoolResults)
				.ThenInclude(i => i.PoolResultSymbols)
			.Include(i => i.PoolDice)
			.FirstOrDefault();
	}
}
