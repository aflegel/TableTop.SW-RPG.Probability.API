using System.Linq;
using FluentAssertions;
using Probability.Generator;
using Probability.Service.Models;
using Visualizer.Tests;
using Xunit;

namespace DataFramework.Tests
{
	public class SeedingTests
	{
		[Fact]
		public void DieComparisonGenerator()
		{
			var pools = DiceSeed.SeedDice.SeedPools(APICommon.TwoOneZero, APICommon.TwoOneZero).SeedCombinationStatistics();

			pools.Should().HaveCount(4, "Incorrect number of pool combinations");

			var pool = pools.First().PositivePool;
			pool.PoolResults.Should().HaveCount(7, "Incorrect number of results");
			pool.RollEstimation().Should().Be(12, "Incorrect number of outcomes");
			pool.PoolResults.Sum(s => s.Frequency).Should().Be(12, "Incorrect number of outcomes");

			pool = pools.Skip(1).First().NegativePool;
			pool.PoolResults.Should().HaveCount(21, "Incorrect number of results");
			pool.RollEstimation().Should().Be(96, "Incorrect number of outcomes");
			pool.PoolResults.Sum(s => s.Frequency).Should().Be(96, "Incorrect number of outcomes");
		}

		[Fact]
		public void DiePoolToString()
		{
			var pool = DiceSeed.SeedDice.SeedPools(APICommon.ZeroThreeTwo, APICommon.ZeroZeroZero).Item1.First();

			pool.ToString().Should().Be("Boost 2, Proficiency 3", "Incorrect pool text");
		}


		[Fact]
		public void DieComparisonBasic()
		{
			var pool = DiceSeed.SeedDice.SeedPools(APICommon.TwoZeroZero, APICommon.TwoZeroZero).SeedCombinationStatistics().First();

			pool.PoolCombinationStatistics.Should().HaveCount(20, "Incorrect number of statistics");
			pool.PoolCombinationStatistics.Where(w => w.Symbol == Symbol.Success).Should().HaveCount(9, "Incorrect number of statistics");

			var successAtOne = pool.PoolCombinationStatistics.First(w => w.Symbol == Symbol.Success && w.Quantity == 1);

			successAtOne.Frequency.Should().Be(1028, "Incorrect number of statistics");
			successAtOne.AlternateTotal.Should().Be(-688, "Incorrect number of statistics");
		}

		[Fact]
		public void DieComparisonAdvanced()
		{
			var pool = DiceSeed.SeedDice.SeedPools(APICommon.ZeroThreeTwo, APICommon.ZeroThreeTwo).SeedCombinationStatistics().First();

			pool.PositivePool.PoolResults.First(w => w.GetHashCode() == new PoolResult() { PoolResultSymbols = APICommon.SuccessThreeAdvantageFour }.GetHashCode()).Frequency.Should().Be(3194, "Frequency of Success(3) Advantage(4) incorrect");
			pool.PoolCombinationStatistics.Should().HaveCount(44, "Incorrect number of statistics");
			pool.PoolCombinationStatistics.Where(w => w.Symbol == Symbol.Success).Should().HaveCount(17, "Incorrect number of statistics");

			var successAtOne = pool.PoolCombinationStatistics.First(w => w.Symbol == Symbol.Success && w.Quantity == 1);

			successAtOne.Frequency.Should().Be(726936064, "Frequency Incorrect");
			successAtOne.AlternateTotal.Should().Be(163682112, "Alternate Total Incorrect");
		}
	}
}
