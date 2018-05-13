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
	class PoolSummary
	{

		/// <summary>
		/// Create the container and run the simulation
		/// </summary>
		/// <param name="PositiveContainer"></param>
		/// <param name="NegativeContainer"></param>
		public PoolSummary(ProbabilityContext context, Pool PositiveContainer, Pool NegativeContainer)
		{
			//print to console
			PrintConsoleLog(PositiveContainer, NegativeContainer);

			ProcessRollComparison(context, PositiveContainer, NegativeContainer);
		}

		/// <summary>
		/// Compares the outcome of each pool's combined rolls
		/// </summary>
		private void ProcessRollComparison(ProbabilityContext context, Pool PositiveContainer, Pool NegativeContainer)
		{
			var poolCombo = new PoolCombination();
			PositiveContainer.PositivePoolCombinations.Add(poolCombo);
			NegativeContainer.NegativePoolCombinations.Add(poolCombo);

			foreach (var positivePoolResult in PositiveContainer.PoolResults)
			{
				//loop through the simple pool to find matches
				foreach (var negativePoolResult in NegativeContainer.PoolResults)
				{
					var analysis = new PoolAnalysis(positivePoolResult, negativePoolResult);

					//add the net success quantity
					poolCombo.AddPoolCombinationStatistic(new PoolCombinationStatistic()
					{
						Symbol = Symbol.Success,
						Quantity = analysis.SuccessNetQuantity,
						Frequency = (long)analysis.Frequency,
						OffSymbolAverage = analysis.AdvantageNetQuantity
					});

					//add the net advantage quantity
					poolCombo.AddPoolCombinationStatistic(new PoolCombinationStatistic()
					{
						Symbol = Symbol.Advantage,
						Quantity = analysis.AdvantageNetQuantity,
						Frequency = (long)analysis.Frequency,
						OffSymbolAverage = analysis.SuccessNetQuantity
					});

					//add the net triumph
					poolCombo.AddPoolCombinationStatistic(new PoolCombinationStatistic()
					{
						Symbol = Symbol.Triumph,
						Quantity = analysis.TriumphNetQuantity,
						Frequency = (long)analysis.Frequency,
						OffSymbolAverage = 0
					});
				}
			}
		}

		/// <summary>
		/// Displays the Unique List of rolls
		/// </summary>
		/// <param name="outcomePool"></param>
		protected void PrintConsoleLog(Pool PositiveContainer, Pool NegativeContainer)
		{
			PrintStartLog(PositiveContainer.Name + ", " + NegativeContainer.Name, (ulong)PositiveContainer.TotalOutcomes * (ulong)NegativeContainer.TotalOutcomes);
			PrintFinishLog((ulong)PositiveContainer.UniqueOutcomes * (ulong)NegativeContainer.UniqueOutcomes);
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
