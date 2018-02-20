using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwRpgProbability.DataContext
{
	public class PoolResultSymbol
	{
		public PoolResultSymbol()
		{
			//something like 8 million records
		}

		public long PoolResultId { get; set; }

		public Dice.Symbol Symbol { get; set; }

		public int Quantity { get; set; }

		public virtual PoolResult PoolResult { get; set; }
	}
}
