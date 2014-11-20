using System;
using System.Collections.Generic;

namespace UGB
{
	public struct SLanguages
	{
		static SLanguages()
		{
			Add(0,"invalid");
			first = 0;
			invalid = 0;
		}
		
		public static SLanguages invalid
		{
			get;
			private set;
		}
		
		public static SLanguages first
		{
			get;
			private set; 
		}
		
		private static Dictionary<int,string> mLanguages = new Dictionary<int, string>();
		
		public static void Add(int pIndex, string pName)
		{
			mLanguages[pIndex] = pName;

			//
			// first language is actually second entry. first entry is always invalid. 
			//
			if(mLanguages.Count == 2)
				first = pIndex;
		}
		
		private string mName;
		private int mIndex;
		
		public SLanguages(string pName)
		{
			mName = mLanguages[0];
			mIndex = 0;
			foreach(KeyValuePair<int, string>kv in mLanguages)
			{
				if(kv.Value == pName)
				{
					mName = pName;
					mIndex = kv.Key;
					return;
				}
			}
			
			
		}

		public static int count
		{
			get { return mLanguages.Count; }
		}
		
		
		public SLanguages(int pIndex)
		{
			mIndex = pIndex;
			mName = mLanguages[mIndex];
		}
		
		public static IEnumerable<int> Enumerate()
		{
			foreach(KeyValuePair<int, string>kv in mLanguages)
			{
				yield return kv.Key;
			}
		}
		
		public override bool Equals (object obj)
		{
			if(obj is SLanguages)
				return Equals((SLanguages)obj);
			return false;
		}
		
		public bool Equals(SLanguages pState)
		{
			return pState.mIndex == this.mIndex;
		}
		
		public static bool operator !=(SLanguages a, SLanguages b)
		{
			return a.mIndex != b.mIndex;
		}
		
		public static bool operator ==(SLanguages a, SLanguages b)
		{
			return a.mIndex == b.mIndex;
		}
		public override int GetHashCode ()
		{
			return mIndex;
		}
		
		public static implicit operator int(SLanguages a)
		{
		    return a.mIndex;
		}
		
		public static implicit operator string(SLanguages a)
		{
		    return a.mName;
		}
		
		public static implicit operator SLanguages(int a)
		{
		    return new SLanguages(a);
		}
		
		public static implicit operator SLanguages(string a)
		{
		    return new SLanguages(a);
		}
		
		public override string ToString ()
		{
			return string.Format ("[{0},{1}]",mIndex,mName);
		}
	}
}