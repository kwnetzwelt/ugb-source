using UnityEngine;
using System.Collections;
namespace UGB.Input
{
	/// <summary>
	/// Contains all Input Delegates used by UGB.Input.GameInput
	/// </summary>
	public class InputDelegates
	{
		/// <summary>
		/// Delegate used by UGB.Input.GameInput to notify other classes of a touch event. 
		/// </summary>

		public delegate void OnTouchEventDelegate(TouchInformation pTouchInfo);

		/// <summary>
		/// Delegate used by UGB.Input.GameInput to notify other classes of a key mapping event. 
		/// </summary>
		public delegate void OnKeyMappingDelegate(string pKeyMappingName);
	}
}