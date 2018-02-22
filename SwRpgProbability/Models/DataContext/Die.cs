using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwRpgProbability.Models.DataContext
{
	public class Die
	{
		public Die()
		{
		}

		public int DieId { get; set; }
		public string Name { get; set; }

		public virtual ICollection<PoolDie> PoolDice { get; set; }
	}
}
