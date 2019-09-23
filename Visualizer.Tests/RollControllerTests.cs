using Xunit;
using Visualizer.Controllers;
using System.Linq;
using FluentAssertions;

namespace Visualizer.Tests
{
	[Collection(APICommon.DatabaseCollection)]
	public class RollControllerTests
	{
		private readonly RollController controller;

		public RollControllerTests(DatabaseFixture fixture)
		{
			controller = new RollController(fixture.Context);
		}

		[Fact]
		public void ResultsPositive()
		{
			var result = controller.Get(APICommon.PositiveModel.Dice.ToList());
			result.PositiveResults.Should().HaveCount(15, "Positive statistics count incorrect");
			result.NegativeResults.Should().HaveCount(15, "Negative statistics count incorrect");
		}

		[Fact]
		public void ResultsNegative()
		{
			var result = controller.Get(APICommon.NegativeModel.Dice.ToList());
			result.PositiveResults.Should().HaveCount(0, "Positive statistics count incorrect");
			result.NegativeResults.Should().HaveCount(0, "Negative statistics count incorrect");
		}
	}
}
