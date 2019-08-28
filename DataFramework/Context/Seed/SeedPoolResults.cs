using System.Collections.Generic;
using System.Linq;
using System.Collections.ObjectModel;
using DataFramework.Models;
using Microsoft.EntityFrameworkCore.Internal;
using System;

namespace DataFramework.Context.Seed
{
	public static class SeedPoolResults
	{
		/// <summary>
		/// Builds a set of unique outcomes for each pool of dice
		/// </summary>
		/// <returns></returns>
		public static Pool SeedPool(this Pool pool)
		{
			if (pool.PoolDice.Any())
			{
				SeedPoolStatistic.PrintStartLog(pool.Name, pool.TotalOutcomes);

				pool.PoolResults = pool.CopyPoolDice().RecursiveProcessing().ToList();
				pool.UniqueOutcomes = pool.PoolResults.Count;

				SeedPoolStatistic.PrintFinishLog(pool.UniqueOutcomes);
			}

			return pool;
		}

		private static IEnumerable<PoolResult> RecursiveProcessing(this IEnumerable<PoolDie> dice)
		{
			//if there are one or two dice left calculate their cross product and return the faces
			if (dice.SumQuantity() <= 2)
			{
				return dice.BinaryRecursiveCrossProduct();
			}

			//split the pool into two
			var split = dice.SplitPoolDice();

			//merge the two cross products
			return PoolCrossProduct(split.Item1.RecursiveProcessing(), split.Item2.RecursiveProcessing());
		}

		/// <summary>
		/// Sorts the different use cases and passes the dice to the cross product calculator
		/// </summary>
		/// <param name="dice"></param>
		/// <returns></returns>
		private static IEnumerable<PoolResult> BinaryRecursiveCrossProduct(this IEnumerable<PoolDie> dice) =>
				//if there is one element/quantity run a cross product against an empty set
				PoolCrossProduct(dice.First().Die.GetDiePool(), dice.SumQuantity() == 1 ? new Collection<PoolResult> { new PoolResult() } : dice.Last().Die.GetDiePool());

		/// <summary>
		/// Processes a cross product of two different dice
		/// </summary>
		/// <param name="topHalf"></param>
		/// <param name="bottomHalf"></param>
		/// <returns></returns>
		private static IEnumerable<PoolResult> PoolCrossProduct(IEnumerable<PoolResult> topHalf, IEnumerable<PoolResult> bottomHalf)
			=> topHalf.SelectMany(first => bottomHalf, (first, second) => new PoolResult()
			{
				PoolResultSymbols = first.PoolResultSymbols.MergePoolSymbols(second.PoolResultSymbols).ToList(),
				Frequency = first.Frequency * (second.Frequency != 0 ? second.Frequency : 1)
			}).GroupBy(g => g.GetHashCode()).Select(s => new PoolResult()
			{
				PoolResultSymbols = s.First().PoolResultSymbols,
				Frequency = s.Sum(sum => sum.Frequency)
			});

		/// <summary>
		/// Merges two symbol pools for a single combined and reduced pool
		/// </summary>
		/// <param name="topHalf"></param>
		/// <param name="bottomHalf"></param>
		/// <returns></returns>
		private static IEnumerable<PoolResultSymbol> MergePoolSymbols(this IEnumerable<PoolResultSymbol> topHalf, IEnumerable<PoolResultSymbol> bottomHalf)
			=> topHalf.Concat(bottomHalf).GroupBy(g => g.Symbol).Select(s => new PoolResultSymbol(s.Key, s.Sum(sum => sum.Quantity)));

		/// <summary>
		/// Splits a pool of dice into two halves.  Remainder is in the bottom half.
		/// </summary>
		/// <param name="dice"></param>
		/// <returns></returns>
		private static Tuple<List<PoolDie>, List<PoolDie>> SplitPoolDice(this IEnumerable<PoolDie> dice)
		{
			var indexDice = new Tuple<List<PoolDie>, List<PoolDie>>(new List<PoolDie>(), dice.ToList());

			//pop the top half of dice
			var target = indexDice.Item2.SumQuantity() / 2;

			while (indexDice.Item1.SumQuantity() < target)
			{
				var die = indexDice.Item2.First();
				//the die quantity is too large, copy the die and reduce it's quantity by target
				if (die.Quantity > target)
				{
					var take = target - indexDice.Item1.SumQuantity();
					indexDice.Item1.Add(new PoolDie(die.Die, take));
					die.Quantity -= take;
				}
				else
				{
					//pop the die off and add it to the new list
					indexDice.Item1.Add(new PoolDie(die.Die, die.Quantity));
					indexDice.Item2.Remove(die);
				}
			}

			return indexDice;
		}
	}
}
