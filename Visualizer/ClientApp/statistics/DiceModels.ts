// -----------------
// STATE - This defines the type of data maintained in the Redux store.
export interface PoolCombinationState {
	isLoading: boolean;
	negativePoolId: number;
	searchDice: PoolDice[];
	poolCombinationContainer: PoolCombinationContainer;
}

export interface PoolCombinationContainer {
	baseline?: PoolCombination;
	baseDice?: PoolDice[];
}

export interface PoolCombination {
	poolCombinationStatistics: PoolCombinationStatistic[];
	positivePoolDice: PoolDice[];
	negativePoolDice: PoolDice[];
}

export interface PoolCombinationStatistic {
	symbol: DieSymbol;
	quantity: number;
	frequency: number;
	alternateTotal: number;
}

export interface PoolDice {
	dieId: DieType;
	quantity: number;
}

export enum DieSymbol {
	Blank = 0,
	Success = 1,
	Failure = 2,
	Advantage = 3,
	Threat = 4,
	Triumph = 5,
	Despair = 6,
	Light = 7,
	Dark = 8
}

export enum DieType {
	Ability = 1,
	Boost = 2,
	Challenge = 3,
	Difficulty = 4,
	Force = 5,
	Proficiency = 6,
	Setback = 7,
}

