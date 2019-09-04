using System;

namespace DataFramework.Context.Seed
{
	public static class ConsoleLogger
	{
		public static void PrintStartLog(string poolText, decimal rollEstimation) => Console.Write($"{poolText,-80}|{rollEstimation,29:n0}");

		public static void PrintFinishLog(decimal rollEstimation) => Console.Write($"  |{rollEstimation,12:n0}\n");

		public static void LogLine(string message) => Console.WriteLine($"{DateTime.Now:hh:mm.ss} {message}");
	}
}
