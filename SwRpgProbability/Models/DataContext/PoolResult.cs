using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwRpgProbability.Models.DataContext
{
	public class PoolResult
	{
		public PoolResult()
		{
			//something like 8 million records
			PoolResultSymbols = new HashSet<PoolResultSymbol>();
		}

		public long PoolResultId { get; set; }

		public long PoolId { get; set; }

		public virtual Pool Pool { get; set; }

		public virtual ICollection<PoolResultSymbol> PoolResultSymbols { get; set; }

	}
}
