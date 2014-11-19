using UnityEngine;
using System.Collections;

namespace UGB
{
	/// <summary>
	/// Use this class to fade between scenes using a (IMGUI-)texture. Access through Game::m_sceneTransition. 
	/// </summary>
	public class SceneTransition : GameComponent
	{
		/// <summary>
		/// The Texture to fade to. This texture will be displayed fullscreen using the Unity3D IMGUI System. 
		/// For advanced loading screen behaviour you can use SceneTransition::mLoadingScreenController. 
		/// </summary>
		public Texture2D mFadeTexture;

		/// <summary>
		/// The duration of the fade animation.
		/// </summary>
		public float mFadeDuration = 0.5f;



		/// <summary>
		/// The Alpha of all other gui element. Will be interpolated to 0, when a transition takes place and back to 1, when the transition is done. 
		/// </summary>
		/// <value>The GUI alpha.</value>
		public static float guiAlpha{
			get;
			private set;
		}

		/// <summary>
		/// Used for Scene Transition Events. 
		/// </summary>
		public delegate void SceneTransitionDelegate(int pSceneID);

		/// <summary>
		/// Will be emitted, when the application is loading the requested scene. 
		/// </summary>
		public event SceneTransitionDelegate OnSceneIsLoading;

		/// <summary>
		/// Will be emitted, when the requested scene was loaded successfully. 
		/// </summary>
		public event SceneTransitionDelegate OnSceneHasChanged;

		/// <summary>
		/// Will be emitted, when the scene was loaded successfully and the transition is done. 
		/// </summary>
		public event SceneTransitionDelegate OnSceneTransitionIsDone;

		bool mTransitionRunning = false;
		float fmGuiALPHA = 0;
		float mGuiAlpha {get { return fmGuiALPHA; } set{
				Debug.Log("set " + value);
				fmGuiALPHA = value;
			}}

		int mNextSceneIdx = 0;
		float mFadeStartTime = 0;

		/// <summary>
		/// A custom controller for loading screen behaviour. 
		/// If you leave this member to null, the SceneTransition::mFadeTexture is used. 
		/// </summary>
		public ILoadingScreenController mLoadingScreenController;

		bool mAnimateInDone = false;
		bool mAnimateOutDone = false;
		void OnAnimateInDone()
		{
			mAnimateInDone = true;
		}
		void OnAnimateOutDone()
		{
			mAnimateOutDone = true;
		}

		
		public void LoadScene(int pSceneID)
		{
			LoadScene(pSceneID, false);
		}
		/// <summary>
		/// Loads the requested scene id.
		/// </summary>
		/// <param name='pSceneID'>
		/// Scene id.
		/// </param>
		/// <param name='pForce'>
		/// P force. use to force a reload of the scene
		/// </param>
		public void LoadScene(int pSceneID, bool pForce)
		{
			
			if(mLoadingScreenController != null && !mLoadingScreenController.isInitialized)
			{
				mLoadingScreenController.Initialize( () => {

					LoadScene(pSceneID, pForce);

				} );
				return;
			}

			if(pSceneID == Application.loadedLevel && !pForce)
				return;
			if(pSceneID == mNextSceneIdx && !pForce)
				return;
			
			if(mTransitionRunning)
			{
				Debug.LogError("A scene transition is already running!", this);
				return;
			}

			Debug.Log("Requested scene: " + pSceneID, this);



			mNextSceneIdx = pSceneID;
			mFadeStartTime = Time.time;

			mAnimateInDone = false;
			mAnimateOutDone = false;

			StartCoroutine(SceneChangeCoroutine());
		}

		
		void Update ()
		{
			guiAlpha = 1-mGuiAlpha;
		}

		bool CanLoadAsync ()
		{
			if(mLoadingScreenController == null)
			{
				if(Application.HasProLicense())
				{
					if(SystemInfo.systemMemorySize < 1024)
						return false;
					
					return true;
				}
				return false;
			}

			return mLoadingScreenController.CanLoadAsync();
		}

		IEnumerator SceneChangeCoroutine()
		{
			mTransitionRunning = true;


			// fade in
			
			if(mLoadingScreenController != null)
			{

				mLoadingScreenController.OnAnimateInBegin(OnAnimateInDone);
				while(!mAnimateInDone)
				{
					yield return 0;
				}
			}else
			{
				while(mGuiAlpha != 1)
				{
					if(mFadeDuration != 0)
						mGuiAlpha = Mathf.Lerp(0,1,(Time.time - mFadeStartTime) / mFadeDuration);
					else
						mGuiAlpha = 1;
					yield return 0;
				}
			}
			yield return new WaitForEndOfFrame();

			// load level 

			TryCall(OnSceneIsLoading);
			if(CanLoadAsync())
			{
				var loadingProcess = Application.LoadLevelAsync(mNextSceneIdx);
				

				while(!loadingProcess.isDone)
					yield return 0;
			}else
			{
				Application.LoadLevel(mNextSceneIdx);
			}
			TryCall(OnSceneHasChanged);
				


			// fade out
			if(mLoadingScreenController != null)
			{
				mLoadingScreenController.OnAnimateOutBegin(OnAnimateOutDone);
				while(!mAnimateOutDone)
				{
					yield return 0;
				}
			}

			mFadeStartTime = Time.time;
			while(mGuiAlpha != 0)
			{
				if(mFadeDuration != 0)
					mGuiAlpha = Mathf.Lerp(1,0,(Time.time - mFadeStartTime) / mFadeDuration);
				else
					mGuiAlpha = 0;
				yield return 0;
			}
			
			TryCall(OnSceneTransitionIsDone);
			mTransitionRunning = false;
		}

		void TryCall(SceneTransitionDelegate pDelegate)
		{
			try
			{
				if(pDelegate != null)
					pDelegate(mNextSceneIdx);
			}catch(System.Exception e)
			{
				Debug.LogException(e);
			}
		}

		void OnGUI()
		{
			if(mGuiAlpha == 0)
				return;
			// this will always be infront of everything
			GUI.depth = int.MinValue;
			GUI.color = new Color(1,1,1,mGuiAlpha);
			
			GUI.DrawTexture(new Rect(0,0,Screen.width,Screen.height) , mFadeTexture, ScaleMode.StretchToFill);
			
		}
	}
}