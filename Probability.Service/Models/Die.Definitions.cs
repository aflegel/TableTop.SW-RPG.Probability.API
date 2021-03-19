using System.Collections.Generic;

namespace Probability.Service.Models
{
	public class Ability : Die
	{
		public Ability()
		{
			Name = nameof(Ability);
			DieFaces = new List<DieFace>
				{
					new DieFace(new List<DieFaceSymbol> { }),
					new DieFace(new List<DieFaceSymbol> { new DieFaceSymbol(Symbol.Success, 1) }),
					new DieFace(new List<DieFaceSymbol> { new DieFaceSymbol(Symbol.Success, 1) }),
					new DieFace(new List<DieFaceSymbol> { new DieFaceSymbol(Symbol.Success, 2) }),
					new DieFace(new List<DieFaceSymbol> { new DieFaceSymbol(Symbol.Advantage, 1) }),
					new DieFace(new List<DieFaceSymbol> { new DieFaceSymbol(Symbol.Advantage, 1) }),
					new DieFace(new List<DieFaceSymbol> { new DieFaceSymbol(Symbol.Success, 1), new DieFaceSymbol(Symbol.Advantage, 1) }),
					new DieFace(new List<DieFaceSymbol> { new DieFaceSymbol(Symbol.Advantage, 2) }),
				};
		}
	}

	public class Boost : Die
	{
		public Boost()
		{
			Name = nameof(Boost);
			DieFaces = new List<DieFace>
				{
					new DieFace(new List<DieFaceSymbol> { }),
					new DieFace(new List<DieFaceSymbol> { }),
					new DieFace(new List<DieFaceSymbol> { new DieFaceSymbol(Symbol.Success, 1) }),
					new DieFace(new List<DieFaceSymbol> { new DieFaceSymbol(Symbol.Success, 1), new DieFaceSymbol(Symbol.Advantage, 1) }),
					new DieFace(new List<DieFaceSymbol> { new DieFaceSymbol(Symbol.Advantage, 2) }),
					new DieFace(new List<DieFaceSymbol> { new DieFaceSymbol(Symbol.Advantage, 1) }),
				};
		}
	}

	public class Challenge : Die
	{
		public Challenge()
		{
			Name = nameof(Challenge);
			DieFaces = new List<DieFace>
				{
					new DieFace(new List<DieFaceSymbol> { }),
					new DieFace(new List<DieFaceSymbol> { new DieFaceSymbol(Symbol.Failure, 1) }),
					new DieFace(new List<DieFaceSymbol> { new DieFaceSymbol(Symbol.Failure, 1) }),
					new DieFace(new List<DieFaceSymbol> { new DieFaceSymbol(Symbol.Failure, 2) }),
					new DieFace(new List<DieFaceSymbol> { new DieFaceSymbol(Symbol.Failure, 2) }),
					new DieFace(new List<DieFaceSymbol> { new DieFaceSymbol(Symbol.Threat, 1) }),
					new DieFace(new List<DieFaceSymbol> { new DieFaceSymbol(Symbol.Threat, 1) }),
					new DieFace(new List<DieFaceSymbol> { new DieFaceSymbol(Symbol.Failure, 1), new DieFaceSymbol(Symbol.Threat, 1) }),
					new DieFace(new List<DieFaceSymbol> { new DieFaceSymbol(Symbol.Failure, 1), new DieFaceSymbol(Symbol.Threat, 1) }),
					new DieFace(new List<DieFaceSymbol> { new DieFaceSymbol(Symbol.Threat, 2) }),
					new DieFace(new List<DieFaceSymbol> { new DieFaceSymbol(Symbol.Threat, 2) }),
					new DieFace(new List<DieFaceSymbol> { new DieFaceSymbol(Symbol.Despair, 1) }),
				};
		}
	}

	public class Difficulty : Die
	{
		public Difficulty()
		{
			Name = nameof(Difficulty);
			DieFaces = new List<DieFace>
				{
					new DieFace(new List<DieFaceSymbol> { }),
					new DieFace(new List<DieFaceSymbol> { new DieFaceSymbol(Symbol.Failure, 1) }),
					new DieFace(new List<DieFaceSymbol> { new DieFaceSymbol(Symbol.Failure, 2) }),
					new DieFace(new List<DieFaceSymbol> { new DieFaceSymbol(Symbol.Threat, 1) }),
					new DieFace(new List<DieFaceSymbol> { new DieFaceSymbol(Symbol.Threat, 1) }),
					new DieFace(new List<DieFaceSymbol> { new DieFaceSymbol(Symbol.Threat, 1) }),
					new DieFace(new List<DieFaceSymbol> { new DieFaceSymbol(Symbol.Threat, 2) }),
					new DieFace(new List<DieFaceSymbol> { new DieFaceSymbol(Symbol.Failure, 1), new DieFaceSymbol(Symbol.Threat, 1) })
				};
		}
	}

