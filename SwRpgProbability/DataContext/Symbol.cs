using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiceCalculator.DataContext
{
	public class Symbol
	{
		public Symbol()
		{
		}

		public int SymbolId { get; set; }
		public string Name { get; set; }

		public virtual ICollection<FaceSymbol> FaceSymbols { get; set; }
	}
}
