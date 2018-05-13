﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataFramework.Models
{
	public class DieFace
	{
		public DieFace()
		{
			DieFaceSymbols = new HashSet<DieFaceSymbol>();
		}

		public DieFace(List<DieFaceSymbol> Faces)
		{
			DieFaceSymbols = new HashSet<DieFaceSymbol>();

			foreach (var face in Faces)
				DieFaceSymbols.Add(face);
		}

		public int DieFaceId { get; set; }

		public int DieId { get; set; }

		public virtual ICollection<DieFaceSymbol> DieFaceSymbols { get; set; }

		[JsonIgnore]
		public virtual Die Die { get; set; }
	}
}