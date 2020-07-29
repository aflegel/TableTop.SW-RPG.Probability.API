using System.Collections.Generic;
using System.Linq;

namespace DataFramework.Extensions
{
	public static class EnumerableExtensions
	{
		/// <summary>
		/// Splits a list into two halves.  Remainder is in the bottom half.
		/// </summary>
		/// <param name="dice"></param>
		/// <returns></returns>
		public static (IEnumerable<T>, IEnumerable<T>) Split<T>(this IEnumerable<T> list) => (list.Take(list.Count() / 2), list.Skip(list.Count() / 2));
	}
}
