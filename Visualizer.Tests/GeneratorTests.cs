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

		[Fact]
		public void DieComparisonGenerator()
		{
			var pool = AbilityTwo;

			new OutcomeGenerator(pool);

			//There should be 15 unique results
			Assert.Equal(15, pool.PoolResults.Count);

			//Total outcomes should be 64
			Assert.Equal(64, pool.RollEstimation);
		}

		[Fact]
		public void DieComparison()
		{
			var positivePool = AbilityTwo;
			var negativePool = DifficultyTwo;

			new OutcomeGenerator(positivePool);
			new OutcomeGenerator(negativePool);

			var pool = new PoolCombination(positivePool, negativePool);

			new OutcomeComparison(pool);

			//There should be 20 unique results
			Assert.Equal(20, pool.PoolCombinationStatistics.Count);

			Assert.Equal(9, pool.PoolCombinationStatistics.Where(w => w.Symbol == Symbol.Success).Count());

			var successAtOne = pool.PoolCombinationStatistics.First(w => w.Symbol == Symbol.Success && w.Quantity == 1);

			Assert.Equal(1028, successAtOne.Frequency);
			Assert.Equal(-688, successAtOne.AlternateTotal);
		}
	}
}
