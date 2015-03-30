using UnityEngine;
using System.Collections;
using System;

namespace UnityGameBase.Core.Input
{
	/// <summary>
	/// Represents a mapping between touch (or mouse) input on a specific screen region and a keycode on a keyboard.
	/// It is used with <see cref="UGB.Input.GameInput"/>. 
	/// </summary>
	[System.Serializable]
	public class KeyMapping
	{
		public enum EKeyMode
		{
			Any,
			Up,
			Down,
			None
		}
		public EKeyMode keyMode;
		public KeyCode keyCode;
		public string name;
		
		public bool isTap;
		public TouchInformation.ESwipeDirection swipeDirection;
		public Rect relativeScreenRect;

		[NonSerialized]
		public int touchId;
		
		public KeyMapping()
		{
			
		}	
	}
}