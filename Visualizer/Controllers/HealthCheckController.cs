using System.Threading.Tasks;
using DataFramework.Context;
using Microsoft.AspNetCore.Mvc;
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
				: (ActionResult)new ServiceUnavailableResult();
	}
}
