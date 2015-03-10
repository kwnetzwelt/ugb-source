using System;
using System.Collections.Generic;

namespace UnityGameBase.Core.Globalization
{
	public struct Languages
	{
		static Languages()
		{
			Add(0, "invalid");
			First = 0;
			Invalid = 0;
		}
		
		public static Languages Invalid
		{
			get;
			private set;
		}
		
		public static Languages First
		{
			get;
			private set; 
		}
		
		private static Dictionary<int,string> languages = new Dictionary<int, string>();
		
		public static void Add(int langIndex, string langName)
		{
			languages [langIndex] = langName;

			//
			// first language is actually second entry. first entry is always invalid. 
			//
			if (languages.Count == 2)
				First = langIndex;
		}
		
		private string name;
		private int index;
		
		public Languages(string langName)
		{
			name = languages [0];
			index = 0;
			foreach (KeyValuePair<int, string>kv in languages)
			{
				if (kv.Value == langName)
				{
					name = langName;
					index = kv.Key;
					return;
				}
			}
		}

		public static int count
		{
			get { return languages.Count; }
		}
		
		
		public Languages(int langIndex)
		{
			index = langIndex;
			name = languages [index];
		}
		
		public static IEnumerable<int> Enumerate()
		{
			foreach (KeyValuePair<int, string>kv in languages)
			{
				yield return kv.Key;
			}
		}
		
		public override bool Equals(object obj)
		{
			if (obj is Languages)
				return Equals((Languages)obj);
			return false;
		}
		
		public bool Equals(Languages state)
		{
			return state.index == this.index;
		}
		
		public static bool operator !=(Languages a, Languages b)
		{
			return a.index != b.index;
		}
		
		public static bool operator ==(Languages a, Languages b)
		{
			return a.index == b.index;
		}
		public override int GetHashCode()
		{
			return index;
		}
		
		public static implicit operator int(Languages a)
		{
			return a.index;
		}
		
		public static implicit operator string(Languages a)
		{
			return a.name;
		}
		
		public static implicit operator Languages(int a)
		{
			return new Languages(a);
		}
		
		public static implicit operator Languages(string a)
		{
			return new Languages(a);
		}
		
		public override string ToString()
		{
			return string.Format("[{0},{1}]", index, name);
		}
	}
}