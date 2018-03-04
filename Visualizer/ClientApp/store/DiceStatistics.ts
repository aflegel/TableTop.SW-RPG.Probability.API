import { fetch, addTask } from 'domain-task';
import { Action, Reducer, ActionCreator } from 'redux';
import { AppThunkAction } from './';

// -----------------
// STATE - This defines the type of data maintained in the Redux store.

export interface PoolCombinationState {
	isLoading: boolean;
	positivePoolId?: number;
	negativePoolId?: number;
	dice: DieType[];
	poolCombinationContainer: PoolCombinationContainer;
}

export interface PoolCombinationContainer {
	baseline?: PoolCombination;
	baseDice?: PoolDice[];
	//boosted?: PoolCombination;
	//setback?: PoolCombination;
	//threatened?: PoolCombination;
	//upgraded?: PoolCombination;
}

export interface PoolCombination {
	positivePoolId: number;
	negativePoolId: number;
	poolCombinationStatistics: PoolCombinationStatistic[];
	positivePoolDice: PoolDice[];
	negativePoolDice: PoolDice[];
}

export interface PoolCombinationStatistic {
	symbol: DieSymbol;
	quantity: number;
	frequency: number;
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

// -----------------
// ACTIONS - These are serializable (hence replayable) descriptions of state transitions.
// They do not themselves have any side-effects; they just describe something that is going to happen.
interface IncrementCountAction { type: 'INCREMENT_COUNT' }
interface DecrementCountAction { type: 'DECREMENT_COUNT' }

interface RequestDiceStatisticsAction {
	type: 'REQUEST_DICE_STATISTICS';
	positivePoolId: number;
	negativePoolId: number;
}

interface ReceiveDiceStatisticsAction {
	type: 'RECEIVE_DICE_STATISTICS';
	positivePoolId: number;
	negativePoolId: number;
	poolCombinationContainer: PoolCombinationContainer;
}

// Declare a 'discriminated union' type. This guarantees that all references to 'type' properties contain one of the
// declared type strings (and not any other arbitrary string).
type KnownAction = RequestDiceStatisticsAction | ReceiveDiceStatisticsAction | IncrementCountAction | DecrementCountAction;

// ----------------
// ACTION CREATORS - These are functions exposed to UI components that will trigger a state transition.
// They don't directly mutate state, but they can have external side-effects (such as loading data).

export const actionCreators = {
	increment: () => <IncrementCountAction>{ type: 'INCREMENT_COUNT' },
	decrement: () => <DecrementCountAction>{ type: 'DECREMENT_COUNT' },
	requestDiceStatistics: (positivePoolId: number, negativePoolId: number): AppThunkAction<KnownAction> => (dispatch, getState) => {

		// Only load data if it's something we don't already have (and are not already loading)
		if (positivePoolId !== getState().diceStatistics.positivePoolId) {
			let fetchTask = fetch(`api/Search/GetStatistics?positivePoolId=${positivePoolId}&negativePoolId=${negativePoolId}`)
				.then(response => response.json() as Promise<PoolCombinationContainer>)
				.then(data => {
					dispatch({ type: 'RECEIVE_DICE_STATISTICS', positivePoolId: positivePoolId, negativePoolId: negativePoolId, poolCombinationContainer: data });
				});

			addTask(fetchTask); // Ensure server-side prerendering waits for this to complete
			dispatch({ type: 'REQUEST_DICE_STATISTICS', positivePoolId: positivePoolId, negativePoolId: negativePoolId });
		}
	}
};

// ----------------
// REDUCER - For a given state and action, returns the new state. To support time travel, this must not mutate the old state.

const unloadedState: PoolCombinationState = { poolCombinationContainer: {}, dice: [], isLoading: false };

export const reducer: Reducer<PoolCombinationState> = (state: PoolCombinationState, incomingAction: Action) => {
	const action = incomingAction as KnownAction;
	switch (action.type) {
		case 'INCREMENT_COUNT':
			return {
				positivePoolId: state.positivePoolId,
				negativePoolId: state.negativePoolId,
				poolCombinationContainer: state.poolCombinationContainer,
				dice: [],
				isLoading: true
			};
		case 'DECREMENT_COUNT':
			return {
				positivePoolId: state.positivePoolId,
				negativePoolId: state.negativePoolId,
				poolCombinationContainer: state.poolCombinationContainer,
				dice: [],
				isLoading: true
			};
		case 'REQUEST_DICE_STATISTICS':
			return {
				positivePoolId: action.positivePoolId,
				negativePoolId: action.negativePoolId,
				poolCombinationContainer: state.poolCombinationContainer,
				dice: [],
				isLoading: true
			};
		case 'RECEIVE_DICE_STATISTICS':
			// Only accept the incoming data if it matches the most recent request. This ensures we correctly
			// handle out-of-order responses.
			if (action.positivePoolId === state.positivePoolId && action.negativePoolId === state.negativePoolId) {
				return {
					positivePoolId: action.positivePoolId,
					negativePoolId: action.negativePoolId,
					poolCombinationContainer: action.poolCombinationContainer,
					dice: [],
					isLoading: false
				};
			}
			break;
		default:
			// The following line guarantees that every action in the KnownAction union has been covered by a case above
			const exhaustiveCheck: never = action;
	}

	return state || unloadedState;
};
