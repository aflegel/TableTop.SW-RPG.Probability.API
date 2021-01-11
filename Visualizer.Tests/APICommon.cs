using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataFramework.Models;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Visualizer.Controllers;
using Visualizer.Models;
using Xunit;

namespace Visualizer.Tests
{
	[Collection(DatabaseCollection)]
	public class APICommon
	{
		public const string DatabaseCollection = "InMemoryContext collection";

		public static (IEnumerable<int>, IEnumerable<int>, IEnumerable<int>) TwoZeroZero => (new List<int> { 2 }, new List<int> { 0 }, new List<int> { 0 });
		public static (IEnumerable<int>, IEnumerable<int>, IEnumerable<int>) ZeroThreeTwo => (new List<int> { 3 }, new List<int> { 3 }, new List<int> { 2 });
		public static (IEnumerable<int>, IEnumerable<int>, IEnumerable<int>) TwoTwoZero => (new List<int> { 1, 2 }, new List<int> { 0, 1 }, new List<int> { 0 });
		public static (IEnumerable<int>, IEnumerable<int>, IEnumerable<int>) TwoOneZero => (new List<int> { 1, 2 }, new List<int> { 1 }, new List<int> { 0 });
		public static (IEnumerable<int>, IEnumerable<int>, IEnumerable<int>) ZeroZeroZero => (new List<int> { 0 }, new List<int> { 0 }, new List<int> { 0 });

		public static List<PoolResultSymbol> SuccessThreeAdvantageFour => new List<PoolResultSymbol> { new PoolResultSymbol(Symbol.Success, 3), new PoolResultSymbol(Symbol.Advantage, 4) };

		private static Pool MakePool(string name, int quantity) => new Pool() { PoolDice = new List<PoolDie> { new PoolDie(new Die { Name = name }, quantity) } };
		public static Pool AbilityTwo => MakePool(nameof(Ability), 2);
		public static Pool DifficultyTwo => MakePool(nameof(Difficulty), 2);
		private static SearchViewModel MakeModel(IEnumerable<PoolDie> dice) => new SearchViewModel((new List<PoolCombinationStatistic> { }, dice));
		public static SearchViewModel PositiveModel => MakeModel(AbilityTwo.PoolDice.Union(DifficultyTwo.PoolDice));
		public static SearchViewModel NegativeModel => MakeModel(AbilityTwo.PoolDice);

		private readonly HealthCheckController controller;

		public APICommon(DatabaseFixture fixture) => controller = new HealthCheckController(fixture.Context);

		[Fact]
		public async Task HealthCheck()
		{
			var result = await controller.Get() as OkResult;
			result.Should().NotBeNull();
			result.StatusCode.Should().Be(200, "Positive statistics count incorrect");
		}
	}
}
