using DataFramework.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using Visualizer.Models;
using DataFramework.Models;

namespace Visualizer.Controllers
{
	[Produces("application/json")]
	[Route("[controller]")]
	public class SearchController : Controller
	{
		private readonly ProbabilityContext context = new ProbabilityContext();

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

			var combinedPool = dice.ToPool(context);

			return (combinedPool.Item1 != null && combinedPool.Item2 != null)
				? new SearchViewModel(GetPoolCombination(combinedPool.Item1.PoolId, combinedPool.Item2.PoolId))
				: new SearchViewModel();
		}

		private PoolCombination GetPoolCombination(int positiveId, int negativeId) => context.PoolCombinations.Where(w => w.PositivePoolId == positiveId && w.NegativePoolId == negativeId)
			.Include(i => i.PoolCombinationStatistics)
			.Include(i => i.PositivePool.PoolDice)
			.Include(i => i.NegativePool.PoolDice)
			.FirstOrDefault();
	}
}
