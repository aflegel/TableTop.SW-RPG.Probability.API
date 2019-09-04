using System.Linq;
using DataFramework.Models;
using Xunit;
using DataFramework.Context.Seed;
using System.Collections.Generic;

namespace Visualizer.Tests
{
	public class SeedingTests
	{
		private static (IEnumerable<int>, IEnumerable<int>, IEnumerable<int>) AbilityTwo => (Enumerable.Range(2, 1), new List<int> { 0 }, new List<int> { 0 });
		private static (IEnumerable<int>, IEnumerable<int>, IEnumerable<int>) DifficultyTwo => (Enumerable.Range(2, 1), new List<int> { 0 }, new List<int> { 0 });
		private static (IEnumerable<int>, IEnumerable<int>, IEnumerable<int>) ProficiencyThreeBoostTwo => (Enumerable.Range(3, 1), Enumerable.Range(3, 1), Enumerable.Range(2, 1));
		private static (IEnumerable<int>, IEnumerable<int>, IEnumerable<int>) ChallengeThreeSetbackTwo => (Enumerable.Range(3, 1), Enumerable.Range(3, 1), Enumerable.Range(2, 1));

		private static List<PoolResultSymbol> SuccessThreeAdvantageFour => new List<PoolResultSymbol> { new PoolResultSymbol(Symbol.Success, 3), new PoolResultSymbol(Symbol.Advantage, 4) };

		[Fact]
		public void DieComparisonGenerator()
		{
			var pools = DiceSeed.SeedDice().ProcessPools(AbilityTwo, (Enumerable.Range(0, 1), new List<int> { 0 }, new List<int> { 0 }));
			var pool = pools.Item1.First();

			Assert.True(1 == pools.Item1.Count(), $"Incorrect number of results {pools.Item1.Count()}");
			Assert.True(15 == pool.PoolResults.Count, $"The number of results did not equal 15. Result was {pool.PoolResults.Count}");
			Assert.True(64 == pool.RollEstimation(), $"The total outcomes did not equal 64. Result was {pool.PoolResults.Count}");
		}

		[Fact]
		public void DieComparisonGeneratorAdvanced()
		{
			var pools = DiceSeed.SeedDice().ProcessPools((Enumerable.Range(1, 2), Enumerable.Range(1, 1), Enumerable.Range(0, 1)), (Enumerable.Range(1, 2), Enumerable.Range(1, 1), Enumerable.Range(0, 1))).CrossProduct();
			//var pool = pools.First();

			Assert.True(4 == pools.Count(), $"Incorrect number of results {pools.Count()}");
		}

		[Fact]
		public void DiePoolHashCode()
		{
			var pool = DiceSeed.SeedDice().ProcessPools(AbilityTwo, (Enumerable.Range(0, 1), new List<int> { 0 }, new List<int> { 0 })).Item1.First();

			Assert.True(pool.ToString() == "Ability 2", $"The text did not equal `Ability 2`. Result was {pool.ToString()}");
		}


		[Fact]
		public void DieComparisonBasic()
		{

			var pools = DiceSeed.SeedDice().ProcessPools(AbilityTwo, DifficultyTwo).CrossProduct();

			var pool = pools.First();

			Assert.True(1 == pools.Count(), $"Incorrect number of results {pools.Count()}");
			Assert.True(20 == pool.PoolCombinationStatistics.Count, $"The number of results did not equal 15. Result was {pool.PoolCombinationStatistics.Count}");
			Assert.True(9 == pool.PoolCombinationStatistics.Where(w => w.Symbol == Symbol.Success).Count());

			var successAtOne = pool.PoolCombinationStatistics.First(w => w.Symbol == Symbol.Success && w.Quantity == 1);

			Assert.True(1028 == successAtOne.Frequency);
			Assert.True(-688 == successAtOne.AlternateTotal);
		}

		[Fact]
		public void DieComparisonAdvanced()
		{
			var pools = DiceSeed.SeedDice().ProcessPools(ProficiencyThreeBoostTwo, ChallengeThreeSetbackTwo).CrossProduct();

			var pool = pools.First();

			Assert.True(3194 == pool.PositivePool.PoolResults.First(w => w.GetHashCode() == new PoolResult() { PoolResultSymbols = SuccessThreeAdvantageFour }.GetHashCode()).Frequency, "Frequency of Success(3) Advantage(4) did not equal 3194");
			Assert.True(44 == pool.PoolCombinationStatistics.Count);
			Assert.True(17 == pool.PoolCombinationStatistics.Where(w => w.Symbol == Symbol.Success).Count());

			var successAtOne = pool.PoolCombinationStatistics.First(w => w.Symbol == Symbol.Success && w.Quantity == 1);

			Assert.True(726936064 == successAtOne.Frequency);
			Assert.True(163682112 == successAtOne.AlternateTotal);
		}
	}
}
