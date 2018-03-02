import { fetch, addTask } from 'domain-task';
import { Action, Reducer, ActionCreator } from 'redux';
import { AppThunkAction } from './';

// -----------------
// STATE - This defines the type of data maintained in the Redux store.

export interface PoolCombinationState {
	isLoading: boolean;
	positivePoolId?: number;
	negativePoolId?: number;
	poolCombinationContainer: PoolCombinationContainer;
}

export interface PoolCombinationContainer {
	baseline?: PoolCombination;
	boosted?: PoolCombination;
	setback?: PoolCombination;
	threatened?: PoolCombination;
	upgraded?: PoolCombination;
}

export interface PoolCombination {
	positivePoolId: number;
	negativePoolId: number;
	poolCombinationStatistics: PoolCombinationStatistic[];
}

export interface PoolCombinationStatistic {
	symbol: DieSymbol;
	quantity: number;
	frequency: number;
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

// -----------------
// ACTIONS - These are serializable (hence replayable) descriptions of state transitions.
// They do not themselves have any side-effects; they just describe something that is going to happen.

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
type KnownAction = RequestDiceStatisticsAction | ReceiveDiceStatisticsAction;

// ----------------
// ACTION CREATORS - These are functions exposed to UI components that will trigger a state transition.
// They don't directly mutate state, but they can have external side-effects (such as loading data).

export const actionCreators = {
	requestDiceStatistics: (positivePoolId: number, negativePoolId: number): AppThunkAction<KnownAction> => (dispatch, getState) => {

		var test = getState();
		// Only load data if it's something we don't already have (and are not already loading)
		if (positivePoolId !== test.diceStatistics.positivePoolId) {
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

const unloadedState: PoolCombinationState = {
	poolCombinationContainer: {}, isLoading: false };

export const reducer: Reducer<PoolCombinationState> = (state: PoolCombinationState, incomingAction: Action) => {
	const action = incomingAction as KnownAction;
	switch (action.type) {
		case 'REQUEST_DICE_STATISTICS':
			return {
				positivePoolId: action.positivePoolId,
				negativePoolId: action.negativePoolId,
				poolCombinationContainer: state.poolCombinationContainer,
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
