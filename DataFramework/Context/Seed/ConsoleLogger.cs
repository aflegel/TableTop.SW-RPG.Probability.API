using System;

namespace DataFramework.Context.Seed
{
	public static class ConsoleLogger
	{
		public static void LogRoll(string poolText, decimal rollEstimation, decimal uniqueRolls) => Console.WriteLine($"{poolText,-80}|{rollEstimation,29:n0}  |{uniqueRolls,12:n0}");

		public static void LogLine(string message) => Console.WriteLine($"{DateTime.Now:hh:mm.ss} {message}");
	}
}
