import * as DiceStatistics from './DiceStatistics';

export class StatisticsCore {
	public totalFrequency: number;
	public successFrequency: number;
	public successAverage: number;
	public successSigma: number;
	public advantageFrequency: number;
	public threatFrequency: number;
	public triumphFrequency: number;
	public despairFrequency: number;

	public constructor(poolCombination: DiceStatistics.PoolCombination) {
		this.totalFrequency = this.GetReduced(poolCombination.poolCombinationStatistics);
		this.successFrequency = this.GetReduced(poolCombination.poolCombinationStatistics, 1);
		this.advantageFrequency = this.GetReduced(poolCombination.poolCombinationStatistics, 3);
		this.threatFrequency = this.GetReduced(poolCombination.poolCombinationStatistics, 4);
		this.triumphFrequency = this.GetReduced(poolCombination.poolCombinationStatistics, 5);
		this.despairFrequency = this.GetReduced(poolCombination.poolCombinationStatistics, 6);
	}

	private GetReduced(combinations: DiceStatistics.PoolCombinationStatistic[], searchSymbol?: number) {
		var filters = combinations;

		if (searchSymbol != null) {
			filters = filters.filter(f => (f.symbol == searchSymbol));
		}

		return filters.reduce((total, obj) => { return total + obj.frequency }, 0);
	}

}

export interface FieldStatistic {
	frequency: number;
	percentage: number;
	average: number;
	sigma: number;
}