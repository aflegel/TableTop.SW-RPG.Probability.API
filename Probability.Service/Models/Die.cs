using System.Collections.Generic;

namespace Probability.Service.Models
{
	public class Die
	{
		public Die()
		{
			DieFaces = new HashSet<DieFace>();
			PoolDice = new HashSet<PoolDie>();
		}

		public int DieId { get; set; }

		public string Name { get; set; }

		public ICollection<DieFace> DieFaces { get; set; }

		public ICollection<PoolDie> PoolDice { get; set; }
	}
}
