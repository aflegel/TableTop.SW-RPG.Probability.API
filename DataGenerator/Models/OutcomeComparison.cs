using System;
using static DataFramework.Models.Die;
using DataFramework.Models;

namespace DataGenerator.Models
{
	public static class OutcomeComparison
	{
		/// <summary>
		/// Compares the outcome of each pool's combined rolls
		/// </summary>
		public static PoolCombination CompareOutcomes(this PoolCombination poolCombination)
		{
			//print to console
			PrintConsoleLog(poolCombination);

			foreach (var positivePoolResult in poolCombination.PositivePool.PoolResults)
			{
				//loop through the simple pool to find matches
				foreach (var negativePoolResult in poolCombination.NegativePool.PoolResults)
				{
					var analysis = new OutcomeAnalysis(positivePoolResult, negativePoolResult);

					//add the net success quantity
					poolCombination.AddPoolCombinationStatistic(new PoolCombinationStatistic
					{
						Symbol = Symbol.Success,
						Quantity = analysis.SuccessNetQuantity,
						Frequency = analysis.Frequency,
						AlternateTotal = analysis.AdvantageNetQuantity
					});

					//add the net advantage quantity
					poolCombination.AddPoolCombinationStatistic(new PoolCombinationStatistic
					{
						Symbol = Symbol.Advantage,
						Quantity = analysis.AdvantageNetQuantity,
						Frequency = analysis.Frequency,
						AlternateTotal = analysis.SuccessNetQuantity
					});

					//add the net triumph
					poolCombination.AddPoolCombinationStatistic(new PoolCombinationStatistic
					{
						Symbol = Symbol.Triumph,
						Quantity = analysis.TriumphNetQuantity,
						Frequency = analysis.Frequency,
						AlternateTotal = analysis.DespairNetQuantity
					});

					//add the net despair
					poolCombination.AddPoolCombinationStatistic(new PoolCombinationStatistic
					{
						Symbol = Symbol.Despair,
						Quantity = analysis.DespairNetQuantity,
						Frequency = analysis.Frequency,
						AlternateTotal = analysis.TriumphNetQuantity
					});
				}
			}

			return poolCombination;
		}

		/// <summary>
		/// Displays the Unique List of rolls
		/// </summary>
		/// <param name="poolCombination"></param>
		private static void PrintConsoleLog(PoolCombination poolCombination)
		{
			PrintStartLog($"{poolCombination.PositivePool.Name}, {poolCombination.NegativePool.Name}", poolCombination.PositivePool.TotalOutcomes * poolCombination.NegativePool.TotalOutcomes);
			PrintFinishLog(poolCombination.PositivePool.UniqueOutcomes * poolCombination.NegativePool.UniqueOutcomes);
		}

		public static void PrintStartLog(string poolText, decimal rollEstimation) => Console.Write($"{poolText,-80}|{rollEstimation,29:n0}");

		public static void PrintFinishLog(decimal rollEstimation) => Console.Write($"  |{rollEstimation,12:n0}\n");
	}
}
