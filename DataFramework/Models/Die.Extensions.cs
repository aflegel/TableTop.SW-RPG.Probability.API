using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace DataFramework.Models
{
	public enum DieNames
	{
		Ability,
		Boost,
		Challenge,
		Difficulty,
		Force,
		Proficiency,
		Setback
	}

	public static class DieExtensions
	{
		public static ImmutableList<DieNames> NegativeDice => new List<DieNames> { DieNames.Challenge, DieNames.Difficulty, DieNames.Setback }.ToImmutableList();

		public static ImmutableList<DieNames> PositiveDice => new List<DieNames> { DieNames.Ability, DieNames.Proficiency, DieNames.Boost }.ToImmutableList();

		/// <summary>
		/// Returns a result for each face of a die
		/// </summary>
		/// <param name="die"></param>
		/// <returns></returns>
		public static IEnumerable<PoolResult> ToPool(this Die die) =>
			die.DieFaces.Select(face =>
				new PoolResult()
				{
					Frequency = 1,
					PoolResultSymbols = face.DieFaceSymbols.Select(facesymbol => new PoolResultSymbol(facesymbol.Symbol, facesymbol.Quantity)).ToList()
				}
			);

		public static Die GetDie(this IEnumerable<Die> dice, DieNames name) => dice.FirstOrDefault(w => w.Name == name.ToString());

		public static DieNames GetName(this string input)
		{
			Enum.TryParse(input, true, out DieNames dieType);

			return dieType;
		}
	}
}
