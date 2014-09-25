using System;
using UnityEngine;

/// <summary>
/// Controls Playback of an animated scene while loading another scene in sceneTransition. 
/// 
/// To use this you will have to assign an instance of this class to the Game::mSceneTransition::mLoadingScreenController member. 
/// 
/// It will expect a CLoadingScene component present in the loading scene with assigned animations. 
/// </summary>
using System.Collections;


public class CLoadingSceneController : MonoBehaviour, ILoadingScreenController
{
	/// <summary>
	/// The name of the scene, which is loaded (additive) to the game on startup. This scene must contain a CLoadingScene Component. 
	/// </summary>
	public string mSceneName;
	bool mInitialized = false;

	CLoadingScene mLoadingScene;

	public void Initialize (Action pDoneCbk)
	{
		mInitialized = false;
		Application.LoadLevelAdditive(mSceneName);
		StartCoroutine(WaitForScene(pDoneCbk));
	}

	IEnumerator WaitForScene(System.Action pDoneCbk)
	{
		while(mLoadingScene == null)
		{
			mLoadingScene = GameObject.FindObjectOfType(typeof(CLoadingScene)) as CLoadingScene;
			if(mLoadingScene == null)
				yield return 0;
		}

		mLoadingScene.mCamera.enabled = false;
		mInitialized = true;

		if(pDoneCbk != null)
			pDoneCbk();
	}

	public void OnAnimateInBegin (Action pDoneCbk)
	{
		mLoadingScene.mCamera.enabled = true;
		mLoadingScene.animation.Play( mLoadingScene.mInAnimation.name );
		StartCoroutine(WaitForEndOfAnimation(mLoadingScene.mInAnimation.name, () => {
		
			mLoadingScene.animation.Play( mLoadingScene.mLoopAnimation.name );
			if(pDoneCbk != null)
				pDoneCbk();

		}));
	}

	public void OnAnimateOutBegin (Action pDoneCbk)
	{
		mLoadingScene.animation.Stop( mLoadingScene.mLoopAnimation.name );
		mLoadingScene.animation.Play(mLoadingScene.mOutAnimation.name);
		StartCoroutine(WaitForEndOfAnimation(mLoadingScene.mOutAnimation.name, () => {

			mLoadingScene.mCamera.enabled = false;

			if(pDoneCbk != null)
				pDoneCbk();

		}));
	}

	public bool isInitialized {
		get {
			return mInitialized;
		}
	}

	public bool CanLoadAsync()
	{
		if(Application.HasProLicense())
		{
			if(SystemInfo.systemMemorySize < 1024)
				return false;

			return true;
		}
		return false;
	}


	IEnumerator WaitForEndOfAnimation(string pAnimation, System.Action pDoneCbk)
	{

		while(mLoadingScene.animation.IsPlaying(pAnimation))
		{
			//Debug.Log("Waiting for Animation: " + pAnimation);
			yield return 0;
		}


		if(pDoneCbk != null)
			pDoneCbk();
	}

}

