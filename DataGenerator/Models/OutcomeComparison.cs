using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using DataGenerator.Models;
using static DataFramework.Models.Die;
using DataFramework.Models;
using DataFramework.Context;

namespace DataGenerator.Models
{
	class OutcomeComparison
	{

		/// <summary>
		/// Create the container and run the simulation
		/// </summary>
		/// <param name="positivePool"></param>
		/// <param name="negativePool"></param>
		public OutcomeComparison(PoolCombination poolCombination)
		{
			//print to console
			PrintConsoleLog(poolCombination);

			ProcessRollComparison(poolCombination);
		}

		/// <summary>
		/// Compares the outcome of each pool's combined rolls
		/// </summary>
		private void ProcessRollComparison(PoolCombination poolCombination)
		{
			foreach (var positivePoolResult in poolCombination.PositivePool.PoolResults)
			{
				//loop through the simple pool to find matches
				foreach (var negativePoolResult in poolCombination.NegativePool.PoolResults)
				{
					var analysis = new OutcomeAnalysis(positivePoolResult, negativePoolResult);

					//add the net success quantity
					poolCombination.AddPoolCombinationStatistic(new PoolCombinationStatistic()
					{
						Symbol = Symbol.Success,
						Quantity = analysis.SuccessNetQuantity,
						Frequency = (long)analysis.Frequency,
						AlternateTotal = analysis.AdvantageNetQuantity
					});

					//add the net advantage quantity
					poolCombination.AddPoolCombinationStatistic(new PoolCombinationStatistic()
					{
						Symbol = Symbol.Advantage,
						Quantity = analysis.AdvantageNetQuantity,
						Frequency = (long)analysis.Frequency,
						AlternateTotal = analysis.SuccessNetQuantity
					});

					//add the net triumph
					poolCombination.AddPoolCombinationStatistic(new PoolCombinationStatistic()
					{
						Symbol = Symbol.Triumph,
						Quantity = analysis.TriumphNetQuantity,
						Frequency = (long)analysis.Frequency,
						AlternateTotal = 0
					});
				}
			}
		}

		/// <summary>
		/// Displays the Unique List of rolls
		/// </summary>
		/// <param name="outcomePool"></param>
		protected void PrintConsoleLog(PoolCombination poolCombination)
		{
			PrintStartLog(poolCombination.PositivePool.Name + ", " + poolCombination.NegativePool.Name, (ulong)poolCombination.PositivePool.TotalOutcomes * (ulong)poolCombination.NegativePool.TotalOutcomes);
			PrintFinishLog((ulong)poolCombination.PositivePool.UniqueOutcomes * (ulong)poolCombination.NegativePool.UniqueOutcomes);
		}

		public static void PrintStartLog(string poolText, ulong rollEstimation)
		{
			Console.Write(string.Format("{0,-80}|{1,29:n0}", poolText, rollEstimation));
		}

		public static void PrintFinishLog(ulong rollEstimation)
		{
			Console.Write(string.Format("  |{0,12:n0}\n", rollEstimation));
		}


	}
}
