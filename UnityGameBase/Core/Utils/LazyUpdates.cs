using UnityEngine;
using System.Collections;
namespace UnityGameBase.Core.Utils
{
	/// <summary>
	/// A helper class, that provides bools which are toggled in a certain frequency. 
	/// So you can run code when they are true to ensure your code is not running every frame. 
	/// Inherit from this class to us this functionality and implement a LazyUpdate method. 
	/// IMPORTANT: This class can only be used to run code LESS-frequent than the update method runs. 
	/// </summary>
	public abstract class LazyUpdates : MonoBehaviour
	{

		float lastLazyUpdateTime;

		/// <summary>
		/// The frequency after which the next update is considered lazy. 
		/// </summary>
		public float LazyUpdateFrequency = 0.16f;

		/// <summary>
		/// Gets a value indicating if the update is even. This returns true every second frame. 
		/// </summary>
		public static bool IsUpdateEven
		{
			get;
			private set;
		}

		/// <summary>
		/// Returns true during the Frame considered the lazy frame. 
		/// </summary>
		public static bool IsLazyUpdate
		{
			get;
			private set;
		}

		/// <summary>
		/// Is called by this class automatically when the set time in UGB.Utils.LazyUpdates::LazyUpdateFrequency has passed. 
		/// </summary>
		protected abstract void LazyUpdate();

		/// <summary>
		/// You can override the update method to do your custom work. 
		/// </summary>
		protected virtual void Update()
		{
			IsUpdateEven = !IsUpdateEven;
			
			IsLazyUpdate = false;
			
			if((lastLazyUpdateTime >= LazyUpdateFrequency))
			{
				lastLazyUpdateTime = 0;
				IsLazyUpdate = true;

				LazyUpdate();

			}

			lastLazyUpdateTime += Time.deltaTime;
		}
	}
}