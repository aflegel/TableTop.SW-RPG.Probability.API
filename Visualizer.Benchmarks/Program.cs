using System.Collections.Generic;
using System.Linq;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using Probability.Generator;
using Probability.Service.Models;

namespace Visualizer.Benchmarks
{
	internal class Program
	{
		private static void Main(string[] args) => BenchmarkRunner.Run<SeedingBenchmark>();
	}

	internal class SeedingBenchmark
	{
		public static (IEnumerable<int>, IEnumerable<int>, IEnumerable<int>) TwoOneZero => (new List<int> { 4, 5, 6 }, new List<int> { 4 }, new List<int> { 0 });
		public (IEnumerable<Pool>, IEnumerable<Pool>) Pools { get; }

		public SeedingBenchmark() => Pools = DiceSeed.SeedDice.SeedPools(TwoOneZero, TwoOneZero);

		[Benchmark]
		public List<PoolCombination> GenerateStats() => Pools.SeedCombinationStatistics().ToList();
	}
}
