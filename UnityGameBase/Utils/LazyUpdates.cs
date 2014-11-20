using UnityEngine;
using System.Collections;
namespace UGB.Utils
{
	/// <summary>
	/// A helper class, that provides bools which are toggled in a certain frequency. 
	/// So you can run code when they are true or register to the <see cref="UGB.LazyUpdates::OnLazyUpdate"/> event.
	/// </summary>
	public class LazyUpdates : GameComponent
	{
		static LazyUpdates()
		{
			GameObject instance = new GameObject("_LazyUpdates");
			DontDestroyOnLoad( instance );
			instance.AddComponent<LazyUpdates>();
		}
		float lastLazyUpdateTime;
		
		public float LazyUpdateFrequency = 0.16f;
		
		public static bool IsUpdateEven
		{
			get;
			private set;
		}
		public static bool IsLazyUpdate
		{
			get;
			private set;
		}
		
		public event System.Action OnLazyUpdate;
		
		void FixedUpdate()
		{
			IsUpdateEven = !IsUpdateEven;
			
			IsLazyUpdate = false;
			
			if((lastLazyUpdateTime + LazyUpdateFrequency) < Time.time)
			{
				lastLazyUpdateTime = Time.time;
				IsLazyUpdate = true;
				
				if(OnLazyUpdate != null)
				{
					OnLazyUpdate();
				}
			}
		}
	}
}