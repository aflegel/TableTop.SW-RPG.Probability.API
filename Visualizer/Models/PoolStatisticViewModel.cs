using DataFramework.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Visualizer.Models
{
	public class PoolStatisticViewModel
	{
		public string Symbol { get; set; }
		public int Quantity { get; set; }
		public ulong Frequency { get; set; }
		public long AlternateTotal { get; set; }
	}
}
