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

		public static (IEnumerable<int>, IEnumerable<int>, IEnumerable<int>) AbilityTwoSeed => (Enumerable.Range(2, 1), new List<int> { 0 }, new List<int> { 0 });
		public static (IEnumerable<int>, IEnumerable<int>, IEnumerable<int>) DifficultyTwoSeed => (Enumerable.Range(2, 1), new List<int> { 0 }, new List<int> { 0 });
		public static (IEnumerable<int>, IEnumerable<int>, IEnumerable<int>) ProficiencyThreeBoostTwoSeed => (Enumerable.Range(3, 1), Enumerable.Range(3, 1), Enumerable.Range(2, 1));
		public static (IEnumerable<int>, IEnumerable<int>, IEnumerable<int>) ChallengeThreeSetbackTwoSeed => (Enumerable.Range(3, 1), Enumerable.Range(3, 1), Enumerable.Range(2, 1));

		public static List<PoolResultSymbol> SuccessThreeAdvantageFour => new List<PoolResultSymbol> { new PoolResultSymbol(Symbol.Success, 3), new PoolResultSymbol(Symbol.Advantage, 4) };

		public static Pool AbilityTwo => new Pool() { PoolDice = new List<PoolDie> { new PoolDie(DiceSeed.AbilityDie, 2) } };
		public static Pool DifficultyTwo => new Pool() { PoolDice = new List<PoolDie> { new PoolDie(DiceSeed.DifficultyDie, 2) } };
		public static SearchViewModel PositiveModel => new SearchViewModel(new PoolCombination() { PositivePool = AbilityTwo, NegativePool = DifficultyTwo });
		public static SearchViewModel NegativeModel => new SearchViewModel(new PoolCombination() { PositivePool = AbilityTwo, NegativePool = new Pool() });
	}
}
