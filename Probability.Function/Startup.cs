using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Probability.Function.Configuration;
using Probability.Generator;
using Probability.Service;

[assembly: FunctionsStartup(typeof(Probability.Function.Startup))]

namespace Probability.Function
{
	public class Startup : FunctionsStartup
	{
		private IConfiguration Configuration { get; }

		public Startup(IConfiguration configuration) => Configuration = configuration;

		public override void Configure(IFunctionsHostBuilder builder)
		{
			builder.Services.AddDbContext<ProbabilityContext>(options => options.UseSqlServer(Configuration.GetConnectionString("ProbabilityContext")));
			builder.Services.AddSingleton<GeneratorService>();
			builder.Services.Configure<GeneratorConfiguration>(Configuration.GetSection(""));
		}
	}
}

