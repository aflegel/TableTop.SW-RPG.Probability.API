using DataFramework.Models;
using System;
using System.Collections.Generic;
using System.Text;
using static DataFramework.Models.Die;

namespace DataGenerator.Models
{
	internal class OutcomeAnalysis
	{
		public OutcomeAnalysis(PoolResult positivePoolResult, PoolResult negativePoolResult)
		{
			Frequency = positivePoolResult.Frequency * negativePoolResult.Frequency;

			//triumphs count as successes but advantages do not and despairs count as failures but threats do not
			var successQuantity = positivePoolResult.CountMatchingKeys(Symbol.Success);
			var failureQuantity = negativePoolResult.CountMatchingKeys(Symbol.Failure);

			var advantageQuantity = positivePoolResult.CountMatchingKeys(Symbol.Advantage);
			var threatQuantity = negativePoolResult.CountMatchingKeys(Symbol.Threat);

			TriumphNetQuantity = positivePoolResult.CountMatchingKeys(Symbol.Triumph);
			DespairNetQuantity = negativePoolResult.CountMatchingKeys(Symbol.Despair);

			SuccessNetQuantity = successQuantity + TriumphNetQuantity - (failureQuantity + DespairNetQuantity);
			AdvantageNetQuantity = advantageQuantity - threatQuantity;
		}

		public decimal Frequency { get; private set; }

		public int SuccessNetQuantity { get; private set; }

		public int TriumphNetQuantity { get; private set; }

		public int AdvantageNetQuantity { get; private set; }

		public int DespairNetQuantity { get; private set; }
	}
}
