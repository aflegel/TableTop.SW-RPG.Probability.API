using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SwRpgProbability.Models.DataContext.Die;

namespace SwRpgProbability.Models.DataContext
{
	public class DieFaceSymbol
	{
		public DieFaceSymbol()
		{

		}

		public DieFaceSymbol(Symbol symbol, int quantity)
		{
			Symbol = symbol;
			Quantity = quantity;
		}

		public int DieFaceId { get; set; }

		public  Symbol Symbol { get; set; }
		public int Quantity { get; set; }

		public virtual DieFace DieFace { get; set; }
	}
}
