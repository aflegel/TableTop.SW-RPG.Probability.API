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
		private static readonly List<DieNames> NegativeDice = new List<DieNames> {
				DieNames.Challenge,
				DieNames.Difficulty,
				DieNames.Setback
		};

		private static readonly List<DieNames> PositiveDice = new List<DieNames> {
				DieNames.Ability,
				DieNames.Proficiency,
				DieNames.Boost
		};

		/// <summary>
		/// Returns the id for the pool with the matching dice
		/// </summary>
		/// <param name="searchForPool"></param>
		/// <returns></returns>
		public static int? GetPoolId(ProbabilityContext context, List<DieViewModel> searchForPool)
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

		public static int? GetPositivePoolId(ProbabilityContext context, List<DieViewModel> dice)
		{
			return GetPoolId(context, FilterDice(context, dice, PositiveDice));
		}

		public static int? GetNegativePoolId(ProbabilityContext context, List<DieViewModel> dice)
		{
			return GetPoolId(context, FilterDice(context, dice, NegativeDice));
		}

		/// <summary>
		/// Removes either the positive or negative dice from the full pool to find the pool half
		/// </summary>
		/// <param name="context"></param>
		/// <param name="dice"></param>
		/// <param name="filters"></param>
		/// <returns></returns>
		private static List<DieViewModel> FilterDice(ProbabilityContext context, List<DieViewModel> dice, List<DieNames> filters)
		{
			return dice.Where(w => GetDiePool(context, filters)
				.Contains(GetDie(context, GetType(w.DieType)).DieId)).ToList();
		}

		public static DieNames GetType(string input)
		{
			Enum.TryParse(input, true, out DieNames dieType);

			return dieType;
		}

		private static List<int> GetDiePool(ProbabilityContext context, List<DieNames> dice)
		{
			var result = new List<int>();

			foreach(var die in dice)
			{
				result.Add(GetDie(context, die).DieId);
			}

			return result;
		}
	}
}
