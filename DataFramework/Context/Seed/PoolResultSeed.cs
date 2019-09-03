using System.Collections.Generic;
using System.Linq;
using DataFramework.Models;
using Microsoft.EntityFrameworkCore.Internal;

namespace DataFramework.Context.Seed
{
	public static class PoolResultSeed
	{
		/// <summary>
		/// Prints start and stop timestamps while building the complete outcome map for a set of dice
		/// </summary>
		/// <param name="context"></param>
		public static (IEnumerable<Pool>, IEnumerable<Pool>) ProcessPools(this IEnumerable<Die> dice, (IEnumerable<int>, IEnumerable<int>, IEnumerable<int>) positiveRange, (IEnumerable<int>, IEnumerable<int>, IEnumerable<int>) negativeRange)
			=> (dice.BuildPositivePool(positiveRange),
				dice.BuildNegativePool(negativeRange));

		/// <summary>
		/// Takes the cross product of all three ranges to build the dice pool
		/// </summary>
		/// <param name="dice"></param>
		/// <returns></returns>
		public static IEnumerable<Pool> BuildPositivePool(this IEnumerable<Die> dice, (IEnumerable<int> ability, IEnumerable<int> proficiency, IEnumerable<int> boost) ranges) =>
			ranges.ability.SelectMany(ability => ranges.proficiency.Where(upgrade => upgrade <= ability), (ability, upgrade) => (ability, upgrade))
				.SelectMany(tuple => ranges.boost, (tuple, boost) =>
				dice.SeedPool(ability: tuple.ability - tuple.upgrade, proficiency: tuple.upgrade, boost: boost).SeedPoolResults());

		/// <summary>
		/// Takes the cross product of all three ranges to build the dice pool
		/// </summary>
		/// <param name="context"></param>
		/// <returns></returns>
		public static IEnumerable<Pool> BuildNegativePool(this IEnumerable<Die> dice, (IEnumerable<int> difficulty, IEnumerable<int> challenge, IEnumerable<int> setback) ranges) =>
			ranges.difficulty.SelectMany(difficulty => ranges.challenge.Where(challenge => challenge <= difficulty), (difficulty, challenge) => (difficulty, challenge))
				.SelectMany(tuple => ranges.setback, (tuple, setback) =>
				dice.SeedPool(difficulty: tuple.difficulty - tuple.challenge, challenge: tuple.challenge, setback: setback).SeedPoolResults());

		public static Pool SeedPool(this IEnumerable<Die> dice, int ability = 0, int proficiency = 0, int difficulty = 0, int challenge = 0, int boost = 0, int setback = 0)
		{
			var poolDice = new List<PoolDie>
			{
				new PoolDie(dice.GetDie(DieNames.Ability), ability),
				new PoolDie(dice.GetDie(DieNames.Boost), boost),
				new PoolDie(dice.GetDie(DieNames.Challenge), challenge),
				new PoolDie(dice.GetDie(DieNames.Difficulty), difficulty),
				new PoolDie(dice.GetDie(DieNames.Proficiency), proficiency),
				new PoolDie(dice.GetDie(DieNames.Setback), setback)
			};

			var pool = new Pool()
			{
				PoolDice = poolDice.Where(w => w.Quantity > 0).ToList(),
			};

			pool.Name = pool.ToString();
			pool.TotalOutcomes = pool.RollEstimation();

			return pool;
		}

		/// <summary>
		/// Builds a set of unique outcomes for each pool of dice
		/// </summary>
		/// <returns></returns>
		public static Pool SeedPoolResults(this Pool pool)
		{
			if (pool.PoolDice.Any())
			{
				PoolStatisticSeed.PrintStartLog(pool.Name, pool.TotalOutcomes);

				//clone the pool to avoid contamination. Exploding makes the recursion easier
				pool.PoolResults = pool.CopyPoolDice().ExplodeDice().RecursiveProcessing().ToList();
				pool.UniqueOutcomes = pool.PoolResults.Count;

				PoolStatisticSeed.PrintFinishLog(pool.UniqueOutcomes);
			}

			return pool;
		}

