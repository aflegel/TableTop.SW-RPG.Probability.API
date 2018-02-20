using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwRpgProbability.Dice
{
	class SetBack : Die
	{

		public SetBack()
		{
			Faces = new List<Face>
			{
				new Face(new Dictionary<Symbol, byte>() { { Symbol.Blank, 1 } }),
				new Face(new Dictionary<Symbol, byte>() { { Symbol.Blank, 1 } }),
				new Face(new Dictionary<Symbol, byte>() { { Symbol.Failure, 1 } }),
				new Face(new Dictionary<Symbol, byte>() { { Symbol.Failure, 1 } }),
				new Face(new Dictionary<Symbol, byte>() { { Symbol.Threat, 1 } }),
				new Face(new Dictionary<Symbol, byte>() { { Symbol.Threat, 1 } })
			};
		}
	}
}
