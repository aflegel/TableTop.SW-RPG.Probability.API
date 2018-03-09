﻿using System;
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

namespace DataGenerator
{
	class Program
	{
		const int ABILITY_LIMIT = 6;
		const int UPGRADE_LIMIT = 6;
		const int DIFFICULTY_LIMIT = 6;
		const int CHALLENGE_LIMIT = 6;
		const int BOOST_LIMIT = 4;
		const int SETBACK_LIMIT = 4;

		static void Main(string[] args)
		{
			var time = DateTime.Now;
			Console.WriteLine(string.Format("{0:h:mm.s} Startup", DateTime.Now));

			ProcessProgram();

			Console.WriteLine(string.Format("Start time: {0:h:mm.s}", time));
			Console.WriteLine(string.Format("Completion time: {0:h:mm.s}", DateTime.Now));
			//Console.WriteLine(string.Format("Total Runtime: {0:h:m.s}", DateTime.Now.Subtract(time)));
			//prevent auto close
			Console.ReadKey();
		}

		private static void InitializeDatabase(ProbabilityContext context)
		{
			Console.WriteLine(string.Format("{0:h:m.s} Database Initialization", DateTime.Now));
			context.Database.EnsureDeleted();
			context.Database.EnsureCreated();
			//context.Database.Migrate();

			Console.WriteLine(string.Format("{0:h:m.s} Database Seeding", DateTime.Now));

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

				Console.WriteLine(string.Format("{0:h:mm.s} Generating Pools", DateTime.Now));

				var positiveDicePools = BuildPositivePool(context);
				var negativeDicePools = BuildNegativePool(context);

				foreach (var positivePool in positiveDicePools)
				{
					foreach (var negativePool in negativeDicePools)
					{
						var summary = new PoolSummary(context, positivePool, negativePool);
					}
				}

				Console.WriteLine(string.Format("{0:h:mm.s} Saving Database Records", DateTime.Now));
				context.SaveChanges();
			}
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="context"></param>
		/// <returns></returns>
		private static List<Pool> BuildPositivePool(ProbabilityContext context)
		{
			var positiveDicePools = new List<Pool>();
			//each ability level
			for (int i = 1; i <= ABILITY_LIMIT; i++)
			{
				//each skill level
				//ensure the proficiency dice don't outweigh the ability dice
				for (int j = 0; (j <= UPGRADE_LIMIT) && (j <= i); j++)
				{
					for (int k = 0; k <= BOOST_LIMIT; k++)
					{
						positiveDicePools.Add(new PoolCalculator(context, BuildPoolDice(context, ability: i - j, proficiency: j, boost: k)).RollOutput);
					}
				}
			}

			return positiveDicePools;
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="context"></param>
		/// <returns></returns>
		private static List<Pool> BuildNegativePool(ProbabilityContext context)
		{
			var negativeDicePools = new List<Pool>();
			//each difficulty
			for (int i = 1; i <= DIFFICULTY_LIMIT; i++)
			{
				//ensure the challende dice don't outweigh the difficulty dice
				for (int j = 0; (j <= CHALLENGE_LIMIT) && (j <= i); j++)
				{
					for (int k = 0; k <= SETBACK_LIMIT; k++)
					{
						negativeDicePools.Add(new PoolCalculator(context, BuildPoolDice(context, difficulty: i - j, challenge: j, setback: k)).RollOutput);
					}
				}
			}

			return negativeDicePools;
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
				pool.PoolDice.Add(new PoolDie(GetDie(context, Die.DieNames.Ability), ability));
			if (boost > 0)
				pool.PoolDice.Add(new PoolDie(GetDie(context, Die.DieNames.Boost), boost));
			if (challenge > 0)
				pool.PoolDice.Add(new PoolDie(GetDie(context, Die.DieNames.Challenge), challenge));
			if (difficulty > 0)
				pool.PoolDice.Add(new PoolDie(GetDie(context, Die.DieNames.Difficulty), difficulty));
			if (proficiency > 0)
				pool.PoolDice.Add(new PoolDie(GetDie(context, Die.DieNames.Proficiency), proficiency));
			if (setback > 0)
				pool.PoolDice.Add(new PoolDie(GetDie(context, Die.DieNames.SetBack), setback));

			pool.Name = pool.GetPoolText();
			pool.TotalOutcomes = (long)pool.GetRollEstimation();

			context.Pools.Add(pool);

			return pool;
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="context"></param>
		/// <param name="die"></param>
		/// <returns></returns>
		protected static Die GetDie(ProbabilityContext context, Die.DieNames die)
		{
			return context.Dice.Where(w => w.Name == die.ToString()).Include(i => i.DieFaces).ThenInclude(t => t.DieFaceSymbols).FirstOrDefault();
		}
	}
}
