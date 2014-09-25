using System;
using UnityEngine;

public class TouchInformation
{
	float mLastUpdateTime;
	Vector2 mLastDirection;
	/// <summary>
	/// joystick radius in inch
	/// </summary>
	const float kJoystickSize = 0.5f;
	/// <summary>
	/// swipe threshold in pixels
	/// </summary>
	const float kSwipeThreshold = 10.0f;
	public TouchInformation (Vector2 pPosition, int pId, int pBtnId)
	{
		mId = pId;
		mFingerId = pBtnId;
		mPhase = TouchPhase.Began;
		mStartPosition = pPosition;
		mEndPosition = mStartPosition;
		mBirthTime = Time.time;
		mLastUpdateTime = Time.time;
	}
	public TouchInformation (Touch pTouch, int pId)
	{
		mId = pId;
		mFingerId = pTouch.fingerId;
		mPhase = TouchPhase.Began;
		mStartPosition = pTouch.position;
		mEndPosition = mStartPosition;
		mBirthTime = Time.time;
		mLastUpdateTime = Time.time;
	}

	public void Update(Vector2 pPosition,bool pBtnReleased)
	{
		if(pBtnReleased)
		{
			mPhase = TouchPhase.Ended;
		}
	
		if(pPosition != mEndPosition)
		{
			mLastDirection = pPosition - mEndPosition;
			mEndPosition = pPosition;
			if(!isDead)
				mPhase = TouchPhase.Moved;
			mDistanceX = mEndPosition.x - mStartPosition.x;
			mDistanceY = mEndPosition.y - mStartPosition.y;
			mDistance = Vector2.Distance(mStartPosition,mEndPosition);
		}else
		{
			if(!isDead)
				mPhase = TouchPhase.Stationary;
		}
		mLastUpdateTime = Time.time;
	}
	
	public void Update (Touch pTouch)
	{
		mPhase = pTouch.phase;
		mEndPosition = pTouch.position;
		mDistanceX = mEndPosition.x - mStartPosition.x;
		mDistanceY = mEndPosition.y - mStartPosition.y;
		mDistance = Vector2.Distance(mStartPosition,mEndPosition);
		
	}

	public bool Handles (Touch pTouch)
	{
		return (mFingerId == pTouch.fingerId);
	}
	
	public bool isDead
	{
		get {
			  return mPhase == TouchPhase.Canceled || mPhase == TouchPhase.Ended;
		}
	}
	
	public bool isTap
	{
		get {
			return isDead && !isSwipe;
		}
	}
	
	public bool isSwipe
	{
		get {
			return mDistance > kSwipeThreshold;
		}
	}
	
	public Vector2 screenPosition
	{
		get {
			return new Vector2(mEndPosition.x,Screen.height - mEndPosition.y);
		}
	}
	public Vector2 relativeScreenPosition
	{
		get {
			Vector2 sp = screenPosition;
			sp.x = sp.x / Screen.width;
			sp.y = sp.y / Screen.height;
			return sp;
		}
	}
	
	public bool isHorizontalSwipe
	{
		get { return isSwipe && Mathf.Abs( mDistanceX ) > Mathf.Abs( mDistanceY ); }
	}
	public bool isVerticalSwipe
	{
		get { return isSwipe && Mathf.Abs( mDistanceY ) > Mathf.Abs( mDistanceX ); }
	}
	public enum ESwipeDirection
	{
		None,
		Left,
		Right,
		Up,
		Down
	}
	public ESwipeDirection GetSwipeDirection()
	{
		if(isHorizontalSwipe)
		{
			if(mDistanceX > 0)
				return ESwipeDirection.Right;
			else
				return ESwipeDirection.Left;
		}else if(isVerticalSwipe)
		{
			if(mDistanceY > 0)
				return ESwipeDirection.Up;
			else
				return ESwipeDirection.Down;
		}
		return ESwipeDirection.None;
	}
	
	
	public float GetHorizontalAxis()
	{
		return Mathf.Clamp(mDistanceX / (GetDPI() * kJoystickSize),-1.0f,1.0f);
	}
	
	public float GetVerticalAxis()
	{
		return Mathf.Clamp(mDistanceY / (GetDPI() * kJoystickSize),-1.0f,1.0f);
	}
	
	float GetDPI()
	{
		if(Screen.dpi == 0)
			return 90;
		return Screen.dpi;
	}
		
	
	public Vector2 mStartPosition;
	public Vector2 mEndPosition;
	public TouchPhase mPhase;
	public int mId;
	public int mFingerId;
	public float mDistanceX;
	public float mDistanceY;
	public float mDistance;
	public float mBirthTime;
	public float lifeTime{	get { return Time.time - mBirthTime;}	}
	
	/// <summary>
	/// Extrapolate the touch in its last direction. Used for kinetic scroll values
	/// </summary>
	/// <param name='pScreenPosition'>
	/// _screen position.
	/// </param>
	public Vector2 Extrapolate(bool pScreenPosition)
	{
		Vector2 o = mEndPosition + mLastDirection*0.5f * Mathf.Clamp01(1 - 2*(Time.time - mLastUpdateTime));
		if(pScreenPosition)
			o.y = Screen.height - o.y;
		return o;
	}
	
}

