using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using DataFramework.Context;
using DataFramework.Context.Seed;

namespace DataFramework
{
	internal class Program
	{
		private static readonly LimitConfiguration abilityLimit = new LimitConfiguration { Start = 0, Count = 5 };
		private static readonly LimitConfiguration upgradeLimit = new LimitConfiguration { Start = 0, Count = 5 };
		private static readonly LimitConfiguration difficultyLimit = new LimitConfiguration { Start = 0, Count = 5 };
		private static readonly LimitConfiguration challengeLimit = new LimitConfiguration { Start = 0, Count = 5 };
		private static readonly LimitConfiguration boostLimit = new LimitConfiguration { Start = 0, Count = 5 };
		private static readonly LimitConfiguration setbackLimit = new LimitConfiguration { Start = 0, Count = 5 };

		private static void LogLine(string message) => Console.WriteLine($"{DateTime.Now:hh:mm.ss} {message}");

		private static void Main()
		{
			var time = DateTime.Now;
			LogLine("Startup");

			var options = new DbContextOptionsBuilder<ProbabilityContext>().UseSqlServer(@"Server=ALEXANDER-HP-85;Database=TableTop.Utility.StarWarsRPGProbability;integrated security=True;MultipleActiveResultSets=true");

			using (var context = new ProbabilityContext(options.Options))
			{
				// Deletes and creates the database and seeds the Dice
				InitializeDatabase(context);

				// Seeds the pool dice and results
				ProcessPools(context);

				// Seeds the statistics from pool combinations
				ProcessComparisons(context);
			}

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

			context.SeedDice();

			CommitData(context, "Dice Seeding");
		}

		/// <summary>
		/// Prints start and stop timestamps while saving the current context state
		/// </summary>
		/// <param name="context"></param>
		private static void CommitData(ProbabilityContext context, string message)
		{
			LogLine($"Initialize {message} Database Commit");
			context.SaveChanges();
			LogLine($"Completed {message} Database Commit");
		}

		/// <summary>
		/// Prints start and stop timestamps while building the complete outcome map for a set of dice
		/// </summary>
		/// <param name="context"></param>
		private static void ProcessPools(ProbabilityContext context)
		{
			LogLine("Initialize Pool Generation");

			//.ToList() triggers the ienumerable execution
			context.BuildPositivePool(abilityLimit.Range, upgradeLimit.Range, boostLimit.Range).ToList();
			context.BuildNegativePool(difficultyLimit.Range, challengeLimit.Range, setbackLimit.Range).ToList();

			LogLine("Completed Pool Generation");

			//save the pools before generating the full comparison
			CommitData(context, "PoolDice Seeding");
		}

		/// <summary>
		/// Processes the comparison of positive and negative pools
		/// </summary>
		/// <param name="context"></param>
		private static void ProcessComparisons(ProbabilityContext context)
		{
			LogLine("Initialize Pool Comparison");

			_ = (context.GetPositivePools().ToList(), context.GetNegativePools().ToList()).CrossProduct().ToList();

			LogLine("Completed Pool Comparison");

			//save the outcome results
			CommitData(context, "Pool Result Seeding");
		}
	}
}
