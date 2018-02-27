using DataFramework.Context;
using DataFramework.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace Visualizer.Controllers
{
	[Produces("application/json")]
    [Route("api/[controller]")]
    public class SearchController : Controller
    {
		ProbabilityContext context = new ProbabilityContext();
		public IEnumerable<PoolCombination> GetStatistics(/*List<PoolDie> dice*/)
		{
			//separate positive and negative dice

			//context.PoolDice.Where(w => w.DieId == 0 && w.Quantity == 0);

			var data = context.PoolCombinations.Where(w => w.PositivePoolId == 1 && w.NegativePoolId == 71).Include(i => i.PoolCombinationStatistics).ToList();

			return data;
		}

		public IEnumerable<PoolCombination> GetStatistics(int number, int numbe)
		{
			//separate positive and negative dice

			//context.PoolDice.Where(w => w.DieId == 0 && w.Quantity == 0);

			var data = context.PoolCombinations.Where(w => w.PositivePoolId == 1 && w.NegativePoolId == 71).Include(i => i.PoolCombinationStatistics).ToList();

			return data;
		}
	}
}