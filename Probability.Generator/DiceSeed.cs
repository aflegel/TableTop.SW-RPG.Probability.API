using System.Collections.Generic;
using Probability.Service.Models;

namespace Probability.Generator
{
	public static class DiceSeed
	{
		/// <summary>
		/// Returns a definition for each die type
		/// </summary>
		/// <returns></returns>
		public static IEnumerable<Die> SeedDice => new List<Die>
		{
			new Ability(),
			new Boost(),
			new Challenge(),
			new Difficulty(),
			new Force(),
			new Proficiency(),
			new Setback()
		};
	}
}
