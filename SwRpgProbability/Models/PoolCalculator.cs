using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace SwRpgProbability.Models
{
	class PoolCalculator
	{
		RollContainer RollOutput { get; set; }

		public PoolCalculator(RollContainer RollOutput)
		{
			this.RollOutput = RollOutput;

			PoolSummary.PrintConsoleLog(RollOutput.GetPoolText(), RollOutput.GetRollEstimation());

			ProcessDicePool();
		}

		/// <summary>
		/// Builds a set of unique outcomes for each pool of dice
		/// </summary>
		/// <returns></returns>
		protected void ProcessDicePool()
		{
			var partialPools = new List<Dictionary<Face, long>>();

			//break the dice pools into groups of 4 for processing
			var groups = RollOutput.DicePool.Select((p, index) => new { p, index }).GroupBy(a => a.index / 4).Select((grp => grp.Select(g => g.p).ToList())).ToList();

			//process the partial group for unique rolls
			foreach (var group in groups)
				partialPools.Add(ProcessPartialDicePool(group));

			//gather cross product of the partial pool
			foreach (var partial in partialPools)
				RollOutput.ResultList = ProcessCrossProduct(RollOutput.ResultList, partial);
		}

		/// <summary>
		/// Builds the table of unique rolls
		/// </summary>
		/// <param name="partialDicePool"></param>
		/// <returns></returns>
		protected Dictionary<Face, long> ProcessPartialDicePool(List<Die> partialDicePool)
		{
			/*
			 * copy pool
			 * Foreach die{
			 * pop die from copy
			 * foreach copy die
			 * merge faces}
			 */

			//this array tracks which face is for selection
			int[] indexTracker = new int[partialDicePool.Count];

			//init to 0
			for (int i = 0; i < partialDicePool.Count; i++)
				indexTracker[i] = 0;

			var bulkPool = new Dictionary<Face, long>();

			//while the tracking array is less than the face count
			while (indexTracker[partialDicePool.Count - 1] < partialDicePool[partialDicePool.Count - 1].Faces.Count)
			{
				for (int i = 0; i < partialDicePool[0].Faces.Count; i++)
				{
					//add the a face from the first die
					var roll = partialDicePool[0].Faces[i];

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

			var bulkPool = new Dictionary<Face, long>();

			foreach (Face startingMap in startingPool.Keys)
			{
				foreach (Face addingMap in additionalPool.Keys)
				{
					//merge the roll faces and calculate the frequency
					var roll = startingMap.Merge(addingMap);
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
	}
}
