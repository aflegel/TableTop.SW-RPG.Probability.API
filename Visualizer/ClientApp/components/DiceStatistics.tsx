import * as React from 'react';
import { Link, RouteComponentProps } from 'react-router-dom';
import { connect } from 'react-redux';
import { ApplicationState } from '../store';
import * as DiceStatisticsState from '../store/DiceStatistics';

import { Line } from 'react-chartjs-2';
import { Chart } from 'chart.js';


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
			<h1>Probability Breakdown</h1>
			<div className="row">{this.renderSummary()}</div>
			<div className="row">
				<div className="col-md-6">
					{this.renderSuccessGraph()}
				</div>
				<div className="col-md-6">
					{this.renderAdvantageGraph()}
				</div>
			</div>
			<div className="row">
				<div className="col-md-6">
					{this.renderTriumphGraph()}
				</div>
				<div className="col-md-6">
					{this.renderResults()}
				</div>
			</div>
		</div>;
	}

	private renderSummary() {
		if (this.props.poolCombinationContainer != null && this.props.poolCombinationContainer.baseline != null) {
			var totalOutcomes = this.props.poolCombinationContainer.baseline.poolCombinationStatistics.filter(f => (f.symbol == 1 || f.symbol == 2)).reduce((total, obj) => { return total + obj.frequency }, 0);
			var successOutcomes = this.props.poolCombinationContainer.baseline.poolCombinationStatistics.filter(f => (f.symbol == 1)).reduce((total, obj) => { return total + obj.frequency }, 0);
			var advantageOutcomes = this.props.poolCombinationContainer.baseline.poolCombinationStatistics.filter(f => (f.symbol == 3)).reduce((total, obj) => { return total + obj.frequency }, 0);
			var threatOutcomes = this.props.poolCombinationContainer.baseline.poolCombinationStatistics.filter(f => (f.symbol == 4)).reduce((total, obj) => { return total + obj.frequency }, 0);
			var triumphOutcomes = this.props.poolCombinationContainer.baseline.poolCombinationStatistics.filter(f => (f.symbol == 5)).reduce((total, obj) => { return total + obj.frequency }, 0);
			var despairOutcomes = this.props.poolCombinationContainer.baseline.poolCombinationStatistics.filter(f => (f.symbol == 6)).reduce((total, obj) => { return total + obj.frequency }, 0);

			return <div>
				<div className="col-md-3">
					<dl>
						<dt>Total Outcomes</dt>
						<dd>{totalOutcomes}</dd>
						<dt>Success Outcomes</dt>
						<dd>{successOutcomes}</dd>
						<dt>Success Rate</dt>
						<dd>{successOutcomes / totalOutcomes * 100}% +/- sigma</dd>
					</dl>
				</div>
				<div className="col-md-3">
					<dl>
						<dt>Advantage Outcomes</dt>
						<dd>{advantageOutcomes}</dd>
						<dt>Advantage Rate</dt>
						<dd>{advantageOutcomes / totalOutcomes * 100}% +/- sigma</dd>
						<dt>Threat Outcomes</dt>
						<dd>{threatOutcomes}</dd>
						<dt>Threat Rate</dt>
						<dd>{threatOutcomes / totalOutcomes * 100}% +/- sigma</dd>
					</dl>
				</div>
				<div className="col-md-3">
					<dl>
						<dt>Triumph Outcomes</dt>
						<dd>{triumphOutcomes}</dd>
						<dt>Triumph Rate</dt>
						<dd>{triumphOutcomes / totalOutcomes * 100}% +/- sigma</dd>
						<dt>Despair Outcomes</dt>
						<dd>{despairOutcomes}</dd>
						<dt>Despair Rate</dt>
						<dd>{despairOutcomes / totalOutcomes * 100}% +/- sigma</dd>
					</dl>
				</div>
			</div>
				;
		}
		else
			return <p></p>;
	}

	private renderResults() {
		if (this.props.poolCombinationContainer != null && this.props.poolCombinationContainer.baseline != null) {
			var baseline = this.props.poolCombinationContainer.baseline;

			return <table className='table'>
				<thead>
					<tr>
						<th>Key</th>
						<th>Symbol</th>
						<th>Quantity</th>
						<th>Frequency</th>
					</tr>
				</thead>
				<tbody>
					{baseline.poolCombinationStatistics.map(combination =>
						<tr>
							<td>{baseline.positivePoolId},{baseline.negativePoolId}</td>
							<td>{combination.symbol}</td>
							<td>{combination.quantity}</td>
							<td>{combination.frequency}</td>
						</tr>
					)}
				</tbody>
			</table>;
		}
		else {

		}
	}



	private renderSuccessGraph() {
		const options = {
			title: {
				display: true,
				text: "Distribution of Success",
			},
			scales: {
				yAxes: [{
					scaleLabel: {
						display: true,
						labelString: 'Probability (%)'
					}
				}],
				xAxes: [{
					scaleLabel: {
						display: true,
						labelString: 'Net Success'
					}
				}]
			}
		}

		return <Line data={this.TransmuteSuccesses()} options={options} />;
	}

	private renderAdvantageGraph() {
		return <Line data={this.TransmuteAdvantages()} />;
	}

	private renderTriumphGraph() {
		return <Line data={this.TransmuteTriunmphs()} />;
	}

	private GetProbability(top: number, bottom: number): number {
		return top / bottom * 100;
	}

	private TransmuteSuccesses() {
		if (this.props.poolCombinationContainer != null && this.props.poolCombinationContainer.baseline != null && this.props.poolCombinationContainer.boosted != null && this.props.poolCombinationContainer.upgraded != null) {
			var totalOutcomes = this.props.poolCombinationContainer.baseline.poolCombinationStatistics.filter(f => (f.symbol == 1 || f.symbol == 2)).reduce((total, obj) => { return total + obj.frequency }, 0);
			var totalBoostOutcomes = this.props.poolCombinationContainer.boosted.poolCombinationStatistics.filter(f => (f.symbol == 1 || f.symbol == 2)).reduce((total, obj) => { return total + obj.frequency }, 0);
			var totalProficiencyOutcomes = this.props.poolCombinationContainer.upgraded.poolCombinationStatistics.filter(f => (f.symbol == 1 || f.symbol == 2)).reduce((total, obj) => { return total + obj.frequency }, 0);

			var datasets = {
				labels: ['-2', '-1', '0', '+1', '2', '3'],
				datasets: [{
					label: 'Success',
					borderColor: 'rgba(99,200,132,1)',
					backgroundColor: 'rgba(99,200,132,00)',
					data: [this.GetProbability(4, totalOutcomes), this.GetProbability(11, totalOutcomes), this.GetProbability(27, totalOutcomes), this.GetProbability(17, totalOutcomes), this.GetProbability(5, totalOutcomes)],
				},
				{
					label: "Success Upgraded",
					borderColor: 'rgba(200,99,132,1)',
					backgroundColor: 'rgba(99,200,132,00)',
					data: [this.GetProbability(4, totalProficiencyOutcomes), this.GetProbability(14, totalProficiencyOutcomes), this.GetProbability(34, totalProficiencyOutcomes), this.GetProbability(34, totalProficiencyOutcomes), this.GetProbability(10, totalProficiencyOutcomes)],
				},
				{
					label: "Success Boosted",
					borderColor: 'rgba(99,132,200,1)',
					backgroundColor: 'rgba(99,200,132,00)',
					data: [this.GetProbability(16, totalBoostOutcomes), this.GetProbability(52, totalBoostOutcomes), this.GetProbability(130, totalBoostOutcomes), this.GetProbability(122, totalBoostOutcomes), this.GetProbability(54, totalBoostOutcomes), this.GetProbability(10, totalBoostOutcomes)],
				}]
			};

			return datasets;
		}
		else {
			var emptyset = {
				labels: ['0'],
				datasets: [{
					label: '+/- Success',
					data: []
				},
				{
					label: "+/- Success Upgraded",
					data: []
				},
				{
					label: "+/- Success Boosted",
					data: []
				}]
			};

			return emptyset;
		}
	}

	private TransmuteAdvantages() {
		var datasets = {
			labels: ['-2', '-1', '0', '+1', '2',],
			datasets: [{
				label: '+/- Advantages',
				data: [4, 19, 25, 13, 3],
			},
			{
				label: "Threats",
				borderColor: 'rgba(255,99,132,1)',
				data: [4, 19, 0, 0, 0]
			},
			{
				label: "Advantages",
				borderColor: 'rgba(99,255,132,1)',
				data: [0, 0, 0, 13, 3]
			}]
		};

		return datasets;
	}

	private TransmuteTriunmphs() {
		var datasets = {
			labels: ['0', '+1',],
			datasets: [{
				label: '+/- Triumph',
				data: [0, 0],
			},
			{
				label: "+/- Triumph Upgraded",
				borderColor: 'rgba(255,99,132,1)',
				data: [59, 5]
			},
			{
				label: "+/- Triumph Boosted",
				borderColor: 'rgba(99,255,132,1)',
				data: [0, 0,]
			}]
		};

		return datasets;
	}

}

export default connect(
	(state: ApplicationState) => state.diceStatistics, // Selects which state properties are merged into the component's props
	DiceStatisticsState.actionCreators                 // Selects which action creators are merged into the component's props
)(FetchData) as typeof FetchData;
