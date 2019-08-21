using System;
using System.Collections.Generic;
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
		[Fact]
		public void DieComparisonGenerator()
		{
			var testPool = new Pool();

			testPool.PoolDice.Add(new PoolDie(ProbabilityContextSeed.BuildAdvantage(), 2));

			new OutcomeGenerator(testPool);

			//There should be 15 unique results
			Assert.Equal(15, testPool.PoolResults.Count);

			//Total outcomes should be 64
			Assert.Equal(64, testPool.RollEstimation);
		}

		[Fact]
		public void DieComparison()
		{
			var positivePool = new Pool();
			positivePool.PoolDice.Add(new PoolDie(ProbabilityContextSeed.BuildAdvantage(), 2));

			new OutcomeGenerator(positivePool);

			var negativePool = new Pool();
			negativePool.PoolDice.Add(new PoolDie(ProbabilityContextSeed.BuildDifficulty(), 2));

			new OutcomeGenerator(negativePool);

			var pool = new PoolCombination(positivePool, negativePool);

			_ = new OutcomeComparison(pool);

			//There should be 20 unique results
			Assert.Equal(20, pool.PoolCombinationStatistics.Count);

			Assert.Equal(9, pool.PoolCombinationStatistics.Where(w => w.Symbol == Symbol.Success).Count());

			var tester = pool.PoolCombinationStatistics.First(w => w.Symbol == Symbol.Success && w.Quantity == 1);

			Assert.Equal(1028, tester.Frequency);
			Assert.Equal(-688, tester.AlternateTotal);
		}
	}
}
