using System;
using DataFramework.Models;
using Xunit;
using DataFramework.Context.Seed;
using DataFramework.Context;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using FluentAssertions;

namespace Visualizer.Tests
{
	public class DatabaseFixture : IDisposable
	{
		public DatabaseFixture()
		{
			var builder = new DbContextOptionsBuilder<ProbabilityContext>().UseInMemoryDatabase(databaseName: "InMemoryContext");

			var context = new ProbabilityContext(builder.Options);

			//prevents initialization doubling
			if (!context.Dice.Any(w => w.Name == "Ability"))
			{
				var pools = DiceSeed.SeedDice().ProcessPools((Enumerable.Range(1, 2), Enumerable.Range(0, 2), Enumerable.Range(0, 1)), (Enumerable.Range(1, 2), Enumerable.Range(0, 2), Enumerable.Range(0, 1))).CrossProduct();

				context.PoolCombinations.AddRange(pools);

				context.SaveChanges();
			}

			Context = context;
		}

		public void Dispose() { }

		public ProbabilityContext Context { get; private set; }
	}

	/// <summary>
	/// This class has no code, and is never created. Its purpose is simply to be the place to apply [CollectionDefinition] and all the ICollectionFixture<> interfaces.
	/// </summary>
	[CollectionDefinition(APICommon.DatabaseCollection)]
	public class DatabaseCollection : ICollectionFixture<DatabaseFixture> { }

	[Collection(APICommon.DatabaseCollection)]
	public class DatabaseTests : IClassFixture<DatabaseFixture>
	{
		private readonly DatabaseFixture fixture;

		public DatabaseTests(DatabaseFixture fixture)
		{
			this.fixture = fixture;
		}

		[Fact]
		public void DbTest()
		{
			fixture.Context.Dice.Should().HaveCount(4, "Incorrect dice present");
			fixture.Context.Dice.First(w => w.Name == "Ability").Should().NotBeNull("Die Name not set");

			fixture.Context.Pools.Should().HaveCount(8, "Incorrect pools present");
			fixture.Context.PoolCombinationStatistics.Should().HaveCount(272, "Incorrect statistics present");

			var poolSearch = fixture.Context.Pools.Where(pool => pool.PoolDice.Any(die => DieExtensions.PositiveDice.Contains(die.Die.Name.GetName())))
				.Include(i => i.PositivePoolCombinations)
						.ThenInclude(tti => tti.PoolCombinationStatistics)
				.Include(i => i.PoolResults)
						.ThenInclude(tti => tti.PoolResultSymbols);

			poolSearch.Should().HaveCount(4, "Incorrect positive pool count");
		}
	}
}
