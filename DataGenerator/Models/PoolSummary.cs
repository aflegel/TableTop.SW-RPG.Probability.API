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
			long successFrequency = 0;
			long advantageFrequency = 0;
			long threatFrequency = 0;
			long triumphFrequency = 0;
			long despairFrequency = 0;

			foreach (var positiveMap in PositiveContainer.PoolResults)
			{

				//loop through the simple pool to find matches
				foreach (var negitiveMap in NegativeContainer.PoolResults)
				{
					long frequency = positiveMap.Quantity * negitiveMap.Quantity;

					//triumphs count as successes but advantages do not
					int successThreshold = CountMatchingKeys(positiveMap, new List<Symbol>() { Symbol.Success, Symbol.Triumph });

					//despairs count as failures but threats do not
					int failureThreshold = CountMatchingKeys(negitiveMap, new List<Symbol>() { Symbol.Failure, Symbol.Despair });

					int advantageThreshold = CountMatchingKeys(positiveMap, new List<Symbol>() { Symbol.Advantage });
					int threatThreshold = CountMatchingKeys(negitiveMap, new List<Symbol>() { Symbol.Threat });

					int triumphThreshold = CountMatchingKeys(positiveMap, new List<Symbol>() { Symbol.Triumph });
					int despairThreshold = CountMatchingKeys(negitiveMap, new List<Symbol>() { Symbol.Despair });

					//if the found threshold is the same as the required threshold add the frequency
					if (successThreshold > failureThreshold)
					{
						successFrequency += frequency;

						//it is only a triumph if it is a success
						if (triumphThreshold > 0)
							triumphFrequency += frequency;
					}
					else
					{
						//if the found threshold is the same as the required threshold add the frequency
						if (despairThreshold > 0)
							despairFrequency += frequency;
					}


					//if the found threshold is the same as the required threshold add the frequency
					if (advantageThreshold > 0 && advantageThreshold > threatThreshold)
						advantageFrequency += frequency;
					else if (threatThreshold > advantageThreshold)
						threatFrequency += frequency;
				}
			}

			var poolCombo = new PoolCombination()
			{
				//PositivePool = PositiveContainer,
				//NegativePool = NegativeContainer,
				SuccessOutcomes = successFrequency,

				AdvantageOutcomes = advantageFrequency,
				ThreatOutcomes = threatFrequency,

				TriumphOutcomes = triumphFrequency,
				DespairOutcomes = despairFrequency,
			};

			PositiveContainer.PositivePoolCombinations.Add(poolCombo);
			NegativeContainer.NegativePoolCombinations.Add(poolCombo);
		}

		/// <summary>
		/// Displays the Unique List of rolls
		/// </summary>
		/// <param name="outcomePool"></param>
		protected void PrintConsoleLog()
		{
			PrintStartLog(PositiveContainer.Name + NegativeContainer.Name, PositiveContainer.TotalOutcomes * NegativeContainer.TotalOutcomes);
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

		/// <summary>
		/// Returns a sum of the Symbols in the map
		/// </summary>
		/// <param name="map"></param>
		/// <param name="keys"></param>
		/// <returns></returns>
		private int CountMatchingKeys(PoolResult map, List<Symbol> keys)
		{
			return map.PoolResultSymbols.Where(a => keys.Contains(a.Symbol)).Sum(s => s.Quantity);
		}
	}
}
