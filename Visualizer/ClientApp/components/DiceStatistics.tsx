import * as React from 'react';
import { Link, RouteComponentProps } from 'react-router-dom';
import { connect } from 'react-redux';
import { ApplicationState } from '../store';
import * as DiceStatistics from '../store/DiceStatistics';

import { Line } from 'react-chartjs-2';
import { Chart } from 'chart.js';


// At runtime, Redux will merge together...
type DiceStatisticsProps =
	DiceStatistics.PoolCombinationState        // ... state we've requested from the Redux store
	& typeof DiceStatistics.actionCreators     // ... plus action creators we've requested
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
			{this.RenderGraphAndData(1)}
			<hr />
			{this.RenderGraphAndData(3)}
			<hr />
			{this.RenderGraphAndData(5)}
			<hr />
			{this.renderResults()}
		</div>;
	}

	private RenderGraphAndData(mode: number) {
		if (this.props.poolCombinationContainer != null && this.props.poolCombinationContainer.baseline != null) {
			var positiveLabeling = "";
			var negativeLabeling = "";

			switch (mode) {
				case 1:
					positiveLabeling = "Success";
					negativeLabeling = "Failure";
					break;
				case 3:
					positiveLabeling = "Advantage";
					negativeLabeling = "Threat";
					break;
				case 5:
					positiveLabeling = "Triumph";
					negativeLabeling = "Despair";
					break;
			}

			//get short list of combinations ordered lowest to highest
			var baseSet = this.props.poolCombinationContainer.baseline.poolCombinationStatistics.filter(f => f.symbol == mode).sort((n1, n2) => n1.quantity - n2.quantity);

			var positiveSet = baseSet.filter(f => f.quantity > 0);
			//success mode requires 0 quantity outcomes as well
			var negativeSet = baseSet.filter(f => (f.quantity < (mode == 1 ? 1 : 0)));

			//statistics data
			var totalFrequency = baseSet.reduce((total, obj) => { return total + obj.frequency }, 0);

			//from short list get quantities
			var xAxis = baseSet.map(map => map.quantity.toString());
			var percentageSet = baseSet.map(map => this.GetProbability(map.frequency, totalFrequency))

			return <div className="row">
				<div className="col-md-6">
					{this.renderGraph(positiveLabeling, { labels: xAxis, datasets: [this.BuildDataSet(percentageSet, positiveLabeling, "rgba(99,200,132,1)")] })}
				</div>
				<div className="col-md-6">
					<div className="row">
						<div className="col-xl-12 col-sm-6">
							{this.RenderBreakdown(positiveLabeling, positiveSet, totalFrequency)}
						</div>
						<div className="col-xl-12 col-sm-6">
							{this.RenderBreakdown(negativeLabeling, negativeSet, totalFrequency)}
						</div>
					</div>
				</div>
			</div>;
		}
	}

	private renderGraph(label: string, graphData: any) {
		const options = {
			title: {
				display: true,
				text: "Distribution of " + label,
			},
			scales: {
				yAxes: [{
					scaleLabel: {
						display: true,
						labelString: "Probability (%)"
					}
				}],
				xAxes: [{
					scaleLabel: {
						display: true,
						labelString: "Net " + label
					}
				}]
			}
		}

		return <Line data={graphData} options={options} />;
	}

	private GetProbability(top: number, bottom: number): number {
		return top / bottom * 100;
	}

	private BuildDataSet(dataset: number[], label: string, color: string) {
		return {
			label: label,
			borderColor: color,
			backgroundColor: 'rgba(0,0,0,0)',
			pointHitRadius: 25,
			data: dataset
		}
	}

	private RenderBreakdown(labeling: string, results: DiceStatistics.PoolCombinationStatistic[], totalFrequency: number) {

		var resultFrequency = results.reduce((total, obj) => { return total + obj.frequency }, 0);



		return <dl>
			<dt>{labeling} Outcomes</dt>
			<dd>{resultFrequency}</dd>
			<dt>{labeling} Rate</dt>
			<dd>{resultFrequency / totalFrequency * 100}%</dd>
			<dt>{labeling} Average</dt>
			<dd>~~%  ~~sigma</dd>
		</dl>;
	}

	private renderResults() {
		if (this.props.poolCombinationContainer != null) {

			var containers: DiceStatistics.PoolCombination[] = [];

			if (this.props.poolCombinationContainer.baseline != null)
				containers = containers.concat(this.props.poolCombinationContainer.baseline);

			if (this.props.poolCombinationContainer.boosted != null)
				containers = containers.concat(this.props.poolCombinationContainer.boosted);

			if (this.props.poolCombinationContainer.upgraded != null)
				containers = containers.concat(this.props.poolCombinationContainer.upgraded);


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
					{containers.map(poolCombination => poolCombination.poolCombinationStatistics.map(combination =>
						<tr>
							<td>{poolCombination.positivePoolId},{poolCombination.negativePoolId}</td>
							<td>{combination.symbol}</td>
							<td>{combination.quantity}</td>
							<td>{combination.frequency}</td>
						</tr>
					))}
				</tbody>
			</table>;
		}
		else {

		}
	}

}

export default connect(
	(state: ApplicationState) => state.diceStatistics, // Selects which state properties are merged into the component's props
	DiceStatistics.actionCreators                 // Selects which action creators are merged into the component's props
)(FetchData) as typeof FetchData;
