using System;
using System.Collections.Generic;
using System.Linq;
using DataFramework.Models;
using Microsoft.EntityFrameworkCore;
using static DataFramework.Models.Die;

namespace DataFramework.Context.Seed
{
	public static class PoolSeed
	{

		/// <summary>
		///
		/// </summary>
		/// <param name="context"></param>
		/// <returns></returns>
		public static IEnumerable<Pool> BuildPositivePool(this ProbabilityContext context, IEnumerable<int> abilityRange, IEnumerable<int> proficiencyRange, IEnumerable<int> boostRange) =>
			abilityRange.SelectMany(ability => proficiencyRange.Where(upgrade => upgrade <= ability), (ability, upgrade) => new Tuple<int, int>(ability, upgrade))
				.SelectMany(tuple => boostRange, (tuple, boost) =>
				context.SeedPool(ability: tuple.Item1 - tuple.Item2, proficiency: tuple.Item2, boost: boost).SeedPoolResults());

		/// <summary>
		///
		/// </summary>
		/// <param name="context"></param>
		/// <returns></returns>
		public static IEnumerable<Pool> BuildNegativePool(this ProbabilityContext context, IEnumerable<int> difficultyRange, IEnumerable<int> challengeRange, IEnumerable<int> setbackRange) =>
			difficultyRange.SelectMany(difficulty => challengeRange.Where(challenge => challenge <= difficulty), (difficulty, challenge) => new Tuple<int, int>(difficulty, challenge))
				.SelectMany(tuple => setbackRange, (tuple, setback) =>
				context.SeedPool(difficulty: tuple.Item1 - tuple.Item2, challenge: tuple.Item2, setback: setback).SeedPoolResults());

		public static IEnumerable<Pool> GetPositivePools(this ProbabilityContext context) => context.Pools.Where(pool => pool.PoolDice.Any(die => PositiveDice.Contains(die.Die.Name.GetName())))
				.Include(i => i.PositivePoolCombinations)
						.ThenInclude(tti => tti.PoolCombinationStatistics)
				.Include(i => i.PoolResults)
						.ThenInclude(tti => tti.PoolResultSymbols);

		public static IEnumerable<Pool> GetNegativePools(this ProbabilityContext context) => context.Pools.Where(pool => pool.PoolDice.Any(die => NegativeDice.Contains(die.Die.Name.GetName())))
				.Include(i => i.NegativePoolCombinations)
					.ThenInclude(tti => tti.PoolCombinationStatistics)
				.Include(i => i.PoolResults)
					.ThenInclude(tti => tti.PoolResultSymbols);
	}
}
