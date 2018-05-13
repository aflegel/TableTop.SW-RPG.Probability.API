import * as React from 'react';
import { Link, RouteComponentProps } from 'react-router-dom';
import { connect } from 'react-redux';
import { ApplicationState } from '../statistics';
import * as DiceStatistics from '../statistics/DiceStatistics';
import DiceUtility from '../framework/DiceUtility';

import { Line } from 'react-chartjs-2';
import { Chart } from 'chart.js';
import { DieType, DieSymbol, PoolCombinationState, PoolCombinationStatistic, PoolDice, PoolCombination } from '../statistics/DiceModels';


// At runtime, Redux will merge together...
type DiceStatisticsProps =
	PoolCombinationState        // ... state we've requested from the Redux store
	& typeof DiceStatistics.actionCreators     // ... plus action creators we've requested
	& RouteComponentProps<{ positivePoolId?: number, negativePoolId?: number }>; // ... plus incoming routing parameters

class FetchDiceStatistics extends React.Component<DiceStatisticsProps, {}> {
	componentWillMount() {
		// This method runs when the component is first added to the page
		//let startDateIndex = parseInt(this.props.match.params.startDateIndex) || 0;
		this.props.requestDiceStatistics();
	}

	componentWillReceiveProps(nextProps: DiceStatisticsProps) {
		// This method runs when incoming props (e.g., route params) change
		//let startDateIndex = parseInt(nextProps.match.params.startDateIndex) || 0;

		if (this.props.poolCombinationContainer.baseDice == null) {
			this.props.requestDiceStatistics();
			return;
		} else {
			if (nextProps.searchDice == null)
				return;
			else if (this.props.searchDice.length != nextProps.searchDice.length) {

			}
			//this.props.requestDiceStatistics();
		}
	}

	public render() {
		return <div>
			{this.RenderSearch()}

			<ul className="collection with-header">
				<li className="collection-header">
					{this.RenderPoolData()}
				</li>
				<li className="collection-item">
					{this.RenderGraphAndData(DieSymbol.Success)}
				</li>
				<li className="collection-item">
					{this.RenderGraphAndData(DieSymbol.Advantage)}
				</li>
				<li className="collection-item">
					{this.RenderGraphAndData(DieSymbol.Triumph)}
				</li>
			</ul>


		</div>;
	}

	/**
	 * Renders the current search icons as well as a search builder
	 */
	private RenderSearch() {
		return <div className="row row-fill">
			<div className="col s12">
				<div className="card">
					<div className="card-content">
						<div className="row">
							<div className="col s6">
								{this.RenderDieCount(DieType.Proficiency)}
								{this.RenderDieCount(DieType.Ability)}
								{this.RenderDieCount(DieType.Boost)}
							</div>
							<div className="col s6">
								{this.RenderDieCount(DieType.Challenge)}
								{this.RenderDieCount(DieType.Difficulty)}
								{this.RenderDieCount(DieType.Setback)}
							</div>
						</div>

						<span>
							<button onClick={() => { this.props.requestDiceStatistics(); }} className="btn btn-primary">Search</button>
						</span>
					</div>
				</div>
			</div>
		</div>;
	}

	/**
	 * Renders the current search icons as well as a search builder
	 */
	private RenderPoolData() {
		if (this.props.poolCombinationContainer.baseDice != null)
			return <div className="row row-fill">
				<div className="col s12">
					<h2>Probability Breakdown</h2>

					<h5>{DiceUtility.RenderDice(this.props.poolCombinationContainer.baseDice)}</h5>
				</div>
			</div>;
	}

