using System;
using System.Collections.Generic;
	
namespace UnityGameBase.Core
{
	public struct SGameState
	{
		static SGameState()
		{
			Add(0,"invalid");
		}
		
		public static SGameState invalid
		{
			get { return new SGameState(0);}
		}
		
		private static Dictionary<int,string> mStates = new Dictionary<int, string>();
		
		public static void Add(int pIndex, string pName)
		{
			mStates[pIndex] = pName;
		}
		
		private string mName;
		private int mIndex;
		
		public SGameState(string pName)
		{
			mName = mStates[0];
			mIndex = 0;
			foreach(KeyValuePair<int, string>kv in mStates)
			{
				if(kv.Value == pName)
				{
					mName = pName;
					mIndex = kv.Key;
					return;
				}
			}
			
			
		}
		
		public SGameState(int pIndex)
		{
			if(!mStates.ContainsKey(pIndex))
			{
				pIndex = 0;
			}
			
			mIndex = pIndex;
			mName = mStates[mIndex];
		}
		
		
		public override bool Equals (object obj)
		{
			if(obj is SGameState)
				return Equals((SGameState)obj);
			return false;
		}
		
		public bool Equals(SGameState pState)
		{
			return pState.mIndex == this.mIndex;
		}
		
		public static bool operator !=(SGameState a, SGameState b)
		{
			return a.mIndex != b.mIndex;
		}
		
		public static bool operator ==(SGameState a, SGameState b)
		{
			return a.mIndex == b.mIndex;
		}
		public override int GetHashCode ()
		{
			return mIndex;
		}
		
		public static implicit operator int(SGameState a)
		{
		    return a.mIndex;
		}
		
		public static implicit operator string(SGameState a)
		{
		    return a.mName;
		}
		
		public static implicit operator SGameState(int a)
		{
		    return new SGameState(a);
		}
		
		public static implicit operator SGameState(string a)
		{
		    return new SGameState(a);
		}
		public override string ToString ()
		{
			return string.Format ("[{0}, {1}]",mIndex,mName);
		}
	}
}