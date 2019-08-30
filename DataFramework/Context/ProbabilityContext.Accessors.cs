using System;
using System.Linq;
using DataFramework.Models;
using Microsoft.EntityFrameworkCore;
using static DataFramework.Models.DieExtensions;

namespace DataFramework.Context
{
	public static class ProbabilityContextAccessors
	{
		/// <summary>
		/// Returns a Die with it's faces and face symbols
		/// </summary>
		/// <param name="context"></param>
		/// <param name="die"></param>
		/// <returns></returns>
		public static Die GetDie(this ProbabilityContext context, DieNames die) => context.GetDie(die.ToString());

		public static Die GetDie(this ProbabilityContext context, string die) => context.Dice.Where(w => w.Name == die.ToString())
			.Include(i => i.DieFaces)
				.ThenInclude(t => t.DieFaceSymbols)
			.FirstOrDefault();

		public static Pool GetPoolByName(this ProbabilityContext context, string poolName) => context.Pools.FirstOrDefault(w => w.Name == poolName);

		public static Pool GetPool(this ProbabilityContext context, long poolId) => context.Pools.Where(w => w.PoolId == poolId)
			.Include(i => i.PoolResults)
				.ThenInclude(i => i.PoolResultSymbols)
			.Include(i => i.PoolDice)
			.FirstOrDefault();

		public static Tuple<Pool, Pool> SplitPoolByDice(this ProbabilityContext context, Pool pool)
			=> new Tuple<Pool, Pool>(context.GetPoolByName(pool.FilterDice(PositiveDice).ToString()), context.GetPoolByName(pool.FilterDice(NegativeDice).ToString()));

		public static PoolCombination GetPoolCombination(this ProbabilityContext context, Pool positivePool, Pool negativePool) => context.PoolCombinations.Where(w => w.PositivePool == positivePool && w.NegativePool == negativePool)
			.Include(i => i.PoolCombinationStatistics)
			.Include(i => i.PositivePool.PoolDice)
			.Include(i => i.NegativePool.PoolDice)
			.FirstOrDefault();
	}
}
