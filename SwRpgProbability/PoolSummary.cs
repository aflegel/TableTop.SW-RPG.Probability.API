using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SwRpgProbability.Dice;
using System.IO;

namespace SwRpgProbability
{
	class PoolSummary
	{
		//this is a decimal to do math
		decimal totalCount;
		List<Die> dicePool;
		DieResult results;

		public PoolSummary(List<Die> testingDice)
		{
			dicePool = testingDice;

			//todo: create PoolMaster and PoolPart records
		}

		public DieResult Run()
		{
			results = new DieResult();

			ProcessPreOutput();

			Dictionary<Face, long> outcomePool = ProcessDicePool();

			ProcessMainOutput(outcomePool);

			//ProcessAnalysisOutput(outcomePool, dicePool);

			SummarizePool(outcomePool);

			return results;
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="dicePool"></param>
		protected void ProcessPreOutput()
		{
			var rollEstimation = dicePool.Aggregate((long)1, (x, y) => x * y.Faces.Count);
			var poolText = dicePool.GroupBy(info => info.ToString()).Select(group => string.Format("{0} {1}", group.Key, group.Count()) ).ToList();

			//update the user
			Console.WriteLine(string.Format("Pool: {0} | Outcomes: {1:n0} ", string.Join(", ", poolText), rollEstimation));
		}

		protected void ProcessPostOutput()
		{
		}

		protected Dictionary<Face, long> ProcessDicePool()
		{

			Dictionary<Face, long> bulkPool = new Dictionary<Face, long>();

			List<Dictionary<Face, long>> partialPools = new List<Dictionary<Face, long>>();

			//break the dice pools into groups of 5 for processing
			var groups = dicePool.Select((p, index) => new { p, index }).GroupBy(a => a.index / 5).Select((grp => grp.Select(g => g.p).ToList())).ToList();

			foreach (var group in groups)
			{
				partialPools.Add(ProcessPartialDicePool(group));
			}

			foreach (var partial in partialPools)
			{
				//Console.WriteLine(string.Format("Partial Rolls: {0,10:n0} Unique: {1,10:n0}", partial.Sum(s => s.Value), partial.Count));
				//gather cross product
				bulkPool = ProcessCrossProduct(bulkPool, partial);

				//Console.WriteLine(string.Format("Progress Rolls: {0,10:n0} Unique: {1,10:n0}", bulkPool.Sum(s => s.Value), bulkPool.Count));
				//Console.WriteLine("--");
			}

			//Console.WriteLine(string.Format("Processed Rolls: {0,10:n0} Unique: {1,10:n0}", bulkPool.Sum(s => s.Value), bulkPool.Count));
			/*
			var test = ProcessDicePool(dicePool);

			Console.WriteLine(string.Format("Confirming Rolls: {0,10:n0} Unique: {1,10:n0}", test.Sum(s => (long)s.Value), test.Count));
			*/
			return bulkPool;
		}

		/// <summary>
		/// Builds
		/// </summary>
		/// <param name="partialDicePool"></param>
		/// <returns></returns>
		protected Dictionary<Face, long> ProcessPartialDicePool(List<Die> partialDicePool)
		{
			//this array tracks which face is for selection
			int[] indexTracker = new int[partialDicePool.Count];
			//init to 0
			for (int i = 0; i < partialDicePool.Count; i++)
				indexTracker[i] = 0;

			Dictionary<Face, long> bulkPool = new Dictionary<Face, long>();

			//while the tracking array is less than the face count
			while (indexTracker[partialDicePool.Count - 1] < partialDicePool[partialDicePool.Count - 1].Faces.Count)
			{
				for (int i = 0; i < partialDicePool[0].Faces.Count; i++)
				{
					//add the a face from the first die
					Face roll = partialDicePool[0].Faces[i];

					//take one face from each remaining die, j = 1 to skip the first die
					for (int j = 1; j < partialDicePool.Count; j++)
					{
						roll = roll.Merge(partialDicePool[j].Faces[indexTracker[j]]);
					}

					//add the roll to the mix
					if (roll.Symbols.Count > 0)
					{
						try
						{
							//update the number of this particular pool
							bulkPool[roll] = bulkPool[roll] + 1;
						}
						catch
						{
							//else add it to the pool
							bulkPool.Add(roll, 1);
						}
					}
				}

				//manually update the next index,
				if (partialDicePool.Count > 1)
					indexTracker[1]++;
				else
					//triggers the loop escape
					indexTracker[0] = partialDicePool[0].Faces.Count;

				//update the indexes
				for (int i = 1; i < partialDicePool.Count; i++)
				{
					//if the current index exceeds the faces of the die roll the counter to 0 and up the next die face
					if (indexTracker[i] >= partialDicePool[i].Faces.Count)
					{
						if (i < partialDicePool.Count - 1)
						{
							indexTracker[i] = 0;
							indexTracker[i + 1]++;
						}
					}
				}
			}

			return bulkPool;
		}

		protected Dictionary<Face, long> ProcessCrossProduct(Dictionary<Face, long> startingPool, Dictionary<Face, long> additionalPool)
		{
			//escape an empty pool
			if (startingPool.Count == 0)
				return additionalPool;

			Dictionary<Face, long> bulkPool = new Dictionary<Face, long>();

			foreach (Face startingMap in startingPool.Keys)
			{
				foreach (Face addingMap in additionalPool.Keys)
				{
					//merge the roll faces and calculate the frequency
					Face roll = startingMap.Merge(addingMap);
					long combinedFrequency = startingPool[startingMap] * additionalPool[addingMap];

					//attempt to combine with an existing roll
					try
					{
						bulkPool[roll] = bulkPool[roll] + combinedFrequency;
					}
					catch
					{
						//add the roll to the bulk pool
						bulkPool.Add(roll, combinedFrequency);
					}
				}
			}

			return bulkPool;
		}


		/// <summary>
		/// Displays the Unique List of rolls
		/// </summary>
		/// <param name="outcomePool"></param>
		protected void ProcessMainOutput(Dictionary<Face, long> outcomePool)
		{

			//hijacking this function to gather initial results
			var poolText = dicePool.GroupBy(info => info.ToString()).Select(group => string.Format("{0} {1}", group.Key, group.Count())).ToList();

			results.Dice = string.Join(", ", poolText);
			results.Count = outcomePool.Sum(s => s.Value);
			results.Unique = outcomePool.Count;
			totalCount = results.Count;
		}


		/// <summary>
		/// Displays the breakdown of rolls and the probability of success
		/// </summary>
		/// <param name="outcomePool"></param>
		/// <param name="dicePool"></param>
		protected void ProcessAnalysisOutput(Dictionary<Face, long> outcomePool, List<Die> dicePool)
		{
			//string format = "| {0,9} | {1,5:#0} | {2,9:#0} | {3,11:#0.0} |";
			//TextOutput.WriteLine();
			//Console.WriteLine("Required Roll Breakdown");
			//TextOutput.WriteLine("-----------------------");
			//Console.WriteLine(string.Format(format, "Face", "Count", "Frequency", "Probability"));


			//var test = dicePool.SelectMany(x => x.faceMaps.Select(y => y.faces.Select(i => i.Key))).Distinct();

			foreach (Symbol face in Die.CountPool(dicePool).Symbols.Keys)
			{
				for (byte i = 1; i <= dicePool.Count * 2; i++)
				{
					long found = ProcessBreakdownPool(outcomePool, new Face(new Dictionary<Symbol, byte>() { { face, i } }));

					//Console.WriteLine(string.Format(format, face.ToString(), i, found, (found / totalCount).ToString("#0.0%")));

					//don't bother continuing to process after no results
					if (found == 0)
						break;
				}
				//Console.WriteLine("---");
			}
		}

		/// <summary>
		/// Searches a pool of dice for a specific outcome and returns the number of rolls of that outcome
		/// </summary>
		/// <param name="outcomePool"></param>
		/// <param name="requiredQuery"></param>
		/// <returns></returns>
		protected long ProcessBreakdownPool(Dictionary<Face, long> outcomePool, Face requiredQuery)
		{
			//todo: This needs to be reconfigured to have a the ability to search for more than one field at a time
			string format = "| {0,9:#0} | {1,11} | {2}";

			Console.Write(string.Format("{0} ", requiredQuery.ToString()));

			//initialize the frequency for this requirement and the threshold for match
			long frequency = 0;
			int searchThreshold = requiredQuery.Symbols.Sum(x => x.Value);

			//expand the search to include the superior versions of the requirement
			if (requiredQuery.Symbols.ContainsKey(Symbol.Advantage))
			{
				//requiredQuery.faces.Add(Face.triumph, requiredQuery.faces[Face.advantage]);
			}
			else if (requiredQuery.Symbols.ContainsKey(Symbol.Success))
			{
				//requiredQuery.faces.Add(Face.advantage, requiredQuery.faces[Face.success]);
				requiredQuery.Symbols.Add(Symbol.Triumph, requiredQuery.Symbols[Symbol.Success]);
			}

			if (requiredQuery.Symbols.ContainsKey(Symbol.Threat))
			{
				//requiredQuery.faces.Add(Face.dispair, requiredQuery.faces[Face.threat]);
			}
			else if (requiredQuery.Symbols.ContainsKey(Symbol.Failure))
			{
				//requiredQuery.faces.Add(Face.threat, requiredQuery.faces[Face.failure]);
				requiredQuery.Symbols.Add(Symbol.Despair, requiredQuery.Symbols[Symbol.Failure]);
			}

			//loop through the simple pool to find matches
			foreach (Face map in outcomePool.Keys)
			{
				int threshold = 0;

				foreach (Symbol face in requiredQuery.Symbols.Keys)
				{
					//if it finds a matching key increase the threshold
					if (map.Symbols.ContainsKey(face))
						threshold += map.Symbols[face];
				}

				//if the found threshold is the same as the required threshold add the frequency and display the roll result
				if (threshold == searchThreshold)
				{
					frequency += outcomePool[map];
					//Console.WriteLine(string.Format(format, outcomePool[map], (outcomePool[map] / totalCount).ToString("#0.0%"), map.ToString()));
				}
			}

			Console.WriteLine(string.Format("Total {0:n0} ({1}) ", frequency, (frequency / totalCount).ToString("#0.000%")));

			return frequency;
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="outcomePool"></param>
		protected void SummarizePool(Dictionary<Face, long> outcomePool)
		{
			//todo: create PoolMaster record

			List<Symbol> successKeys = new List<Symbol>() { Symbol.Success, Symbol.Triumph };
			List<Symbol> failureKeys = new List<Symbol>() { Symbol.Failure, Symbol.Despair };

			long successFrequency = 0;

			long advantageFrequency = 0;
			long threatFrequency = 0;
			long triumphFrequency = 0;
			long despairFrequency = 0;

			//loop through the simple pool to find matches
			foreach (Face map in outcomePool.Keys)
			{
				//todo: create RollPart record
				int successThreshold = 0;
				int failureThreshold = 0;

				int advantageThreshold = 0;
				int threatThreshold = 0;

				foreach (Symbol face in successKeys)
				{
					//if it finds a matching key increase the threshold
					if (map.Symbols.ContainsKey(face))
						successThreshold += map.Symbols[face];
				}

				foreach (Symbol face in failureKeys)
				{
					//if it finds a matching key increase the threshold
					if (map.Symbols.ContainsKey(face))
						failureThreshold += map.Symbols[face];
				}

				//if the found threshold is the same as the required threshold add the frequency and display the roll result
				if (successThreshold > 0 && successThreshold > failureThreshold)
				{
					successFrequency += outcomePool[map];
				}


				//if it finds a matching key increase the threshold
				if (map.Symbols.ContainsKey(Symbol.Advantage))
					advantageThreshold += map.Symbols[Symbol.Advantage];

				//if it finds a matching key increase the threshold
				if (map.Symbols.ContainsKey(Symbol.Threat))
					threatThreshold += map.Symbols[Symbol.Threat];

				//if the found threshold is the same as the required threshold add the frequency and display the roll result
				if (advantageThreshold > 0 && advantageThreshold > threatThreshold)
				{
					advantageFrequency += outcomePool[map];
				}
				else if (threatThreshold > advantageThreshold)
				{
					threatFrequency += outcomePool[map];
				}

				//if it finds a matching key increase the threshold
				if (map.Symbols.ContainsKey(Symbol.Triumph))
					triumphFrequency += outcomePool[map];

				//if it finds a matching key increase the threshold
				if (map.Symbols.ContainsKey(Symbol.Despair))
					despairFrequency += outcomePool[map];
			}

			//todo: Update PoolMaster record

			results.Success = successFrequency;
			results.Failure = results.Count - successFrequency;

			results.Advantage = advantageFrequency;
			results.Threat = threatFrequency;
			results.Stalemate = results.Count - (advantageFrequency + threatFrequency);

			results.Triumph = triumphFrequency;
			results.Despair = despairFrequency;
		}
	}
}
