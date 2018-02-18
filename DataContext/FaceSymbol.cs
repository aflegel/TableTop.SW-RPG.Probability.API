using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiceCalculator.DataContext
{
	class FaceSymbol
	{
		public FaceSymbol()
		{

		}

		public int FaceId { get; set; }

		public int SymbolId { get; set; }

		public virtual Symbol Symbol { get; set; }
		public virtual Face Face { get; set; }
	}
}
