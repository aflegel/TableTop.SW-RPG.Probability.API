using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataFramework.Models
{
	public class PoolDie
	{
		public PoolDie() { }

		public PoolDie(Die Die, int Quantity)
		{
			this.Die = Die;
			this.Quantity = Quantity;
		}

		public int DieId { get; set; }

		[JsonIgnore]
		public int PoolId { get; set; }

		public int Quantity { get; set; }

		[JsonIgnore]
		public virtual Pool Pool { get; set; }
		[JsonIgnore]
		public virtual Die Die { get; set; }
	}
}
