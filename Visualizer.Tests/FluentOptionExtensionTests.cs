using System;
using System.Threading.Tasks;
using FluentAssertions;
using Functional;
using Xunit;

namespace Visualizer.Tests
{
	public class FluentOptionExtenionTests
	{
		[Fact]
		public void NoneBeSome()
		{
			Action act = () => Option.None<int>().Should().BeSome();

			act.Should().Throw<Exception>().WithMessage("*but found None*");
		}

		[Fact]
		public void NoneBeNone()
		{
			Action act = () => Option.None<int>().Should().BeNone();

			act.Should().NotThrow<Exception>();
		}

		[Fact]
		public void SomeBeNone()
		{
			Action act = () => Option.Some(100).Should().BeNone();

			act.Should().Throw<Exception>().WithMessage("*but found Some*");
		}

		[Fact]
		public void SomeBeSome()
		{
			Action act = () => Option.Some(100).Should().BeSome();

			act.Should().NotThrow<Exception>();
		}

		[Fact]
		public void NoneBeSomeValue()
		{
			Action act = () => Option.None<int>().Should().BeSome(100);

			act.Should().Throw<Exception>().WithMessage("*but found None*");
		}

		[Fact]
		public void SomeBeSomeIncorrectValue()
		{
			Action act = () => Option.Some(100).Should().BeSome(50);

			act.Should().Throw<Exception>().WithMessage("*but found 100*");
		}

		[Fact]
		public void SomeBeSomeValue()
		{
			Action act = () => Option.Some(100).Should().BeSome(100);

			act.Should().NotThrow();
		}

		[Fact]
		public void NoneBeNoneAsync()
		{
			Func<Task> act = async () => await Task.FromResult(Option.None<int>()).Should().BeNoneAsync();

			act.Should().NotThrowAsync<Exception>();
		}

		[Fact]
		public void NoneBeSomeAsync()
		{
			Func<Task> act = async () => await Task.FromResult(Option.None<int>()).Should().BeSomeAsync();

			act.Should().ThrowAsync<Exception>().WithMessage("*but found None*");
		}

		[Fact]
		public void SomeBeNoneAsync()
		{
			Func<Task> act = async () => await Task.FromResult(Option.Some(100)).Should().BeNoneAsync();


			act.Should().ThrowAsync<Exception>().WithMessage("*but found Some*");
		}

		[Fact]
		public void SomeBeSomeAsync()
		{
			Func<Task> act = async () => await Task.FromResult(Option.Some(100)).Should().BeSomeAsync();

			act.Should().NotThrowAsync<Exception>();
		}

		[Fact]
		public void NoneBeSomeValueAsync()
		{
			Func<Task> act = async () => await Task.FromResult(Option.None<int>()).Should().BeSomeAsync(100);

			act.Should().ThrowAsync<Exception>().WithMessage("*but found None*");
		}

		[Fact]
		public void SomeBeSomeIncorrectValueAsync()
		{
			Func<Task> act = async () => await Task.FromResult(Option.Some(100)).Should().BeSomeAsync(50);

			act.Should().ThrowAsync<Exception>().WithMessage("*but found 100*");
		}

		[Fact]
		public void SomeBeSomeValueAsync()
		{
			Func<Task> act = async () => await Task.FromResult(Option.Some(100)).Should().BeSomeAsync(100);

			act.Should().NotThrowAsync();
		}
	}
}
