using Newtonsoft.Json;
using System.Collections.Generic;

namespace DataFramework.Models
{
	public class DieFace
	{
		public DieFace()
		{
			DieFaceSymbols = new HashSet<DieFaceSymbol>();
		}

		public DieFace(List<DieFaceSymbol> faces)
		{
			DieFaceSymbols = new HashSet<DieFaceSymbol>();

			faces.ForEach(face => DieFaceSymbols.Add(face));
		}

		public int DieFaceId { get; set; }

		public int DieId { get; set; }

		public ICollection<DieFaceSymbol> DieFaceSymbols { get; set; }

		[JsonIgnore]
		public Die Die { get; set; }
	}
}
