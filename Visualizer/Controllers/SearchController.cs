using DataFramework.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using Visualizer.Models;
using static DataFramework.Models.Die;

namespace Visualizer.Controllers
{
	[Produces("application/json")]
	[Route("[controller]")]
	public class SearchController : Controller
	{
		private readonly ProbabilityContext context = new ProbabilityContext();

		/// <summary>
		/// Returns the id for the pool with the matching dice
		/// </summary>
		/// <param name="searchForPool"></param>
		/// <returns></returns>
		private long? GetPoolId(List<DieViewModel> searchForPool)
		{
			//fetch results for each type of die and
			var resultList = new List<int>();
			var initialized = false;
			foreach (var die in searchForPool)
			{
				var dieSearch = context.PoolDice.Where(w => w.DieId == GetDie(context,GetType(die.DieType)).DieId && w.Quantity == die.Quantity && w.Pool.PoolDice.Count == searchForPool.Count).Select(s => s.PoolId).ToList();
				resultList = !initialized ? dieSearch : resultList.Intersect(dieSearch).ToList();

				//increase the count and ensure the count is greater than 0 so an empty result will not be skipped
				initialized = true;
			}

			return resultList.FirstOrDefault();
		}

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
			var positiveId = GetPoolId(dice.Where(w => new List<int>() { GetDie(context, DieNames.Ability).DieId , GetDie(context, DieNames.Proficiency).DieId, GetDie(context, DieNames.Boost).DieId }
			.Contains(GetDie(context, GetType(w.DieType)).DieId)).ToList());
			var negativeId = GetPoolId(dice.Where(w => new List<int>() { GetDie(context, DieNames.Difficulty).DieId, GetDie(context, DieNames.Challenge).DieId, GetDie(context, DieNames.Setback).DieId }
			.Contains(GetDie(context, GetType(w.DieType)).DieId)).ToList());


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

		private DieNames GetType(string input)
		{
			Enum.TryParse(input, true, out DieNames dieType);

			return dieType;
		}
	}
}
