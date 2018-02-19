using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiceCalculator.DataContext
{
	public class Die
	{
		public Die()
		{
			Faces = new HashSet<Face>();
		}

		public int DieId { get; set; }
		public string Name { get; set; }

		public virtual ICollection<Face> Faces { get; set; }
	}
}
