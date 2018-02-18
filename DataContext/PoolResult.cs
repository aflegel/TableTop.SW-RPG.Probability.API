using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiceCalculator.DataContext
{
	class PoolResult
	{
		public PoolResult()
		{
			//something like 8 million records
		}

		public int FaceId { get; set; }

		public int PoolId { get; set; }

		public virtual Pool Pool { get; set; }
		public virtual Face Face { get; set; }
	}
}
