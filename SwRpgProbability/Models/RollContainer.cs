using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SwRpgProbability.Models
{
	public class RollContainer
	{
		public List<Die> DicePool { get; set; }

		public Dictionary<Face, long> ResultList { get; set; }


		public RollContainer(List<Die> DicePool)
		{
			this.DicePool = DicePool;
			ResultList = new Dictionary<Face, long>();
		}

		public string GetPoolText()
		{
			return GetPoolText(DicePool);
		}

		public static string GetPoolText(List<Die> pool)
		{
			return string.Join(", ", pool.GroupBy(info => info.ToString()).Select(group => string.Format("{0} {1}", group.Key, group.Count())).ToList());
		}

		public long GetRollEstimation()
		{
			return GetRollEstimation(DicePool);
		}

		public static long GetRollEstimation(List<Die> pool)
		{
			return pool.Aggregate((long)1, (x, y) => x * y.Faces.Count);
		}
	}
}
