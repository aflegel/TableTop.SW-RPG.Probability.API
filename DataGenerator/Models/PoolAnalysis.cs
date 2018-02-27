using DataFramework.Models;
using System;
using System.Collections.Generic;
using System.Text;
using static DataFramework.Models.Die;

namespace DataGenerator.Models
{
	class PoolAnalysis
	{
		public PoolAnalysis(PoolResult positivePoolResult, PoolResult negativePoolResult)
		{
			Frequency = positivePoolResult.Quantity * negativePoolResult.Quantity;

			//triumphs count as successes but advantages do not and despairs count as failures but threats do not
			SuccessQuantity = positivePoolResult.CountMatchingKeys(Symbol.Success);
			FailureQuantity = negativePoolResult.CountMatchingKeys(Symbol.Failure);

			AdvantageQuantity = positivePoolResult.CountMatchingKeys(Symbol.Advantage);
			ThreatQuantity = negativePoolResult.CountMatchingKeys(Symbol.Threat);

			TriumphQuantity = positivePoolResult.CountMatchingKeys(Symbol.Triumph);
			DespairQuantity = negativePoolResult.CountMatchingKeys(Symbol.Despair);

			SuccessThreshold = SuccessQuantity + TriumphQuantity;
			FailureThreshold = FailureQuantity + DespairQuantity;

			SuccessNetQuantity = SuccessThreshold - FailureThreshold;
			AdvantageNetQuantity = AdvantageQuantity - ThreatQuantity;
			TriumphNetQuantity = TriumphQuantity - Math.Min(SuccessNetQuantity, 0);
			DespairNetQuantity = DespairQuantity - Math.Min(FailureNetQuantity, 0);
		}

		public long Frequency { get; private set; }
		public int SuccessQuantity { get; private set; }
		public int FailureQuantity { get; private set; }
		public int AdvantageQuantity { get; private set; }
		public int ThreatQuantity { get; private set; }
		public int TriumphQuantity { get; private set; }
		public int DespairQuantity { get; private set; }

		public int SuccessThreshold { get; private set; }
		public int FailureThreshold { get; private set; }

		public int SuccessNetQuantity { get; private set; }
		public int TriumphNetQuantity { get; private set; }
		public int DespairNetQuantity { get; private set; }
		public int AdvantageNetQuantity { get; private set; }
		public int ThreatNetQuantity { get { return -AdvantageNetQuantity; } }
		public int FailureNetQuantity { get { return -SuccessNetQuantity; } }


		public bool IsSuccess { get { return SuccessNetQuantity > 0; } }
		public bool IsTriumph { get { return TriumphNetQuantity > 0; } }
		public bool IsDespair { get { return DespairNetQuantity > 0; } }
		public bool IsAdvantage { get { return AdvantageNetQuantity > 0; } }
		public bool IsThreat { get { return ThreatNetQuantity > 0; } }


	}
}
