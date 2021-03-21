using System.Collections.Generic;
using System.Linq;

namespace Probability.Function.Configuration
{
	public class DieGenerationLimit
	{
		public int Start { private get; set; }
		public int Count { private get; set; }

		public IEnumerable<int> Range => Enumerable.Range(Start, Count);
	}
}
