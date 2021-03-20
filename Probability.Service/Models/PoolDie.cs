namespace Probability.Service.Models
{
	public class PoolDie
	{
		public PoolDie() { }

		public PoolDie(Die die, int quantity)
		{
			Die = die;
			Quantity = quantity;
		}

		public int DieId { get; set; }

		public int PoolId { get; set; }

		public int Quantity { get; set; }

		public Pool Pool { get; set; }

		public Die Die { get; set; }
	}
}
