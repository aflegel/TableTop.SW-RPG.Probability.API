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

			var TriumphThreshold = TriumphQuantity - Math.Min(IsSuccess ? SuccessNetQuantity : TriumphQuantity, 0);
			var DespairThreshold = DespairQuantity - Math.Min(!IsSuccess ? -SuccessNetQuantity : DespairQuantity, 0);

			if (TriumphThreshold > 0)
				TriumphNetQuantity = TriumphThreshold;
			else if (DespairThreshold > 0)
				TriumphNetQuantity = -DespairThreshold;
			else
				TriumphNetQuantity = 0;
		}

		public long Frequency { get; private set; }

		public int SuccessNetQuantity { get; private set; }
		public int TriumphNetQuantity { get; private set; }
		public int AdvantageNetQuantity { get; private set; }
		private int DespairNetQuantity { get; set; }

		private bool IsSuccess { get { return SuccessNetQuantity > 0; } }
		private bool IsTriumph { get { return TriumphNetQuantity > 0; } }
		private bool IsDespair { get { return DespairNetQuantity > 0; } }
	}
}
