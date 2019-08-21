using System.Collections.Generic;
using DataFramework.Models;
using static DataFramework.Models.Die;

namespace DataFramework.Context
{
	public class ProbabilityContextSeed
	{
		public static void SeedData(ProbabilityContext context)
		{
			context.Dice.AddRange(new List<Die>
			{
				AbilityDie,
				BoostDie,
				ChallengeDie,
				DifficultyDie,
				ForceDie,
				ProficiencyDie,
				SetbackDie
			});

			context.SaveChanges();
		}

		public static Die AbilityDie => new Die
		{
			Name = DieNames.Ability.ToString(),
			DieFaces = new List<DieFace>
			{
				new DieFace(new List<DieFaceSymbol>() /*{ new DieFaceSymbol(Symbol.Blank, 1) }*/),
				new DieFace(new List<DieFaceSymbol> { new DieFaceSymbol(Symbol.Success, 1) }),
				new DieFace(new List<DieFaceSymbol> { new DieFaceSymbol(Symbol.Success, 1) }),
				new DieFace(new List<DieFaceSymbol> { new DieFaceSymbol(Symbol.Success, 2) }),
				new DieFace(new List<DieFaceSymbol> { new DieFaceSymbol(Symbol.Advantage, 1) }),
				new DieFace(new List<DieFaceSymbol> { new DieFaceSymbol(Symbol.Advantage, 1) }),
				new DieFace(new List<DieFaceSymbol> { new DieFaceSymbol(Symbol.Success, 1), new DieFaceSymbol(Symbol.Advantage, 1) }),
				new DieFace(new List<DieFaceSymbol> { new DieFaceSymbol(Symbol.Advantage, 2) }),
			}
		};

		public static Die BoostDie => new Die
		{
			Name = DieNames.Boost.ToString(),
			DieFaces = new List<DieFace>
				{
					new DieFace(new List<DieFaceSymbol>() /*{ new DieFaceSymbol(Symbol.Blank, 1) }*/),
					new DieFace(new List<DieFaceSymbol>() /*{ new DieFaceSymbol(Symbol.Blank, 1) }*/),
					new DieFace(new List<DieFaceSymbol> { new DieFaceSymbol(Symbol.Success, 1) }),
					new DieFace(new List<DieFaceSymbol> { new DieFaceSymbol(Symbol.Success, 1), new DieFaceSymbol(Symbol.Advantage, 1) }),
					new DieFace(new List<DieFaceSymbol> { new DieFaceSymbol(Symbol.Advantage, 2) }),
					new DieFace(new List<DieFaceSymbol> { new DieFaceSymbol(Symbol.Advantage, 1) }),
				}
		};

		public static Die ChallengeDie => new Die
		{
			Name = DieNames.Challenge.ToString(),
			DieFaces = new List<DieFace>
			{
				new DieFace(new List<DieFaceSymbol>() /*{ new DieFaceSymbol(Symbol.Blank, 1) }*/),
				new DieFace(new List<DieFaceSymbol> { new DieFaceSymbol(Symbol.Failure, 1) }),
				new DieFace(new List<DieFaceSymbol> { new DieFaceSymbol(Symbol.Failure, 1) }),
				new DieFace(new List<DieFaceSymbol> { new DieFaceSymbol(Symbol.Failure, 2) }),
				new DieFace(new List<DieFaceSymbol> { new DieFaceSymbol(Symbol.Failure, 2) }),
				new DieFace(new List<DieFaceSymbol> { new DieFaceSymbol(Symbol.Threat, 1) }),
				new DieFace(new List<DieFaceSymbol> { new DieFaceSymbol(Symbol.Threat, 1) }),
				new DieFace(new List<DieFaceSymbol> { new DieFaceSymbol(Symbol.Failure, 1), new DieFaceSymbol(Symbol.Threat, 1) }),
				new DieFace(new List<DieFaceSymbol> { new DieFaceSymbol(Symbol.Failure, 1), new DieFaceSymbol(Symbol.Threat, 1) }),
				new DieFace(new List<DieFaceSymbol> { new DieFaceSymbol(Symbol.Threat, 2) }),
				new DieFace(new List<DieFaceSymbol> { new DieFaceSymbol(Symbol.Threat, 2) }),
				new DieFace(new List<DieFaceSymbol> { new DieFaceSymbol(Symbol.Despair, 1) }),
			}
		};

