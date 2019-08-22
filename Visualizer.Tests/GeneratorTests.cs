using System.Collections.ObjectModel;
using System.Linq;
using DataFramework.Context;
using DataFramework.Models;
using DataGenerator.Models;
using Xunit;
using static DataFramework.Models.Die;

namespace Visualizer.Tests
{
	public class GeneratorTests
	{
		private static Pool AbilityTwo => new Pool() { PoolDice = new Collection<PoolDie> { new PoolDie(ProbabilityContextSeed.AbilityDie, 2) } };
		private static Pool DifficultyTwo => new Pool() { PoolDice = new Collection<PoolDie> { new PoolDie(ProbabilityContextSeed.DifficultyDie, 2) } };

		private static Pool ProficiencyThreeBoostTwo => new Pool() { PoolDice = new Collection<PoolDie> { new PoolDie(ProbabilityContextSeed.ProficiencyDie, 3), new PoolDie(ProbabilityContextSeed.BoostDie, 2) } };
		private static Pool ChallengeThreeSetbackTwo => new Pool() { PoolDice = new Collection<PoolDie> { new PoolDie(ProbabilityContextSeed.ChallengeDie, 3), new PoolDie(ProbabilityContextSeed.SetbackDie, 2) } };

		private static Collection<PoolResultSymbol> SuccessThreeAdvantageFour => new Collection<PoolResultSymbol> { new PoolResultSymbol(Symbol.Success, 3), new PoolResultSymbol(Symbol.Advantage, 4) };

		[Fact]
		public void DieComparisonGenerator()
		{
			var pool = AbilityTwo.BuildOutcomes();

			//There should be 15 unique results
			Assert.True(15 == pool.PoolResults.Count);

			//Total outcomes should be 64
			Assert.True(64 == pool.RollEstimation);
		}

		[Fact]
		public void DieComparisonBasic()
		{
			var positivePool = AbilityTwo.BuildOutcomes();
			var negativePool = DifficultyTwo.BuildOutcomes();

			var pool = new PoolCombination(positivePool, negativePool).CompareOutcomes();

			//There should be 20 unique results
			Assert.True(20 == pool.PoolCombinationStatistics.Count);

			Assert.True(9 == pool.PoolCombinationStatistics.Where(w => w.Symbol == Symbol.Success).Count());

			var successAtOne = pool.PoolCombinationStatistics.First(w => w.Symbol == Symbol.Success && w.Quantity == 1);

			Assert.True(1028 == successAtOne.Frequency);
			Assert.True(-688 == successAtOne.AlternateTotal);
		}

		[Fact]
		public void DieComparisonAdvanced()
		{
			var positivePool = ProficiencyThreeBoostTwo.BuildOutcomes();
			var negativePool = ChallengeThreeSetbackTwo.BuildOutcomes();

			var pool = new PoolCombination(positivePool, negativePool).CompareOutcomes();

			//var match = result.GetMatch(mergedPool);
			var test = positivePool.PoolResults.GetMatch(new PoolResult() { PoolResultSymbols = SuccessThreeAdvantageFour });

			Assert.True(3194 == test.Frequency, "Frequency does not match");

			Assert.True(44 == pool.PoolCombinationStatistics.Count);

			Assert.True(17 == pool.PoolCombinationStatistics.Where(w => w.Symbol == Symbol.Success).Count());

			var successAtOne = pool.PoolCombinationStatistics.First(w => w.Symbol == Symbol.Success && w.Quantity == 1);

			Assert.True(726936064 == successAtOne.Frequency);
			Assert.True(163682112 == successAtOne.AlternateTotal);
		}
	}
}
