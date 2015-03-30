using System;
using System.Collections.Generic;

namespace UnityGameBase.Core.Audio
{
	/// <summary>
	/// Music state struct. Can be used to add audio clips to your custom music states. 
	/// </summary>
	public class SMusicState
	{

		static SMusicState()
		{
			Add(0,"invalid");
			invalid = 0;
		}

		public static SMusicState invalid
		{
			get;
			private set;
		}

		
		private static Dictionary<int,SMusicState> mStates = new Dictionary<int, SMusicState>();

		/// <summary>
		/// Add a new Music State to the central music state system. You can add clips to the returns instance. 
		/// </summary>
		/// <param name="pIndex">index.</param>
		/// <param name="pName">name.</param>
		/// <returns>The static instance for the given index. </returns>
		public static SMusicState Add(int pIndex, string pName)
		{
			var state = new SMusicState(pIndex, pName);
			mStates[pIndex] = state;
			return state;
		}

		/// <summary>
		/// Add an Audio clip to this state. Be sure to set the loading type to stream from disc for all audio clips you add here to avoid memory bloat. 
		/// The Clips are loaded from the resources folder. 
		/// </summary>
		/// <param name="pClip">clip.</param>
		public void AddClip(string pClip)
		{
			mMusicClips.Add(pClip);
		}

		/// <summary>
		/// Returns the next randomly chosen clip for this state
		/// </summary>
		/// <returns>The next clip.</returns>
		public UnityEngine.AudioClip GetNextClip( )
		{

			GetNextClip( (new Random()).Next() );
			return mClip;
		}
		/// <summary>
		/// Returns the next randomly chosen clip for this state. You can pass the seed for constant results. 
		/// </summary>
		/// <returns>The next clip.</returns>
		/// <param name="pSeed">seed</param>
		public UnityEngine.AudioClip GetNextClip( int pSeed )
		{
			System.Random r = new Random( pSeed );
			if(mMusicClips.Count == 0)
				return null;

			mLastIndex = r.Next(0,mMusicClips.Count);

			Unload();
			
			mClip = UnityEngine.Resources.Load(mMusicClips[mLastIndex]) as UnityEngine.AudioClip;
			return mClip;
		}

		public void Unload()
		{
			if(mClip != null)
				UnityEngine.Resources.UnloadAsset(mClip);
		}

		private UnityEngine.AudioClip mClip;
		private int mLastIndex = 0;
		private string mName;
		private int mIndex;

		private List<string> mMusicClips;


		private SMusicState(int pIndex, string pName)
		{
			mIndex = pIndex;
			mName = pName;
			mMusicClips = new List<string>();
		}


		private static SMusicState GetStateByIndex(int pIndex)
		{
			return mStates[ pIndex ];
		}

		private static SMusicState GetStateByName(string pName)
		{
			foreach(var kv in mStates)
				if(kv.Value.mName == pName)
					return kv.Value;

			return null;
		}


		public override bool Equals (object obj)
		{
			if(obj is SMusicState)
				return Equals((SMusicState)obj);
			return false;
		}
		
		public bool Equals(SMusicState pState)
		{
			return pState.mIndex == this.mIndex;
		}
		
		public static bool operator !=(SMusicState a, SMusicState b)
		{
			return a.mIndex != b.mIndex;
		}
		
		public static bool operator ==(SMusicState a, SMusicState b)
		{
			return a.mIndex == b.mIndex;
		}
		public override int GetHashCode ()
		{
			return mIndex;
		}
		
		public static implicit operator int(SMusicState a)
		{
			return a.mIndex;
		}
		
		public static implicit operator string(SMusicState a)
		{
			return a.mName;
		}
		
		public static implicit operator SMusicState(int a)
		{
			return GetStateByIndex(a);
		}
		
		public static implicit operator SMusicState(string a)
		{
			return GetStateByName(a);
		}
		
		public override string ToString ()
		{
			return string.Format ("[{0},{1}]",mIndex,mName);
		}

	}
}