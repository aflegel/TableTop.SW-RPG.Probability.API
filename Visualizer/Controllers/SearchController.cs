using DataFramework.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using Visualizer.Models;
using Visualizer.Framework;
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

			//separate positive and negative dice
			var positiveId = Common.GetPositivePoolId(context, dice);
			var negativeId = Common.GetNegativePoolId(context, dice);


			if ((positiveId ?? 0) > 0 && (negativeId ?? 0) > 0)
			{
				var result = new SearchViewModel(GetPoolCombination(positiveId.Value, negativeId.Value));
				return result;
			}
			else
			{
				return new SearchViewModel();
			}
		}

		private PoolCombination GetPoolCombination(int positiveId, int negativeId)
		{
			return context.PoolCombinations.Where(w => w.PositivePoolId == positiveId && w.NegativePoolId == negativeId)
					.Include(i => i.PoolCombinationStatistics).Include(i => i.PositivePool.PoolDice).Include(i => i.NegativePool.PoolDice).FirstOrDefault();
		}
	}
}
