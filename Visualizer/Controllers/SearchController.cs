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
	[Route("api/[controller]")]
	public class SearchController : Controller
	{
		private ProbabilityContext context = new ProbabilityContext();
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

		[HttpGet("[action]")]
		public SearchViewModel GetTests()
		{
			var data = new SearchViewModel(context.PoolCombinations.Where(w => (w.PositivePoolId == 47 && w.NegativePoolId == 90)).Include(i => i.PoolCombinationStatistics).Include(i => i.PositivePool.PoolDice).Include(i => i.NegativePool.PoolDice).FirstOrDefault());

			return data;
		}

		private long? GetPoolId(List<PoolDie> searchForPool)
		{
			//fetch results for each type of die and
			var positiveTest = new List<int>();
			foreach (var die in searchForPool)
			{
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

		[HttpGet]
		public SearchViewModel Get(string data)
		{
			List<PoolDie> searchDice = null;

			try
			{
				searchDice = ((PoolDie[])JsonConvert.DeserializeObject(data, typeof(PoolDie[]))).ToList();
			}
			catch { }

			if (searchDice == null)
			{
				searchDice = new List<PoolDie>
				{
					new PoolDie() { DieId = (int)DieType.Ability, Quantity = 1 },
					new PoolDie() { DieId = (int)DieType.Difficulty, Quantity = 1 }
				};
			}

			//separate positive and negative dice
			var positiveId = GetPoolId(searchDice.Where(w => new List<int>() { (int)DieType.Ability, (int)DieType.Proficiency, (int)DieType.Boost }
			.Contains(w.DieId)).ToList());
			var negativeId = GetPoolId(searchDice.Where(w => new List<int>() { (int)DieType.Difficulty, (int)DieType.Challenge, (int)DieType.Setback }
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
