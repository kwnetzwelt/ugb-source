using UnityEngine;
using System.Collections;

namespace UnityGameBase.Core
{

	/// <summary>
	/// Use this class to fade between scenes using a (IMGUI-)texture. Access through Game::m_sceneTransition. 
	/// </summary>
	public class SceneTransition : MonoBehaviour
	{
		/// <summary>
		/// The Texture to fade to. This texture will be displayed fullscreen using the Unity3D IMGUI System. 
		/// For advanced loading screen behaviour you can use SceneTransition::mLoadingScreenController. 
		/// </summary>
		public Texture2D fadeTexture = new Texture2D(1, 1);

		/// <summary>
		/// The duration of the fade animation.
		/// </summary>
		public float fadeDuration = 0.5f;

		float guiAlpha;
		
		NextScene nextScene;
		float fadeStartTime = 0f;

		bool transitionRunning = false;
		bool animateInDone = false;
		bool animateOutDone = false;

		/// <summary>
		/// The Alpha of all other gui element. Will be interpolated to 0, when a transition takes place and back to 1, when the transition is done. 
		/// </summary>
		/// <value>The GUI alpha.</value>
		public static float GUIAlpha
		{
			get;
			private set;
		}

		/// <summary>
		/// Used for Scene Transition Events. 
		/// </summary>
		public delegate void SceneTransitionDelegate(NextScene scene);

		/// <summary>
		/// Will be emitted, when the application is loading the requested scene. 
		/// </summary>
		public event SceneTransitionDelegate SceneIsLoading;

		/// <summary>
		/// Will be emitted, when the requested scene was loaded successfully. 
		/// </summary>
		public event SceneTransitionDelegate SceneHasChanged;

		/// <summary>
		/// Will be emitted, when the scene was loaded successfully and the transition is done. 
		/// </summary>
		public event SceneTransitionDelegate SceneTransitionIsDone;


		/// <summary>
		/// A custom controller for loading screen behaviour. 
		/// If you leave this member to null, the SceneTransition::mFadeTexture is used. 
		/// </summary>
		public Animation.ILoadingScreenController loadingScreenController;

		/// <summary>
		/// Loads the scene with the given Index
		/// </summary>
		public void LoadScene(int sceneId)
		{
			var scene = new NextScene(sceneId);
			LoadScene<NextScene>(scene, false);
		}

		public void LoadScene(string sceneName)
		{
			var scene = new NextScene(sceneName);
			LoadScene<NextScene>(scene, false);
		}

		/// <summary>
		/// Loads the requested scene id. Use the force! uhm ... to reload a scene. 
		/// </summary>
		public void LoadScene<T>(T scene, bool force) where T : NextScene
		{
			if (loadingScreenController != null && !loadingScreenController.IsInitialized)
			{
				loadingScreenController.Initialize(() => {
					
					LoadScene(scene, force);
					
				});
				return;
			}
			
			if (CancelSceneChange(scene, force))
			{
				return;
			}
			
			Debug.Log("Requested " + scene.ToString(), this);

			nextScene = scene;
			
			animateInDone = false;
			animateOutDone = false;
			
			StartCoroutine(SceneChangeCoroutine());
		}

		void OnAnimateInDone()
		{
			animateInDone = true;
		}

		void OnAnimateOutDone()
		{
			animateOutDone = true;
		}

		
		void Update()
		{
			GUIAlpha = 1 - guiAlpha;
		}

		bool CanLoadAsync()
		{
			if (loadingScreenController == null)
			{
				if (Application.HasProLicense())
				{
					if (SystemInfo.systemMemorySize < 1024)
						return false;
					
					return true;
				}
				return false;
			}

			return loadingScreenController.CanLoadAsync();
		}

		bool CancelSceneChange(NextScene scene, bool force)
		{
			if ((scene.IsLoadedLevel && !force)
				|| (scene.Equals(nextScene) && !force))
			{
				return true;
			}
			
			if (transitionRunning)
			{
				Debug.LogError("A scene transition is already running!", this);
				return true;
			}
			return false;
		}

		IEnumerator SceneChangeCoroutine()
		{
			transitionRunning = true;

			if (loadingScreenController != null)
			{
				loadingScreenController.AnimateInBegin(OnAnimateInDone);
			}

			fadeStartTime = Time.time;
			while (FadeIn())
			{
				yield return 0;
			}

			while (!nextScene.IsPrepared)
			{
				yield return new WaitForEndOfFrame();
			}

			// load scene
			TryCall(SceneIsLoading);
			if (CanLoadAsync())
			{
				var loadingProcess = nextScene.LoadAsync();

				while (!loadingProcess.isDone)
				{
					yield return 0;
				}
			} else
			{
				nextScene.Load();
			}
			TryCall(SceneHasChanged);

			// fade out
			if (loadingScreenController != null)
			{
				loadingScreenController.AnimateOutBegin(OnAnimateOutDone);
			}

			fadeStartTime = Time.time;
			while (FadeOut())
			{
				yield return 0;
			}
			
			TryCall(SceneTransitionIsDone);
			transitionRunning = false;
		}

		bool FadeIn()
		{
			if (fadeDuration != 0)
			{
				guiAlpha = Mathf.Lerp(0, 1, (Time.time - fadeStartTime) / fadeDuration);
			} else
			{
				guiAlpha = 1;
			}

			return !(animateInDone || guiAlpha == 1f);
		}

		bool FadeOut()
		{
			if (fadeDuration != 0)
			{
				guiAlpha = Mathf.Lerp(1, 0, (Time.time - fadeStartTime) / fadeDuration);
			} else
			{
				guiAlpha = 0;
			}
			
			return !(animateOutDone || guiAlpha == 0f);
		}

		void TryCall(SceneTransitionDelegate callback)
		{
			try
			{
				if (callback != null)
				{
					callback(nextScene);
				}
			} catch (System.Exception e)
			{
				Debug.LogException(e);
			}
		}

		void OnGUI()
		{
			if (guiAlpha == 0)
			{
				return;
			}

			// this will always be infront of everything
			GUI.depth = int.MinValue;
			GUI.color = new Color(1, 1, 1, guiAlpha);
			
			GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), fadeTexture, ScaleMode.StretchToFill);
		}
	}
}