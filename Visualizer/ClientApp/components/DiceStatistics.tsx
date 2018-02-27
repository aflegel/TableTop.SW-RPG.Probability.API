import * as React from 'react';
import { Link, RouteComponentProps } from 'react-router-dom';
import { connect } from 'react-redux';
import { ApplicationState } from '../store';
import * as DiceStatisticsState from '../store/DiceStatistics';

// At runtime, Redux will merge together...
type DiceStatisticsProps =
	DiceStatisticsState.PoolCombinationState        // ... state we've requested from the Redux store
	& typeof DiceStatisticsState.actionCreators      // ... plus action creators we've requested
	& RouteComponentProps<{ startDateIndex: string }>; // ... plus incoming routing parameters

class FetchData extends React.Component<DiceStatisticsProps, {}> {
	componentWillMount() {
		// This method runs when the component is first added to the page
		let startDateIndex = parseInt(this.props.match.params.startDateIndex) || 0;
		this.props.requestDiceStatistics(0, 0);
	}

	componentWillReceiveProps(nextProps: DiceStatisticsProps) {
		// This method runs when incoming props (e.g., route params) change
		let startDateIndex = parseInt(nextProps.match.params.startDateIndex) || 0;
		this.props.requestDiceStatistics(0, 0);
	}

	public render() {
		return <div>
			<h1>Weather forecast</h1>
			<p>This component demonstrates fetching data from the server and working with URL parameters.</p>
			{this.renderForecastsTable()}
			{this.renderPagination()}
		</div>;
	}

	private renderForecastsTable() {
		return <table className='table'>
			<thead>
				<tr>
					<th>Date</th>
					<th>Temp. (C)</th>
					<th>Temp. (F)</th>
					<th>Summary</th>
				</tr>
			</thead>
			<tbody>
				{this.props.poolCombinations.map(combination =>
					<tr key={combination.positivePoolId}>
						<td>{combination.negativePoolId}</td>
					</tr>
				)}
			</tbody>
		</table>;
	}

	private renderPagination() {
		let prevStartDateIndex = (this.props.positivePoolId || 0) - 5;
		let nextStartDateIndex = (this.props.negativePoolId || 0) + 5;

		return <p className='clearfix text-center'>
			<Link className='btn btn-default pull-left' to={`/fetchdata/${prevStartDateIndex}`}>Previous</Link>
			<Link className='btn btn-default pull-right' to={`/fetchdata/${nextStartDateIndex}`}>Next</Link>
			{this.props.isLoading ? <span>Loading...</span> : []}
		</p>;
	}
}

export default connect(
	(state: ApplicationState) => state.diceStatistics, // Selects which state properties are merged into the component's props
	DiceStatisticsState.actionCreators                 // Selects which action creators are merged into the component's props
)(FetchData) as typeof FetchData;
