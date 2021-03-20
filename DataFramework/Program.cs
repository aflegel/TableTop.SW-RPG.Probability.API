using System;
using System.Linq;
using DataFramework.Context.Seed;
using Microsoft.EntityFrameworkCore;
using Probability.Generator;
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

		private static readonly GeneratorService generatorService;

		private static void Main()
		{
			var time = DateTime.Now;
			ConsoleLogger.LogLine("Startup");

			var options = new DbContextOptionsBuilder<ProbabilityContext>().UseSqlServer(@"Server=ALEXANDER-HP-85;Database=TableTop.Utility.StarWarsRPGProbability;integrated security=True;MultipleActiveResultSets=true");

			using (var context = new ProbabilityContext(options.Options))
			{
				// Deletes and creates the database and seeds the Dice
				generatorService.InitializeDatabase().GetAwaiter().GetResult();

				generatorService.CalculateOutcomes(
					(abilityLimit.Range, proficencyLimit.Range, boostLimit.Range),
					(difficultyLimit.Range, challengeLimit.Range, setbackLimit.Range));

				generatorService.CommitData("All Records").GetAwaiter().GetResult();
			}

			Console.WriteLine($"Start time: {time:hh:mm.ss}");
			Console.WriteLine($"Completion time: {DateTime.Now:hh:mm.ss}");
		}
	}
}
