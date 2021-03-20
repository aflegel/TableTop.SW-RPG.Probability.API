using Microsoft.Extensions.Logging;

namespace Probability.Function.Extensions
{
	public static class LoggingExtensions
	{
		public static void LogRoll(this ILogger logger, string poolText, decimal rollEstimation, decimal uniqueRolls)
			=> logger.LogInformation($"{poolText,-80}|{rollEstimation,29:n0}  |{uniqueRolls,12:n0}");
	}
}
