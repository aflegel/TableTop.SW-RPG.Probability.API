using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiceCalculator.Dice
{
	class Proficiency : Die
	{

		public Proficiency()
		{
			Faces = new List<Face>
			{
				new Face(new Dictionary<Symbol, byte>() { { Symbol.Blank, 1 } }),
				new Face(new Dictionary<Symbol, byte>() { { Symbol.Success, 1 } }),
				new Face(new Dictionary<Symbol, byte>() { { Symbol.Success, 1 } }),
				new Face(new Dictionary<Symbol, byte>() { { Symbol.Success, 2 } }),
				new Face(new Dictionary<Symbol, byte>() { { Symbol.Success, 2 } }),
				new Face(new Dictionary<Symbol, byte>() { { Symbol.Advantage, 1 } }),
				new Face(new Dictionary<Symbol, byte>() { { Symbol.Advantage, 1 }, { Symbol.Success, 1 } }),
				new Face(new Dictionary<Symbol, byte>() { { Symbol.Advantage, 1 }, { Symbol.Success, 1 } }),
				new Face(new Dictionary<Symbol, byte>() { { Symbol.Advantage, 1 }, { Symbol.Success, 1 } }),
				new Face(new Dictionary<Symbol, byte>() { { Symbol.Advantage, 2 } }),
				new Face(new Dictionary<Symbol, byte>() { { Symbol.Advantage, 2 } }),
				new Face(new Dictionary<Symbol, byte>() { { Symbol.Triumph, 1 } })
			};
		}
	}
}
