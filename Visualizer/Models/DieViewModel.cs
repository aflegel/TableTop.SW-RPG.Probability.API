using System;
using System.Collections.Generic;
using System.Linq;
using DataFramework.Context;
using DataFramework.Models;

namespace Visualizer.Models
{
	public class DieViewModel
	{
		public string DieType { get; set; }

		public int Quantity { get; set; }
	}

	public static class DieViewModelExtensions
	{
		public static Tuple<Pool, Pool> ToPool(this List<DieViewModel> searchForPool, ProbabilityContext context)
			=> new Pool { PoolDice = searchForPool.Select(die => new PoolDie { Die = context.GetDie(die.DieType), Quantity = die.Quantity }).ToList() }.SplitPoolByDice(context);
	}
}
