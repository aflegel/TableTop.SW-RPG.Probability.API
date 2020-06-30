using System.Collections.Generic;
using System.Linq;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using DataFramework.Context.Seed;
using DataFramework.Models;

namespace Visualizer.Benchmarks
{
	public class Program
    {
		public static void Main(string[] args) => BenchmarkRunner.Run<SeedingBenchmark>();
	}

	public class SeedingBenchmark
	{
		public static (IEnumerable<int>, IEnumerable<int>, IEnumerable<int>) TwoOneZero => (new List<int> { 4, 5, 6 }, new List<int> { 4 }, new List<int> { 0 });

		public (IEnumerable<Pool>, IEnumerable<Pool>) Pools { get; }

		public SeedingBenchmark()
		{
			Pools = DiceSeed.SeedDice.SeedPools(TwoOneZero, TwoOneZero);
		}

		[Benchmark]
		public List<PoolCombination> GenerateStats() => Pools.SeedCombinationStatistics().ToList();
	}
}
