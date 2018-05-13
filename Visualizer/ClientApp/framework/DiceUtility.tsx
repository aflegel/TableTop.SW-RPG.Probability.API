import * as React from "react";
import {DieSymbol, DieType, PoolDice} from "../statistics/DiceModels";

export default class DiceUtilty {
	/**
	 * Returns an icon element with the appropriate css classes
	 * @param dieSymbol
	 */
	public static RenderDieSymbol(dieSymbol: DieSymbol) {
		return <i className={"ffi ffi-swrpg-" + DieSymbol[dieSymbol].toString().toLowerCase()}></i>;
	}

	/**
	 * Returns an icon element with the appropriate css classes
	 * @param dieSymbol
	 */
	public static RenderDie(dieType: DieType) {
		var dieSize = 0;
		switch (dieType) {
			case DieType.Ability:
			case DieType.Difficulty:
				dieSize = 8;
				break;
			case DieType.Boost:
			case DieType.Setback:
				dieSize = 6;
				break;
			case DieType.Challenge:
			case DieType.Proficiency:
			case DieType.Force:
				dieSize = 12;
				break;
		}

		return <i className={"die-stroke ffi ffi-d" + dieSize + " ffi ffi-swrpg-" + DieType[dieType].toString().toLowerCase() + "-color"}></i>;
	}


	public static RenderDice(dice: PoolDice[]) {
		var output: JSX.Element[] = [];
		if (dice != null) {
			dice.sort((a, b) => {
				switch (a.dieId) {
					case DieType.Proficiency:
					case DieType.Ability:
					case DieType.Boost:
						switch (b.dieId) {
							case DieType.Proficiency:
								return 1;
							case DieType.Ability:
							case DieType.Boost:
								return 0;
							default:
								return -1;
						}
					case DieType.Challenge:
					case DieType.Difficulty:
					case DieType.Setback:
						switch (b.dieId) {
							case DieType.Challenge:
								return 1;
							case DieType.Difficulty:
							case DieType.Setback:
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
	public static RenderDieSet(dieType: DieType, quantity: number): JSX.Element[] {
		var output: JSX.Element[] = [];

		for (var i = 0; i < quantity; i++) {
			output.push(this.RenderDie(dieType));
		}

		return output;
	}
}
