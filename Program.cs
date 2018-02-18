using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using DiceCalculator.Dice;
using System.Web.Mvc;

namespace DiceCalculator
{
	class Program
	{
		const int ABILITY_LIMIT = 6;
		const int UPGRADE_LIMIT = 4;
		const int DIFFICULTY_LIMIT = 6;
		const int CHALLENGE_LIMIT = 4;
		const int BOOST_LIMIT = 2;
		const int SETBACK_LIMIT = 2;

		static void Main(string[] args)
		{
			//LargeTest();
			//MatchTest();
			SkillBreakdown();
		}

		/// <summary>
		/// Runs a series of tests to calculate the total output
		/// </summary>
		public static void SkillBreakdown()
		{
			StreamWriter writer = new StreamWriter("DiceResults.csv");

			List<DieResult> resultList = new List<DieResult>();

			List<Die> pool = GetPool(1, 1, 1, 1, 0, 0);
			BreakdownCalculator calculator = new BreakdownCalculator(pool);
			resultList.Add(calculator.Run());

			//each ability level
			for (int i = 1; i <= ABILITY_LIMIT; i++)
			{
				//each skill level
				//ensure the proficiency dice don't outweigh the ability dice
				for (int j = 0; (j <= UPGRADE_LIMIT) && (j <= i); j++)
				{
					//each difficulty
					for (int k = 1; k <= DIFFICULTY_LIMIT; k++)
					{
						//ensure the proficiency dice don't outweigh the ability dice
						for (int l = 0; (l <= CHALLENGE_LIMIT) && (l <= k); l++)
						{
							for (int m = 0; m < BOOST_LIMIT; m++)
							{
								for(int n = 0; n < SETBACK_LIMIT; n++)
								{
									//List<Die> pool = GetPool(i - j, j, k - l, l, m, n);
									//BreakdownCalculator calculator = new BreakdownCalculator(pool);
									//resultList.Add(calculator.Run());
								}
							}
						}
					}
				}
			}

			writer.WriteLine("pool,total,unique,successes,%,failures,%,advantages,%,threats,%,stalemate,%,triumphs,%,despairs,%");
			writer.WriteLine(string.Format("{0}", string.Join("\n", resultList)));
			writer.WriteLine();

			writer.Close();
		}

		protected static List<Die> GetPool(int ability, int proficiency, int difficulty, int challenge, int boost, int setback)
		{
			List<Die> testPool = new List<Die>();

			for (int i = 0; i < ability; i++)
				testPool.Add(new Ability());

			for (int i = 0; i < proficiency; i++)
				testPool.Add(new Proficiency());

			for (int i = 0; i < difficulty; i++)
				testPool.Add(new Difficulty());

			for (int i = 0; i < challenge; i++)
				testPool.Add(new Challenge());

			for (int i = 0; i < boost; i++)
				testPool.Add(new Boost());

			for (int i = 0; i < setback; i++)
				testPool.Add(new SetBack());

			//var poolText = testPool.GroupBy(info => info.ToString()).Select(group => string.Format("{0} {1}", group.Key, group.Count())).ToList();
			//Console.WriteLine(string.Format("Pool: {0}", string.Join(", ", poolText)));

			return testPool;
		}

		public static void MatchTest()
		{
			BreakdownCalculator diceCalculator = new BreakdownCalculator(new List<Die>()
			{
				//new Boost(),
				new Ability(),
				new Ability(),
				new Ability(),
				new Ability(),
				//new Proficiency(),
				//new Ability(),
				//new Ability(),
				//new Difficulty(),
				//new Difficulty(),
				new Difficulty(),
				new Difficulty(),
				new Difficulty(),
				new Difficulty()
			});
		}
	}
}
