using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataFramework.Context;
using Microsoft.EntityFrameworkCore;

namespace DataFramework.Models
{
	public class Die
	{
		/// <summary>
		/// An Enum to capture the different kind of faces
		/// </summary>
		public enum Symbol
		{
			Blank = 0,
			Success = 1,
			Failure = 2,
			Advantage = 3,
			Threat = 4,
			Triumph = 5,
			Despair = 6,
			Light = 7,
			Dark = 8
		}

		public enum DieNames
		{
			Ability,
			Boost,
			Challenge,
			Difficulty,
			Force,
			Proficiency,
			Setback
		}

		public Die()
		{
			DieFaces = new HashSet<DieFace>();
			PoolDice = new HashSet<PoolDie>();
		}

		public int DieId { get; set; }

		public string Name { get; set; }

		public ICollection<DieFace> DieFaces { get; set; }

		public ICollection<PoolDie> PoolDice { get; set; }

		/// <summary>
		///
		/// </summary>
		/// <param name="context"></param>
		/// <param name="die"></param>
		/// <returns></returns>
		public static Die GetDie(ProbabilityContext context, DieNames die) => context.Dice.Where(w => w.Name == die.ToString()).Include(i => i.DieFaces).ThenInclude(t => t.DieFaceSymbols).FirstOrDefault();
	}
}
