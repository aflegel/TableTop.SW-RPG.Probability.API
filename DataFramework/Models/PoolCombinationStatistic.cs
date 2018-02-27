using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DataFramework.Models.Die;

namespace DataFramework.Models
{
	public class PoolCombinationStatistic
	{
		public PoolCombinationStatistic()
		{
		}

		public long PositivePoolId { get; set; }
		public long NegativePoolId { get; set; }
		public Symbol Symbol { get; set; }

		public long Quantity { get; set; }
		public long Frequency { get; set; }

		public override string ToString()
		{
			return string.Format("{0}{1}", Symbol, Quantity).ToString();
		}

		public override int GetHashCode()
		{
			return ToString().GetHashCode();
		}

		[JsonIgnore]
		public virtual PoolCombination PoolCombination { get; set; }
	}
}
