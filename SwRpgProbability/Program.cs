using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using SwRpgProbability.Models;
using SwRpgProbability.Models.Dice;

namespace SwRpgProbability
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
			InitializeDatabase();

			ProcessProgram();

			//prevent auto close
			Console.ReadKey();
		}

		private static void InitializeDatabase()
		{
			using (var db = new Models.DataContext.ProbabilityContext())
			{
				db.Database.ExecuteSqlCommand("DELETE FROM PoolResultSymbol");
				db.Database.ExecuteSqlCommand("DELETE FROM PoolResult");
				db.Database.ExecuteSqlCommand("DELETE FROM PoolDie");
				db.Database.ExecuteSqlCommand("DELETE FROM Pool");
			}
		}

		/// <summary>
		/// Test Function
		/// </summary>
		/// <returns></returns>
		public static List<RollResult> Sample()
		{
			var resultList = new List<RollResult>();
			var pool = GetPool(1, 1, 1, 1, 0, 0);
			var outputContainer = new RollContainer(pool);
			var calculator = new PoolCalculator(outputContainer);
			//resultList.Add(calculator.Run());

			return resultList;
		}


		private static void ProcessProgram()
		{
			var positiveDicePools = BuildPositivePool();
			var negativeDicePools = BuildNegativePool();

			foreach (var positivePool in positiveDicePools)
			{
				foreach (var negativePool in negativeDicePools)
				{
					var summary = new PoolSummary(positivePool, negativePool);
				}
			}
		}

		private static List<RollContainer> BuildPositivePool()
		{
			var positiveDicePools = new List<RollContainer>();
			//each ability level
			for (int i = 1; i <= ABILITY_LIMIT; i++)
			{
				//each skill level
				//ensure the proficiency dice don't outweigh the ability dice
				for (int j = 0; (j <= UPGRADE_LIMIT) && (j <= i); j++)
				{
					for (int k = 0; k <= BOOST_LIMIT; k++)
					{
						var pool = GetPool(i - j, j, 0, 0, k, 0);
						var outputContainer = new RollContainer(pool);
						var calculator = new PoolCalculator(outputContainer);

						positiveDicePools.Add(outputContainer);
					}
				}
			}

			return positiveDicePools;
		}

		private static List<RollContainer> BuildNegativePool()
		{
			var negativeDicePools = new List<RollContainer>();
			//each difficulty
			for (int i = 1; i <= DIFFICULTY_LIMIT; i++)
			{
				//ensure the proficiency dice don't outweigh the ability dice
				for (int j = 0; (j <= CHALLENGE_LIMIT) && (j <= i); j++)
				{
					for (int k = 0; k <= SETBACK_LIMIT; k++)
					{
						var pool = GetPool(0, 0, i - j, j, 0, k);
						var outputContainer = new RollContainer(pool);
						var calculator = new PoolCalculator(outputContainer);

						negativeDicePools.Add(outputContainer);
					}
				}
			}

			return negativeDicePools;
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

			return testPool;
		}
	}
}
