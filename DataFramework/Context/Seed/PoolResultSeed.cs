using System.Collections.Generic;
using System.Linq;
using DataFramework.Models;

namespace DataFramework.Context.Seed
{
	public static class PoolResultSeed
	{
		/// <summary>
		/// Prints start and stop timestamps while building the complete outcome map for a set of dice
		/// </summary>
		/// <param name="context"></param>
		public static (IEnumerable<Pool>, IEnumerable<Pool>) SeedPools(this IEnumerable<Die> dice, (IEnumerable<int>, IEnumerable<int>, IEnumerable<int>) positiveRange, (IEnumerable<int>, IEnumerable<int>, IEnumerable<int>) negativeRange)
			=> (positiveRange.ToTuple().Select(s => dice.SeedPool(ability: s.Item1 - s.Item2, proficiency: s.Item2, boost: s.Item3)),
			//The second list needs to be enumerated here or it will be enumerated multiple times during the cross product
			negativeRange.ToTuple().Select(s => dice.SeedPool(difficulty: s.Item1 - s.Item2, challenge: s.Item2, setback: s.Item3)).ToList());

		/// <summary>
		/// Transforms the set of ranges into a list of the range outcomes
		/// </summary>
		/// <param name="ranges"></param>
		/// <returns></returns>
		private static IEnumerable<(int, int, int)> ToTuple(this (IEnumerable<int>, IEnumerable<int>, IEnumerable<int>) ranges) =>
			//Filter out any records where the "upgraded" dice outnumber the basic dice
			ranges.Item1.SelectMany(basic => ranges.Item2.Where(upgrade => upgrade <= basic), (basic, upgrade) => (basic, upgrade))
			//Filter out any records where there are no basic or upgraded dice
				.SelectMany(tuple => ranges.Item3, (tuple, bonus) => (tuple.basic, tuple.upgrade, bonus)).Where(w => w.basic + w.upgrade > 0);

		private static Pool SeedPool(this IEnumerable<Die> dice, int ability = 0, int proficiency = 0, int difficulty = 0, int challenge = 0, int boost = 0, int setback = 0)
		{
			var pooldice = new List<PoolDie>
			{
				new PoolDie(dice.GetDie(DieNames.Ability), ability),
				new PoolDie(dice.GetDie(DieNames.Boost), boost),
				new PoolDie(dice.GetDie(DieNames.Challenge), challenge),
				new PoolDie(dice.GetDie(DieNames.Difficulty), difficulty),
				new PoolDie(dice.GetDie(DieNames.Proficiency), proficiency),
				new PoolDie(dice.GetDie(DieNames.Setback), setback)
			}.Where(w => w.Quantity > 0);

			var pool = new Pool()
			{
				PoolDice = pooldice.ToList(),
				PoolResults = pooldice.ExplodeDice().RecursiveProcessing().ToList()
			};

			pool.Name = pool.ToString();
			pool.TotalOutcomes = pool.RollEstimation();
			pool.UniqueOutcomes = pool.PoolResults.Count;

			ConsoleLogger.LogRoll(pool.Name, pool.TotalOutcomes, pool.UniqueOutcomes);

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
