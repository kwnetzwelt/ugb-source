using UnityEngine;
using System.Collections;
namespace UnityGameBase.Core.Input
{
	public class PinchGesture : BaseGesture
	{

		public TouchInformation firstTouch
		{
			get
			{
				return RelatedTouches[0];
			}
		}

		public TouchInformation secondTouch
		{
			get
			{
				return RelatedTouches[1];
			}
		}

		
		public float startDistance;
		
		
		public float GetCurrentDistance()
		{
			return Vector2.Distance(firstTouch.endPosition, secondTouch.endPosition);
		}
		
		/// <summary>
		/// Using the given camera the two touches are projectes onto the given plane and the distance is calculated
		/// </summary>
		/// <returns>
		/// The current distance.
		/// </returns>
		public float GetCurrentDistance(Camera camera, Plane plane, out Vector3 center)
		{
			Ray r1 = camera.ScreenPointToRay(firstTouch.endPosition);
			Ray r2 = camera.ScreenPointToRay(secondTouch.endPosition);
			
			float dist1 = 0;
			float dist2 = 0;
			
			plane.Raycast(r1, out dist1);
			plane.Raycast(r2, out dist2);
			
			Vector3 p1 = r1.GetPoint(dist1);
			Vector3 p2 = r2.GetPoint(dist2);
			
			center = (p2 - p1) * 0.5f + p2;
			return Vector3.Distance(p1,p2);
		}
	}

}