		/// <summary>
		/// Uses binary recursion to create the cross products
		/// </summary>
		/// <param name="dice"></param>
		/// <returns></returns>
		private static IEnumerable<PoolResult> RecursiveProcessing(this IEnumerable<PoolDie> dice) =>
			//if there are one or two dice left their results are their faces
			(dice.Count() <= 2 ? dice.ToTuple() : dice.Split().RecurseTuple())
			//merge the two cross products
			.PoolCrossProduct();

		/// <summary>
		/// Runs recursion on the two halves of the die pool
		/// </summary>
		/// <param name="splitPools"></param>
		/// <returns></returns>
		private static (IEnumerable<PoolResult>, IEnumerable<PoolResult>) RecurseTuple(this (IEnumerable<PoolDie>, IEnumerable<PoolDie>) splitPools)
			=> (splitPools.Item1.RecursiveProcessing(), splitPools.Item2.RecursiveProcessing());

		/// <summary>
		/// Creates a tuple from the one or two remaining dice
		/// </summary>
		/// <param name="dice"></param>
		/// <returns></returns>
		private static (IEnumerable<PoolResult>, IEnumerable<PoolResult>) ToTuple(this IEnumerable<PoolDie> dice)
			//if there is one element/quantity run a cross product against an empty set
			=> (dice.First().Die.ToPool(), dice.Count() == 1 ? new List<PoolResult> { new PoolResult() } : dice.Last().Die.ToPool());

		/// <summary>
		/// Processes a cross product of two different dice
		/// </summary>
		/// <param name="firstHalf"></param>
		/// <param name="secondHalf"></param>
		/// <returns></returns>
		private static IEnumerable<PoolResult> PoolCrossProduct(this (IEnumerable<PoolResult> firstHalf, IEnumerable<PoolResult> secondHalf) splitPools)
			//run a full cross product
			=> splitPools.firstHalf.SelectMany(first => splitPools.secondHalf, (first, second) => new PoolResult
			{
				PoolResultSymbols = (first.PoolResultSymbols, second.PoolResultSymbols).MergePoolSymbols().ToList(),
				Frequency = first.Frequency * (second.Frequency != 0 ? second.Frequency : 1)
			})
			// merge all identical results
			.GroupBy(g => g.GetHashCode()).Select(s => new PoolResult
			{
				PoolResultSymbols = s.First().PoolResultSymbols,
				Frequency = s.Sum(sum => sum.Frequency)
			});

		/// <summary>
		/// Merges two symbol pools for a single combined and reduced pool
		/// </summary>
		/// <param name="symbols"></param>
		/// <returns></returns>
		private static IEnumerable<PoolResultSymbol> MergePoolSymbols(this (IEnumerable<PoolResultSymbol> firstHalf, IEnumerable<PoolResultSymbol> secondHalf) symbols)
			=> symbols.firstHalf.Concat(symbols.secondHalf).GroupBy(g => g.Symbol).Select(s => new PoolResultSymbol(s.Key, s.Sum(sum => sum.Quantity)));

		/// <summary>
		/// Splits a pool of dice into two halves.  Remainder is in the bottom half.
		/// </summary>
		/// <param name="dice"></param>
		/// <returns></returns>
		private static (IEnumerable<PoolDie>, IEnumerable<PoolDie>) Split(this IEnumerable<PoolDie> dice)
			=> (dice.Take(dice.Count() / 2), dice.Skip(dice.Count() / 2));

		/// <summary>
		/// Explodes the items into individual 1 quantity pools
		/// </summary>
		/// <param name="dice"></param>
		/// <returns></returns>
		private static IEnumerable<PoolDie> ExplodeDice(this IEnumerable<PoolDie> dice)
			=> dice.SelectMany(e => Enumerable.Range(0, e.Quantity).Select(f => new PoolDie { Die = e.Die, Quantity = 1 }));
	}
}
