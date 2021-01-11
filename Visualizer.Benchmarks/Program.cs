using System.Collections.Generic;
using System.Linq;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using DataFramework.Context.Seed;
using DataFramework.Extensions;
using DataFramework.Models;

namespace Visualizer.Benchmarks
{
	public class Program
	{
		public static void Main(string[] args) => BenchmarkRunner.Run<SeedingBenchmark>();
	}

	public class SeedingBenchmark
	{
		private static Pool MakePool(string name, int quantity) => new Pool() { PoolDice = new List<PoolDie> { new PoolDie(new Die { Name = name }, quantity) } };
		private static Pool AbilityTwo => MakePool(nameof(Ability), 2);
		private static Pool DifficultyTwo => MakePool(nameof(Difficulty), 2);

		public SeedingBenchmark() { }

		[Benchmark]
		public (IEnumerable<PoolDie>, IEnumerable<PoolDie>) Skip() => AbilityTwo.PoolDice.Union(DifficultyTwo.PoolDice).Split();

		[Benchmark]
		public (IEnumerable<PoolDie>, IEnumerable<PoolDie>) Chunk() => AbilityTwo.PoolDice.Union(DifficultyTwo.PoolDice).ChunkBy();
	}

	public static class ListExtensions
	{


		public static (IEnumerable<T>, IEnumerable<T>) ChunkBy<T>(this IEnumerable<T> source) => source.ChunkBy(source.Count() / 2);

		public static (IEnumerable<T>, IEnumerable<T>) ChunkBy<T>(this IEnumerable<T> source, int chunkSize) => (source.Take(chunkSize), source.Skip(chunkSize));
	}
}
