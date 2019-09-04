using System.Collections.Generic;
using System.Linq;
using DataFramework.Models;

namespace Visualizer.Models
{
    public static class DieViewModelExtensions
    {
		/// <summary>
		/// Converts a list of Dice from the UI into a Pool object
		/// </summary>
		/// <param name="searchForPool"></param>
		/// <returns></returns>
        public static Pool ToPool(this List<DieViewModel> searchForPool)
            => new Pool { PoolDice = searchForPool.Select(die => new PoolDie { Die = new Die { Name = die.DieType }, Quantity = die.Quantity }).ToList() };
    }
}
