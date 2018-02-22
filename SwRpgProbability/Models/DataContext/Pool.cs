using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwRpgProbability.Models.DataContext
{
	public class Pool
	{
		public Pool()
		{
			PoolDice = new HashSet<PoolDie>();
			PoolResults = new HashSet<PoolResult>();

		}

		public long PoolId { get; set; }
		public string Name { get; set; }

		//todo summary information ~600 entries
		public long TotalOutcomes { get; set; }
		public long UniqueOutcomes { get; set; }

		public long SuccessOutcomes { get; set; }
		public long FailureOutcomes { get; set; }
		public long AdvantageOutcomes { get; set; }
		public long ThreatOutcomes { get; set; }
		public long NeutralOutcomes { get; set; }
		public long TriumphOutcomes { get; set; }
		public long DespairOutcomes { get; set; }

		public virtual ICollection<PoolDie> PoolDice { get; set; }
		public virtual ICollection<PoolResult> PoolResults { get; set; }

	}
}
