using System.Collections.Generic;
using System.Linq;

namespace DataGenerator
{
	public class LimitConfiguration
	{
		public int Start { get; set; }
		public int End { get; set; }

		public IEnumerable<int> Range => Enumerable.Range(Start, End);
	}
}
