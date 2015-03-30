using System;
using UnityEngine;

namespace UnityGameBase
{
	/// <summary>
	/// Contains persistent information about a single touch or mouse click+drag on the screen. 
	/// It will be updated automatically. 
	/// </summary>
	public class TouchInformation
	{
		/// <summary>
		/// Virtual joystick radius in inches. See 
		/// </summary>
		public static float joystickSize = 0.5f;
		/// <summary>
		/// Swipe threshold in pixels. If a touch is moved more than this threshold it is considered a swipe. 
		/// <see cref="UGB.Input.TouchInformation.IsSwipe"/>
		/// </summary>
		public static float swipeThreshold = 10.0f;


		/// <summary>
		/// The start position of the touch (in pixels).
		/// </summary>
		public Vector2 startPosition;

		/// <summary>
		/// The current position of the touch (in pixels). If the touch ended this is the last position that the touch was tracked at. 
		/// </summary>
		public Vector2 endPosition;

		/// <summary>
		/// The phase the touch is currently in. 
		/// </summary>
		public TouchPhase phase;

		/// <summary>
		/// The id of this touch (used for comparison)
		/// </summary>
		public int id;


		/// <summary>
		/// An integer representing the finger which was used for this touch
		/// </summary>
		public int fingerId;

		/// <summary>
		/// The distance along the horizontal axis (in pixels) on the screen, that this touch has travelled. 
		/// </summary>
		public float distanceX;

		/// <summary>
		/// The distance along the vertical axis (in pixels) on the screen, that this touch has travelled. 
		/// </summary>
		public float distanceY;

		/// <summary>
		/// The distance between start and end point of this touch (in pixels). 
		/// </summary>
		public float distance;

		/// <summary>
		/// The time (Time.time) at which this touch started. 
		/// </summary>
		public float birthTime;


		float lastUpdateTime;


		Vector2 lastDirection;


		/// <summary>
		/// This can be used to simulate touches created by a mouse. 
		/// </summary>
		/// <param name="position">Position.</param>
		/// <param name="id">Identifier.</param>
		/// <param name="btnId">Button identifier.</param>
		public TouchInformation (Vector2 position, int id, int btnId)
		{
			this.id = id;
			fingerId = btnId;
			phase = TouchPhase.Began;
			startPosition = position;
			endPosition = startPosition;
			birthTime = Time.time;
			lastUpdateTime = Time.time;
		}
		/// <summary>
		/// Creates a touch created by an actual touch device. 
		/// </summary>
		/// <param name="touch">Touch.</param>
		/// <param name="id">Identifier.</param>
		public TouchInformation (Touch touch, int id)
		{
			this.id = id;
			fingerId = touch.fingerId;
			phase = TouchPhase.Began;
			startPosition = touch.position;
			endPosition = startPosition;
			birthTime = Time.time;
			lastUpdateTime = Time.time;
		}
		/// <summary>
		/// Updates this instance and repositions it. 
		/// </summary>
		/// <param name="position">Position.</param>
		/// <param name="btnReleased">If set to <c>true</c> button released.</param>
		public void Update(Vector2 position,bool btnReleased)
		{
			if(btnReleased)
			{
				phase = TouchPhase.Ended;
			}
		
			if(position != endPosition)
			{
				lastDirection = position - endPosition;
				endPosition = position;
				if(!IsDead)
					phase = TouchPhase.Moved;
				distanceX = endPosition.x - startPosition.x;
				distanceY = endPosition.y - startPosition.y;
				distance = Vector2.Distance(startPosition,endPosition);
			}else
			{
				if(!IsDead)
					phase = TouchPhase.Stationary;
			}
			lastUpdateTime = Time.time;
		}

		/// <summary>
		/// Updates this instance with a touch created by the Unity Engine. 
		/// </summary>
		/// <param name="touch">Touch.</param>
		public void Update (Touch touch)
		{
			phase = touch.phase;
			endPosition = touch.position;
			distanceX = endPosition.x - startPosition.x;
			distanceY = endPosition.y - startPosition.y;
			distance = Vector2.Distance(startPosition,endPosition);
			
		}

		/// <summary>
		/// Determines if this instance handles a touch created by the Unity Engine. 
		/// </summary>
		/// <param name="touch">Touch.</param>
		public bool Handles (Touch touch)
		{
			return (fingerId == touch.fingerId);
		}

		/// <summary>
		/// If the touch was cancelled, button was released or the touch ended, this instance is considered dead. 
		/// </summary>
		/// <value><c>true</c> if this instance is dead; otherwise, <c>false</c>.</value>
		public bool IsDead
		{
			get {
				  return phase == TouchPhase.Canceled || phase == TouchPhase.Ended;
			}
		}

