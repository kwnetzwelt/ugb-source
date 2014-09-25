using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

/// <summary>
/// debug state display. You can get Entries and set their values. 
/// This class will automatically take care of displaying them in a debug window. 
/// </summary>
public class CDebugState : MonoBehaviour
{
	static CDebugState(){
		GameObject go = new GameObject("CDebugState");
		DontDestroyOnLoad( go );
		go.AddComponent<CDebugState>();
	}
	
	
	
	
	
	
	
#region debug entry handling
	
	private static List<DebugEntry> mEntries = new List<DebugEntry>();
	
	public static DebugEntry GetEntry()
	{
		DebugEntry e = new DebugEntry();
		mEntries.Add(e);
		return e;
	}
	public static void DestroyEntry(DebugEntry pEntry)
	{
		if(mEntries.Contains(pEntry))
			mEntries.Remove(pEntry);
	}
	
#endregion

	Rect mBoxRect;
	float mCurrentPosition;
	float mVisiblePosition;
	float mHiddenPosition;
	
	Texture2D mHandleImage;
	Rect mHandleRect;
	
	Rect mContentRect;
	
	Rect mHiddenTriggerRect;
	Rect mVisibleTriggerRect;
	
	bool mHidden = true;
	
	Vector2 mScrollPosition;
	float mFrag;
	
	void Start()
	{
		mHandleImage = Resources.Load("debug/DebugBtn") as Texture2D;
		mHandleRect = new Rect(0,0,mHandleImage.width, mHandleImage.height);
		if(Mathf.Max( Screen.width, Screen.height) <= 1024)
		{
			mHandleRect.width = mHandleRect.width / 2;
			mHandleRect.height = mHandleRect.height / 2;
		}
		
		mHandleRect.x = (Screen.width - mHandleRect.width) / 2;
		
		mHiddenPosition = 0;
		mVisiblePosition = Screen.height / 3;
		
		mContentRect = new Rect((Screen.width - (Screen.width / 3 * 2)) / 2,0,Screen.width / 3 * 2, mVisiblePosition);
		
		mHiddenTriggerRect = mHandleRect;
		mVisibleTriggerRect = mContentRect;
		
		Game.instance.mGameInput.OnTapEvent += OnTap;
		DontDestroyOnLoad(this.gameObject);
	}

	void OnDestroy()
	{
		Game.instance.mGameInput.OnTapEvent -= OnTap;
	}
	
	
	void OnTap (TouchInformation _pTouchInfo)
	{
		if(mHidden && mHiddenTriggerRect.Contains( _pTouchInfo.screenPosition ))
		{
			mHidden = false;
		}
		if(!mHidden && !mVisibleTriggerRect.Contains( _pTouchInfo.screenPosition ))
		{
			mHidden = true;
		}
	}
	void Update()
	{
		if(mHidden)
			mFrag -= Time.deltaTime * 3.0f;
		else
			mFrag += Time.deltaTime * 3.0f;

		mFrag = Mathf.Clamp01(mFrag);
		mCurrentPosition = Mathf.Lerp(mHiddenPosition,mVisiblePosition, mFrag);
		
		mHandleRect.y = mCurrentPosition;
		mContentRect.y = -mContentRect.height + mCurrentPosition;
	}
	
	void OnGUI()
	{
		if(!(Application.isEditor || Debug.isDebugBuild))
			return;
			
		GUI.Box(mContentRect,"");
		
		GUILayout.BeginArea(mContentRect);
		
		mScrollPosition = GUILayout.BeginScrollView(mScrollPosition);
		
		foreach(DebugEntry e in mEntries)
		{
			DrawEntry(e);
		}
		
		GUILayout.EndScrollView();
		
		GUILayout.EndArea();
		
		GUI.DrawTexture(mHandleRect,mHandleImage);
	}
	
	void DrawEntry(DebugEntry pEntry)
	{
		
		if((pEntry.mState & EDebugEntryState.hidden) != 0)
			return;
		
		GUILayout.Label(pEntry.mText);
		
	}
}
