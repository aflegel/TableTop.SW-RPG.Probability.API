using System;
using System.Linq;
using DataFramework.Context.Seed;
using Microsoft.EntityFrameworkCore;
using Probability.Service;

namespace DataFramework
{
	internal class Program
	{
		private static readonly LimitConfiguration abilityLimit = new LimitConfiguration { Start = 0, Count = 5 };
		private static readonly LimitConfiguration proficencyLimit = new LimitConfiguration { Start = 0, Count = 5 };
		private static readonly LimitConfiguration difficultyLimit = new LimitConfiguration { Start = 0, Count = 5 };
		private static readonly LimitConfiguration challengeLimit = new LimitConfiguration { Start = 0, Count = 5 };
		private static readonly LimitConfiguration boostLimit = new LimitConfiguration { Start = 0, Count = 5 };
		private static readonly LimitConfiguration setbackLimit = new LimitConfiguration { Start = 0, Count = 5 };


		private static void Main()
		{
			var time = DateTime.Now;
			ConsoleLogger.LogLine("Startup");

			var options = new DbContextOptionsBuilder<ProbabilityContext>().UseSqlServer(@"Server=ALEXANDER-HP-85;Database=TableTop.Utility.StarWarsRPGProbability;integrated security=True;MultipleActiveResultSets=true");

			using (var context = new ProbabilityContext(options.Options))
			{
				// Deletes and creates the database and seeds the Dice
				InitializeDatabase(context);

				context.PoolCombinations.AddRange(DiceSeed.SeedDice
					.SeedPools((abilityLimit.Range, proficencyLimit.Range, boostLimit.Range), (difficultyLimit.Range, challengeLimit.Range, setbackLimit.Range))
					.SeedCombinationStatistics().ToList());

				CommitData(context, "All Records");
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
			ConsoleLogger.LogLine("Database Initialization");

			//delete and recreate the database
			context.Database.EnsureDeleted();
			context.Database.EnsureCreated();

			ConsoleLogger.LogLine("Database Seeding");
		}

		/// <summary>
		/// Prints start and stop timestamps while saving the current context state
		/// </summary>
		/// <param name="context"></param>
		private static void CommitData(ProbabilityContext context, string message)
		{
			ConsoleLogger.LogLine($"Initialize {message} Database Commit");
			context.SaveChanges();
			ConsoleLogger.LogLine($"Completed {message} Database Commit");
		}
	}
}
