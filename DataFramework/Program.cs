using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using DataFramework.Context;
using DataFramework.Models;
using static DataFramework.Models.Die;
using DataFramework.Context.Seed;

namespace DataFramework
{
	internal class Program
	{
		private static readonly LimitConfiguration abilityLimit = new LimitConfiguration { Start = 0, Count = 4 };
		private static readonly LimitConfiguration upgradeLimit = new LimitConfiguration { Start = 0, Count = 4 };
		private static readonly LimitConfiguration difficultyLimit = new LimitConfiguration { Start = 0, Count = 4 };
		private static readonly LimitConfiguration challengeLimit = new LimitConfiguration { Start = 0, Count = 4 };
		private static readonly LimitConfiguration boostLimit = new LimitConfiguration { Start = 0, Count = 4 };
		private static readonly LimitConfiguration setbackLimit = new LimitConfiguration { Start = 0, Count = 4 };

		private static void LogLine(string message) => Console.WriteLine($"{DateTime.Now:hh:mm.ss} {message}");

		private static void Main(string[] args)
		{
			var time = DateTime.Now;
			LogLine("Startup");

			// ProcessProgram();

			Console.WriteLine($"Start time: {time:hh:mm.ss}");
			Console.WriteLine($"Completion time: {DateTime.Now:hh:mm.ss}");
		}

		/// <summary>
		/// Destroys and recreates the database and seeds the database with the dice information
		/// </summary>
		/// <param name="context"></param>
		private static void InitializeDatabase(ProbabilityContext context)
		{
			//todo: wait for confirmation before deleting
			LogLine("Database Initialization");

			//delete and recreate the database
			context.Database.EnsureDeleted();
			context.Database.EnsureCreated();

			LogLine("Database Seeding");

			context.BuildDice();
		}

		/// <summary>
		///
		/// </summary>
		private static void ProcessProgram()
		{
			using (var context = new ProbabilityContext())
			{
				InitializeDatabase(context);

				//partial pools are each half of a roll
				ProcessPartialPools(context);

				//save the pools before generating the full comparison
				CommitData(context);

				ProcessPoolComparison(context);

				//save the outcome results
				CommitData(context);
			}
		}

		/// <summary>
		/// Prints start and stop timestamps while saving the current context state
		/// </summary>
		/// <param name="context"></param>
		private static void CommitData(ProbabilityContext context)
		{
			LogLine("Initialize Database Commit");
			context.SaveChanges();
			LogLine("Completed Database Commit");
		}

		/// <summary>
		/// Prints start and stop timestamps while building the complete outcome map for a set of dice
		/// </summary>
		/// <param name="context"></param>
		private static void ProcessPartialPools(ProbabilityContext context)
		{
			LogLine("Initialize Pool Generation");
			BuildPositivePool(context);
			BuildNegativePool(context);
			LogLine("Completed Pool Generation");
		}

		/// <summary>
		/// Processes the comparison of positive and negative pools
		/// </summary>
		/// <param name="context"></param>
		private static void ProcessPoolComparison(ProbabilityContext context)
		{
			LogLine("Initialize Pool Comparison");
			var positivePools = context.Pools.Where(pool => pool.PoolDice.Any(die => PositiveDice.Contains(die.Die.Name.GetName())))
				.Include(i => i.PositivePoolCombinations)
						.ThenInclude(tti => tti.PoolCombinationStatistics)
				.Include(i => i.PoolResults)
						.ThenInclude(tti => tti.PoolResultSymbols);

			var negativePools = context.Pools.Where(pool => pool.PoolDice.Any(die => NegativeDice.Contains(die.Die.Name.GetName())))
				.Include(i => i.NegativePoolCombinations)
					.ThenInclude(tti => tti.PoolCombinationStatistics)
				.Include(i => i.PoolResults)
					.ThenInclude(tti => tti.PoolResultSymbols);

			_ = positivePools.SelectMany(positivePool => negativePools, (positivePool, negativePool) => new PoolCombination(positivePool, negativePool).BuildPoolStatistics());

			LogLine("Completed Pool Comparison");
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="context"></param>
		/// <returns></returns>
		private static void BuildPositivePool(ProbabilityContext context) =>
			_ = abilityLimit.Range.SelectMany(ability => upgradeLimit.Range.Where(upgrade => upgrade <= ability), (ability, upgrade) => new Tuple<int, int>(ability, upgrade))
				.SelectMany(tuple => boostLimit.Range, (tuple, boost) => BuildPoolDice(context, tuple.Item1 - tuple.Item2, tuple.Item2, boost: boost).BuildPoolResults());

		/// <summary>
		///
		/// </summary>
		/// <param name="context"></param>
		/// <returns></returns>
		private static void BuildNegativePool(ProbabilityContext context) =>
			_ = difficultyLimit.Range.SelectMany(difficulty => challengeLimit.Range.Where(challenge => challenge <= difficulty), (difficulty, challenge) => new Tuple<int, int>(difficulty, challenge))
				.SelectMany(tuple => setbackLimit.Range, (tuple, setback) => BuildPoolDice(context, tuple.Item1 - tuple.Item2, tuple.Item2, boost: setback).BuildPoolResults());

		/// <summary>
		///
		/// </summary>
		/// <param name="context"></param>
		/// <param name="ability"></param>
		/// <param name="proficiency"></param>
		/// <param name="difficulty"></param>
		/// <param name="challenge"></param>
		/// <param name="boost"></param>
		/// <param name="setback"></param>
		/// <returns></returns>
		protected static Pool BuildPoolDice(ProbabilityContext context, int ability = 0, int proficiency = 0, int difficulty = 0, int challenge = 0, int boost = 0, int setback = 0)
		{
			var pool = new Pool();

			if (ability > 0)
				pool.PoolDice.Add(new PoolDie(context.GetDie(DieNames.Ability), ability));
			if (boost > 0)
				pool.PoolDice.Add(new PoolDie(context.GetDie(DieNames.Boost), boost));
			if (challenge > 0)
				pool.PoolDice.Add(new PoolDie(context.GetDie(DieNames.Challenge), challenge));
			if (difficulty > 0)
				pool.PoolDice.Add(new PoolDie(context.GetDie(DieNames.Difficulty), difficulty));
			if (proficiency > 0)
				pool.PoolDice.Add(new PoolDie(context.GetDie(DieNames.Proficiency), proficiency));
			if (setback > 0)
				pool.PoolDice.Add(new PoolDie(context.GetDie(DieNames.Setback), setback));

			pool.Name = pool.PoolText;
			pool.TotalOutcomes = pool.RollEstimation;

			context.Pools.Add(pool);

			return pool;
		}
	}
}
