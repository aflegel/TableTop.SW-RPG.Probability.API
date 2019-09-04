using Xunit;
using Visualizer.Controllers;
using System.Linq;
using FluentAssertions;

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
		public void SearchPositive()
		{
			var result = controller.Get(APICommon.PositiveModel.Dice.ToList());
			result.Statistics.Should().HaveCount(20, "Die statistics count incorrect");
			result.Dice.Should().HaveCount(2, "Die count Incorrect");

			controller.Dispose();
		}

		[Fact]
		public void SearchNegative()
		{
			var result = controller.Get(APICommon.NegativeModel.Dice.ToList());
			result.Statistics.Should().HaveCount(0, "Die statistics count incorrect");
			result.Dice.Should().HaveCount(0, "Die count Incorrect");

			controller.Dispose();
		}
	}
}
