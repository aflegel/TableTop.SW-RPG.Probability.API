import * as React from 'react';
import { Link, RouteComponentProps } from 'react-router-dom';
import { connect } from 'react-redux';
import { ApplicationState } from '../store';
import * as DiceStatisticsState from '../store/DiceStatistics';

import { Line } from 'react-chartjs-2';


// At runtime, Redux will merge together...
type DiceStatisticsProps =
	DiceStatisticsState.PoolCombinationState        // ... state we've requested from the Redux store
	& typeof DiceStatisticsState.actionCreators     // ... plus action creators we've requested
	& RouteComponentProps<{ positivePoolId?: number, negativePoolId?: number }>; // ... plus incoming routing parameters

class FetchData extends React.Component<DiceStatisticsProps, {}> {
	componentWillMount() {
		// This method runs when the component is first added to the page
		//let startDateIndex = parseInt(this.props.match.params.startDateIndex) || 0;
		this.props.requestDiceStatistics(0, 0);
	}

	componentWillReceiveProps(nextProps: DiceStatisticsProps) {
		// This method runs when incoming props (e.g., route params) change
		//let startDateIndex = parseInt(nextProps.match.params.startDateIndex) || 0;
		this.props.requestDiceStatistics(0, 0);
	}

	public render() {
		return <div>
			<h1>Weather forecast</h1>
			<p>This component demonstrates fetching data from the server and working with URL parameters.</p>
			{this.renderResults()}
			{this.renderTable()}
			{this.renderTable2()}
			{this.renderPagination()}
		</div>;
	}

	private renderResults() {
		return <table className='table'>
			<thead>
				<tr>
					<th>Symbol</th>
					<th>Quantity</th>
					<th>Frequency</th>
				</tr>
			</thead>
			<tbody>
				{this.props.poolCombinations.map(combination =>

					combination.poolCombinationStatistics.map(stat =>
						<tr>
							<td>{stat.symbol}</td>
							<td>{stat.quantity}</td>
							<td>{stat.frequency}</td>
						</tr>
					)
				)}
			</tbody>
		</table>;
	}

	private renderTable() {

		return <Line data={this.TransmuteData()} />;
	}

	private renderTable2() {

		return <Line data={this.TransmuteData2()} />;
	}

	private TransmuteData() {
		var datasets = {
			labels: ['1', '2', '3', '4', '5', '6', '7', '8', 'Failures', 'Advantages'],
			datasets: [{
				label: 'Successes',
				data: ['', 17, 5]
			},
			{
				label: 'Failures',
				data: [27, 11, 4]
			}]
		};

		return datasets;
	}

	private TransmuteData2() {
		var datasets = {
			labels: ['-2', '-1', '0', '+1', '2',],
			datasets: [{
				label: '+/- Successes',
				data: [4, 11, 27, 17, 5],
				color: 'ff0000'
			}]
		};

		return datasets;
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
