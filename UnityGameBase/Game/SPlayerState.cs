using System;
using System.Collections.Generic;
namespace UGB
{
	public struct SPlayerState
	{
		static SPlayerState()
		{
			Add(0,"invalid");
		}
		
		public static SPlayerState invalid
		{
			get { return new SPlayerState(0); }
		}
		
		private static Dictionary<int,string> mStates = new Dictionary<int, string>();
		
		public static void Add(int pIndex, string pName)
		{
			mStates[pIndex] = pName;
		}
		
		private string mName;
		private int mIndex;
		
		public SPlayerState(string pName)
		{
			mName = "invalid";
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
		
		public SPlayerState(int pIndex)
		{
			mName = "invalid";
			mIndex = 0;
			
			if(!mStates.ContainsKey(pIndex))
			{
				pIndex = 0;
			}
			
			mIndex = pIndex;
			mName = mStates[mIndex];
			
		}
		
		
		public override bool Equals (object obj)
		{
			if(obj is SPlayerState)
				return Equals((SPlayerState)obj);
			return false;
		}
		
		public bool Equals(SPlayerState pState)
		{
			return pState.mIndex == this.mIndex;
		}
		
		public static bool operator !=(SPlayerState a, SPlayerState b)
		{
			return a.mIndex != b.mIndex;
		}
		
		public static bool operator ==(SPlayerState a, SPlayerState b)
		{
			return a.mIndex == b.mIndex;
		}
		public override int GetHashCode ()
		{
			return mIndex;
		}
		
		public static implicit operator int(SPlayerState a)
		{
		    return a.mIndex;
		}
		
		public static implicit operator string(SPlayerState a)
		{
		    return a.mName;
		}
		
		public static implicit operator SPlayerState(int a)
		{
		    return new SPlayerState(a);
		}
		
		public static implicit operator SPlayerState(string a)
		{
		    return new SPlayerState(a);
		}
		public override string ToString ()
		{
			return string.Format ("[{0},{1}]",mIndex,mName);
		}
	}
}