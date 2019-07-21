using DataFramework.Context;
using DataFramework.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using Visualizer.Framework;
using Visualizer.Models;

namespace Visualizer.Controllers
{
	[Produces("application/json")]
	[Route("[controller]")]
	public class RollController : Controller
	{
		private readonly ProbabilityContext context = new ProbabilityContext();

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

			//separate positive and negative dice
			var positiveId = Common.GetPositivePoolId(context, dice);
			var negativeId = Common.GetNegativePoolId(context, dice);


			if ((positiveId ?? 0) > 0 && (negativeId ?? 0) > 0)
			{
				var result = new SearchRollViewModel(GetPool(positiveId.Value), GetPool(negativeId.Value));
				return result;
			}
			else
			{
				return new SearchRollViewModel();
			}
		}

		private Pool GetPool(long poolId)
		{
			return context.Pools.Where(w => w.PoolId == poolId)
					.Include(i => i.PoolResults).ThenInclude(i => i.PoolResultSymbols).Include(i => i.PoolDice).FirstOrDefault();
		}
	}
}
