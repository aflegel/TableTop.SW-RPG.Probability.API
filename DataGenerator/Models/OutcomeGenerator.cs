using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using DataGenerator.Models;
using System.Collections.ObjectModel;
using DataFramework.Context;
using DataFramework.Models;
using Microsoft.EntityFrameworkCore.Internal;

namespace DataGenerator.Models
{
	class OutcomeGenerator
	{
		public OutcomeGenerator(Pool pool)
		{
			ProcessDicePool(pool);
		}

		/// <summary>
		/// Builds a set of unique outcomes for each pool of dice
		/// </summary>
		/// <returns></returns>
		protected void ProcessDicePool(Pool pool)
		{
			OutcomeComparison.PrintStartLog(pool.Name, pool.TotalOutcomes);

			var indexDice = CopyPoolDice(pool);

			foreach (var die in RecursiveProcessing(indexDice))
				pool.PoolResults.Add(die);

			pool.UniqueOutcomes = pool.PoolResults.Count;

			OutcomeComparison.PrintFinishLog(pool.UniqueOutcomes);
		}

		protected Collection<PoolDie> CopyPoolDice(Pool pool)
		{
			var indexDice = new Collection<PoolDie>();

			foreach (var poolDie in pool.PoolDice)
			{
				indexDice.Add(new PoolDie(poolDie.Die, poolDie.Quantity));
			}

			return indexDice;
		}

		protected Collection<PoolResult> RecursiveProcessing(Collection<PoolDie> Dice)
		{
			var resultingList = new Collection<PoolResultSymbol>();

			//if there are one or two dice left calculate their cross product and return the faces
			if (GetPoolDiceCount(Dice) <= 2)
			{
				return BinaryRecursiveCrossProduct(Dice);
			}

			//split the pool into two
			var split = SplitPoolDice(Dice);

			//merge the two cross products
			return PoolCrossProduct(RecursiveProcessing(split[0]), RecursiveProcessing(split[1]));
		}

		/// <summary>
		/// Sorts the different use cases and passes the dice to the cross product calculator
		/// </summary>
		/// <param name="Dice"></param>
		/// <returns></returns>
		protected Collection<PoolResult> BinaryRecursiveCrossProduct(Collection<PoolDie> Dice)
		{
			//if there is one element
			if (Dice.Count == 1)
			{
				var partial = GetDiePool(Dice.FirstOrDefault().Die);

				if (GetPoolDiceCount(Dice) == 1)
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
				return PoolCrossProduct(GetDiePool(Dice.FirstOrDefault().Die), GetDiePool(Dice.LastOrDefault().Die));
			}
		}

		/// <summary>
		/// Processes a cross product of two different dice
		/// </summary>
		/// <param name="TopHalf"></param>
		/// <param name="BottomHalf"></param>
		/// <returns></returns>
		protected Collection<PoolResult> PoolCrossProduct(Collection<PoolResult> TopHalf, Collection<PoolResult> BottomHalf)
		{
			var result = new Collection<PoolResult>();

			foreach (var topPool in TopHalf)
			{
				foreach (var bottomPool in BottomHalf)
				{
					var mergedPool = new PoolResult(MergePoolSymbols(topPool.PoolResultSymbols, bottomPool.PoolResultSymbols))
					{
						//cross the quantity
						Frequency = topPool.Frequency * (bottomPool.Frequency != 0 ? bottomPool.Frequency : 1)
					};

					int? match = EntryExists(result, mergedPool);

					//if the new merged pool exists, up the quantity
					if (match.HasValue)
					{
						result[match.Value].Frequency += mergedPool.Frequency;
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

		private int? EntryExists(Collection<PoolResult> result, PoolResult mergedPool)
		{
			foreach (var existing in result)
			{
				if (existing.PoolResultSymbols.Count != mergedPool.PoolResultSymbols.Count)
					continue;

				var comparer = new PoolResultSymbolEqualityComparer();

				var notA = existing.PoolResultSymbols.Except(mergedPool.PoolResultSymbols, comparer);
				var notB = mergedPool.PoolResultSymbols.Except(existing.PoolResultSymbols, comparer);

				if (!notA.Any() && !notB.Any())
					return result.IndexOf(existing);
			}

			return null;
		}

		/// <summary>
		/// Merges two symbol pools for a single combined and reduced pool
		/// </summary>
		/// <param name="TopHalf"></param>
		/// <param name="BottomHalf"></param>
		/// <returns></returns>
		protected List<PoolResultSymbol> MergePoolSymbols(ICollection<PoolResultSymbol> TopHalf, ICollection<PoolResultSymbol> BottomHalf)
		{
			var result = new List<PoolResultSymbol>();

			//prime the result with the top half
			foreach (var key in TopHalf)
			{
				result.Add(new PoolResultSymbol(key.Symbol, key.Quantity));
			}

			foreach (var key in BottomHalf)
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
		/// Returns a result for each face of a die
		/// </summary>
		/// <param name="Die"></param>
		/// <returns></returns>
		protected Collection<PoolResult> GetDiePool(Die Die)
		{
			var resultingList = new Collection<PoolResult>();

			foreach (var face in Die.DieFaces)
			{
				var poolResult = new PoolResult()
				{
					Frequency = 1
				};

				foreach (var facesymbol in face.DieFaceSymbols)
				{
					poolResult.PoolResultSymbols.Add(new PoolResultSymbol(facesymbol.Symbol, facesymbol.Quantity));
				}

				resultingList.Add(poolResult);
			}
			return resultingList;
		}


		/// <summary>
		/// Splits a pool of dice into two halves.  Remainder is in the bottom half.
		/// </summary>
		/// <param name="Dice"></param>
		/// <returns></returns>
		protected Collection<Collection<PoolDie>> SplitPoolDice(Collection<PoolDie> Dice)
		{
			var indexDice = new Collection<Collection<PoolDie>>(){
				new Collection<PoolDie>(),
				new Collection<PoolDie>()
			};

			//pop the top half of dice
			var target = GetPoolDiceCount(Dice) / 2;

			while (GetPoolDiceCount(indexDice[0]) < target)
			{
				//the die quantity is too large, copy the die and reduce it's quantity by target
				if (Dice[0].Quantity > target)
				{
					var take = target - GetPoolDiceCount(indexDice[0]);
					indexDice[0].Add(new PoolDie(Dice[0].Die, take));
					Dice[0].Quantity -= take;
				}
				else
				{
					//pop the die off and add it to the new list
					indexDice[0].Add(new PoolDie(Dice[0].Die, Dice[0].Quantity));
					Dice.RemoveAt(0);
				}
			}

			indexDice[1] = Dice;

			return indexDice;
		}

		protected int GetPoolDiceCount(Collection<PoolDie> Dice) => Dice.Sum(die => die.Quantity);
	}
}