		public static Die DifficultyDie => new Die
		{
			Name = DieNames.Difficulty.ToString(),
			DieFaces = new List<DieFace>
				{
					new DieFace(new List<DieFaceSymbol>() /*{ new DieFaceSymbol(Symbol.Blank, 1) }*/),
					new DieFace(new List<DieFaceSymbol> { new DieFaceSymbol(Symbol.Failure, 1) }),
					new DieFace(new List<DieFaceSymbol> { new DieFaceSymbol(Symbol.Failure, 2) }),
					new DieFace(new List<DieFaceSymbol> { new DieFaceSymbol(Symbol.Threat, 1) }),
					new DieFace(new List<DieFaceSymbol> { new DieFaceSymbol(Symbol.Threat, 1) }),
					new DieFace(new List<DieFaceSymbol> { new DieFaceSymbol(Symbol.Threat, 1) }),
					new DieFace(new List<DieFaceSymbol> { new DieFaceSymbol(Symbol.Threat, 2) }),
					new DieFace(new List<DieFaceSymbol> { new DieFaceSymbol(Symbol.Failure, 1), new DieFaceSymbol(Symbol.Threat, 1) })
				}
		};

		public static Die ForceDie => new Die
		{
			Name = DieNames.Force.ToString(),
			DieFaces = new List<DieFace>
			{
				new DieFace(new List<DieFaceSymbol> { new DieFaceSymbol(Symbol.Dark, 1) }),
				new DieFace(new List<DieFaceSymbol> { new DieFaceSymbol(Symbol.Dark, 1) }),
				new DieFace(new List<DieFaceSymbol> { new DieFaceSymbol(Symbol.Dark, 1) }),
				new DieFace(new List<DieFaceSymbol> { new DieFaceSymbol(Symbol.Dark, 1) }),
				new DieFace(new List<DieFaceSymbol> { new DieFaceSymbol(Symbol.Dark, 1) }),
				new DieFace(new List<DieFaceSymbol> { new DieFaceSymbol(Symbol.Dark, 1) }),
				new DieFace(new List<DieFaceSymbol> { new DieFaceSymbol(Symbol.Dark, 2) }),
				new DieFace(new List<DieFaceSymbol> { new DieFaceSymbol(Symbol.Light, 1) }),
				new DieFace(new List<DieFaceSymbol> { new DieFaceSymbol(Symbol.Light, 1) }),
				new DieFace(new List<DieFaceSymbol> { new DieFaceSymbol(Symbol.Light, 2) }),
				new DieFace(new List<DieFaceSymbol> { new DieFaceSymbol(Symbol.Light, 2) }),
				new DieFace(new List<DieFaceSymbol> { new DieFaceSymbol(Symbol.Light, 2) }),
			}
		};

		public static Die ProficiencyDie => new Die
		{
			Name = DieNames.Proficiency.ToString(),
			DieFaces = new List<DieFace>
			{
				new DieFace(new List<DieFaceSymbol>() /*{ new DieFaceSymbol(Symbol.Blank, 1) }*/),
				new DieFace(new List<DieFaceSymbol> { new DieFaceSymbol(Symbol.Success, 1) }),
				new DieFace(new List<DieFaceSymbol> { new DieFaceSymbol(Symbol.Success, 1) }),
				new DieFace(new List<DieFaceSymbol> { new DieFaceSymbol(Symbol.Success, 2) }),
				new DieFace(new List<DieFaceSymbol> { new DieFaceSymbol(Symbol.Success, 2) }),
				new DieFace(new List<DieFaceSymbol> { new DieFaceSymbol(Symbol.Advantage, 1) }),
				new DieFace(new List<DieFaceSymbol> { new DieFaceSymbol(Symbol.Advantage, 1), new DieFaceSymbol(Symbol.Success, 1) }),
				new DieFace(new List<DieFaceSymbol> { new DieFaceSymbol(Symbol.Advantage, 1), new DieFaceSymbol(Symbol.Success, 1) }),
				new DieFace(new List<DieFaceSymbol> { new DieFaceSymbol(Symbol.Advantage, 1), new DieFaceSymbol(Symbol.Success, 1) }),
				new DieFace(new List<DieFaceSymbol> { new DieFaceSymbol(Symbol.Advantage, 2) }),
				new DieFace(new List<DieFaceSymbol> { new DieFaceSymbol(Symbol.Advantage, 2) }),
				new DieFace(new List<DieFaceSymbol> { new DieFaceSymbol(Symbol.Triumph, 1) }),
			}
		};

		public static Die SetbackDie => new Die
		{
			Name = DieNames.Setback.ToString(),
			DieFaces = new List<DieFace>
				{
				new DieFace(new List<DieFaceSymbol>() /*{ new DieFaceSymbol(Symbol.Blank, 1) }*/),
				new DieFace(new List<DieFaceSymbol>() /*{ new DieFaceSymbol(Symbol.Blank, 1) }*/),
				new DieFace(new List<DieFaceSymbol> { new DieFaceSymbol(Symbol.Failure, 1) }),
				new DieFace(new List<DieFaceSymbol> { new DieFaceSymbol(Symbol.Failure, 1) }),
				new DieFace(new List<DieFaceSymbol> { new DieFaceSymbol(Symbol.Threat, 1) }),
				new DieFace(new List<DieFaceSymbol> { new DieFaceSymbol(Symbol.Threat, 1) }),
			}
		};
	}
}
