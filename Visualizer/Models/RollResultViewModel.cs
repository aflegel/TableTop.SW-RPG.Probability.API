using System.Collections.Generic;

namespace Visualizer.Models
{
	public class RollResultViewModel
	{
		public IEnumerable<RollSymbolViewModel> Symbols { get; set; }
		public decimal Frequency { get; set; }
	}
}
