using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Probability.Function.Configuration;
using Probability.Generator;

namespace Probability.Function
{
	public class GenerateData
	{
		private GeneratorService GeneratorService { get; }
		private GeneratorConfiguration GeneratorConfiguration { get; }
		private ILogger<GenerateData> Logger { get; }

		public GenerateData(GeneratorService context, IOptions<GeneratorConfiguration> configuration, ILogger<GenerateData> logger)
		{
			GeneratorService = context;
			GeneratorConfiguration = configuration.Value;
			Logger = logger;
		}

		[FunctionName("GenerateData")]
		public async Task<IActionResult> Run(
			[HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req)
		{
			Logger.LogInformation("Startup");

			// Deletes and creates the database and seeds the Dice
			await GeneratorService.InitializeDatabase();

			GeneratorService.CalculateOutcomes(
				(GeneratorConfiguration.Ability.Range, GeneratorConfiguration.Proficiency.Range, GeneratorConfiguration.Boost.Range),
				(GeneratorConfiguration.Difficulty.Range, GeneratorConfiguration.Challenge.Range, GeneratorConfiguration.Setback.Range));

			await GeneratorService.CommitData("All Records");

			Logger.LogInformation("Shutdown");

			return new OkResult();
		}
	}
}
