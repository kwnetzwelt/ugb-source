using UnityEngine;
using System.Collections;

namespace UGB
{
	/// <summary>
	/// Reacts on the "OnApplicationPause" Message from Unity and sets the IsPaused static member. 
	/// </summary>
	public class GamePause : GameComponent
	{
		
		void OnApplicationPause(bool pPause)
		{
			IsPaused = pPause;
		}
		
	}
}