using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace SwRpgProbability.Models
{
	class PoolSummary
	{
		RollContainer PositiveContainer { get; set; }
		RollContainer NegativeContainer { get; set; }

		/// <summary>
		/// Create the container and run the simulation
		/// </summary>
		/// <param name="PositiveContainer"></param>
		/// <param name="NegativeContainer"></param>
		public PoolSummary(RollContainer PositiveContainer, RollContainer NegativeContainer)
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

			foreach (Face positiveMap in PositiveContainer.ResultList.Keys)
			{

				//loop through the simple pool to find matches
				foreach (Face negitiveMap in NegativeContainer.ResultList.Keys)
				{
					long frequency = PositiveContainer.ResultList[positiveMap] * NegativeContainer.ResultList[negitiveMap];

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
							triumphFrequency = +frequency;
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

			var results = new RollResult
			{
				Count = PositiveContainer.ResultList.Sum(s => s.Value) * NegativeContainer.ResultList.Sum(s => s.Value),
				Unique = PositiveContainer.ResultList.Count * NegativeContainer.ResultList.Count,
				Dice = RollContainer.GetPoolText(PositiveContainer.DicePool.Union(NegativeContainer.DicePool).ToList()),
				Success = successFrequency,

				Triumph = triumphFrequency,
				Despair = despairFrequency,
				Advantage = advantageFrequency,
				Threat = threatFrequency,
			};

			ProcessDatabaseRecords(results);
		}

		/// <summary>
		/// Displays the Unique List of rolls
		/// </summary>
		/// <param name="outcomePool"></param>
		protected void PrintConsoleLog()
		{
			var largePool = PositiveContainer.DicePool.Union(NegativeContainer.DicePool).ToList();

			PrintConsoleLog(RollContainer.GetPoolText(largePool), RollContainer.GetRollEstimation(largePool));
		}

		public static void PrintConsoleLog(string poolText, long rollEstimation)
		{
			Console.WriteLine(string.Format("Pool: {0,-80} | Outcomes: {1,30:n0}", poolText, rollEstimation));
		}

		/// <summary>
		/// Adds a record of the specific combination to the database with a cache of the outcome results
		/// </summary>
		/// <param name="outcomePool"></param>

		private void ProcessDatabaseRecords(RollResult result)
		{
			using (var db = new DataContext.ProbabilityContext())
			{
				var pool = new DataContext.Pool()
				{
					Name = result.Dice,
					TotalOutcomes = result.Count,
					UniqueOutcomes = result.Unique,
					SuccessOutcomes = result.Success,
					FailureOutcomes = result.Failure,
					AdvantageOutcomes = result.Advantage,
					ThreatOutcomes = result.Threat,
					DespairOutcomes = result.Despair,
					TriumphOutcomes = result.Triumph,
					NeutralOutcomes = result.Neutral
				};
				db.Pools.Add(pool);

				//create the record of which dice are used
				foreach (var die in PositiveContainer.DicePool.GroupBy(info => info.ToString()).Select(group => new { group.Key, Count = group.Count() }).ToList())
				{
					var dbDie = db.Dice.FirstOrDefault(w => w.Name == die.Key);
					pool.PoolDice.Add(new DataContext.PoolDie() { Die = dbDie, Quantity = die.Count });
				}

				/*
				//create the record of the outcomes
				foreach (var outcome in outcomePool)
				{
					//each KVP represents a unique roll
					var poolResult = new DataContext.PoolResult();
					pool.PoolResults.Add(poolResult);

					foreach (var symbol in outcome.Key.Symbols)
					{
						//each symbol and count
						var resultSymbol = new DataContext.PoolResultSymbol()
						{
							Symbol = symbol.Key,
							Quantity = symbol.Value
						};

						poolResult.PoolResultSymbols.Add(resultSymbol);
					}
				}
				*/
				db.SaveChanges();
			}
		}

		private int CountMatchingKeys(Face map, List<Symbol> keys)
		{
			int sum = 0;
			foreach (Symbol face in keys)
			{
				//if it finds a matching key increase the threshold
				if (map.Symbols.ContainsKey(face))
					sum += map.Symbols[face];
			}

			return sum;
		}
	}
}
