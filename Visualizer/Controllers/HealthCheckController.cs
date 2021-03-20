using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Probability.Service;
using Visualizer.Extensions;

namespace Visualizer.Controllers
{
	[Route("[controller]")]
	[ApiController]
	public class HealthCheckController : ControllerBase
	{
		private ProbabilityContext Context { get; }
		private ILogger<HealthCheckController> Logger { get; }

		public HealthCheckController(ProbabilityContext context, ILogger<HealthCheckController> logger)
		{
			Logger = logger;
			Context = context;
		}

		[HttpGet]
		public async Task<ActionResult> Get()
		{
			Logger.LogInformation("Serivce testing");
			return await Context.Database.CanConnectAsync()
				? Ok()
				: new ServiceUnavailableResult();
		}
	}
}
