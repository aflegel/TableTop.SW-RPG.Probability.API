using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiceCalculator.DataContext
{
	public class Face
	{
		public Face()
		{
			FaceSymbols = new HashSet<FaceSymbol>();
		}

		public int FaceId { get; set; }

		public int DieId { get; set; }

		public virtual ICollection<FaceSymbol> FaceSymbols { get; set; }
		public virtual ICollection<PoolResult> PoolResults { get; set; }

		public virtual Die Die { get; set; }
	}
}
