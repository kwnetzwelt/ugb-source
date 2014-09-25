using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TouchDetection : GameComponent
{

	public int currentTouchCnt
	{
		get { return mTouches.Count;}
	}
	
	protected List<TouchInformation>mTouches = new List<TouchInformation>();
	
	
	public static int touchCnt
	{
		get { return mTouchCnt;}
	}
	
	private static int mTouchCnt = 0;
	
	public delegate void OnTouchEventDelegate(TouchInformation _pTouchInfo);
	
	// Tap Event
	public event OnTouchEventDelegate OnTapEvent;
	// Swipe Event
	public event OnTouchEventDelegate OnSwipeEvent;
	// Finger Up
	public event OnTouchEventDelegate OnTouchEnd;
	// Finger Down
	public event OnTouchEventDelegate OnTouchStart;
	// Finger generic update
	public event OnTouchEventDelegate OnTouchUpdate;
	
	TouchInformation mMouseTouch;
	
	// Use this for initialization
	void Start ()
	{
	
	}
	
	// Update is called once per frame
	protected virtual void Update ()
	{
		bool anyTouchBegan = false;
		foreach(Touch t in Input.touches)
		{
			if(t.phase == TouchPhase.Began)
				anyTouchBegan = true;
			UpdateTouch(t);
		}
		if(!anyTouchBegan)
			UpdateMouse();

	}
	protected void UpdateMouse()
	{
		if(Input.GetMouseButtonDown(0))
		{
			CreateTouch(Input.mousePosition,0);
		}
		
		if(mMouseTouch != null)
		{
			mMouseTouch.Update(Input.mousePosition,Input.GetMouseButtonUp(0));
			
			if(OnTouchUpdate != null)
			{
				OnTouchUpdate(mMouseTouch);
			}
			
			if(mMouseTouch.isTap)
			{
				HandleTap(mMouseTouch);
			}
			if(mMouseTouch.isSwipe)
			{
				HandleSwipe(mMouseTouch);
			}
			
			if(mMouseTouch.isDead)
			{
				DestroyTouch(mMouseTouch);
				mMouseTouch = null;
				return;
			}
		}
	}
	protected void UpdateTouch(Touch pTouchInfo)
	{
		TouchInformation ti = GetTouch(pTouchInfo);
		
		if(pTouchInfo.phase == TouchPhase.Began)
		{
			if(ti != null)
			{
				// Touch was started twice; weird! Bug?
				ti.mPhase = TouchPhase.Canceled;
				DestroyTouch(ti);		
			}
			CreateTouch(pTouchInfo);
			return;
		}
		
		
		if(ti != null)
		{
			ti.Update(pTouchInfo);
			if(OnTouchUpdate != null)
			{
				OnTouchUpdate(ti);
			}
		}

		if(ti.isTap)
		{
			HandleTap(ti);
		}
		
		if(ti.isSwipe)
		{
			HandleSwipe(ti);
		}
		
		if(ti.isDead)
		{
			DestroyTouch(ti);
			return;
		}
		
	}
			
	void HandleSwipe(TouchInformation pTouchInfo)
	{
		if(OnSwipeEvent != null)
		{
			OnSwipeEvent(pTouchInfo);
		}
	}
	
	void HandleTap(TouchInformation pTouchInfo)
	{
		if(OnTapEvent != null)
		{
			OnTapEvent(pTouchInfo);
		}
	}
	protected TouchInformation GetTouch(int pId)
	{
		foreach(TouchInformation ti in mTouches)
		{
			if(ti.mId == pId)
				return ti;
		}
		return null;
	}
	protected TouchInformation GetTouch(Touch pTouch)
	{
		foreach(TouchInformation ti in mTouches)
		{
			if(ti.Handles(pTouch))
				return ti;
		}
		return null;
	}

	void DestroyTouch (TouchInformation pTouchInfo)
	{
		if(OnTouchEnd != null)
		{
			OnTouchEnd(pTouchInfo);
		}
		
		mTouches.Remove(pTouchInfo);
	}
	void CreateTouch(Vector2 pPosition,int pBtnId)
	{
		mTouchCnt = mTouchCnt+1;
		TouchInformation ti = new TouchInformation(pPosition,mTouchCnt,pBtnId);
		CreateTouch(ti);
		if(mMouseTouch != null)
		{
			DestroyTouch(mMouseTouch);
		}
		mMouseTouch = ti;
	}
	void CreateTouch(Touch pTouch)
	{
		mTouchCnt = mTouchCnt+1;
		
		TouchInformation ti = new TouchInformation(pTouch,mTouchCnt);
		CreateTouch(ti);
	}
	void CreateTouch(TouchInformation pTouchInfo)
	{
		
		mTouches.Add(pTouchInfo);
		
		if(OnTouchStart != null)
		{
			OnTouchStart(pTouchInfo);
		}
	}
}

