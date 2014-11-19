using System;
using UnityEngine;

/// <summary>
/// Component for use with CLoadingSceneController. 
/// <see cref="CLoadingSceneController"/>
/// </summary>

public class CLoadingScene : MonoBehaviour
{

	public AnimationClip mInAnimation;

	public AnimationClip mLoopAnimation;

	public AnimationClip mOutAnimation;

	public Camera mCamera;

	void Awake()
	{
		GameObject.DontDestroyOnLoad(this.gameObject);
		if(mCamera != null)
			GameObject.DontDestroyOnLoad(mCamera.gameObject);
	}

}

