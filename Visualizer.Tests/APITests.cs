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
			Assert.True(context.Dice.Count() == 7, $"Too many dice present: {context.Dice.Count()}");
			Assert.True(context.Dice.Where(w => w.Name == "Ability").First().Name == "Ability", "die name not set");
			Assert.True(context.PoolCombinationStatistics.Count() == 20, "Too many statistics present");
		}

		[Fact]
		public void SearchTest()
		{
			var controller = new SearchController(context);

			var model = new SearchViewModel(new PoolCombination(AbilityTwo, DifficultyTwo));

			var result = controller.Get(model.Dice.ToList());
			Assert.True(20 == result.Statistics.Count(), $"Die statistics did not equal 20.  Count was {result.Statistics.Count()}");

			controller.Dispose();
		}

		[Fact]
		public void ResultsTest()
		{
			var controller = new RollController(context);

			var model = new SearchViewModel(new PoolCombination(AbilityTwo, DifficultyTwo));

			var result = controller.Get(model.Dice.ToList());
			Assert.True(15 == result.PositiveRolls.Results.Count(), $"Die statistics did not equal 15.  Count was {result.PositiveRolls.Results.Count()}");

			controller.Dispose();
		}
	}
}
