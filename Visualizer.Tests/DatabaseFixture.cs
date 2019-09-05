using System;
using Xunit;
using DataFramework.Context.Seed;
using DataFramework.Context;
using Microsoft.EntityFrameworkCore;
using System.Linq;

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
				var pools = DiceSeed.SeedDice().SeedPools(APICommon.TwoTwoZero, APICommon.TwoTwoZero).SeedCombinationStatistics();

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
}
