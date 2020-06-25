using Xunit;
using Visualizer.Controllers;
using System.Linq;
using FluentAssertions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

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
			result.Value.PositiveResults.Should().HaveCount(15, "Positive statistics count incorrect");
			result.Value.NegativeResults.Should().HaveCount(15, "Negative statistics count incorrect");
		}

		[Fact]
		public void ResultsNegative()
		{
			var result = controller.Get(APICommon.NegativeModel.Dice.ToList());
			result.Result.Should().BeOfType<NotFoundResult>();
		}
	}
}
