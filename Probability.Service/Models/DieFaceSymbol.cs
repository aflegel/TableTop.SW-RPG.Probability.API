namespace Probability.Service.Models
{
	public class DieFaceSymbol
	{
		public DieFaceSymbol(Symbol symbol, int quantity)
		{
			Symbol = symbol;
			Quantity = quantity;
		}

		public int DieFaceId { get; set; }

		public Symbol Symbol { get; set; }

		public int Quantity { get; set; }

		public DieFace DieFace { get; set; }
	}
}
