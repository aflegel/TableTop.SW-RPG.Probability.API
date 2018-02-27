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
		ProbabilityContext context = new ProbabilityContext();

		Pool PositiveContainer { get; set; }
		Pool NegativeContainer { get; set; }

		/// <summary>
		/// Create the container and run the simulation
		/// </summary>
		/// <param name="PositiveContainer"></param>
		/// <param name="NegativeContainer"></param>
		public PoolSummary(Pool PositiveContainer, Pool NegativeContainer)
		{
			this.PositiveContainer = PositiveContainer;
			this.NegativeContainer = NegativeContainer;

			//print to console
			PrintConsoleLog();

			ProcessRollComparison();
		}

		/// <summary>
		/// Compares the outcome of each pool's combined rolls
		/// </summary>
		private void ProcessRollComparison()
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

					//if the found threshold is the same as the required threshold add the frequency
					if (analysis.IsSuccess)
					{
						poolCombo.AddPoolCombinationStatistic(new PoolCombinationStatistic()
						{
							Symbol = Symbol.Success,
							Quantity = analysis.SuccessNetQuantity,
							Frequency = analysis.Frequency
						});

						//it is only a triumph if it is a success
						if (analysis.IsTriumph)
						{
							//if there are more successes than failures the truimpn count is the max triumph, otherwise reduce by the difference
							poolCombo.AddPoolCombinationStatistic(new PoolCombinationStatistic()
							{
								Symbol = Symbol.Triumph,
								Quantity = analysis.TriumphNetQuantity,
								Frequency = analysis.Frequency
							});
						}
					}
					else
					{
						poolCombo.AddPoolCombinationStatistic(new PoolCombinationStatistic()
						{
							Symbol = Symbol.Failure,
							Quantity = analysis.FailureNetQuantity,
							Frequency = analysis.Frequency
						});

						//if the found threshold is the same as the required threshold add the frequency
						if (analysis.IsDespair)
						{
							poolCombo.AddPoolCombinationStatistic(new PoolCombinationStatistic()
							{
								Symbol = Symbol.Despair,
								Quantity = analysis.DespairNetQuantity,
								Frequency = analysis.Frequency
							});
						}
					}


					//if the found threshold is the same as the required threshold add the frequency
					if (analysis.IsAdvantage)
					{
						poolCombo.AddPoolCombinationStatistic(new PoolCombinationStatistic()
						{
							Symbol = Symbol.Advantage,
							Quantity = analysis.AdvantageNetQuantity,
							Frequency = analysis.Frequency
						});
					}
					else if (analysis.IsThreat)
					{
						poolCombo.AddPoolCombinationStatistic(new PoolCombinationStatistic()
						{
							Symbol = Symbol.Threat,
							Quantity = analysis.ThreatNetQuantity,
							Frequency = analysis.Frequency
						});
					}
				}
			}
		}

		/// <summary>
		/// Displays the Unique List of rolls
		/// </summary>
		/// <param name="outcomePool"></param>
		protected void PrintConsoleLog()
		{
			PrintStartLog(PositiveContainer.Name + ", " + NegativeContainer.Name, PositiveContainer.TotalOutcomes * NegativeContainer.TotalOutcomes);
			PrintFinishLog(PositiveContainer.UniqueOutcomes * NegativeContainer.UniqueOutcomes);
		}

		public static void PrintStartLog(string poolText, long rollEstimation)
		{
			Console.Write(string.Format("{0,-80}|{1,23:n0}", poolText, rollEstimation));
		}

		public static void PrintFinishLog(long rollEstimation)
		{
			Console.Write(string.Format("  |{0,15:n0}\n", rollEstimation));
		}


	}
}
