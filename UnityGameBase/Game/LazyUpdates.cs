using UnityEngine;
using System.Collections;

public class LazyUpdates : MonoBehaviour
{
	static LazyUpdates()
	{
		GameObject mInstance = new GameObject("lazyUpdates");
		DontDestroyOnLoad( mInstance );
		mInstance.AddComponent<LazyUpdates>();
	}
	float mLastLazyUpdateTime;
	
	public float kLazyUpdateFrequency = 0.16f;
	
	public static bool isUpdateEven
	{
		get;
		private set;
	}
	public static bool isLazyUpdate
	{
		get;
		private set;
	}
	
	void FixedUpdate()
	{
		isUpdateEven = !isUpdateEven;
		
		isLazyUpdate = false;
		
		if((mLastLazyUpdateTime + kLazyUpdateFrequency) < Time.time)
		{
			mLastLazyUpdateTime = Time.time;
			isLazyUpdate = true;
		}
	}
}

