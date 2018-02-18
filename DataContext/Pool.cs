using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiceCalculator.DataContext
{
	class Pool
	{
		public Pool()
		{
			Dice = new HashSet<Die>();

		}

		public long PoolId { get; set; }
		public string Name { get; set; }

		//todo summary information ~600 entries

		public virtual ICollection<Die> Dice { get; set; }
	}
}
