using System;
using System.Collections.Generic;
using System.Linq;
using DataFramework.Context;
using DataFramework.Models;
using Microsoft.EntityFrameworkCore;
using Visualizer.Models;
using static DataFramework.Models.Die;

namespace Visualizer.Framework
{
	public static class Common
	{
		/// <summary>
		/// Returns the id for the pool with the matching dice
		/// </summary>
		/// <param name="searchForPool"></param>
		/// <returns></returns>
		public static long? GetPoolId(ProbabilityContext context, List<DieViewModel> searchForPool)
		{
			//fetch results for each type of die and
			var resultList = new List<int>();
			var initialized = false;
			foreach (var die in searchForPool)
			{
				var dieSearch = context.PoolDice.Where(w => w.DieId == GetDie(context, GetType(die.DieType)).DieId && w.Quantity == die.Quantity && w.Pool.PoolDice.Count == searchForPool.Count).Select(s => s.PoolId).ToList();
				resultList = !initialized ? dieSearch : resultList.Intersect(dieSearch).ToList();

				//increase the count and ensure the count is greater than 0 so an empty result will not be skipped
				initialized = true;
			}

			return resultList.FirstOrDefault();
		}

		public static DieNames GetType(string input)
		{
			Enum.TryParse(input, true, out DieNames dieType);

			return dieType;
		}
	}
}
