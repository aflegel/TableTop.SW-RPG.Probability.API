using System.Collections.Generic;
using DataFramework.Models;

namespace DataFramework.Context.Seed
{
	internal class OutcomeAnalysis
	{
		public OutcomeAnalysis(PoolResult positivePoolResult, PoolResult negativePoolResult)
		{
			Frequency = positivePoolResult.Frequency * negativePoolResult.Frequency;

			TriumphNetQuantity = positivePoolResult.CountMatchingKeys(Symbol.Triumph);
			DespairNetQuantity = negativePoolResult.CountMatchingKeys(Symbol.Despair);

			//triumphs count as successes but advantages do not and despairs count as failures but threats do not
			SuccessNetQuantity = positivePoolResult.CountMatchingKeys(Symbol.Success) + TriumphNetQuantity - (negativePoolResult.CountMatchingKeys(Symbol.Failure) + DespairNetQuantity);
			AdvantageNetQuantity = positivePoolResult.CountMatchingKeys(Symbol.Advantage) - negativePoolResult.CountMatchingKeys(Symbol.Threat);
		}

		private decimal Frequency { get; set; }

		private int SuccessNetQuantity { get; set; }

		private int TriumphNetQuantity { get; set; }

		private int AdvantageNetQuantity { get; set; }

		private int DespairNetQuantity { get; set; }

		/// <summary>
		/// Creates a list of 4 statistics: Success, Advantage, Triumph, and Despair
		/// </summary>
		/// <returns></returns>
		public IEnumerable<PoolCombinationStatistic> ToStatistics() => new List<PoolCombinationStatistic>
		{
			new PoolCombinationStatistic
			{
				Symbol = Symbol.Success,
				Quantity = SuccessNetQuantity,
				Frequency = Frequency,
				AlternateTotal = AdvantageNetQuantity
			},
			new PoolCombinationStatistic
			{
				Symbol = Symbol.Advantage,
				Quantity = AdvantageNetQuantity,
				Frequency = Frequency,
				AlternateTotal = SuccessNetQuantity
			},
			new PoolCombinationStatistic
			{
				Symbol = Symbol.Triumph,
				Quantity = TriumphNetQuantity,
				Frequency = Frequency,
				AlternateTotal = DespairNetQuantity
			},
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
