using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Probability.Service.Models;
using Visualizer.Controllers;
using Visualizer.Models;
using Xunit;

namespace Visualizer.Tests
{
	[Collection(DatabaseCollection)]
	public class APICommon
	{
		public const string DatabaseCollection = "InMemoryContext collection";

		public static (IEnumerable<int>, IEnumerable<int>, IEnumerable<int>) TwoTwoZero => (new List<int> { 1, 2 }, new List<int> { 0, 1 }, new List<int> { 0 });

		private static Pool MakePool(string name, int quantity) => new Pool() { PoolDice = new List<PoolDie> { new PoolDie(new Die { Name = name }, quantity) } };
		private static Pool AbilityTwo => MakePool(nameof(Ability), 2);
		private static Pool DifficultyTwo => MakePool(nameof(Difficulty), 2);
		private static SearchViewModel MakeModel(IEnumerable<PoolDie> dice) => new SearchViewModel((new List<PoolCombinationStatistic> { }, dice));
		public static SearchViewModel PositiveModel => MakeModel(AbilityTwo.PoolDice.Union(DifficultyTwo.PoolDice));
		public static SearchViewModel NegativeModel => MakeModel(AbilityTwo.PoolDice);

		private readonly HealthCheckController controller;

		public APICommon(DatabaseFixture fixture)
		{
			controller = new HealthCheckController(fixture.Context);
		}

		[Fact]
		public async Task HealthCheck()
		{
			var result = await controller.Get() as OkResult;
			result.Should().NotBeNull();
			result.StatusCode.Should().Be(200, "Positive statistics count incorrect");
		}
	}
}
