using System.Collections.Generic;
using System.Linq;
using System.Collections.ObjectModel;
using DataFramework.Models;
using Microsoft.EntityFrameworkCore.Internal;

namespace DataGenerator.Models
{
	public static class OutcomeGenerator
	{
		/// <summary>
		/// Builds a set of unique outcomes for each pool of dice
		/// </summary>
		/// <returns></returns>
		public static Pool BuildOutcomes(this Pool pool)
		{
			if (pool.PoolDice.Any())
			{
				OutcomeComparison.PrintStartLog(pool.Name, pool.TotalOutcomes);

				RecursiveProcessing(pool.CopyPoolDice()).ToList().ForEach(die => pool.PoolResults.Add(die));

				pool.UniqueOutcomes = pool.PoolResults.Count;

				OutcomeComparison.PrintFinishLog(pool.UniqueOutcomes);
			}

			return pool;
		}

		private static Collection<PoolResult> RecursiveProcessing(ICollection<PoolDie> dice)
		{
			//if there are one or two dice left calculate their cross product and return the faces
			if (dice.SumQuantity() <= 2)
			{
				return BinaryRecursiveCrossProduct(dice);
			}

			//split the pool into two
			var split = SplitPoolDice(dice);

			//merge the two cross products
			return PoolCrossProduct(RecursiveProcessing(split[0]), RecursiveProcessing(split[1]));
		}

		/// <summary>
		/// Sorts the different use cases and passes the dice to the cross product calculator
		/// </summary>
		/// <param name="dice"></param>
		/// <returns></returns>
		private static Collection<PoolResult> BinaryRecursiveCrossProduct(ICollection<PoolDie> dice)
		{
			//if there is one element
			if (dice.Count == 1)
			{
				var partial = dice.FirstOrDefault().Die.GetDiePool();

				if (dice.SumQuantity() == 1)
				{
					//there is only one die left
					return PoolCrossProduct(partial, new Collection<PoolResult> { new PoolResult() });
				}
				else
				{
					//there are two
					return PoolCrossProduct(partial, partial);
				}
			}
			else
			{
				//there are two elements
				return PoolCrossProduct(dice.FirstOrDefault().Die.GetDiePool(), dice.LastOrDefault().Die.GetDiePool());
			}
		}

		/// <summary>
		/// Processes a cross product of two different dice
		/// </summary>
		/// <param name="topHalf"></param>
		/// <param name="bottomHalf"></param>
		/// <returns></returns>
		private static Collection<PoolResult> PoolCrossProduct(ICollection<PoolResult> topHalf, ICollection<PoolResult> bottomHalf)
		{
			var result = new Collection<PoolResult>();

			foreach (var topPool in topHalf)
			{
				foreach (var bottomPool in bottomHalf)
				{
					var mergedPool = new PoolResult(MergePoolSymbols(topPool.PoolResultSymbols, bottomPool.PoolResultSymbols))
					{
						//cross the quantity
						Frequency = topPool.Frequency * (bottomPool.Frequency != 0 ? bottomPool.Frequency : 1)
					};

					var match = result.GetMatch(mergedPool);

					//if the new merged pool exists, up the quantity
					if (match != null)
					{
						match.Frequency += mergedPool.Frequency;
					}
					else
					{
						//if unique add a new one
						result.Add(mergedPool);
					}
				}
			}

			return result;
		}

		/// <summary>
		/// Merges two symbol pools for a single combined and reduced pool
		/// </summary>
		/// <param name="topHalf"></param>
		/// <param name="bottomHalf"></param>
		/// <returns></returns>
		private static List<PoolResultSymbol> MergePoolSymbols(ICollection<PoolResultSymbol> topHalf, ICollection<PoolResultSymbol> bottomHalf)
		{
			var result = topHalf.Select(key => new PoolResultSymbol(key.Symbol, key.Quantity)).ToList();

			foreach (var key in bottomHalf)
			{
				var topKey = result.FirstOrDefault(w => w.Symbol == key.Symbol);

				//if there is matching keys, up the quantity, else add a new record
				if (topKey != null)
					topKey.Quantity += key.Quantity;
				else
					result.Add(new PoolResultSymbol(key.Symbol, key.Quantity));
			}

			return result;
		}

		/// <summary>
		/// Splits a pool of dice into two halves.  Remainder is in the bottom half.
		/// </summary>
		/// <param name="dice"></param>
		/// <returns></returns>
		private static Collection<ICollection<PoolDie>> SplitPoolDice(ICollection<PoolDie> dice)
		{
			var indexDice = new Collection<ICollection<PoolDie>>
			{
				new Collection<PoolDie>(),
				new Collection<PoolDie>()
			};

			//pop the top half of dice
			var target = dice.SumQuantity() / 2;

			while (indexDice[0].SumQuantity() < target)
			{
				var die = dice.First();
				//the die quantity is too large, copy the die and reduce it's quantity by target
				if (die.Quantity > target)
				{
					var take = target - indexDice[0].SumQuantity();
					indexDice[0].Add(new PoolDie(die.Die, take));
					die.Quantity -= take;
				}
				else
				{
					//pop the die off and add it to the new list
					indexDice[0].Add(new PoolDie(die.Die, die.Quantity));
					dice.Remove(die);
				}
			}

			indexDice[1] = dice;

			return indexDice;
		}
	}
}
