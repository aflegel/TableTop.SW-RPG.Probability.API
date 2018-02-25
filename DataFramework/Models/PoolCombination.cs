using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataFramework.Models
{
	public class PoolCombination
	{
		public PoolCombination()
		{
		}

		public long PositivePoolId { get; set; }
		public long NegativePoolId { get; set; }

		public long SuccessOutcomes { get; set; }
		public long AdvantageOutcomes { get; set; }
		public long ThreatOutcomes { get; set; }
		public long TriumphOutcomes { get; set; }
		public long DespairOutcomes { get; set; }

		public virtual Pool PositivePool { get; set; }
		public virtual Pool NegativePool { get; set; }

	}
}
