using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace UnityGameBase.Core.Input
{
	/// <summary>
	/// A class which handles touch detection and mouse events and emits corresponding events. 
	/// Touches are persistant. That is, whenever a touch starts a new instance of UGB.Input.TouchInformation is created. 
	/// Its data is then updated automatically by this class. 
	/// </summary>
	public class TouchDetection : MonoBehaviour
	{
		private bool inputEnabled = true;

		public bool InputEnabled
		{
			get { return inputEnabled; }
			set
			{
				// if input gets disabled, we end all touches. 
				if (inputEnabled != value)
				{
					inputEnabled = value;
				}
			}
		}

		/// <summary>
		/// The number of currently active touches (not dead)
		/// </summary>
		/// <value>The current touch count.</value>
		public int CurrentTouchCount
		{
			get { return touches.Count;}
		}


		protected List<TouchInformation>touches = new List<TouchInformation>();
		
		/// <summary>
		/// Number of touches ever started
		/// </summary>
		/// <value>The touch count.</value>
		public static int TouchCount
		{
			get { return touchCount;}
		}
		
		private static int touchCount = 0;
		
		public delegate void TouchEventDelegate(TouchInformation touchInfo);
		
		// Tap Event
		public event TouchEventDelegate TapEvent;
		// Swipe Event
		public event TouchEventDelegate SwipeEvent;
		// Finger Up
		public event TouchEventDelegate TouchEnd;
		// Finger Down
		public event TouchEventDelegate TouchStart;
		// Finger generic update
		public event TouchEventDelegate TouchUpdate;
		
		TouchInformation mouseTouch;
		
		// Use this for initialization
		void Start()
		{
		}
		
		// Update is called once per frame
		protected virtual void Update()
		{
			bool anyTouchBegan = false;
			foreach (Touch t in UnityEngine.Input.touches)
			{
				if (t.phase == TouchPhase.Began)
					anyTouchBegan = true;
				UpdateTouch(t);
			}
			if (!anyTouchBegan)
				UpdateMouse();

		}
		protected void UpdateMouse()
		{
			if (inputEnabled && UnityEngine.Input.GetMouseButtonDown(0))
			{
				CreateTouch(UnityEngine.Input.mousePosition, 0);
			}
			
			if (mouseTouch != null)
			{
				if (!inputEnabled)
				{
					mouseTouch.Update(UnityEngine.Input.mousePosition, true);
				} else
				{
					mouseTouch.Update(UnityEngine.Input.mousePosition, UnityEngine.Input.GetMouseButtonUp(0));
				}
				
				if (TouchUpdate != null)
				{
					TouchUpdate(mouseTouch);
				}
				
				if (mouseTouch.IsTap)
				{
					HandleTap(mouseTouch);
				}
				if (mouseTouch.IsSwipe)
				{
					HandleSwipe(mouseTouch);
				}
				
				if (mouseTouch.IsDead)
				{
					DestroyTouch(mouseTouch);
					mouseTouch = null;
					return;
				}
			}
		}
		protected void UpdateTouch(Touch touchInfo)
		{
			TouchInformation ti = GetTouch(touchInfo);
			
			if (touchInfo.phase == TouchPhase.Began)
			{
				if (ti != null)
				{
					// Touch was started twice; weird! Bug?
					ti.phase = TouchPhase.Canceled;
					DestroyTouch(ti);		
				}
				CreateTouch(touchInfo);
				return;
			}
			
			
			if (ti != null)
			{

				ti.Update(touchInfo);

				if (!inputEnabled)
				{
					ti.phase = TouchPhase.Canceled;
				}

				if (TouchUpdate != null)
				{
					TouchUpdate(ti);
				}
			}

			if (ti.IsTap)
			{
				HandleTap(ti);
			}
			
			if (ti.IsSwipe)
			{
				HandleSwipe(ti);
			}
			
			if (ti.IsDead)
			{
				DestroyTouch(ti);
				return;
			}
			
		}
				
		void HandleSwipe(TouchInformation touchInfo)
		{
			if (SwipeEvent != null)
			{
				SwipeEvent(touchInfo);
			}
		}
		
		void HandleTap(TouchInformation touchInfo)
		{
			if (TapEvent != null)
			{
				TapEvent(touchInfo);
			}
		}
		protected TouchInformation GetTouch(int id)
		{
			foreach (TouchInformation ti in touches)
			{
				if (ti.id == id)
					return ti;
			}
			return null;
		}
		protected TouchInformation GetTouch(Touch touch)
		{
			foreach (TouchInformation ti in touches)
			{
				if (ti.Handles(touch))
					return ti;
			}
			return null;
		}

		void DestroyTouch(TouchInformation touchInfo)
		{
			if (TouchEnd != null)
			{
				TouchEnd(touchInfo);
			}
			
			touches.Remove(touchInfo);
		}


		void CreateTouch(Vector2 position, int btnId)
		{
			TouchInformation ti = null;
			// we don't create any touches if input is disabled. 
			if (InputEnabled)
			{
				touchCount++;
				ti = new TouchInformation(position, touchCount, btnId);
				CreateTouch(ti);
			}

			if (mouseTouch != null)
			{
				DestroyTouch(mouseTouch);
			}

			mouseTouch = ti;
		}


		void CreateTouch(Touch touch)
		{
			// we don't create any touches if input is disabled. 
			if (InputEnabled)
			{

				touchCount++;
				
				TouchInformation ti = new TouchInformation(touch, touchCount);
				CreateTouch(ti);
			}
		}


		void CreateTouch(TouchInformation touchInfo)
		{
			// we don't create any touches if input is disabled. 
			if (InputEnabled)
			{

				touches.Add(touchInfo);
				
				if (TouchStart != null)
				{
					TouchStart(touchInfo);
				}
			}
		}
	}
}