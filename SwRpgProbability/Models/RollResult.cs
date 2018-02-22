using System;
using System.Collections.Generic;
using System.Text;

namespace SwRpgProbability.Models
{
	class RollResult
	{
		public override string ToString()
		{
			return string.Format("\"{0}\",{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14},{15},{16}",
				Dice, Count, Unique,
				Success, Success / (decimal)Count,
				Failure, Failure / (decimal)Count,
				Advantage, Advantage / (decimal)Count,
				Threat, Threat / (decimal)Count,
				Neutral, Neutral / (decimal)Count,
				Triumph, Triumph / (decimal)Count,
				Despair, Despair / (decimal)Count);
		}

		public string Dice { get; set; }
		public long Count { get; set; }
		public long Unique { get; set; }
		public long Success { get; set; }
		public long Failure { get; set; }
		public long Advantage { get; set; }
		public long Threat { get; set; }
		public long Neutral { get; set; }
		public long Triumph { get; set; }
		public long Despair { get; set; }
	}
}
