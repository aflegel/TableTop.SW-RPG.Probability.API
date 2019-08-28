using System;
using System.Collections.Generic;
using System.Linq;
using DataFramework.Context;
using Microsoft.EntityFrameworkCore;
using static DataFramework.Models.Die;

namespace DataFramework.Models
{
	public class Die
	{
		public static List<DieNames> NegativeDice => new List<DieNames> { DieNames.Challenge, DieNames.Difficulty, DieNames.Setback };

		public static List<DieNames> PositiveDice => new List<DieNames> { DieNames.Ability, DieNames.Proficiency, DieNames.Boost };

		/// <summary>
		/// An Enum to capture the different kind of faces
		/// </summary>
		public enum Symbol
		{
			Blank = 0,
			Success = 1,
			Failure = 2,
			Advantage = 3,
			Threat = 4,
			Triumph = 5,
			Despair = 6,
			Light = 7,
			Dark = 8
		}

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

		public Die()
		{
			DieFaces = new HashSet<DieFace>();
			PoolDice = new HashSet<PoolDie>();
		}

		public int DieId { get; set; }

		public string Name { get; set; }

		public ICollection<DieFace> DieFaces { get; set; }

		public ICollection<PoolDie> PoolDice { get; set; }
	}

	public static class DieExtensions
	{
		/// <summary>
		/// Returns a Die with it's faces and face symbols
		/// </summary>
		/// <param name="context"></param>
		/// <param name="die"></param>
		/// <returns></returns>
		public static Die GetDie(this ProbabilityContext context, DieNames die) => context.GetDie(die.ToString());

		public static Die GetDie(this ProbabilityContext context, string die) => context.Dice.Where(w => w.Name == die.ToString()).Include(i => i.DieFaces).ThenInclude(t => t.DieFaceSymbols).FirstOrDefault();

		/// <summary>
		/// Returns a result for each face of a die
		/// </summary>
		/// <param name="die"></param>
		/// <returns></returns>
		public static IEnumerable<PoolResult> GetDiePool(this Die die) =>
			die.DieFaces.Select(face =>
				new PoolResult()
				{
					Frequency = 1,
					PoolResultSymbols = face.DieFaceSymbols.Select(facesymbol => new PoolResultSymbol(facesymbol.Symbol, facesymbol.Quantity)).ToList()
				}
			);

		public static DieNames GetName(this string input)
		{
			Enum.TryParse(input, true, out DieNames dieType);

			return dieType;
		}
	}
}
