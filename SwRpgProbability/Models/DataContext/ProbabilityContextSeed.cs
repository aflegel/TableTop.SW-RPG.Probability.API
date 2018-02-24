namespace SwRpgProbability.Models.DataContext
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using Microsoft.EntityFrameworkCore;
	using static SwRpgProbability.Models.DataContext.Die;

	public class ProbabilityContextSeed
	{
		public static void SeedData(ProbabilityContext context)
		{
			context.Dice.Add(BuildAdvantage());
			context.Dice.Add(BuildBoost());
			context.Dice.Add(BuildChallenge());
			context.Dice.Add(BuildDifficulty());
			context.Dice.Add(BuildForce());
			context.Dice.Add(BuildProficiency());
			context.Dice.Add(BuildSetBack());

			context.SaveChanges();
		}

		protected static Die BuildAdvantage()
		{
			var die = new Die()
			{
				Name = Die.DieNames.Ability.ToString()
			};

			var faceList = new List<DieFace>()
			{
				new DieFace(new List<DieFaceSymbol>() /*{ new DieFaceSymbol(Symbol.Blank, 1) }*/),
				new DieFace(new List<DieFaceSymbol>() { new DieFaceSymbol(Symbol.Success, 1) }),
				new DieFace(new List<DieFaceSymbol>() { new DieFaceSymbol(Symbol.Success, 1) }),
				new DieFace(new List<DieFaceSymbol>() { new DieFaceSymbol(Symbol.Success, 2) }),
				new DieFace(new List<DieFaceSymbol>() { new DieFaceSymbol(Symbol.Advantage, 1) }),
				new DieFace(new List<DieFaceSymbol>() { new DieFaceSymbol(Symbol.Advantage, 1) }),
				new DieFace(new List<DieFaceSymbol>() { new DieFaceSymbol(Symbol.Success, 1), new DieFaceSymbol(Symbol.Advantage, 1) }),
				new DieFace(new List<DieFaceSymbol>() { new DieFaceSymbol(Symbol.Advantage, 2) }),
			};

			foreach (var face in faceList)
				die.DieFaces.Add(face);

			return die;
		}

		protected static Die BuildBoost()
		{
			var die = new Die()
			{
				Name = Die.DieNames.Boost.ToString()
			};

			var faceList = new List<DieFace>()
			{
				new DieFace(new List<DieFaceSymbol>() /*{ new DieFaceSymbol(Symbol.Blank, 1) }*/),
				new DieFace(new List<DieFaceSymbol>() /*{ new DieFaceSymbol(Symbol.Blank, 1) }*/),
				new DieFace(new List<DieFaceSymbol>() { new DieFaceSymbol(Symbol.Success, 1) }),
				new DieFace(new List<DieFaceSymbol>() { new DieFaceSymbol(Symbol.Success, 1), new DieFaceSymbol(Symbol.Advantage, 1) }),
				new DieFace(new List<DieFaceSymbol>() { new DieFaceSymbol(Symbol.Success, 2) }),
				new DieFace(new List<DieFaceSymbol>() { new DieFaceSymbol(Symbol.Advantage, 2) }),
				new DieFace(new List<DieFaceSymbol>() { new DieFaceSymbol(Symbol.Advantage, 1) }),
			};

			foreach (var face in faceList)
				die.DieFaces.Add(face);

			return die;
		}

		protected static Die BuildChallenge()
		{
			var die = new Die()
			{
				Name = Die.DieNames.Challenge.ToString()
			};

			var faceList = new List<DieFace>()
			{
				new DieFace(new List<DieFaceSymbol>() /*{ new DieFaceSymbol(Symbol.Blank, 1) }*/),
				new DieFace(new List<DieFaceSymbol>() { new DieFaceSymbol(Symbol.Failure, 1) }),
				new DieFace(new List<DieFaceSymbol>() { new DieFaceSymbol(Symbol.Failure, 1) }),
				new DieFace(new List<DieFaceSymbol>() { new DieFaceSymbol(Symbol.Failure, 1) }),
				new DieFace(new List<DieFaceSymbol>() { new DieFaceSymbol(Symbol.Failure, 1) }),
				new DieFace(new List<DieFaceSymbol>() { new DieFaceSymbol(Symbol.Threat, 1) }),
				new DieFace(new List<DieFaceSymbol>() { new DieFaceSymbol(Symbol.Threat, 1) }),
				new DieFace(new List<DieFaceSymbol>() { new DieFaceSymbol(Symbol.Failure, 1), new DieFaceSymbol(Symbol.Threat, 1) }),
				new DieFace(new List<DieFaceSymbol>() { new DieFaceSymbol(Symbol.Failure, 1), new DieFaceSymbol(Symbol.Threat, 1) }),
				new DieFace(new List<DieFaceSymbol>() { new DieFaceSymbol(Symbol.Threat, 1) }),
				new DieFace(new List<DieFaceSymbol>() { new DieFaceSymbol(Symbol.Threat, 1) }),
				new DieFace(new List<DieFaceSymbol>() { new DieFaceSymbol(Symbol.Despair, 1) }),
			};

			foreach (var face in faceList)
				die.DieFaces.Add(face);

			return die;
		}

		protected static Die BuildDifficulty()
		{
			var die = new Die()
			{
				Name = Die.DieNames.Difficulty.ToString()
			};

			var faceList = new List<DieFace>()
			{
				new DieFace(new List<DieFaceSymbol>() /*{ new DieFaceSymbol(Symbol.Blank, 1) }*/),
				new DieFace(new List<DieFaceSymbol>() { new DieFaceSymbol(Symbol.Failure, 1) }),
				new DieFace(new List<DieFaceSymbol>() { new DieFaceSymbol(Symbol.Failure, 2) }),
				new DieFace(new List<DieFaceSymbol>() { new DieFaceSymbol(Symbol.Threat, 1) }),
				new DieFace(new List<DieFaceSymbol>() { new DieFaceSymbol(Symbol.Threat, 1) }),
				new DieFace(new List<DieFaceSymbol>() { new DieFaceSymbol(Symbol.Threat, 1) }),
				new DieFace(new List<DieFaceSymbol>() { new DieFaceSymbol(Symbol.Threat, 1) }),
				new DieFace(new List<DieFaceSymbol>() { new DieFaceSymbol(Symbol.Failure, 1), new DieFaceSymbol(Symbol.Threat, 1) }),
			};

			foreach (var face in faceList)
				die.DieFaces.Add(face);

			return die;
		}

		protected static Die BuildForce()
		{
			var die = new Die()
			{
				Name = Die.DieNames.Force.ToString()
			};

			var faceList = new List<DieFace>()
			{
				new DieFace(new List<DieFaceSymbol>() { new DieFaceSymbol(Symbol.Dark, 1) }),
				new DieFace(new List<DieFaceSymbol>() { new DieFaceSymbol(Symbol.Dark, 1) }),
				new DieFace(new List<DieFaceSymbol>() { new DieFaceSymbol(Symbol.Dark, 1) }),
				new DieFace(new List<DieFaceSymbol>() { new DieFaceSymbol(Symbol.Dark, 1) }),
				new DieFace(new List<DieFaceSymbol>() { new DieFaceSymbol(Symbol.Dark, 1) }),
				new DieFace(new List<DieFaceSymbol>() { new DieFaceSymbol(Symbol.Dark, 1) }),
				new DieFace(new List<DieFaceSymbol>() { new DieFaceSymbol(Symbol.Dark, 2) }),
				new DieFace(new List<DieFaceSymbol>() { new DieFaceSymbol(Symbol.Light, 1) }),
				new DieFace(new List<DieFaceSymbol>() { new DieFaceSymbol(Symbol.Light, 1) }),
				new DieFace(new List<DieFaceSymbol>() { new DieFaceSymbol(Symbol.Light, 2) }),
				new DieFace(new List<DieFaceSymbol>() { new DieFaceSymbol(Symbol.Light, 2) }),
				new DieFace(new List<DieFaceSymbol>() { new DieFaceSymbol(Symbol.Light, 2) }),
			};

			foreach (var face in faceList)
				die.DieFaces.Add(face);

			return die;
		}

		protected static Die BuildProficiency()
		{
			var die = new Die()
			{
				Name = Die.DieNames.Proficiency.ToString()
			};

			var faceList = new List<DieFace>()
			{
				new DieFace(new List<DieFaceSymbol>() /*{ new DieFaceSymbol(Symbol.Blank, 1) }*/),
				new DieFace(new List<DieFaceSymbol>() { new DieFaceSymbol(Symbol.Success, 1) }),
				new DieFace(new List<DieFaceSymbol>() { new DieFaceSymbol(Symbol.Success, 1) }),
				new DieFace(new List<DieFaceSymbol>() { new DieFaceSymbol(Symbol.Success, 2) }),
				new DieFace(new List<DieFaceSymbol>() { new DieFaceSymbol(Symbol.Success, 2) }),
				new DieFace(new List<DieFaceSymbol>() { new DieFaceSymbol(Symbol.Advantage, 1) }),
				new DieFace(new List<DieFaceSymbol>() { new DieFaceSymbol(Symbol.Advantage, 1), new DieFaceSymbol(Symbol.Success, 1) }),
				new DieFace(new List<DieFaceSymbol>() { new DieFaceSymbol(Symbol.Advantage, 1), new DieFaceSymbol(Symbol.Success, 1) }),
				new DieFace(new List<DieFaceSymbol>() { new DieFaceSymbol(Symbol.Advantage, 1), new DieFaceSymbol(Symbol.Success, 1) }),
				new DieFace(new List<DieFaceSymbol>() { new DieFaceSymbol(Symbol.Advantage, 2) }),
				new DieFace(new List<DieFaceSymbol>() { new DieFaceSymbol(Symbol.Advantage, 2) }),
				new DieFace(new List<DieFaceSymbol>() { new DieFaceSymbol(Symbol.Triumph, 1) }),
			};

			foreach (var face in faceList)
				die.DieFaces.Add(face);

			return die;
		}

		protected static Die BuildSetBack()
		{
			var die = new Die()
			{
				Name = Die.DieNames.SetBack.ToString()
			};

			var faceList = new List<DieFace>()
			{
				new DieFace(new List<DieFaceSymbol>() /*{ new DieFaceSymbol(Symbol.Blank, 1) }*/),
				new DieFace(new List<DieFaceSymbol>() /*{ new DieFaceSymbol(Symbol.Blank, 1) }*/),
				new DieFace(new List<DieFaceSymbol>() { new DieFaceSymbol(Symbol.Failure, 1) }),
				new DieFace(new List<DieFaceSymbol>() { new DieFaceSymbol(Symbol.Failure, 1) }),
				new DieFace(new List<DieFaceSymbol>() { new DieFaceSymbol(Symbol.Threat, 1) }),
				new DieFace(new List<DieFaceSymbol>() { new DieFaceSymbol(Symbol.Threat, 1) }),
			};

			foreach (var face in faceList)
				die.DieFaces.Add(face);

			return die;
		}
	}
}