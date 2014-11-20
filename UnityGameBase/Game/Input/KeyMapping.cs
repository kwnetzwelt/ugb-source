using UnityEngine;
using System.Collections;

namespace UGB.Input
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
		public EKeyMode mKeyMode;
		public KeyCode mKeyCode;
		public string mName;
		
		public bool mIsTap;
		public TouchInformation.ESwipeDirection mSwipeDirection;
		public Rect mRelativeScreenRect;
		
		private int mTouchId = -1;
		public int GetTouchId()
		{
			return mTouchId;
		}
		public void SetTouchId(int pValue)
		{
			mTouchId = pValue;
		}
		
		public KeyMapping()
		{
			
		}	
	}
}