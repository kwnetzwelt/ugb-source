using UnityEngine;
using System.Collections;

namespace UnityGameBase.Core.Input
{
	/// <summary>
	/// Abstract base class for all gestures. 
	/// </summary>
	public abstract class BaseGesture
	{
		/// <summary>
		/// All touched involved in this gesture
		/// </summary>
		public TouchInformation[] RelatedTouches
		{
			get;
			private set;
		}

		/// <summary>
		/// The input system this gesture is based on. 
		/// </summary>
		protected GameInput InputSystem
		{
			get;
			private set;
		}

		public bool IsDead
		{
			get;
			private set;
		}

		public void Initialize(TouchInformation[] touches, GameInput inputSystem)
		{
			RelatedTouches = touches;

			this.InputSystem = inputSystem;
		}

		/// <summary>
		/// Starts the gesture. Emits an event 
		/// </summary>
		public void StartGesture()
		{
			this.InputSystem.EmitGesture(this);
		}

		/// <summary>
		/// The gesture will be set dead and an event will be emitted through the GameInput System. 
		/// </summary>
		public void EndGesture()
		{
			IsDead = true;
			this.InputSystem.EmitGesture(this);
		}

	}

}