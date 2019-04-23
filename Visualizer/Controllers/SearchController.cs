using DataFramework.Context;
using DataFramework.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using Visualizer.Models;

namespace Visualizer.Controllers
{
	[Produces("application/json")]
	[Route("[controller]")]
	public class SearchController : Controller
	{
		private readonly ProbabilityContext context = new ProbabilityContext();

		private enum DieType
		{
			Ability = 1,
			Boost = 2,
			Challenge = 3,
			Difficulty = 4,
			Force = 5,
			Proficiency = 6,
			Setback = 7,
		}

		/// <summary>
		/// Returns the id for the pool with the matching dice
		/// </summary>
		/// <param name="searchForPool"></param>
		/// <returns></returns>
		private long? GetPoolId(List<PoolDie> searchForPool)
		{
			//fetch results for each type of die and
			var positiveTest = new List<int>();
			foreach (var die in searchForPool)
			{
				//todo: This function will return an incorrect result if the number of dice match but the desired result does not
				var dieSearch = context.PoolDice.Where(w => w.DieId == die.DieId && w.Quantity == die.Quantity && w.Pool.PoolDice.Count == searchForPool.Count).Select(s => s.PoolId).ToList();
				if (positiveTest.Count == 0)
				{
					positiveTest = dieSearch;
				}
				else
				{
					positiveTest = positiveTest.Intersect(dieSearch).ToList();
				}
			}

			return positiveTest.FirstOrDefault();
		}

		/// <summary>
		/// Returns the corresponding cached statistics for a given pool of dice
		/// </summary>
		/// <param name="dice"></param>
		/// <returns></returns>
		[HttpPost]
		public SearchViewModel Get([FromBody]List<PoolDie> dice)
		{
			if (dice == null)
			{
				return new SearchViewModel();
			}

			//separate positive and negative dice
			var positiveId = GetPoolId(dice.Where(w => new List<int>() { (int)DieType.Ability, (int)DieType.Proficiency, (int)DieType.Boost }
			.Contains(w.DieId)).ToList());
			var negativeId = GetPoolId(dice.Where(w => new List<int>() { (int)DieType.Difficulty, (int)DieType.Challenge, (int)DieType.Setback }
			.Contains(w.DieId)).ToList());


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
