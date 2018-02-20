using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwRpgProbability.DataContext
{
	public class PoolDie
	{
		public PoolDie()
		{
		}

		public int DieId { get; set; }
		public long PoolId { get; set; }

		public int Quantity { get; set; }

		public virtual Pool Pool { get; set; }
		public virtual Die Die { get; set; }
	}
}
