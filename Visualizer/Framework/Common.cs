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
		public static int? GetPoolId(ProbabilityContext context, List<DieViewModel> searchForPool) =>
			searchForPool.Select(die => context.PoolDice.Where(w => w.DieId == context.GetDie(die.DieType).DieId && w.Quantity == die.Quantity && w.Pool.PoolDice.Count == searchForPool.Count)
			.Select(s => s.PoolId)).Aggregate((result, next) => result.Intersect(next)).FirstOrDefault();

		public static int? GetPositivePoolId(this ProbabilityContext context, List<DieViewModel> dice) => GetPoolId(context, FilterDice(context, dice, PositiveDice));

		public static int? GetNegativePoolId(this ProbabilityContext context, List<DieViewModel> dice) => GetPoolId(context, FilterDice(context, dice, NegativeDice));

		private static List<int> GetDiePoolIds(this ProbabilityContext context, List<DieNames> dice) => dice.Select(die => context.GetDie(die).DieId).ToList();

		/// <summary>
		/// Removes either the positive or negative dice from the full pool to find the pool half
		/// </summary>
		/// <param name="context"></param>
		/// <param name="dice"></param>
		/// <param name="filters"></param>
		/// <returns></returns>
		private static List<DieViewModel> FilterDice(ProbabilityContext context, List<DieViewModel> dice, List<DieNames> filters)
			=> dice.Where(w => context.GetDiePoolIds(filters)
				.Contains(context.GetDie(w.DieType).DieId)).ToList();
	}
}
