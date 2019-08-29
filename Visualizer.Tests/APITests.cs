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
			//context.Dice.AddRange(new List<Die> { DiceSeed.AbilityDie, DiceSeed.ChallengeDie });

			var ab1 = AbilityTwo.SeedPool();
			var dif2 = DifficultyTwo.SeedPool();
			context.Pools.Add(ab1);
			context.Pools.Add(dif2);
			context.SaveChanges();

			var pool = new PoolCombination(ab1, dif2).SeedStatistics();
			context.PoolCombinations.Add(pool);

			context.SaveChanges();

			this.context = context;
		}


		[Fact]
		public void DbTest()
		{
			Assert.True(context.Dice.Count() == 2, "Too many dice present");
			Assert.True(context.Pools.Count() == 2, "Too many pools present");
		}


		[Fact]
		public void SearchTest()
		{
			var controller = new SearchController(context);

			var model = new SearchViewModel(new PoolCombination(AbilityTwo, DifficultyTwo));

			var result = controller.Get(model.Dice.ToList());
			Assert.True(result != null, "Result was null");
			Assert.True(20 == result.Statistics.Count(), $"Die statistics did not equal 20.  Count was {result.Statistics.Count()}");

			controller.Dispose();
		}

		[Fact]
		public void ResultsTest()
		{

		}
	}
}