	private RenderDieCount(dieType: DieType) {
		var count = 0;
		var test = this.props.searchDice.filter(f => f.dieId == dieType);

		if (test.length > 0) {
			count = test[0].quantity
		}

		return <div className="row">
			<div className="col s4 right-align">
				<h5 className="">{DiceUtility.RenderDie(dieType)}</h5>
			</div>
			<div className="col s8">
				<div className="row">
					<div className="col s4">
						<button className="btn light-green darken-3" onClick={() => { this.AddDie(dieType) }}>+</button>
					</div>
					<div className="col s4 center-align">
						{count}
					</div>
					<div className="col s4">
						<button className="btn light-green darken-3" onClick={() => { this.DeleteDie(dieType) }}>-</button>
					</div>
				</div>
			</div>
		</div>;
	}


	private DeleteDie(dieType: DieType) {
		this.props.removeSearchDie({ dieId: dieType, quantity: 1 });
	}

	private AddDie(dieType: DieType) {
		var poolDie: PoolDice = { dieId: dieType, quantity: 1 };

		this.props.addSearchDie(poolDie);
	}

	/**
	 * Configures the data for a given symbol and renders a graph and a statistics breakdown panel
	 * @param mode
	 */
	private RenderGraphAndData(mode: DieSymbol) {
		if (this.props.poolCombinationContainer != null && this.props.poolCombinationContainer.baseline != null) {

			var counterMode: DieSymbol = DieSymbol.Failure;
			switch (mode) {
				case DieSymbol.Success:
					counterMode = DieSymbol.Failure;
					break;
				case DieSymbol.Advantage:
					counterMode = DieSymbol.Threat;
					break;
				case DieSymbol.Triumph:
					counterMode = DieSymbol.Despair;
					break;
			}

			//get short list of combinations ordered lowest to highest
			var baseSet = this.props.poolCombinationContainer.baseline.poolCombinationStatistics.filter(f => f.symbol == mode).sort((n1, n2) => n1.quantity - n2.quantity);

			//from short list get quantities
			var xAxis = baseSet.map(map => map.quantity.toString());
			var totalFrequency = baseSet.reduce((total, obj) => { return total + obj.frequency }, 0);
			var percentageSet = baseSet.map(map => this.GetProbability(map.frequency, totalFrequency));

			return <div className="row row-fill">
				<div className="col s12">
					<h3>Distribution of {DieSymbol[mode]} and {DieSymbol[counterMode]}</h3>

					<div className="row">
						<div className="col s6">
							{this.RenderGraph(DieSymbol[mode], { labels: xAxis, datasets: [this.BuildDataSet(percentageSet, DieSymbol[mode], "#b71c1c")] })}
						</div>
						<div className="col s3">
							{this.RenderBreakdown(mode, counterMode, baseSet, totalFrequency)}
						</div>
						<div className="col s3">
							{this.RenderAdditionalDetails(mode)}
						</div>
					</div>
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
			pointBackgroundColor: color,
			borderColor: color,
			pointHoverBackgroundColor: color,
			fill: false,
			pointRadius: 5,
			pointHitRadius: 10,
			pointHoverRadius: 10,
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
	private RenderBreakdown(mode: DieSymbol, counterMode: DieSymbol, baseSet: PoolCombinationStatistic[], totalFrequency: number) {

		var positiveSet = baseSet.filter(f => f.quantity > 0);
		//success mode requires 0 quantity outcomes as well
		var negativeSet = baseSet.filter(f => (f.quantity < (mode == DieSymbol.Success ? 1 : 0)));

		var positiveFrequency = positiveSet.reduce((total, obj) => { return total + obj.frequency }, 0);
		var negativeFrequency = negativeSet.reduce((total, obj) => { return total + obj.frequency }, 0);

		var average = baseSet.reduce((total, obj) => { return total + (obj.quantity * obj.frequency) }, 0) / totalFrequency;
		//val - avg squared * qty
		var deviationSet = baseSet.map(map => ((map.quantity - average) ** 2) * map.frequency);
		var standardDeviation = Math.sqrt(deviationSet.reduce((total, obj) => { return total + obj }, 0) / totalFrequency);

		var totalLabeling = mode == DieSymbol.Success ? "Total Frequency" : "";
		var totalData = mode == DieSymbol.Success ? new Intl.NumberFormat('en-Us').format(totalFrequency) : null;


		return <dl>
			<dt>{totalLabeling}</dt>
			<dd>{totalData}</dd>
			<dt>{DieSymbol[mode]} Frequency</dt>
			<dd>{new Intl.NumberFormat('en-Us').format(positiveFrequency)}</dd>
			<dt>Probability of {DieSymbol[mode]}</dt>
			<dd>{new Intl.NumberFormat('en-Us', { minimumFractionDigits: 4 }).format(positiveFrequency / totalFrequency * 100)}%</dd>
			<dt>{DieSymbol[counterMode]} Frequency</dt>
			<dd>{new Intl.NumberFormat('en-Us').format(negativeFrequency)}</dd>
			<dt>Probability of {DieSymbol[counterMode]}</dt>
			<dd>{new Intl.NumberFormat('en-Us', { minimumFractionDigits: 4 }).format(negativeFrequency / totalFrequency * 100)}%</dd>
			<dt>Average {DieSymbol[mode]}</dt>
			<dd>{new Intl.NumberFormat('en-Us', { minimumFractionDigits: 4 }).format(average)}</dd>
			<dt>Standard Deviation</dt>
			<dd>{new Intl.NumberFormat('en-Us', { minimumFractionDigits: 4 }).format(standardDeviation)}</dd>
		</dl>;
	}

	/**
	 *
	 * @param mode
	 */
	private RenderAdditionalDetails(mode: DieSymbol) {

		switch (mode) {
			case DieSymbol.Success:
				return <dl>
					<dt>Success Symbols</dt>
					<dd><i className="ffi ffi-swrpg-success"></i> and <i className="ffi ffi-swrpg-triumph"></i></dd>
					<dt>Failure Symbols</dt>
					<dd><i className="ffi ffi-swrpg-failure"></i> and <i className="ffi ffi-swrpg-despair"></i></dd>
					<dt>Calculation</dt>
					<dd>(<i className="ffi ffi-swrpg-success"></i> + <i className="ffi ffi-swrpg-triumph"></i>) - (<i className="ffi ffi-swrpg-failure"></i> + <i className="ffi ffi-swrpg-despair"></i>)</dd>
				</dl>;
			case DieSymbol.Advantage:
				return <dl>
					<dt>Advantage Symbol</dt>
					<dd><i className="ffi ffi-swrpg-advantage"></i></dd>
					<dt>Threat Symbol</dt>
					<dd><i className="ffi ffi-swrpg-threat"></i></dd>
					<dt>Calculation</dt>
					<dd><i className="ffi ffi-swrpg-advantage"></i> - <i className="ffi ffi-swrpg-threat"></i></dd>
				</dl>;
			case DieSymbol.Triumph:
				return <dl>
					<dt>Triumph Symbol</dt>
					<dd><i className="ffi ffi-swrpg-triumph"></i></dd>
					<dt>Despair Symbol</dt>
					<dd><i className="ffi ffi-swrpg-despair"></i></dd>
					<dt>Calculation</dt>
					<dd>It's rather complicated</dd>
				</dl>;
			default:
				return <span></span>;
		}
		//			<p><i className="ffi ffi-swrpg-triumph"></i> is countered by both <i className="ffi ffi-swrpg-failure"></i> and <i className="ffi ffi-swrpg-despair"></i>. As a result a triumph can only occur on a success and net count of <i className="ffi ffi-swrpg-triumph"></i> is adjusted by the uncountered <i className="ffi ffi-swrpg-triumph"></i></p>
	}
	/**
	 * Renders a table with the raw data used for populating the tables and statistics data
	 */
	private RenderResults() {
		if (this.props.poolCombinationContainer != null) {

			var containers: PoolCombination[] = [];

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
							<td>{DieSymbol[combination.symbol]}</td>
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
	DiceStatistics.actionCreators					  // Selects which action creators are merged into the component's props
)(FetchDiceStatistics) as typeof FetchDiceStatistics;
