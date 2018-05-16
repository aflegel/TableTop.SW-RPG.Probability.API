import * as React from "react";
import { DieSymbol, DieType, PoolDice } from "../statistics/DiceModels";
import { Line } from "react-chartjs-2";

export default class ChartUtilty {

	/**
	 * Renders a standardized chart.js graph given a dataset.
	 * @param label
	 * @param graphData
	 */
	public static RenderGraph(label: string, offLabel: string, mode: DieSymbol, graphData: any) {

		var yAxes = [{
			id: "Probability",
			position: "left",
			scaleLabel: {
				display: true,
				labelString: "Probability (%)"
			}
		}
		];

		if (mode == DieSymbol.Advantage || mode == DieSymbol.Success) {
			yAxes = yAxes.concat({
				id: "Average",
				position: "right",
				scaleLabel: {
					display: true,
					labelString: offLabel
				}
			});
		}

		const options = {
			title: {
				display: true,
				text: "Distribution of " + label,
			},
			legend: {
				display: true
			},
			scales: {
				yAxes: yAxes,
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
	 * Returns a standardized object for the chart.js utility
	 * @param dataset
	 * @param label
	 * @param color
	 */
	public static BuildDataSet(dataset: number[], label: string, color: string, yAxisId: string) {
		return {
			label: label,
			yAxisID: yAxisId,
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
}
