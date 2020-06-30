using System.Linq;
using DataFramework.Context;
using DataFramework.Models;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Visualizer.Tests
{
	[Collection(APICommon.DatabaseCollection)]
	public class DatabaseTests : IClassFixture<DatabaseFixture>
	{
		private readonly ProbabilityContext context;

		public DatabaseTests(DatabaseFixture fixture)
		{
			context = fixture.Context;
		}

		[Fact]
		public void DbTest()
		{
			context.Dice.Should().HaveCount(4, "Incorrect dice present");
			context.Dice.First(w => w.Name == "Ability").Should().NotBeNull("Die Name not set");

			context.Pools.Should().HaveCount(8, "Incorrect pools present");
			context.PoolCombinationStatistics.Should().HaveCount(272, "Incorrect statistics present");

			var poolSearch = context.Pools.AsQueryable().Where(pool => pool.PoolDice.Any(die => DieExtensions.PositiveDice.Contains(die.Die.Name.GetName())))
				.Include(i => i.PositivePoolCombinations)
						.ThenInclude(tti => tti.PoolCombinationStatistics)
				.Include(i => i.PoolResults)
						.ThenInclude(tti => tti.PoolResultSymbols);

			poolSearch.Should().HaveCount(4, "Incorrect positive pool count");
		}
	}
}
