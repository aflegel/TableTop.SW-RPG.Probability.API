using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwRpgProbability.Models.Dice
{
	class Force : Die
	{
		public Force()
		{
			Faces = new List<Face>
			{
				new Face(new Dictionary<Symbol, byte>() { { Symbol.Dark, 1 } }),
				new Face(new Dictionary<Symbol, byte>() { { Symbol.Dark, 1 } }),
				new Face(new Dictionary<Symbol, byte>() { { Symbol.Dark, 1 } }),
				new Face(new Dictionary<Symbol, byte>() { { Symbol.Dark, 1 } }),
				new Face(new Dictionary<Symbol, byte>() { { Symbol.Dark, 1 } }),
				new Face(new Dictionary<Symbol, byte>() { { Symbol.Dark, 1 } }),
				new Face(new Dictionary<Symbol, byte>() { { Symbol.Dark, 2 } }),
				new Face(new Dictionary<Symbol, byte>() { { Symbol.Light, 1 } }),
				new Face(new Dictionary<Symbol, byte>() { { Symbol.Light, 1 } }),
				new Face(new Dictionary<Symbol, byte>() { { Symbol.Light, 2 } }),
				new Face(new Dictionary<Symbol, byte>() { { Symbol.Light, 2 } }),
				new Face(new Dictionary<Symbol, byte>() { { Symbol.Light, 2 } })
			};
		}
	}
}
