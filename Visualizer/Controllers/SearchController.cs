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
		ProbabilityContext context = new ProbabilityContext();
		[HttpGet("[action]")]
		public ProbabilityBreakdown GetStatistics(/*List<PoolDie> dice*/)
		{
			//separate positive and negative dice

			//context.PoolDice.Where(w => w.DieId == 0 && w.Quantity == 0);
			var data = new ProbabilityBreakdown()
			{
				Baseline = context.PoolCombinations.Where(w => (w.PositivePoolId == 13 && w.NegativePoolId == 85)).Include(i => i.PoolCombinationStatistics).FirstOrDefault(),
				Boosted = null,//context.PoolCombinations.Where(w => (w.PositivePoolId == 2 && w.NegativePoolId == 73)).Include(i => i.PoolCombinationStatistics).FirstOrDefault(),
				Upgraded = null,//context.PoolCombinations.Where(w => (w.PositivePoolId == 4 && w.NegativePoolId == 73)).Include(i => i.PoolCombinationStatistics).FirstOrDefault(),
				Setback = null,
				Threatened = null
			};

			return data;
		}

		public ProbabilityBreakdown GetStatistics(int number, int numbe)
		{
			//separate positive and negative dice

			//context.PoolDice.Where(w => w.DieId == 0 && w.Quantity == 0);

			var data = GetStatistics();

			return data;
		}
	}
}