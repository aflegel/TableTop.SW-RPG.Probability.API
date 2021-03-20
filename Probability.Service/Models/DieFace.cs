using System.Collections.Generic;

namespace Probability.Service.Models
{
	public class DieFace
	{
		public DieFace() => DieFaceSymbols = new HashSet<DieFaceSymbol>();

		public DieFace(List<DieFaceSymbol> faces) => DieFaceSymbols = new HashSet<DieFaceSymbol>(faces);

		public int DieFaceId { get; set; }

		public int DieId { get; set; }

		public ICollection<DieFaceSymbol> DieFaceSymbols { get; set; }

		public Die Die { get; set; }
	}
}
