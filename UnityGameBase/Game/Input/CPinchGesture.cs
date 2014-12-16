using System;
using UnityEngine;
using System.Collections.Generic;

namespace UGB.Input
{
	public class CPinchGesture : GestureHandlerComponent<PinchGesture>
	{
		PinchGesture currentGesture;
		TouchInformation firstTouch;


		#region implemented abstract members of GestureHandlerComponent

		protected override void HandleTouchEnd (TouchInformation touchInfo)
		{
			if(firstTouch == touchInfo)
			{
				firstTouch = null;
				return;
			}
			
			if(currentGesture == null)
			{
				return;
			}
			
			// we are currently pinching. if any of the touches involved ended, we end the gesture. 
			
			foreach(var p in currentGesture.RelatedTouches)
			{
				if(p.IsDead)
				{
					currentGesture.EndGesture();
					currentGesture = null;
				}
			}
		}
		protected override void HandleTouchStart (TouchInformation touchInfo)
		{
			if(firstTouch != null)
			{
				currentGesture = CreateGesture(firstTouch,touchInfo);
				currentGesture.mStartDistance = currentGesture.GetCurrentDistance();
				
				currentGesture.StartGesture();
				
			}else
			{
				firstTouch = touchInfo;
			}
		}
		#endregion
	}
}