using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging.Abstractions;
using Probability.Service;
using Visualizer.Controllers;
using Xunit;

namespace Visualizer.Tests
{
	[Collection(APICommon.DatabaseCollection)]
	public class SearchControllerTests
	{
		private readonly SearchController controller;

		public SearchControllerTests(DatabaseFixture fixture) => controller = new SearchController(new DataService(fixture.Context), new NullLogger<SearchController>());

		[Fact]
		public async Task SearchPositiveAsync()
		{
			var result = await controller.Get(APICommon.PositiveModel.Dice.ToList());
			result.Value.Statistics.Should().HaveCount(20, "Die statistics count incorrect");
			result.Value.Dice.Should().HaveCount(2, "Die count Incorrect");
		}

		[Fact]
		public async Task SearchNegativeAsync()
		{
			var result = await controller.Get(APICommon.NegativeModel.Dice.ToList());
			result.Result.Should().BeOfType<NotFoundResult>();
		}
	}
}
