import * as React from "react";
import * as DiceStatistics from "../store/DiceStatistics";

export default class DiceUtilty {
	/**
	 * Returns an icon element with the appropriate css classes
	 * @param dieSymbol
	 */
	public static RenderDieSymbol(dieSymbol: DiceStatistics.DieSymbol) {
		return <i className={"ffi ffi-swrpg-" + DiceStatistics.DieSymbol[dieSymbol].toString().toLowerCase()}></i>;
	}

	/**
	 * Returns an icon element with the appropriate css classes
	 * @param dieSymbol
	 */
	public static RenderDie(dieType: DiceStatistics.DieType) {
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
			case DiceStatistics.DieType.Force:
				dieSize = 12;
				break;
		}

		return <i className={"die-stroke ffi ffi-d" + dieSize + " ffi ffi-swrpg-" + DiceStatistics.DieType[dieType].toString().toLowerCase() + "-color"}></i>;
	}


	public static RenderDice(dice: DiceStatistics.PoolDice[]) {
		var output: JSX.Element[] = [];
		if (dice != null) {
			dice.sort((a, b) => {
				switch (a.dieId) {
					case DiceStatistics.DieType.Proficiency:
					case DiceStatistics.DieType.Ability:
					case DiceStatistics.DieType.Boost:
						switch (b.dieId) {
							case DiceStatistics.DieType.Proficiency:
								return 1;
							case DiceStatistics.DieType.Ability:
							case DiceStatistics.DieType.Boost:
								return 0;
							default:
								return -1;
						}
					case DiceStatistics.DieType.Challenge:
					case DiceStatistics.DieType.Difficulty:
					case DiceStatistics.DieType.Setback:
						switch (b.dieId) {
							case DiceStatistics.DieType.Challenge:
								return 1;
							case DiceStatistics.DieType.Difficulty:
							case DiceStatistics.DieType.Setback:
								return 0;
							default:
								return -1;
						}
					default:
						return 0;
				}
			}).forEach(item => output = output.concat(this.RenderDieSet(item.dieId, item.quantity)));
		}
		return output;
	}

	/**
	 * Returns an icon with proper css classes for the die type and size
	 * @param dieType
	 * @param quantity
	 */
	public static RenderDieSet(dieType: DiceStatistics.DieType, quantity: number): JSX.Element[] {
		var output: JSX.Element[] = [];

		for (var i = 0; i < quantity; i++) {
			output.push(this.RenderDie(dieType));
		}

		return output;
	}
}