	public class Force : Die
	{
		public Force()
		{
			Name = nameof(Force);
			DieFaces = new List<DieFace>
				{
					new DieFace(new List<DieFaceSymbol> { new DieFaceSymbol(Symbol.Dark, 1) }),
					new DieFace(new List<DieFaceSymbol> { new DieFaceSymbol(Symbol.Dark, 1) }),
					new DieFace(new List<DieFaceSymbol> { new DieFaceSymbol(Symbol.Dark, 1) }),
					new DieFace(new List<DieFaceSymbol> { new DieFaceSymbol(Symbol.Dark, 1) }),
					new DieFace(new List<DieFaceSymbol> { new DieFaceSymbol(Symbol.Dark, 1) }),
					new DieFace(new List<DieFaceSymbol> { new DieFaceSymbol(Symbol.Dark, 1) }),
					new DieFace(new List<DieFaceSymbol> { new DieFaceSymbol(Symbol.Dark, 2) }),
					new DieFace(new List<DieFaceSymbol> { new DieFaceSymbol(Symbol.Light, 1) }),
					new DieFace(new List<DieFaceSymbol> { new DieFaceSymbol(Symbol.Light, 1) }),
					new DieFace(new List<DieFaceSymbol> { new DieFaceSymbol(Symbol.Light, 2) }),
					new DieFace(new List<DieFaceSymbol> { new DieFaceSymbol(Symbol.Light, 2) }),
					new DieFace(new List<DieFaceSymbol> { new DieFaceSymbol(Symbol.Light, 2) }),
				};
		}
	}

	public class Proficiency : Die
	{
		public Proficiency()
		{
			Name = nameof(Proficiency);
			DieFaces = new List<DieFace>
				{
					new DieFace(new List<DieFaceSymbol> { }),
					new DieFace(new List<DieFaceSymbol> { new DieFaceSymbol(Symbol.Success, 1) }),
					new DieFace(new List<DieFaceSymbol> { new DieFaceSymbol(Symbol.Success, 1) }),
					new DieFace(new List<DieFaceSymbol> { new DieFaceSymbol(Symbol.Success, 2) }),
					new DieFace(new List<DieFaceSymbol> { new DieFaceSymbol(Symbol.Success, 2) }),
					new DieFace(new List<DieFaceSymbol> { new DieFaceSymbol(Symbol.Advantage, 1) }),
					new DieFace(new List<DieFaceSymbol> { new DieFaceSymbol(Symbol.Advantage, 1), new DieFaceSymbol(Symbol.Success, 1) }),
					new DieFace(new List<DieFaceSymbol> { new DieFaceSymbol(Symbol.Advantage, 1), new DieFaceSymbol(Symbol.Success, 1) }),
					new DieFace(new List<DieFaceSymbol> { new DieFaceSymbol(Symbol.Advantage, 1), new DieFaceSymbol(Symbol.Success, 1) }),
					new DieFace(new List<DieFaceSymbol> { new DieFaceSymbol(Symbol.Advantage, 2) }),
					new DieFace(new List<DieFaceSymbol> { new DieFaceSymbol(Symbol.Advantage, 2) }),
					new DieFace(new List<DieFaceSymbol> { new DieFaceSymbol(Symbol.Triumph, 1) }),
				};
		}
	}

	public class Setback : Die
	{
		public Setback()
		{
			Name = nameof(Setback);
			DieFaces = new List<DieFace>
				{
					new DieFace(new List<DieFaceSymbol> { }),
					new DieFace(new List<DieFaceSymbol> { }),
					new DieFace(new List<DieFaceSymbol> { new DieFaceSymbol(Symbol.Failure, 1) }),
					new DieFace(new List<DieFaceSymbol> { new DieFaceSymbol(Symbol.Failure, 1) }),
					new DieFace(new List<DieFaceSymbol> { new DieFaceSymbol(Symbol.Threat, 1) }),
					new DieFace(new List<DieFaceSymbol> { new DieFaceSymbol(Symbol.Threat, 1) }),
				};
		}
	}
}
