using System;
using System.Collections.ObjectModel;
using System.Linq;
using DataFramework.Models;
using Xunit;
using static DataFramework.Models.Die;
using DataFramework.Context.Seed;

namespace Visualizer.Tests
{
	public class GeneratorTests
	{
		private static Pool AbilityTwo => new Pool() { PoolDice = new Collection<PoolDie> { new PoolDie(SeedDice.AbilityDie, 2) } };
		private static Pool DifficultyTwo => new Pool() { PoolDice = new Collection<PoolDie> { new PoolDie(SeedDice.DifficultyDie, 2) } };

		private static Pool ProficiencyThreeBoostTwo => new Pool() { PoolDice = new Collection<PoolDie> { new PoolDie(SeedDice.ProficiencyDie, 3), new PoolDie(SeedDice.BoostDie, 2) } };
		private static Pool ChallengeThreeSetbackTwo => new Pool() { PoolDice = new Collection<PoolDie> { new PoolDie(SeedDice.ChallengeDie, 3), new PoolDie(SeedDice.SetbackDie, 2) } };

		private static Collection<PoolResultSymbol> SuccessThreeAdvantageFour => new Collection<PoolResultSymbol> { new PoolResultSymbol(Symbol.Success, 3), new PoolResultSymbol(Symbol.Advantage, 4) };

		[Fact]
		public void DieComparisonGenerator()
		{
			var pool = AbilityTwo.BuildPoolResults();

			Assert.True(15 == pool.PoolResults.Count, $"The number of results did not equal 15. Result was {pool.PoolResults.Count}");
			Assert.True(64 == pool.RollEstimation, $"The total outcomes did not equal 64. Result was {pool.PoolResults.Count}");
		}

		[Fact]
		public void DieComparisonBasic()
		{
			var pool = new PoolCombination(AbilityTwo.BuildPoolResults(), DifficultyTwo.BuildPoolResults()).BuildPoolStatistics();

			Assert.True(20 == pool.PoolCombinationStatistics.Count, $"The number of results did not equal 15. Result was {pool.PoolCombinationStatistics.Count}");
			Assert.True(9 == pool.PoolCombinationStatistics.Where(w => w.Symbol == Symbol.Success).Count());

			var successAtOne = pool.PoolCombinationStatistics.First(w => w.Symbol == Symbol.Success && w.Quantity == 1);

			Assert.True(1028 == successAtOne.Frequency);
			Assert.True(-688 == successAtOne.AlternateTotal);
		}

		[Fact]
		public void DieComparisonAdvanced()
		{
			var pool = new PoolCombination(ProficiencyThreeBoostTwo.BuildPoolResults(), ChallengeThreeSetbackTwo.BuildPoolResults()).BuildPoolStatistics();

			Assert.True(3194 == pool.PositivePool.PoolResults.GetMatch(new PoolResult() { PoolResultSymbols = SuccessThreeAdvantageFour }).Frequency, "Frequency of Success(3) Advantage(4) did not equal 3194");
			Assert.True(44 == pool.PoolCombinationStatistics.Count);
			Assert.True(17 == pool.PoolCombinationStatistics.Where(w => w.Symbol == Symbol.Success).Count());

			var successAtOne = pool.PoolCombinationStatistics.First(w => w.Symbol == Symbol.Success && w.Quantity == 1);

			Assert.True(726936064 == successAtOne.Frequency);
			Assert.True(163682112 == successAtOne.AlternateTotal);
		}
	}
}
