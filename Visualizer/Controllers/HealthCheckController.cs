using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Probability.Service;
using Visualizer.Extensions;

namespace Visualizer.Controllers
{
	[Route("[controller]")]
	[ApiController]
	public class HealthCheckController : ControllerBase
	{
		private readonly ProbabilityContext context;

		public HealthCheckController(ProbabilityContext context) => this.context = context;

		[HttpGet]
		public async Task<ActionResult> Get() =>
			await context.Database.CanConnectAsync()
				? Ok()
				: new ServiceUnavailableResult();
	}
}
