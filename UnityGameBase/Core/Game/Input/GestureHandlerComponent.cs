using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace UnityGameBase.Core.Input
{
	/// <summary>
	/// Abstract base class for all components, that create gestures. 
	/// Create a custom class to use this system and derive from this class. 
	/// You should register your class with GInput.RegisterGestureHandler(GestureHandlerComponent handler);
	/// </summary>
	public abstract class GestureHandlerComponent<T> : GestureHandlerBase where T : BaseGesture, new()
	{

		/// <summary>
		/// Returns the gesture type this handler creates and handles. 
		/// </summary>
		/// <value>The type of the gesture.</value>
		public System.Type GestureType
		{
			get
			{
				return typeof(T);
			}
		}



		protected T CreateGesture( params TouchInformation[] relatedTouches)
		{
			T gesture = new T();
			gesture.Initialize(relatedTouches, InputSystem);
			return gesture;
		}


		protected T CreateGesture(List<TouchInformation> relatedTouches)
		{
			T gesture = new T();
			gesture.Initialize(relatedTouches.ToArray(), InputSystem);
			return gesture;
		}
	}
}