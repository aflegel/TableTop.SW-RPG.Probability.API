using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SwRpgProbability.DataContext;

namespace SwRpgProbability.Dice
{
	/// <summary>
	/// An Enum to capture the different kind of faces
	/// </summary>
	public enum Symbol
	{
		Blank = 0,
		Success = 1,
		Failure = 2,
		Advantage = 3,
		Threat = 4,
		Triumph = 5,
		Despair = 6,
		Light = 7,
		Dark = 8
	}

	public struct DieResult
	{
		public override string ToString()
		{
			return string.Format("\"{0}\",{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14},{15},{16}",
				Dice, Count, Unique,
				Success, Success / (decimal)Count,
				Failure, Failure / (decimal)Count,
				Advantage, Advantage / (decimal)Count,
				Threat, Threat / (decimal)Count,
				Stalemate, Stalemate / (decimal)Count,
				Triumph, Triumph / (decimal)Count,
				Despair, Despair / (decimal)Count);
		}

		public string Dice { get; set; }
		public long Count { get; set; }
		public long Unique { get; set; }
		public long Success { get; set; }
		public long Failure { get; set; }
		public long Advantage { get; set; }
		public long Threat { get; set; }
		public long Stalemate { get; set; }
		public long Triumph { get; set; }
		public long Despair { get; set; }
	}

	public struct Face
	{
		public Dictionary<Symbol, byte> Symbols { get; set; }

		public Face(Dictionary<Symbol, byte> keys)
		{
			Symbols = keys;
		}

		public override int GetHashCode()
		{
			//gets a unique has by summing the hash of the string and a hash of the value
			return Symbols.Sum(x => x.Key.GetHashCode() + x.Value.GetHashCode());
		}

		public override bool Equals(object obj)
		{
			if (obj.GetType() != typeof(Face))
				return false;

			if (((Face)obj).Symbols.Count != Symbols.Count)
				return false;

			var keys = ((Face)obj).Symbols.Keys.Union(Symbols.Keys);

			foreach (Symbol key in keys)
			{
				if (!(((Face)obj).Symbols.ContainsKey(key) && Symbols.ContainsKey(key)))
					return false;
				else if (((Face)obj).Symbols[key] != Symbols[key])
					return false;
			}

			return true;
		}

		/// <summary>
		/// Returns a summarized set of Faces
		/// </summary>
		/// <param name="firstSet"></param>
		/// <returns></returns>
		public Face Merge(Face firstSet)
		{
			Dictionary<Symbol, byte> merged = new Dictionary<Symbol, byte>();

			var keys = firstSet.Symbols.Keys.Union(Symbols.Keys);

			foreach (Symbol key in keys)
			{
				if (firstSet.Symbols.ContainsKey(key))
				{
					//if the face contains the symbol add the number of symbols together
					if (Symbols.ContainsKey(key))
						merged.Add(key, (byte)(firstSet.Symbols[key] + Symbols[key]));
					else
						merged.Add(key, firstSet.Symbols[key]);
				}
				else
				{
					merged.Add(key, Symbols[key]);
				}
			}

			return new Face(merged);
		}

		public override string ToString()
		{
			var ordered = Symbols.OrderBy(x => x.Key.ToString());

			return string.Join(", ", ordered.Select(x => string.Format("{0,9} ({1})", x.Key.ToString(), x.Value)));
		}
	}
}
