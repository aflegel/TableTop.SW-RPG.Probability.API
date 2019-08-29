using System.Linq;
using DataFramework.Models;
using Xunit;
using static DataFramework.Models.Die;
using DataFramework.Context.Seed;
using System.Collections.Generic;

namespace Visualizer.Tests
{
	public class SeedingTests
	{
		private static Pool AbilityTwo => new Pool() { PoolDice = new List<PoolDie> { new PoolDie(DiceSeed.AbilityDie, 2) } };
		private static Pool DifficultyTwo => new Pool() { PoolDice = new List<PoolDie> { new PoolDie(DiceSeed.DifficultyDie, 2) } };

		private static Pool ProficiencyThreeBoostTwo => new Pool() { PoolDice = new List<PoolDie> { new PoolDie(DiceSeed.ProficiencyDie, 3), new PoolDie(DiceSeed.BoostDie, 2) } };
		private static Pool ChallengeThreeSetbackTwo => new Pool() { PoolDice = new List<PoolDie> { new PoolDie(DiceSeed.ChallengeDie, 3), new PoolDie(DiceSeed.SetbackDie, 2) } };

		private static List<PoolResultSymbol> SuccessThreeAdvantageFour => new List<PoolResultSymbol> { new PoolResultSymbol(Symbol.Success, 3), new PoolResultSymbol(Symbol.Advantage, 4) };

		[Fact]
		public void DieComparisonGenerator()
		{
			var pool = AbilityTwo.SeedPoolResults();

			Assert.True(15 == pool.PoolResults.Count, $"The number of results did not equal 15. Result was {pool.PoolResults.Count}");
			Assert.True(64 == pool.RollEstimation, $"The total outcomes did not equal 64. Result was {pool.PoolResults.Count}");
		}

		[Fact]
		public void DiePoolHashCode()
		{
			var pool = AbilityTwo.SeedPoolResults();

			Assert.True(pool.PoolText == "Ability 2", $"The text did not equal `Ability 2`. Result was {pool.PoolText}");
		}


		[Fact]
		public void DieComparisonBasic()
		{
			var pool = new PoolCombination(AbilityTwo.SeedPoolResults(), DifficultyTwo.SeedPoolResults()).SeedStatistics();

			Assert.True(20 == pool.PoolCombinationStatistics.Count, $"The number of results did not equal 15. Result was {pool.PoolCombinationStatistics.Count}");
			Assert.True(9 == pool.PoolCombinationStatistics.Where(w => w.Symbol == Symbol.Success).Count());

			var successAtOne = pool.PoolCombinationStatistics.First(w => w.Symbol == Symbol.Success && w.Quantity == 1);

			Assert.True(1028 == successAtOne.Frequency);
			Assert.True(-688 == successAtOne.AlternateTotal);
		}

		[Fact]
		public void DieComparisonAdvanced()
		{
			var pool = new PoolCombination(ProficiencyThreeBoostTwo.SeedPoolResults(), ChallengeThreeSetbackTwo.SeedPoolResults()).SeedStatistics();

			Assert.True(3194 == pool.PositivePool.PoolResults.First(w => w.GetHashCode() == new PoolResult() { PoolResultSymbols = SuccessThreeAdvantageFour }.GetHashCode()).Frequency, "Frequency of Success(3) Advantage(4) did not equal 3194");
			Assert.True(44 == pool.PoolCombinationStatistics.Count);
			Assert.True(17 == pool.PoolCombinationStatistics.Where(w => w.Symbol == Symbol.Success).Count());

			var successAtOne = pool.PoolCombinationStatistics.First(w => w.Symbol == Symbol.Success && w.Quantity == 1);

			Assert.True(726936064 == successAtOne.Frequency);
			Assert.True(163682112 == successAtOne.AlternateTotal);
		}
	}
}
