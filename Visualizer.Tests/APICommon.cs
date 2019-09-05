using DataFramework.Models;
using DataFramework.Context.Seed;
using System.Collections.Generic;
using Visualizer.Models;
using System.Linq;

namespace Visualizer.Tests
{
	public class APICommon
	{
		public const string DatabaseCollection = "InMemoryContext collection";

		public static (IEnumerable<int>, IEnumerable<int>, IEnumerable<int>) TwoZeroZero => (new List<int> { 2 }, new List<int> { 0 }, new List<int> { 0 });
		public static (IEnumerable<int>, IEnumerable<int>, IEnumerable<int>) ZeroThreeTwo => (new List<int> { 3 }, new List<int> { 3 }, new List<int> { 2 });
		public static (IEnumerable<int>, IEnumerable<int>, IEnumerable<int>) TwoTwoZero => (new List<int> { 1, 2 }, new List<int> { 0, 1 }, new List<int> { 0 });
		public static (IEnumerable<int>, IEnumerable<int>, IEnumerable<int>) TwoOneZero => (new List<int> { 1, 2 }, new List<int> { 1 }, new List<int> { 0 });
		public static (IEnumerable<int>, IEnumerable<int>, IEnumerable<int>) ZeroZeroZero => (new List<int> { 0 }, new List<int> { 0 }, new List<int> { 0 });

		public static List<PoolResultSymbol> SuccessThreeAdvantageFour => new List<PoolResultSymbol> { new PoolResultSymbol(Symbol.Success, 3), new PoolResultSymbol(Symbol.Advantage, 4) };

		private static Pool AbilityTwo => new Pool() { PoolDice = new List<PoolDie> { new PoolDie(DiceSeed.AbilityDie, 2) } };
		private static Pool DifficultyTwo => new Pool() { PoolDice = new List<PoolDie> { new PoolDie(DiceSeed.DifficultyDie, 2) } };
		public static SearchViewModel PositiveModel => new SearchViewModel(new List<PoolCombinationStatistic> { }, AbilityTwo.PoolDice.Union(DifficultyTwo.PoolDice));
		public static SearchViewModel NegativeModel => new SearchViewModel(new List<PoolCombinationStatistic> { }, AbilityTwo.PoolDice);
	}
}
