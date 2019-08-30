using System.Collections.Generic;
using DataFramework.Models;

namespace DataFramework.Context.Seed
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

		private decimal Frequency { get; set; }

		private int SuccessNetQuantity { get; set; }

		private int TriumphNetQuantity { get; set; }

		private int AdvantageNetQuantity { get; set; }

		private int DespairNetQuantity { get; set; }

		public IEnumerable<PoolCombinationStatistic> ToStatistics() => new List<PoolCombinationStatistic>
		{
			new PoolCombinationStatistic
			{
				Symbol = Symbol.Success,
				Quantity = SuccessNetQuantity,
				Frequency = Frequency,
				AlternateTotal = AdvantageNetQuantity
			},

			//add the net advantage quantity
			new PoolCombinationStatistic
			{
				Symbol = Symbol.Advantage,
				Quantity = AdvantageNetQuantity,
				Frequency = Frequency,
				AlternateTotal = SuccessNetQuantity
			},

			//add the net triumph
			new PoolCombinationStatistic
			{
				Symbol = Symbol.Triumph,
				Quantity = TriumphNetQuantity,
				Frequency = Frequency,
				AlternateTotal = DespairNetQuantity
			},

			//add the net despair
			new PoolCombinationStatistic
			{
				Symbol = Symbol.Despair,
				Quantity = DespairNetQuantity,
				Frequency = Frequency,
				AlternateTotal = TriumphNetQuantity
			}
		};
	}
}
