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

		private static Pool MakePool(DieNames name, int quantity) => new Pool() { PoolDice = new List<PoolDie> { new PoolDie(new Die { Name = name.ToString() }, quantity) } };
		private static Pool AbilityTwo => MakePool(DieNames.Ability, 2);
		private static Pool DifficultyTwo => MakePool(DieNames.Difficulty, 2);
		private static SearchViewModel MakeModel(IEnumerable<PoolDie> dice) => new SearchViewModel(new List<PoolCombinationStatistic> { }, dice);
		public static SearchViewModel PositiveModel => MakeModel(AbilityTwo.PoolDice.Union(DifficultyTwo.PoolDice));
		public static SearchViewModel NegativeModel => MakeModel(AbilityTwo.PoolDice);
	}
}
