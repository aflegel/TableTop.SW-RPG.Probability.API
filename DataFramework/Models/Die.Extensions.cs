using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace DataFramework.Models
{

	public static class DieExtensions
	{
		public static ImmutableList<string> NegativeDice => new List<string> { nameof(Challenge), nameof(Difficulty), nameof(Setback) }.ToImmutableList();

		public static ImmutableList<string> PositiveDice => new List<string> { nameof(Ability), nameof(Proficiency), nameof(Boost) }.ToImmutableList();

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

		public static Die GetDie<T>(this IEnumerable<Die> dice) where T : Die => dice.FirstOrDefault(w => w.Name == typeof(T).Name);
	}
}
