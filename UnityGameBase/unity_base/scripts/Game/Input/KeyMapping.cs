using UnityEngine;
using System.Collections;

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

