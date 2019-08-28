﻿using System.Collections.Generic;
using System.Linq;
using System.Collections.ObjectModel;
using DataFramework.Models;
using Microsoft.EntityFrameworkCore.Internal;

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

				pool.PoolResults = pool.CopyPoolDice().ExplodeDice().RecursiveProcessing().ToList();
				pool.UniqueOutcomes = pool.PoolResults.Count;

				SeedPoolStatistic.PrintFinishLog(pool.UniqueOutcomes);
			}

			return pool;
		}

		/// <summary>
		/// Uses binary recursion to create the cross products
		/// </summary>
		/// <param name="dice"></param>
		/// <returns></returns>
		private static IEnumerable<PoolResult> RecursiveProcessing(this IEnumerable<PoolDie> dice)
		{
			//if there are one or two dice left calculate their cross product and return the faces
			if (dice.SumQuantity() <= 2)
			{
				//if there is one element/quantity run a cross product against an empty set
				return PoolCrossProduct(dice.First().Die.GetDiePool(), dice.SumQuantity() == 1 ? new Collection<PoolResult> { new PoolResult() } : dice.Last().Die.GetDiePool());
			}

			//split the pool into two
			var split = dice.SplitPoolDice();

			//merge the two cross products
			return PoolCrossProduct(split.First().RecursiveProcessing(), split.Last().RecursiveProcessing());
		}

		/// <summary>
		/// Processes a cross product of two different dice
		/// </summary>
		/// <param name="topHalf"></param>
		/// <param name="bottomHalf"></param>
		/// <returns></returns>
		private static IEnumerable<PoolResult> PoolCrossProduct(IEnumerable<PoolResult> topHalf, IEnumerable<PoolResult> bottomHalf)
			//run a full cross product
			=> topHalf.SelectMany(first => bottomHalf, (first, second) => new PoolResult
			{
				PoolResultSymbols = first.PoolResultSymbols.MergePoolSymbols(second.PoolResultSymbols).ToList(),
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
		private static IEnumerable<IEnumerable<PoolDie>> SplitPoolDice(this IEnumerable<PoolDie> dice)
			=> new List<IEnumerable<PoolDie>> { dice.Take(dice.Count() / 2), dice.Skip(dice.Count() / 2) };

		/// <summary>
		/// Explodes the items into individual 1 quantity pools
		/// </summary>
		/// <param name="dice"></param>
		/// <returns></returns>
		private static IEnumerable<PoolDie> ExplodeDice(this IEnumerable<PoolDie> dice)
			=> dice.SelectMany(e => Enumerable.Range(0, e.Quantity).Select(f => new PoolDie { Die = e.Die, Quantity = 1 }));
	}
}
