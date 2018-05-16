using DataFramework.Models;
using System;
using System.Collections.Generic;
using System.Text;
using static DataFramework.Models.Die;

namespace DataGenerator.Models
{
	class OutcomeAnalysis
	{
		public OutcomeAnalysis(PoolResult positivePoolResult, PoolResult negativePoolResult)
		{
			Frequency = (ulong)positivePoolResult.Frequency * (ulong)negativePoolResult.Frequency;

			//triumphs count as successes but advantages do not and despairs count as failures but threats do not
			var SuccessQuantity = positivePoolResult.CountMatchingKeys(Symbol.Success);
			var FailureQuantity = negativePoolResult.CountMatchingKeys(Symbol.Failure);

			var AdvantageQuantity = positivePoolResult.CountMatchingKeys(Symbol.Advantage);
			var ThreatQuantity = negativePoolResult.CountMatchingKeys(Symbol.Threat);

			var TriumphQuantity = positivePoolResult.CountMatchingKeys(Symbol.Triumph);
			var DespairQuantity = negativePoolResult.CountMatchingKeys(Symbol.Despair);

			var SuccessThreshold = SuccessQuantity + TriumphQuantity;
			var FailureThreshold = FailureQuantity + DespairQuantity;

			SuccessNetQuantity = SuccessThreshold - FailureThreshold;
			AdvantageNetQuantity = AdvantageQuantity - ThreatQuantity;

			//adjust the triumph quantity by the difference between the success quantity and failure threshold
			//if the difference is greater than 0, take 0 instead
			//if it is not a success, no triumph can take place, adjust the triumph quantity to 0
			var TriumphThreshold = TriumphQuantity + (IsSuccess ? Math.Min(SuccessQuantity - FailureThreshold, 0) : -TriumphQuantity);
			var DespairThreshold = DespairQuantity + (!IsSuccess ? Math.Min(FailureQuantity - SuccessThreshold, 0) : -DespairQuantity);

			if (TriumphThreshold > 0)
				TriumphNetQuantity = TriumphThreshold;
			else if (DespairThreshold > 0)
				TriumphNetQuantity = -DespairThreshold;
			else
				TriumphNetQuantity = 0;
		}

		public ulong Frequency { get; private set; }

		public int SuccessNetQuantity { get; private set; }
		public int TriumphNetQuantity { get; private set; }
		public int AdvantageNetQuantity { get; private set; }
		private int DespairNetQuantity { get; set; }

		private bool IsSuccess { get { return SuccessNetQuantity > 0; } }
	}
}
