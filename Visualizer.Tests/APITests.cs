using DataFramework.Models;
using Xunit;
using DataFramework.Context.Seed;
using DataFramework.Context;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using Visualizer.Controllers;
using Visualizer.Models;
using System.Linq;
using static DataFramework.Models.DieExtensions;

namespace Visualizer.Tests
{
	public class APITests
	{
		private readonly ProbabilityContext context;

		private static Pool AbilityTwo => new Pool() { PoolDice = new List<PoolDie> { new PoolDie(DiceSeed.AbilityDie, 2) } };
		private static Pool DifficultyTwo => new Pool() { PoolDice = new List<PoolDie> { new PoolDie(DiceSeed.DifficultyDie, 2) } };

		private static SearchViewModel PositiveModel => new SearchViewModel(new PoolCombination() { PositivePool = AbilityTwo, NegativePool = DifficultyTwo });

		private static SearchViewModel NegativeModel => new SearchViewModel(new PoolCombination() { PositivePool = AbilityTwo, NegativePool = new Pool() });

		public APITests()
		{
			var builder = new DbContextOptionsBuilder<ProbabilityContext>().UseInMemoryDatabase(databaseName: "Add_writes_to_database");

			var context = new ProbabilityContext(builder.Options);

			//prevents initialization doubling
			if (!context.Dice.Any(w => w.Name == "Ability"))
			{
				var pools = DiceSeed.SeedDice().ProcessPools((Enumerable.Range(1, 2), Enumerable.Range(0, 2), Enumerable.Range(0, 1)), (Enumerable.Range(1, 2), Enumerable.Range(0, 2), Enumerable.Range(0, 1))).CrossProduct();

				context.PoolCombinations.AddRange(pools);

				context.SaveChanges();
			}

			this.context = context;
		}

		[Fact]
		public void DbTest()
		{
			Assert.True(context.Dice.Count() == 4, $"Incorrect dice present: {context.Dice.Count()}");
			Assert.True(context.Dice.Where(w => w.Name == "Ability").First().Name == "Ability", "Die Name not set");

			var poolSearch = context.Pools.Where(pool => pool.PoolDice.Any(die => PositiveDice.Contains(die.Die.Name.GetName())))
				.Include(i => i.PositivePoolCombinations)
						.ThenInclude(tti => tti.PoolCombinationStatistics)
				.Include(i => i.PoolResults)
						.ThenInclude(tti => tti.PoolResultSymbols);
			Assert.True(poolSearch.Count() == 4, $"Incorrect positive pool count: {poolSearch.Count()}");

			var pools = context.Pools;
			Assert.True(pools.Count() == 8, $"Incorrect pools present: {pools.Count()}");

			var statistics = context.PoolCombinationStatistics;
			Assert.True(statistics.Count() == 272, $"Incorrect statistics present: {statistics.Count()}");
		}

		[Fact]
		public void SearchPositive()
		{
			var controller = new SearchController(context);

			var result = controller.Get(PositiveModel.Dice.ToList());
			Assert.True(20 == result.Statistics.Count(), $"Die statistics count incorrect.  Count was {result.Statistics.Count()}");
			Assert.True(2 == result.Dice.Count(), $"Die count Incorrect.  Count was {result.Statistics.Count()}");

			controller.Dispose();
		}

		[Fact]
		public void SearchNegative()
		{
			var controller = new SearchController(context);

			var result = controller.Get(NegativeModel.Dice.ToList());
			Assert.True(0 == result.Statistics.Count(), $"Statistics count incorrect.  Count was {result.Statistics.Count()}");
			Assert.True(0 == result.Dice.Count(), $"Die count Incorrect.  Count was {result.Statistics.Count()}");

			controller.Dispose();
		}

		[Fact]
		public void ResultsPositive()
		{
			var controller = new RollController(context);

			var result = controller.Get(PositiveModel.Dice.ToList());
			Assert.True(15 == result.PositiveRolls.Results.Count(), $"Statistics count incorrect.  Count was {result.PositiveRolls.Results.Count()}");
			Assert.True(15 == result.NegativeRolls.Results.Count(), $"Statistics count incorrect.  Count was {result.PositiveRolls.Results.Count()}");

			controller.Dispose();
		}

		[Fact]
		public void ResultsNegative()
		{
			var controller = new RollController(context);

			var result = controller.Get(NegativeModel.Dice.ToList());
			Assert.True(0 == result.PositiveRolls.Results.Count(), $"Statistics count incorrect.  Count was {result.PositiveRolls.Results.Count()}");
			Assert.True(0 == result.NegativeRolls.Results.Count(), $"Statistics count incorrect.  Count was {result.NegativeRolls.Results.Count()}");

			controller.Dispose();
		}
	}
}
