using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwRpgProbability.Models
{

	/// <summary>
	/// A Base class for the child dice with some static counting functions
	/// </summary>
	public class Die
	{

		public List<Face> Faces { get; set; }

		/// <summary>
		///
		/// </summary>
		/// <param name="pool"></param>
		/// <param name="addition"></param>
		/// <returns></returns>
		public static List<Face> ProcessPool(List<Face> pool, Die addition)
		{
			//escape for new lists
			if (pool.Count == 0)
				return addition.Faces;

			List<Face> nextPool = new List<Face>();

			foreach (Face dieFaces in addition.Faces)
			{
				foreach (Face poolFaces in pool)
				{
					nextPool.Add(poolFaces.Merge(dieFaces));
				}
			}

			return nextPool;
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="pool"></param>
		/// <returns></returns>
		public static Face CountPool(List<Die> pool)
		{
			//escape for new lists
			if (pool.Count == 0)
				return new Face(new Dictionary<Symbol, byte>());

			Face nextPool = new Face(new Dictionary<Symbol, byte>());

			foreach (Die dieFaces in pool)
			{
				foreach (Face face in dieFaces.Faces)
				{
					nextPool = face.Merge(nextPool);
				}
			}

			return nextPool;
		}



		public override string ToString()
		{
			return GetType().Name;
		}
	}
}
