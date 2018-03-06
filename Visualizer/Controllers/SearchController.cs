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
		public ProbabilityBreakdown GetTests()
		{
			//context.PoolDice.Where(w => w.DieId == 0 && w.Quantity == 0);
			var data = new ProbabilityBreakdown()
			{
				//Baseline = context.PoolCombinations.Where(w => (w.PositivePoolId == 72 && w.NegativePoolId == 144)).Include(i => i.PoolCombinationStatistics).FirstOrDefault(),
				//Baseline = context.PoolCombinations.Where(w => (w.PositivePoolId == 13 && w.NegativePoolId == 85)).Include(i => i.PoolCombinationStatistics).FirstOrDefault(),
				Baseline = context.PoolCombinations.Where(w => (w.PositivePoolId == 47 && w.NegativePoolId == 90)).Include(i => i.PoolCombinationStatistics).Include(i => i.PositivePool.PoolDice).Include(i => i.NegativePool.PoolDice).FirstOrDefault(),
			};

			data.BaseDice = data.Baseline.PositivePool.PoolDice.Union(data.Baseline.NegativePool.PoolDice);

			return data;
		}

		[HttpGet("[action]")]
		public ProbabilityBreakdown GetStatistics(List<PoolDie> dice)
		{
			if(dice.Count == 0)
			{
				dice.Add(new PoolDie() { DieId = (int)DieType.Ability, Quantity = 1 });
				dice.Add(new PoolDie() { DieId = (int)DieType.Difficulty, Quantity = 1 });
			}

			var tester = new List<int>() { (int)DieType.Ability, (int)DieType.Proficiency, (int)DieType.Boost };
			var tester2 = new List<int>() { (int)DieType.Difficulty, (int)DieType.Challenge, (int)DieType.Setback };

			//separate positive and negative dice
			var positivePool = dice.Where(w => tester.Contains(w.DieId));
			var negativePool = dice.Where(w => tester2.Contains(w.DieId));

			var positiveTest = context.Pools.Where(w => !w.PoolDice.Except(positivePool).Any()).ToList();
			var negativeTest = context.Pools.Where(w => !w.PoolDice.Except(negativePool).Any()).ToList();
			//				if (listA.Except(listB).Any())

			var result = new ProbabilityBreakdown();

			if(positiveTest.Count > 0 && negativeTest.Count > 0)
			{

				result.Baseline =  context.PoolCombinations.Where(w => w.PositivePoolId == positiveTest.FirstOrDefault().PoolId && w.NegativePoolId == negativeTest.FirstOrDefault().PoolId).Include(i => i.PoolCombinationStatistics).Include(i => i.PositivePool.PoolDice).Include(i => i.NegativePool.PoolDice).FirstOrDefault();
				result.BaseDice = result.Baseline.PositivePool.PoolDice.Union(result.Baseline.NegativePool.PoolDice);
			}
			else
			{
				result = GetTests();
			}

			return result;
		}
	}
}