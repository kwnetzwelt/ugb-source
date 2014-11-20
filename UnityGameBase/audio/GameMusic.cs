using UnityEngine;
using System.Collections;
using UGB.audio;

namespace UGB.Audio
{
	/// <summary>
	/// Fades and Plays Music (if enabled) according to current game state. 
	/// </summary>
	public class GameMusic : MultiChannelController
	{

		public bool mEnabled = true;
		
		
		public SMusicState currentState
		{
			get;
			private set;
		}
		
		public float mFadeTime = 0.5f;


		bool mInitialized = false;
		ChannelInfo mCurrentChannel;
		
		
		
		void Start()
		{
			currentState = SMusicState.invalid;

			if(mInitialized)
				return;
	#if UNITY_ANDROID
			Init(1);
	#else
			Init (3);
	#endif
			fadeDuration = mFadeTime;
		
			mInitialized = true;
			GOptions.OnAnyOptionChanged += OnAnyOptionChangedEvent;
			
		}
		protected override void OnDestroy()
		{
			base.OnDestroy();
			GOptions.OnAnyOptionChanged -= OnAnyOptionChangedEvent;
			
		}
		
		
		public void OnAnyOptionChangedEvent()
		{
			if(mEnabled != GOptions.IsMusicOn)
			{
				mEnabled = GOptions.IsMusicOn;
				mute = !GOptions.IsMusicOn;
			}
		}
		
		
		
		
		public void StopAllClips(bool pImmediately)
		{
			currentState = SMusicState.invalid;

			if(mCurrentChannel != null)
				Stop(mCurrentChannel, pImmediately);
		}
		

		
		public void PlayClip(SMusicState pState)
		{
			if(currentState != pState)
			{
				Debug.Log("GameMusic: " + pState);
				
				AudioClip _requestedClip  = GetClipForState(pState);
				if(mCurrentChannel != null)
				{
					Stop (mCurrentChannel, false);
				}
				mCurrentChannel = Play(_requestedClip, true);
				currentState = pState;
			}
		}


		
		AudioClip GetClipForState(SMusicState pState)
		{
			Debug.Log("Music requested: " + pState);
			AudioClip ac = pState.GetNextClip();
			return ac;
		}
	}
}