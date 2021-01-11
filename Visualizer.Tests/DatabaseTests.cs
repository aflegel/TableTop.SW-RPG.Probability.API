using System;
using System.Linq;
using System.Threading.Tasks;
using DataFramework.Context;
using DataFramework.Models;
using DataFramework.Services;
using FluentAssertions;
using Functional;
using Microsoft.EntityFrameworkCore;
using Xunit;


namespace Visualizer.Tests
{
	[Collection(APICommon.DatabaseCollection)]
	public class DatabaseTests : IClassFixture<DatabaseFixture>
	{
		private readonly ProbabilityContext context;

		public DatabaseTests(DatabaseFixture fixture) => context = fixture.Context;

		[Fact]
		public void DbTest()
		{
			context.Dice.Should().HaveCount(4, "Incorrect dice present");
			context.Dice.AsQueryable().First(w => w.Name == "Ability").Should().NotBeNull("Die Name not set");

			context.Pools.Should().HaveCount(8, "Incorrect pools present");
			context.PoolCombinationStatistics.Should().HaveCount(272, "Incorrect statistics present");

			var poolSearch = context.Pools.AsQueryable().Where(pool => pool.PoolDice.Any(die => DieExtensions.PositiveDice.Contains(die.Die.Name)))
				.Include(i => i.PositivePoolCombinations)
						.ThenInclude(tti => tti.PoolCombinationStatistics)
				.Include(i => i.PoolResults)
						.ThenInclude(tti => tti.PoolResultSymbols);

			poolSearch.Should().HaveCount(4, "Incorrect positive pool count");
		}

		[Fact]
		public async Task FunctionalTestAsync()
		{
			var service = new DataService(context);

			var test = await service.GetPoolIdsF(APICommon.AbilityTwo);

			test.Should().BeNone();

			var test2 = await service.GetPoolIdsF(new Pool { PoolDice = APICommon.AbilityTwo.PoolDice.Union(APICommon.DifficultyTwo.PoolDice).ToList() });

			test2.Should().BeSome();
		}
	}
}
