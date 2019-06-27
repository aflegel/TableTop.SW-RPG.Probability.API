using DataFramework.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Visualizer.Models
{
	public class RollResultViewModel
	{
		public IEnumerable<RollSymbolViewModel> Symbols { get; set; }

		public decimal Frequency { get; set; }
	}
}
