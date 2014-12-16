using System;
using UnityEngine;

namespace UGB.Animation
{
	/// <summary>
	/// Component for use with CLoadingSceneController. 
	/// <see cref="CLoadingSceneController"/>
	/// </summary>

	public class CLoadingScene : MonoBehaviour
	{

		public AnimationClip inAnimation;
		public AnimationClip loopAnimation;
		public AnimationClip outAnimation;

		public Camera loadingCamera;

		void Awake()
		{
			GameObject.DontDestroyOnLoad(this.gameObject);
			if(loadingCamera != null)
				GameObject.DontDestroyOnLoad(loadingCamera.gameObject);
		}

	}

}