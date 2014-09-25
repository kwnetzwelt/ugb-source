using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameInput : TouchDetection
{
	public const string kMappingGenericTap = "genericTap";
	
	
	public List<KeyMapping> mKeyMappings = new List<KeyMapping>();
#pragma warning disable 067
	public event InputDelegates.OnKeyMappingDelegate OnKeyMappingTriggered;
#pragma warning restore
	/// <summary>
	/// Occurs when on key up or finger up.
	/// </summary>
	public event InputDelegates.OnKeyMappingDelegate OnKeyUp;
	/// <summary>
	/// Occurs when on key down or finger down.
	/// </summary>
	public event InputDelegates.OnKeyMappingDelegate OnKeyDown;
	
	
	protected void Start()
	{
		OnTouchStart += HandleOnTouchStart;
		OnTouchEnd += HandleOnTouchEnd;
		OnSwipeEvent += HandleOnSwipeEvent;
	}
	protected override void OnDestroy()
	{
		base.OnDestroy();
		OnTouchStart -= HandleOnTouchStart;
		OnTouchEnd -= HandleOnTouchEnd;
		OnSwipeEvent -= HandleOnSwipeEvent;
	}
	void HandleOnTouchEnd (TouchInformation pTouchInfo)
	{
#if UNITY_ANDROID || UNITY_IOS || UNITY_BB10
		if(OnKeyUp == null)
			return;
		
		foreach(KeyMapping km in mKeyMappings)
		{
			if(km.GetTouchId() == pTouchInfo.mId)
			{
				OnKeyUp(km.mName);
			}
		}	
#endif
	}

	void HandleOnTouchStart (TouchInformation pTouchInfo)
	{
#if UNITY_ANDROID || UNITY_IOS || UNITY_BB10
		foreach(KeyMapping km in mKeyMappings)
		{
			if(km.mRelativeScreenRect.Contains(pTouchInfo.relativeScreenPosition))
			{
				km.SetTouchId(pTouchInfo.mId);
				if(OnKeyDown != null)
					OnKeyDown(km.mName);
			}
		}	
#endif		
	}
	/// <summary>
	/// Handles the on swipe event. and calls keymapping delegates
	/// </summary>
	/// <param name='pTouchInfo'>
	/// _ti.
	/// </param>
	void HandleOnSwipeEvent (TouchInformation pTouchInfo)
	{
#if UNITY_ANDROID || UNITY_IOS || UNITY_BB10
		foreach(KeyMapping km in mKeyMappings)
		{
			if(km.mIsTap && km.mSwipeDirection == pTouchInfo.GetSwipeDirection())
			{
				if(km.mRelativeScreenRect.Contains(pTouchInfo.relativeScreenPosition))
				{
					if(OnKeyMappingTriggered != null)
					{
						OnKeyMappingTriggered(km.mName);
					}
				}
			}
		}
#endif
	}
	
	/// <summary>
	/// Handles the on tap event and calls keymapping delegates. 
	/// </summary>
	/// <param name='pTouchInfo'>
	/// _ti.
	/// </param>
	void HandleOnTapEvent (TouchInformation pTouchInfo)
	{
#if UNITY_ANDROID || UNITY_IOS || UNITY_BB10
		foreach(KeyMapping km in mKeyMappings)
		{
			if(km.mIsTap && km.mSwipeDirection == TouchInformation.ESwipeDirection.None)
			{
				if(km.mRelativeScreenRect.Contains(pTouchInfo.relativeScreenPosition))
				{
					if(OnKeyMappingTriggered != null)
					{
						OnKeyMappingTriggered(km.mName);
					}
				}
			}

		}
#endif		
	}
	public KeyMapping GetKeyMapping(string pKeyMappingName)
	{
		foreach(KeyMapping km in mKeyMappings)
		{
			if(km.mName == pKeyMappingName)
				return km;
		}
		return null;
	}
	public TouchInformation GetTouch(string _keyMappingName)
	{
		KeyMapping km = GetKeyMapping(_keyMappingName);
		if(km != null)
		{
			
			return GetTouch(km.GetTouchId());
		}
		return null;
	}
	
	protected override void Update()
	{
		base.Update();
		
		
		UpdateKeyMappings();
	}
	
	
	void UpdateKeyMappings()
	{
		foreach(KeyMapping km in mKeyMappings)
		{
			if(km.mKeyMode != KeyMapping.EKeyMode.None)
			{
				
				if((km.mKeyMode == KeyMapping.EKeyMode.Down || km.mKeyMode == KeyMapping.EKeyMode.Any) && Input.GetKeyDown(km.mKeyCode))
				{
					if(OnKeyDown != null)
					{
						OnKeyDown(km.mName);
					}
				}
				
				if((km.mKeyMode == KeyMapping.EKeyMode.Up || km.mKeyMode == KeyMapping.EKeyMode.Any)  && Input.GetKeyUp(km.mKeyCode))
				{
					if(OnKeyUp != null)
					{
						OnKeyUp(km.mName);
					}
				}
			}
		}
	}
}

