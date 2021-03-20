using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Probability.Service;

namespace Probability.Generator
{
	public class GeneratorService
	{
		private ProbabilityContext ProbabilityContext { get; }
		private ILogger<GeneratorService> Logger { get; }

		public GeneratorService(ILogger<GeneratorService> logger, ProbabilityContext context)
		{
			Logger = logger;
			ProbabilityContext = context;
		}

		public async Task InitializeDatabase()
		{
			//todo: wait for confirmation before deleting
			Logger.LogInformation("Database Initialization");

			//delete and recreate the database
			await ProbabilityContext.Database.EnsureDeletedAsync();
			await ProbabilityContext.Database.EnsureCreatedAsync();

			Logger.LogInformation("Database Seeding");
		}

		public void CalculateOutcomes((IEnumerable<int> ability, IEnumerable<int> proficiency, IEnumerable<int> boost) positiveRange, (IEnumerable<int>, IEnumerable<int>, IEnumerable<int>) negativeRange)
		{
			ProbabilityContext.PoolCombinations.AddRange(DiceSeed.SeedDice
					.SeedPools(positiveRange, negativeRange)
					.SeedCombinationStatistics().ToList());
		}

		public async Task CommitData(string message)
		{
			Logger.LogInformation($"Initialize {message} Database Commit");
			await ProbabilityContext.SaveChangesAsync();
			Logger.LogInformation($"Completed {message} Database Commit");
		}
	}
}
