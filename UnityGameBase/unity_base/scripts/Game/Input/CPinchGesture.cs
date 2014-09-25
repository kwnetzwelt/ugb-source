using System;
using UnityEngine;
using System.Collections.Generic;

public class CPinchGesture : GameComponent
{
	
	public event System.Action<PinchGestureEvent>OnPinchStart;
	
	public event System.Action<PinchGestureEvent>OnPinchEnd;

	
	List<PinchGestureEvent> mPinchGestures = new List<PinchGestureEvent>();
	
	PinchGestureEvent mCurrentPinch;
	
	void OnEnable()
	{
		GInput.OnTouchStart += OnTouchStart;
		GInput.OnTouchEnd += OnTouchEnd;
	}
	
	void OnDisable()
	{
		GInput.OnTouchStart -= OnTouchStart;
		GInput.OnTouchEnd -= OnTouchEnd;
		OnPinchEnd = null;
		OnPinchStart = null;
	}

	void OnTouchEnd (TouchInformation _pTouchInfo)
	{
		if(mCurrentPinch == null)
			return;
		
		if(mCurrentPinch.mTouchOne == _pTouchInfo)
		{
			mCurrentPinch = null;
			return;
		}
		
		foreach(var p in mPinchGestures)
		{
			if(p.mTouchOne == _pTouchInfo || p.mTouchTwo == _pTouchInfo)
			{
				mPinchGestures.Remove(p);
				p.mIsDead = true;
				try
				{
					
					if(OnPinchEnd != null)
						OnPinchEnd(p);
					
				}catch(Exception e)
				{
					Debug.LogException(e);
				}
				break;
			}
		}
		
	}

	void OnTouchStart (TouchInformation _pTouchInfo)
	{
		if(mCurrentPinch != null)
		{
			mCurrentPinch.mTouchTwo = _pTouchInfo;
			mCurrentPinch.mStartDistance = mCurrentPinch.GetCurrentDistance();
			mPinchGestures.Add(mCurrentPinch);
			
			try
			{
				
				if(OnPinchStart != null)
					OnPinchStart(mCurrentPinch);
				
			} catch(Exception e)
			{
				Debug.LogException(e);
			}
			
			mCurrentPinch = null;
			
			
		}else
		{
			mCurrentPinch = new PinchGestureEvent();
			mCurrentPinch.mTouchOne = _pTouchInfo;
		}
	}
	
}


public class PinchGestureEvent
{
	public TouchInformation mTouchOne;
	
	public TouchInformation mTouchTwo;
	
	public float mStartDistance;
	
	
	public float GetCurrentDistance()
	{
		return Vector2.Distance(mTouchOne.mEndPosition, mTouchTwo.mEndPosition);
	}
	
	/// <summary>
	/// Using the given camera the two touches are projectes onto the given plane and the distance is calculated
	/// </summary>
	/// <returns>
	/// The current distance.
	/// </returns>
	/// <param name='pCamera'>
	/// P camera.
	/// </param>
	/// <param name='pPlane'>
	/// P plane.
	/// </param>
	public float GetCurrentDistance(Camera pCamera, Plane pPlane, out Vector3 pCenter)
	{
		Ray r1 = pCamera.ScreenPointToRay(mTouchOne.mEndPosition);
		Ray r2 = pCamera.ScreenPointToRay(mTouchTwo.mEndPosition);
		
		float dist1 = 0;
		float dist2 = 0;
		
		pPlane.Raycast(r1, out dist1);
		pPlane.Raycast(r2, out dist2);
		
		Vector3 p1 = r1.GetPoint(dist1);
		Vector3 p2 = r2.GetPoint(dist2);
		
		pCenter = (p2 - p1) * 0.5f + p2;
		return Vector3.Distance(p1,p2);
	}
	
	public bool mIsDead = false;
}

