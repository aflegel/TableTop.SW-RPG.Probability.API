using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using DataGenerator.Models;
using DataFramework.Context;
using DataFramework.Models;
using static DataFramework.Models.Die;

namespace DataGenerator
{
	class Program
	{
		const int ABILITY_LIMIT = 4;
		const int UPGRADE_LIMIT = 4;
		const int DIFFICULTY_LIMIT = 4;
		const int CHALLENGE_LIMIT = 4;
		const int BOOST_LIMIT = 4;
		const int SETBACK_LIMIT = 4;

		static void Main(string[] args)
		{
			var time = DateTime.Now;
			Console.WriteLine(string.Format("{0:hh:mm.ss} Startup", DateTime.Now));

			ProcessProgram();

			Console.WriteLine(string.Format("Start time: {0:hh:mm.ss}", time));
			Console.WriteLine(string.Format("Completion time: {0:hh:mm.ss}", DateTime.Now));
			//Console.WriteLine(string.Format("Total Runtime: {0:hh:mm.ss}", DateTime.Now.Subtract(time)));
			//prevent auto close
			Console.ReadKey();
		}

		/// <summary>
		/// Destroys and recreates the database and seeds the database with the dice information
		/// </summary>
		/// <param name="context"></param>
		private static void InitializeDatabase(ProbabilityContext context)
		{
			//todo: wait for confirmation before deleting
			Console.WriteLine(string.Format("{0:hh:mm.ss} Database Initialization", DateTime.Now));

			//delete and recreate the database
			context.Database.EnsureDeleted();
			context.Database.EnsureCreated();

			Console.WriteLine(string.Format("{0:hh:mm.ss} Database Seeding", DateTime.Now));

			ProbabilityContextSeed.SeedData(context);
		}

		/// <summary>
		///
		/// </summary>
		private static void ProcessProgram()
		{
			using (var context = new ProbabilityContext())
			{
				InitializeDatabase(context);

				//partial pools are each half of a roll
				ProcessPartialPools(context);

				//save the pools before generating the full comparison
				CommitData(context);

				ProcessPoolComparison(context);

				//save the outcome results
				CommitData(context);
			}
		}

		/// <summary>
		/// Prints start and stop timestamps while saving the current context state
		/// </summary>
		/// <param name="context"></param>
		private static void CommitData(ProbabilityContext context)
		{
			Console.WriteLine(string.Format("{0:hhh:mm.sss} Initialize Database Commit", DateTime.Now));
			context.SaveChanges();
			Console.WriteLine(string.Format("{0:hhh:mm.sss} Completed Database Commit", DateTime.Now));
		}

		/// <summary>
		/// Prints start and stop timestamps while building the complete outcome map for a set of dice
		/// </summary>
		/// <param name="context"></param>
		private static void ProcessPartialPools(ProbabilityContext context)
		{
			Console.WriteLine(string.Format("{0:hh:mm.ss} Initialize Pool Generation", DateTime.Now));
			BuildPositivePool(context);
			BuildNegativePool(context);
			Console.WriteLine(string.Format("{0:hh:mm.ss} Completed Pool Generation", DateTime.Now));
		}

		/// <summary>
		/// Processes the comparison of positive and negative pools
		/// </summary>
		/// <param name="context"></param>
		private static void ProcessPoolComparison(ProbabilityContext context)
		{
			Console.WriteLine(string.Format("{0:hh:mm.ss} Initialize Pool Comparison", DateTime.Now));
			var positivePools = context.Pools.Where(w => w.PoolDice.Any(a => a.Die.Name == Die.DieNames.Ability.ToString() || a.Die.Name == Die.DieNames.Boost.ToString() || a.Die.Name == Die.DieNames.Proficiency.ToString()))
				.Include(i => i.PositivePoolCombinations)
						.ThenInclude(tti => tti.PoolCombinationStatistics)
				.Include(i => i.PoolResults)
						.ThenInclude(tti => tti.PoolResultSymbols);

			var negativePools = context.Pools.Where(w => w.PoolDice.Any(a => a.Die.Name == Die.DieNames.Difficulty.ToString() || a.Die.Name == Die.DieNames.Setback.ToString() || a.Die.Name == Die.DieNames.Challenge.ToString()))
				.Include(i => i.NegativePoolCombinations)
					.ThenInclude(tti => tti.PoolCombinationStatistics)
				.Include(i => i.PoolResults)
					.ThenInclude(tti => tti.PoolResultSymbols);


			foreach (var positivePool in positivePools)
			{
				foreach (var negativePool in negativePools)
				{
					new OutcomeComparison(new PoolCombination(positivePool, negativePool));
				}
			}
			Console.WriteLine(string.Format("{0:hh:mm.ss} Completed Pool Comparison", DateTime.Now));
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="context"></param>
		/// <returns></returns>
		private static void BuildPositivePool(ProbabilityContext context)
		{
			//each ability level
			for (int i = 1; i <= ABILITY_LIMIT; i++)
			{
				//each skill level
				//ensure the proficiency dice don't outweigh the ability dice
				for (int j = 0; (j <= UPGRADE_LIMIT) && (j <= i); j++)
				{
					for (int k = 0; k <= BOOST_LIMIT; k++)
					{
						new OutcomeGenerator(BuildPoolDice(context, ability: i - j, proficiency: j, boost: k));
					}
				}
			}
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="context"></param>
		/// <returns></returns>
		private static void BuildNegativePool(ProbabilityContext context)
		{
			//each difficulty
			for (int i = 1; i <= DIFFICULTY_LIMIT; i++)
			{
				//ensure the challende dice don't outweigh the difficulty dice
				for (int j = 0; (j <= CHALLENGE_LIMIT) && (j <= i); j++)
				{
					for (int k = 0; k <= SETBACK_LIMIT; k++)
					{
						new OutcomeGenerator(BuildPoolDice(context, difficulty: i - j, challenge: j, setback: k));
					}
				}
			}
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="context"></param>
		/// <param name="ability"></param>
		/// <param name="proficiency"></param>
		/// <param name="difficulty"></param>
		/// <param name="challenge"></param>
		/// <param name="boost"></param>
		/// <param name="setback"></param>
		/// <returns></returns>
		protected static Pool BuildPoolDice(ProbabilityContext context, int ability = 0, int proficiency = 0, int difficulty = 0, int challenge = 0, int boost = 0, int setback = 0)
		{
			var pool = new Pool();

			if (ability > 0)
				pool.PoolDice.Add(new PoolDie(GetDie(context, DieNames.Ability), ability));
			if (boost > 0)
				pool.PoolDice.Add(new PoolDie(GetDie(context, DieNames.Boost), boost));
			if (challenge > 0)
				pool.PoolDice.Add(new PoolDie(GetDie(context, DieNames.Challenge), challenge));
			if (difficulty > 0)
				pool.PoolDice.Add(new PoolDie(GetDie(context, DieNames.Difficulty), difficulty));
			if (proficiency > 0)
				pool.PoolDice.Add(new PoolDie(GetDie(context, DieNames.Proficiency), proficiency));
			if (setback > 0)
				pool.PoolDice.Add(new PoolDie(GetDie(context, DieNames.Setback), setback));

			pool.Name = pool.GetPoolText();
			pool.TotalOutcomes = (long)pool.GetRollEstimation();

			context.Pools.Add(pool);

			return pool;
		}
	}
}
