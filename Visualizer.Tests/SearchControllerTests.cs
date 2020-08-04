using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Visualizer.Controllers;
using Xunit;

namespace Visualizer.Tests
{
	[Collection(APICommon.DatabaseCollection)]
	public class SearchControllerTests
	{
		private readonly SearchController controller;

		public SearchControllerTests(DatabaseFixture fixture)
		{
			controller = new SearchController(fixture.Context);
		}

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
