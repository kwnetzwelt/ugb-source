using System;
using System.Collections.Generic;
namespace UnityGameBase.Core.Player
{
	public struct PlayerState
	{
		static PlayerState()
		{
			Add(0, "invalid");
		}
		
		public static PlayerState Invalid
		{
			get { return new PlayerState(0); }
		}
		
		private static Dictionary<int,string> states = new Dictionary<int, string>();
		
		public static void Add(int stateIndex, string stateName)
		{
			states [stateIndex] = stateName;
		}
		
		private string name;
		private int index;
		
		public PlayerState(string stateName)
		{
			name = "invalid";
			index = 0;
			foreach (KeyValuePair<int, string>kv in states)
			{
				if (kv.Value == stateName)
				{
					name = stateName;
					index = kv.Key;
					return;
				}
			}
		}
		
		public PlayerState(int stateIndex)
		{
			name = "invalid";
			index = 0;
			
			if (!states.ContainsKey(stateIndex))
			{
				stateIndex = 0;
			}
			
			index = stateIndex;
			name = states [index];
		}
		
		
		public override bool Equals(object obj)
		{
			if (obj is PlayerState)
				return Equals((PlayerState)obj);
			return false;
		}
		
		public bool Equals(PlayerState state)
		{
			return state.index == this.index;
		}
		
		public static bool operator !=(PlayerState a, PlayerState b)
		{
			return a.index != b.index;
		}
		
		public static bool operator ==(PlayerState a, PlayerState b)
		{
			return a.index == b.index;
		}
		public override int GetHashCode()
		{
			return index;
		}
		
		public static implicit operator int(PlayerState a)
		{
			return a.index;
		}
		
		public static implicit operator string(PlayerState a)
		{
			return a.name;
		}
		
		public static implicit operator PlayerState(int a)
		{
			return new PlayerState(a);
		}
		
		public static implicit operator PlayerState(string a)
		{
			return new PlayerState(a);
		}
		public override string ToString()
		{
			return string.Format("[{0},{1}]", index, name);
		}
	}
}