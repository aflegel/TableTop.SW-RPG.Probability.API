namespace DataFramework.Models
{
	public class PoolResultSymbol
	{
		public PoolResultSymbol(Symbol symbol, int quantity)
		{
			Symbol = symbol;
			Quantity = quantity;
		}

		public int PoolResultId { get; set; }

		public Symbol Symbol { get; set; }

		public int Quantity { get; set; }

		public PoolResult PoolResult { get; set; }

		public override int GetHashCode() => (Symbol, Quantity).GetHashCode();
	}
}
