using DataFramework.Models;

namespace Visualizer.Models
{
	public class ProbabilityBreakdown
	{
		public PoolCombination Baseline { get; set; }
		public PoolCombination Upgraded { get; set; }
		public PoolCombination Boosted { get; set; }
		public PoolCombination Setback { get; set; }
		public PoolCombination Threatened { get; set; }
	}
}