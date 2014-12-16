using UnityEngine;
using System.Collections;
namespace UGB.Input
{
	public class PinchGesture : BaseGesture
	{

		public TouchInformation mTouchOne;
		
		public TouchInformation mTouchTwo;
		
		public float mStartDistance;
		
		
		public float GetCurrentDistance()
		{
			return Vector2.Distance(mTouchOne.endPosition, mTouchTwo.endPosition);
		}
		
		/// <summary>
		/// Using the given camera the two touches are projectes onto the given plane and the distance is calculated
		/// </summary>
		/// <returns>
		/// The current distance.
		/// </returns>
		/// <param name='pCamera'>
		/// P camera.
		/// </param>
		/// <param name='pPlane'>
		/// P plane.
		/// </param>
		public float GetCurrentDistance(Camera pCamera, Plane pPlane, out Vector3 pCenter)
		{
			Ray r1 = pCamera.ScreenPointToRay(mTouchOne.endPosition);
			Ray r2 = pCamera.ScreenPointToRay(mTouchTwo.endPosition);
			
			float dist1 = 0;
			float dist2 = 0;
			
			pPlane.Raycast(r1, out dist1);
			pPlane.Raycast(r2, out dist2);
			
			Vector3 p1 = r1.GetPoint(dist1);
			Vector3 p2 = r2.GetPoint(dist2);
			
			pCenter = (p2 - p1) * 0.5f + p2;
			return Vector3.Distance(p1,p2);
		}
		
		public bool mIsDead = false;
	}

}