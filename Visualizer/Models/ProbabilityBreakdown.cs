using DataFramework.Models;
using System.Collections.Generic;

namespace Visualizer.Models
{
	public class ProbabilityBreakdown
	{
		public PoolCombination Baseline { get; set; }
		public IEnumerable<PoolDie> BaseDice { get; set; }
		//public PoolCombination Upgraded { get; set; }
		//public PoolCombination Boosted { get; set; }
		//public PoolCombination Setback { get; set; }
		//public PoolCombination Threatened { get; set; }
	}
}