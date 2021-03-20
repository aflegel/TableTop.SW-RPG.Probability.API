using System.Collections.Generic;
using System.Linq;

namespace Probability.Generator.Extensions
{
	public static class EnumerableExtensions
	{
		/// <summary>
		/// Splits a list into two halves.  Remainder is in the bottom half.
		/// </summary>
		/// <param name="dice"></param>
		/// <returns></returns>
		public static (IEnumerable<T>, IEnumerable<T>) Split<T>(this IEnumerable<T> list) => list.Split(list.Count() / 2);

		public static (IEnumerable<T>, IEnumerable<T>) Split<T>(this IEnumerable<T> list, int count) => (list.Take(count), list.Skip(count));
	}
}
