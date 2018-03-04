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
			{this.RenderSearch()}
			<h2>Distribution of Success and Failure</h2>
			{this.RenderGraphAndData(DiceStatistics.DieSymbol.Success)}
			<hr />
			<h2>Distribution of Advantage and Threat</h2>
			{this.RenderGraphAndData(DiceStatistics.DieSymbol.Advantage)}
			<hr />
			<h2>Distribution of Triumph and Despair</h2>
			{this.RenderGraphAndData(DiceStatistics.DieSymbol.Triumph)}
			<hr />
			{this.RenderResults()}
		</div>;
	}

	/**
	 * Renders the current search icons as well as a search builder
	 */
	private RenderSearch() {
		var output: JSX.Element[] = [];
		if (this.props.poolCombinationContainer.baseDice != null) {
			this.props.poolCombinationContainer.baseDice.forEach(item => output = output.concat(this.RenderDie(item.dieId, item.quantity)));
		}

		return <div className="row">
			<div className="col-md-12">
				<h3>Current Pool: {output}</h3>
				<button className="hidden" onClick={() => { this.AddDie(DiceStatistics.DieType.Ability) }}>Increment</button>
			</div>
		</div>;
	}

	/**
	 * Returns an icon with proper css classes for the die type and size
	 * @param dieType
	 * @param quantity
	 */
	private RenderDie(dieType: DiceStatistics.DieType, quantity: number): JSX.Element[] {
		var dieSize = 0;
		switch (dieType) {
			case DiceStatistics.DieType.Ability:
			case DiceStatistics.DieType.Difficulty:
				dieSize = 8;
				break;
			case DiceStatistics.DieType.Boost:
			case DiceStatistics.DieType.Setback:
				dieSize = 6;
				break;
			case DiceStatistics.DieType.Challenge:
			case DiceStatistics.DieType.Proficiency:
				dieSize = 12;
				break;
		}

		var output: JSX.Element[] = [];

		for (var i = 0; i < quantity; i++) {
			output.push(<i className={"ffi-d" + dieSize + " ffi-swrpg-" + DiceStatistics.DieType[dieType].toString().toLowerCase()}></i>)
		}

		return output;
	}

	/**
	 * Returns an icon element with the appropriate css classes
	 * @param dieSymbol
	 */
	private RenderDieSymbol(dieSymbol: DiceStatistics.DieSymbol) {
		return <i className={"ffi-swrpg-" + DiceStatistics.DieSymbol[dieSymbol].toString().toLowerCase()}></i>;
	}

	private AddDie(dieType: DiceStatistics.DieType) {

		//this.props.dice = this.props.dice.concat(dieType);
	}

	/**
	 * Configures the data for a given symbol and renders a graph and a statistics breakdown panel
	 * @param mode
	 */
	private RenderGraphAndData(mode: DiceStatistics.DieSymbol) {
		if (this.props.poolCombinationContainer != null && this.props.poolCombinationContainer.baseline != null) {

			var counterMode: DiceStatistics.DieSymbol = DiceStatistics.DieSymbol.Failure;
			switch (mode) {
				case DiceStatistics.DieSymbol.Success:
					counterMode = DiceStatistics.DieSymbol.Failure;
					break;
				case DiceStatistics.DieSymbol.Advantage:
					counterMode = DiceStatistics.DieSymbol.Threat;
					break;
				case DiceStatistics.DieSymbol.Triumph:
					counterMode = DiceStatistics.DieSymbol.Despair;
					break;
			}

			//get short list of combinations ordered lowest to highest
			var baseSet = this.props.poolCombinationContainer.baseline.poolCombinationStatistics.filter(f => f.symbol == mode).sort((n1, n2) => n1.quantity - n2.quantity);

			//from short list get quantities
			var xAxis = baseSet.map(map => map.quantity.toString());
			var totalFrequency = baseSet.reduce((total, obj) => { return total + obj.frequency }, 0);
			var percentageSet = baseSet.map(map => this.GetProbability(map.frequency, totalFrequency));

			return <div className="row">
				<div className="col-lg-6 col-md-12">
					{this.RenderGraph(DiceStatistics.DieSymbol[mode], { labels: xAxis, datasets: [this.BuildDataSet(percentageSet, DiceStatistics.DieSymbol[mode], "rgba(99,200,132,1)")] })}
				</div>
				<div className="col-lg-6 col-md-12">
					{this.RenderBreakdown(mode, counterMode, baseSet, totalFrequency)}
				</div>
			</div>;
		}
	}

	/**
	 * Renders a standardized chart.js graph given a dataset.
	 * @param label
	 * @param graphData
	 */
	private RenderGraph(label: string, graphData: any) {
		const options = {
			title: {
				display: true,
				text: "Distribution of " + label,
			},
			legend: {
				display: false
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

	/**
	 * Calculates the probability returned as a number between 0 and 100
	 * @param top
	 * @param bottom
	 */
	private GetProbability(numerator: number, denominator: number): number {
		return numerator / denominator * 100;
	}

	/**
	 * Returns a standardized object for the chart.js utility
	 * @param dataset
	 * @param label
	 * @param color
	 */
	private BuildDataSet(dataset: number[], label: string, color: string) {
		return {
			label: label,
			borderColor: color,
			backgroundColor: 'rgba(0,0,0,0)',
			pointHitRadius: 25,
			data: dataset
		}
	}

	/**
	 * Calculates the statictical model and builds a definition list for that data
	 * @param mode
	 * @param counterMode
	 * @param baseSet
	 * @param totalFrequency
	 */
	private RenderBreakdown(mode: DiceStatistics.DieSymbol, counterMode: DiceStatistics.DieSymbol, baseSet: DiceStatistics.PoolCombinationStatistic[], totalFrequency: number) {

		var positiveSet = baseSet.filter(f => f.quantity > 0);
		//success mode requires 0 quantity outcomes as well
		var negativeSet = baseSet.filter(f => (f.quantity < (mode == DiceStatistics.DieSymbol.Success ? 1 : 0)));

		var positiveFrequency = positiveSet.reduce((total, obj) => { return total + obj.frequency }, 0);
		var negativeFrequency = negativeSet.reduce((total, obj) => { return total + obj.frequency }, 0);

		var average = baseSet.reduce((total, obj) => { return total + (obj.quantity * obj.frequency) }, 0) / totalFrequency;
		//val - avg squared * qty
		var deviationSet = baseSet.map(map => ((map.quantity - average) ** 2) * map.frequency);
		var standardDeviation = Math.sqrt(deviationSet.reduce((total, obj) => { return total + obj }, 0) / totalFrequency);

		var totalLabeling = mode == DiceStatistics.DieSymbol.Success ? "Total Frequency" : "";
		var totalData = mode == DiceStatistics.DieSymbol.Success ? new Intl.NumberFormat('en-Us').format(totalFrequency) : null;


		return <dl>
			<dt>{totalLabeling}</dt>
			<dd>{totalData}</dd>
			<dt>{this.RenderDieSymbol(mode)}{DiceStatistics.DieSymbol[mode]} Frequency</dt>
			<dd>{new Intl.NumberFormat('en-Us').format(positiveFrequency)}</dd>
			<dt>Probability of {DiceStatistics.DieSymbol[mode]}</dt>
			<dd>{new Intl.NumberFormat('en-Us').format(positiveFrequency / totalFrequency * 100)}%</dd>
			<dt>{this.RenderDieSymbol(counterMode)}{DiceStatistics.DieSymbol[counterMode]} Frequency</dt>
			<dd>{new Intl.NumberFormat('en-Us').format(negativeFrequency)}</dd>
			<dt>Probability of {DiceStatistics.DieSymbol[counterMode]}</dt>
			<dd>{new Intl.NumberFormat('en-Us').format(negativeFrequency / totalFrequency * 100)}%</dd>
			<dt>Average {DiceStatistics.DieSymbol[mode]}</dt>
			<dd>{new Intl.NumberFormat('en-Us').format(average)}</dd>
			<dt>Standard Deviation</dt>
			<dd>{new Intl.NumberFormat('en-Us').format(standardDeviation)}</dd>
		</dl>;
	}

	/**
	 * Renders a table with the raw data used for populating the tables and statistics data
	 */
	private RenderResults() {
		if (this.props.poolCombinationContainer != null) {

			var containers: DiceStatistics.PoolCombination[] = [];

			if (this.props.poolCombinationContainer.baseline != null)
				containers = containers.concat(this.props.poolCombinationContainer.baseline);

			return <table className='table'>
				<thead>
					<tr>
						<th>Symbol</th>
						<th>Quantity</th>
						<th>Frequency</th>
					</tr>
				</thead>
				<tbody>
					{containers.map(poolCombination => poolCombination.poolCombinationStatistics.map(combination =>
						<tr>
							<td>{DiceStatistics.DieSymbol[combination.symbol]}</td>
							<td>{combination.quantity}</td>
							<td>{new Intl.NumberFormat('en-Us').format(combination.frequency)}</td>
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
