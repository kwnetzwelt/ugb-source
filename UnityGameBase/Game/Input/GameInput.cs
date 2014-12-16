using UnityEngine;
using System.Collections;
using System.Collections.Generic;
namespace UGB.Input
{
	public class GameInput : TouchDetection
	{
		public List<KeyMapping> keyMappings = new List<KeyMapping>();
	#pragma warning disable 067
		public event InputDelegates.KeyMappingDelegate KeyMappingTriggered;
	#pragma warning restore
		/// <summary>
		/// Occurs when on key up or finger up.
		/// </summary>
		public event InputDelegates.KeyMappingDelegate KeyUp;
		/// <summary>
		/// Occurs when on key down or finger down.
		/// </summary>
		public event InputDelegates.KeyMappingDelegate KeyDown;
		
		
		protected void Start()
		{
			TouchStart += HandleTouchStart;
			TouchEnd += HandleTouchEnd;
			SwipeEvent += HandleSwipeEvent;
		}
		protected override void OnDestroy()
		{
			base.OnDestroy();
			TouchStart -= HandleTouchStart;
			TouchEnd -= HandleTouchEnd;
			SwipeEvent -= HandleSwipeEvent;
		}
		void HandleTouchEnd (TouchInformation touchInfo)
		{
			if(KeyUp == null)
				return;
			
			foreach(KeyMapping km in keyMappings)
			{
				if(km.GetTouchId() == touchInfo.id)
				{
					KeyUp(km.mName);
				}
			}
		}

		void HandleTouchStart (TouchInformation touchInfo)
		{
			if(KeyDown == null)
				return;
			foreach(KeyMapping km in keyMappings)
			{
				if(km.mRelativeScreenRect.Contains(touchInfo.RelativeScreenPosition))
				{
					km.SetTouchId(touchInfo.id);
					KeyDown(km.mName);
				}
			}	
		}
		/// <summary>
		/// Handles the on swipe event. and calls keymapping delegates
		/// </summary>
		/// <param name='pTouchInfo'>
		/// _ti.
		/// </param>
		void HandleSwipeEvent (TouchInformation pTouchInfo)
		{
			if(KeyMappingTriggered == null)
				return;

			foreach(KeyMapping km in keyMappings)
			{
				if(km.mIsTap && km.mSwipeDirection == pTouchInfo.GetSwipeDirection())
				{
					if(km.mRelativeScreenRect.Contains(pTouchInfo.RelativeScreenPosition))
					{
						KeyMappingTriggered(km.mName);
					}
				}
			}
		}
		
		/// <summary>
		/// Handles the on tap event and calls keymapping delegates. 
		/// </summary>
		/// <param name='pTouchInfo'>
		/// _ti.
		/// </param>
		void HandleOnTapEvent (TouchInformation pTouchInfo)
		{
			if(KeyMappingTriggered == null)
				return;

			foreach(KeyMapping km in keyMappings)
			{
				if(km.mIsTap && km.mSwipeDirection == TouchInformation.ESwipeDirection.None)
				{
					if(km.mRelativeScreenRect.Contains(pTouchInfo.RelativeScreenPosition))
					{
						KeyMappingTriggered(km.mName);
					}
				}
			}	
		}
		public KeyMapping GetKeyMapping(string keyMappingName)
		{
			foreach(KeyMapping km in keyMappings)
			{
				if(km.mName == keyMappingName)
					return km;
			}
			return null;
		}
		public TouchInformation GetTouch(string keyMappingName)
		{
			KeyMapping km = GetKeyMapping(keyMappingName);
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
			foreach(KeyMapping km in keyMappings)
			{
				if(km.mKeyMode != KeyMapping.EKeyMode.None)
				{
					
					if((km.mKeyMode == KeyMapping.EKeyMode.Down || km.mKeyMode == KeyMapping.EKeyMode.Any) && UnityEngine.Input.GetKeyDown(km.mKeyCode))
					{
						if(KeyDown != null)
						{
							KeyDown(km.mName);
						}
					}
					
					if((km.mKeyMode == KeyMapping.EKeyMode.Up || km.mKeyMode == KeyMapping.EKeyMode.Any)  && UnityEngine.Input.GetKeyUp(km.mKeyCode))
					{
						if(KeyUp != null)
						{
							KeyUp(km.mName);
						}
					}
				}
			}
		}
	}
}
