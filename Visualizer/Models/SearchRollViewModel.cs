using DataFramework.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Visualizer.Models
{
	public class SearchRollViewModel
	{
		public RollViewModel PositiveRolls { get; set; }
		public RollViewModel NegativeRolls { get; set; }

		public SearchRollViewModel()
		{
			PositiveRolls = new RollViewModel();
			NegativeRolls = new RollViewModel();
		}

		public SearchRollViewModel(Pool positivePool, Pool negativePool)
		{
			PositiveRolls = new RollViewModel(positivePool);
			NegativeRolls = new RollViewModel(negativePool);
		}
	}
}
