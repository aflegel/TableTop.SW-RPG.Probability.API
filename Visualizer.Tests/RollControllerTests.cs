﻿using System.Linq;
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
	public class RollControllerTests
	{
		private readonly RollController controller;

		public RollControllerTests(DatabaseFixture fixture) => controller = new RollController(new DataService(fixture.Context), new NullLogger<RollController>());

		[Fact]
		public async Task ResultsPositive()
		{
			var result = await controller.Get(APICommon.PositiveModel.Dice.ToList());
			result.Value.PositiveResults.Should().HaveCount(15, "Positive statistics count incorrect");
			result.Value.NegativeResults.Should().HaveCount(15, "Negative statistics count incorrect");
		}

		[Fact]
		public async Task ResultsNegativeAsync()
		{
			var result = await controller.Get(APICommon.NegativeModel.Dice.ToList());
			result.Result.Should().BeOfType<NotFoundResult>();
		}
	}
}
