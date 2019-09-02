using DataFramework.Models;
using Xunit;
using DataFramework.Context.Seed;
using DataFramework.Context;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using Visualizer.Controllers;
using Visualizer.Models;
using System.Linq;

namespace Visualizer.Tests
{
	public class APITests
	{
		private readonly ProbabilityContext context;

		private static Pool AbilityTwo => new Pool() { PoolDice = new List<PoolDie> { new PoolDie(DiceSeed.AbilityDie, 2) } };
		private static Pool DifficultyTwo => new Pool() { PoolDice = new List<PoolDie> { new PoolDie(DiceSeed.DifficultyDie, 2) } };

		public APITests()
		{
			var builder = new DbContextOptionsBuilder<ProbabilityContext>().UseInMemoryDatabase(databaseName: "Add_writes_to_database");

			var context = new ProbabilityContext(builder.Options);

			//prevents initialization doubling
			if (!context.Dice.Any(w => w.Name == "Ability"))
			{
				context.SeedDice();
				context.SaveChanges();
				var pool = new PoolCombination(context.SeedPool(2).SeedPoolResults(), context.SeedPool(difficulty: 2).SeedPoolResults()).SeedStatistics();
				context.PoolCombinations.Add(pool);

				context.SaveChanges();
			}

			this.context = context;
		}

		[Fact]
		public void DbTest()
		{
			Assert.True(context.Dice.Count() == 7, $"Incorrect dice present: {context.Dice.Count()}");
			Assert.True(context.Dice.Where(w => w.Name == "Ability").First().Name == "Ability", "Die Name not set");

			var statistics = context.PoolCombinationStatistics;
			Assert.True(statistics.Count() == 20, $"Incorrect statistics present: {statistics.Count()}");

			var pools = context.GetPositivePools();
			Assert.True(pools.Count() == 1, $"Incorrect pool selection: {pools.Count()}");
		}

		[Fact]
		public void SearchPositive()
		{
			var controller = new SearchController(context);

			var model = new SearchViewModel(new PoolCombination(AbilityTwo, DifficultyTwo));

			var result = controller.Get(model.Dice.ToList());
			Assert.True(20 == result.Statistics.Count(), $"Die statistics count incorrect.  Count was {result.Statistics.Count()}");
			Assert.True(2 == result.Dice.Count(), $"Die count Incorrect.  Count was {result.Statistics.Count()}");

			controller.Dispose();
		}

		[Fact]
		public void SearchNegative()
		{
			var controller = new SearchController(context);

			var model = new SearchViewModel(new PoolCombination(AbilityTwo, new Pool()));

			var result = controller.Get(model.Dice.ToList());
			Assert.True(0 == result.Statistics.Count(), $"Statistics count incorrect.  Count was {result.Statistics.Count()}");
			Assert.True(0 == result.Dice.Count(), $"Die count Incorrect.  Count was {result.Statistics.Count()}");

			controller.Dispose();
		}

		[Fact]
		public void ResultsPositive()
		{
			var controller = new RollController(context);

			var model = new SearchViewModel(new PoolCombination(AbilityTwo, DifficultyTwo));

			var result = controller.Get(model.Dice.ToList());
			Assert.True(15 == result.PositiveRolls.Results.Count(), $"Statistics count incorrect.  Count was {result.PositiveRolls.Results.Count()}");
			Assert.True(15 == result.NegativeRolls.Results.Count(), $"Statistics count incorrect.  Count was {result.PositiveRolls.Results.Count()}");

			controller.Dispose();
		}

		[Fact]
		public void ResultsNegative()
		{
			var controller = new RollController(context);

			var model = new SearchViewModel(new PoolCombination(AbilityTwo, new Pool()));

			var result = controller.Get(model.Dice.ToList());
			Assert.True(0 == result.PositiveRolls.Results.Count(), $"Statistics count incorrect.  Count was {result.PositiveRolls.Results.Count()}");
			Assert.True(0 == result.NegativeRolls.Results.Count(), $"Statistics count incorrect.  Count was {result.NegativeRolls.Results.Count()}");

			controller.Dispose();
		}
	}
}
