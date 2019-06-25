using DataFramework.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using Visualizer.Framework;
using Visualizer.Models;
using static DataFramework.Models.Die;

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
		public RollViewModel Get([FromBody]List<DieViewModel> dice)
		{
			if (dice == null)
			{
				return new RollViewModel();
			}

			//separate positive and negative dice
			var positiveId = Common.GetPoolId(context, dice.Where(w => new List<int> { GetDie(context, DieNames.Ability).DieId, GetDie(context, DieNames.Proficiency).DieId, GetDie(context, DieNames.Boost).DieId }
			.Contains(GetDie(context, Common.GetType(w.DieType)).DieId)).ToList());


			if ((positiveId ?? 0) > 0)
			{
				var result = new RollViewModel(context.Pools.Where(w => w.PoolId == positiveId.Value)
					.Include(i => i.PoolResults).ThenInclude(i => i.PoolResultSymbols).Include(i => i.PoolDice).FirstOrDefault());
				return result;
			}
			else
			{
				return new RollViewModel();
			}
		}
	}
}
