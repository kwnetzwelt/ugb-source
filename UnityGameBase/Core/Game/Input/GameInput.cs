using UnityEngine;
using System.Collections;
using System.Collections.Generic;
namespace UnityGameBase.Core.Input
{
	public class GameInput : TouchDetection
	{
		public List<GestureHandlerBase> gestureHandlers = new List<GestureHandlerBase>();

		public List<KeyMapping> keyMappings = new List<KeyMapping>();
	
		/// <summary>
		/// This even will fire, when a keymapping is triggered. You can configure keymappings either in the editor or at runtime. 
		/// </summary>
		public event InputDelegates.KeyMappingDelegate KeyMappingTriggered;
	
		/// <summary>
		/// Occurs when on key up or finger up.
		/// </summary>
		public event InputDelegates.KeyMappingDelegate KeyUp;
		/// <summary>
		/// Occurs when on key down or finger down.
		/// </summary>
		public event InputDelegates.KeyMappingDelegate KeyDown;

		/// <summary>
		/// Occurs when gesture start.
		/// </summary>
		public event InputDelegates.GestureDelegate GestureStart;

		/// <summary>
		/// Occurs when gesture ends.
		/// </summary>
		public event InputDelegates.GestureDelegate GestureEnd;

		/// <summary>
		/// Used to emit gesture events. If you want to use gestures register you gesture implementation with 
		/// </summary>
		/// <param name="gesture">Gesture.</param>
		public void EmitGesture (BaseGesture gesture)
		{
			if(gesture.IsDead)
			{
				if(GestureEnd != null)
					GestureEnd(gesture);
			}else
			{
				if(GestureStart != null)
					GestureStart(gesture);
			}
		}

		public void RegisterGestureHandler(GestureHandlerBase component)
		{

			gestureHandlers.Add(component);
			component.Initialize(Game.Instance.gameInput);
		}

		public void DeregisterGestureHandler(GestureHandlerBase component)
		{
			gestureHandlers.Remove(component);
		}

		
		public KeyMapping GetKeyMapping(string keyMappingName)
		{
			foreach(KeyMapping km in keyMappings)
			{
				if(km.name == keyMappingName)
					return km;
			}
			return null;
		}
		public TouchInformation GetTouch(string keyMappingName)
		{
			KeyMapping km = GetKeyMapping(keyMappingName);
			if(km != null)
			{
				return GetTouch(km.touchId);
			}
			return null;
		}
		
		protected void Start()
		{
			TouchStart += HandleTouchStart;
			TouchEnd += HandleTouchEnd;
			SwipeEvent += HandleSwipeEvent;
		}
		protected virtual void OnDestroy()
		{
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
				if(km.touchId == touchInfo.id)
				{
					KeyUp(km.name);
				}
			}
		}

		void HandleTouchStart (TouchInformation touchInfo)
		{
			if(KeyDown == null)
				return;
			foreach(KeyMapping km in keyMappings)
			{
				if(km.relativeScreenRect.Contains(touchInfo.RelativeScreenPosition))
				{
					km.touchId = touchInfo.id;
					KeyDown(km.name);
				}
			}	
		}

		void HandleSwipeEvent (TouchInformation pTouchInfo)
		{
			if(KeyMappingTriggered == null)
				return;

			foreach(KeyMapping km in keyMappings)
			{
				if(km.isTap && km.swipeDirection == pTouchInfo.GetSwipeDirection())
				{
					if(km.relativeScreenRect.Contains(pTouchInfo.RelativeScreenPosition))
					{
						KeyMappingTriggered(km.name);
					}
				}
			}
		}

		void HandleOnTapEvent (TouchInformation pTouchInfo)
		{
			if(KeyMappingTriggered == null)
				return;

			foreach(KeyMapping km in keyMappings)
			{
				if(km.isTap && km.swipeDirection == TouchInformation.ESwipeDirection.None)
				{
					if(km.relativeScreenRect.Contains(pTouchInfo.RelativeScreenPosition))
					{
						KeyMappingTriggered(km.name);
					}
				}
			}	
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
				if(km.keyMode != KeyMapping.EKeyMode.None)
				{
					
					if((km.keyMode == KeyMapping.EKeyMode.Down || km.keyMode == KeyMapping.EKeyMode.Any) && UnityEngine.Input.GetKeyDown(km.keyCode))
					{
						if(KeyDown != null)
						{
							KeyDown(km.name);
						}
					}
					
					if((km.keyMode == KeyMapping.EKeyMode.Up || km.keyMode == KeyMapping.EKeyMode.Any)  && UnityEngine.Input.GetKeyUp(km.keyCode))
					{
						if(KeyUp != null)
						{
							KeyUp(km.name);
						}
					}
				}
			}
		}
	}
}