		/// <summary>
		/// Determines if this is a tap. This is a tap, if it is dead and is not considered a swipe. 
		/// </summary>
		/// <value><c>true</c> if this instance is tap; otherwise, <c>false</c>.</value>
		public bool IsTap
		{
			get {
				return IsDead && !IsSwipe;
			}
		}

		/// <summary>
		/// If the calculated distance of this touch is more than the swipeThreshold this returns true. 
		/// </summary>
		/// <value><c>true</c> if this touch is a swipe; otherwise, <c>false</c>.</value>
		public bool IsSwipe
		{
			get {
				return distance > swipeThreshold;
			}
		}

		/// <summary>
		/// The screen position (in pixels) of this instance. The axis origin is in the top left corner of the screen. 
		/// </summary>
		/// <value>The screen position.</value>
		public Vector2 ScreenPosition
		{
			get {
				return new Vector2(endPosition.x,Screen.height - endPosition.y);
			}
		}

		/// <summary>
		/// The relative screen position (0 to 1 on both axis) of this instance. The axis origin is in the top left corner of the screen. 
		/// </summary>
		/// <value>The relative screen position.</value>
		public Vector2 RelativeScreenPosition
		{
			get {
				Vector2 sp = ScreenPosition;
				sp.x = sp.x / Screen.width;
				sp.y = sp.y / Screen.height;
				return sp;
			}
		}

		/// <summary>
		/// Determines whether this instance is considered a horizontal swipe. (distanceX > distanceY)
		/// Is only true if the touch is considered a swipe at all. 
		/// </summary>
		/// <value><c>true</c> if is horizontal swipe; otherwise, <c>false</c>.</value>
		public bool IsHorizontalSwipe
		{
			get { return IsSwipe && Mathf.Abs( distanceX ) > Mathf.Abs( distanceY ); }
		}

		/// <summary>
		/// Determines whether this instance is considered a vertical swipe. (distanceY > distance X)
		/// Is only true if the touch is considered a swipe at all. 
		/// </summary>
		/// <value><c>true</c> if this instance is vertical swipe; otherwise, <c>false</c>.</value>
		public bool IsVerticalSwipe
		{
			get { return IsSwipe && Mathf.Abs( distanceY ) > Mathf.Abs( distanceX ); }
		}

		public enum ESwipeDirection
		{
			None,
			Left,
			Right,
			Up,
			Down
		}

		/// <summary>
		/// Returns the swipe direction of this touch. 
		/// </summary>
		public ESwipeDirection GetSwipeDirection()
		{
			if(IsHorizontalSwipe)
			{
				if(distanceX > 0)
					return ESwipeDirection.Right;
				else
					return ESwipeDirection.Left;
			}else if(IsVerticalSwipe)
			{
				if(distanceY > 0)
					return ESwipeDirection.Up;
				else
					return ESwipeDirection.Down;
			}
			return ESwipeDirection.None;
		}
		
		/// <summary>
		/// Virtual Joystick. Returns the current horizontal axis value (relative: -1 to 1) of this instance. 
		/// The Joystick is created at the startposition of this touch. Its size is fixed. 
		/// </summary>
		public float GetHorizontalAxis()
		{
			return Mathf.Clamp(distanceX / (GetDPI() * joystickSize),-1.0f,1.0f);
		}
		
		/// <summary>
		/// Virtual Joystick. Returns the current vertical axis value (relative: -1 to 1) of this instance. 
		/// The Joystick is created at the startposition of this touch. Its size is fixed. 
		/// </summary>
		public float GetVerticalAxis()
		{
			return Mathf.Clamp(distanceY / (GetDPI() * joystickSize),-1.0f,1.0f);
		}
					
		/// <summary>
		/// Returns the time in seconds passed since the birth of this touch. 
		/// </summary>
		public float LifeTime{	get { return Time.time - birthTime;}	}
		
		/// <summary>
		/// Extrapolate the touch in its last direction. Used for kinetic scroll values
		/// </summary>
		public Vector2 Extrapolate(bool screenPosition)
		{
			Vector2 o = endPosition + lastDirection * 0.5f * Mathf.Clamp01(1 - 2*(Time.time - lastUpdateTime));
			if(screenPosition)
				o.y = Screen.height - o.y;
			return o;
		}

		
		float GetDPI()
		{
			if(Screen.dpi == 0)
				return 90;
			return Screen.dpi;
		}
	}
}