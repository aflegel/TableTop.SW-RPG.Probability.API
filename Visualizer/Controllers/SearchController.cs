using DataFramework.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using Visualizer.Models;
using Visualizer.Framework;
using static DataFramework.Models.Die;

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
			var positiveId = Common.GetPoolId(context, dice.Where(w => new List<int> { GetDie(context, DieNames.Ability).DieId , GetDie(context, DieNames.Proficiency).DieId, GetDie(context, DieNames.Boost).DieId }
			.Contains(GetDie(context, Common.GetType(w.DieType)).DieId)).ToList());
			var negativeId = Common.GetPoolId(context, dice.Where(w => new List<int> { GetDie(context, DieNames.Difficulty).DieId, GetDie(context, DieNames.Challenge).DieId, GetDie(context, DieNames.Setback).DieId }
			.Contains(GetDie(context, Common.GetType(w.DieType)).DieId)).ToList());


			if ((positiveId ?? 0) > 0 && (negativeId ?? 0) > 0)
			{
				var result = new SearchViewModel(context.PoolCombinations.Where(w => w.PositivePoolId == positiveId.Value && w.NegativePoolId == negativeId.Value)
					.Include(i => i.PoolCombinationStatistics).Include(i => i.PositivePool.PoolDice).Include(i => i.NegativePool.PoolDice).FirstOrDefault());
				return result;
			}
			else
			{
				return new SearchViewModel();
			}
		}
	}
}
