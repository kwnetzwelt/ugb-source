using UnityEngine;
using System.Collections;
using System;

namespace UnityGameBase.Core.audio
{
	/// <summary>
	/// A single channel of audio. This is directly linked to a single UnityEngine.AudioSource instance which is 
	/// automatically created and destroyed with the channel. 
	/// 
	/// 
	/// </summary>
	public class Channel : IDisposable
	{
		MultiChannelController controller;
		float fadeFrag;

		/// <summary>
		/// If this channel plays a one shot, this resembles the time this channel will remain in this state. 
		/// </summary>
		public float OneShotTimeOut
		{
			get;
			private set;
		}


		public enum ChannelState
		{
			oneShot,
			stopped,
			playing,
			fadeIn,
			fadeOut
		}
		
		/// <summary>
		/// Initializes a new instance of the <see cref="UGB.audio.Channel"/> class linked to the given controller.
		/// It automatically creates an AudioSource instance and controls it. 
		/// </summary>
		/// <param name="multiChannelController">Multi channel controller.</param>
		public Channel (MultiChannelController multiChannelController)
		{
			controller = multiChannelController;
			Volume = 1;
			FadeDuration = 0.5f;
			Source = controller.gameObject.AddComponent<AudioSource>();
			Source.loop = false;
			State = ChannelState.stopped;
		}
		/// <summary>
		/// whenever the channel state changes, this event is called. It provides the old and new state as a parameter. 
		/// </summary>
		public event System.Action<ChannelState,ChannelState> ChannelStateChanged;

		/// <summary>
		/// occurs, when the audio is looped. 
		/// </summary>
		public event System.Action Loop;

		/// <summary>
		/// how long (seconds) it takes this channel to fade out or fade in. 
		/// </summary>
		/// <value>The duration of the fade.</value>
		public float FadeDuration
		{
			get;
			set;
		}

		/// <summary>
		/// the current state of the channel. Can be used to check if the channel is currently playing or free to be used. 
		/// </summary>
		/// <value>The state.</value>
		public ChannelState State
		{
			get;
			private set;
		}

		/// <summary>
		/// The volume, which is currently set on the AudioSource this Channel controls. 
		/// </summary>
		/// <value>The actual volume.</value>
		public float ActualVolume
		{
			get;
			private set;
		}

		/// <summary>
		/// The target volume that the channel currently tries to achieve. 
		/// </summary>
		/// <value>The volume.</value>
		public float Volume
		{
			get;
			set;
		}

		/// <summary>
		/// Returns whether the AudioSource linked to this instance is currently playing any sound. 
		/// </summary>
		/// <value><c>true</c> if is playing; otherwise, <c>false</c>.</value>
		public bool IsPlaying
		{
			get
			{
				return Source.isPlaying;
			}
		}

		public bool Mute
		{
			get;
			set;
		}

		public bool Loops
		{
			get
			{
				return Source.loop;
			}

			set
			{
				Source.loop = value;
			}
		
		}

		public AudioClip Clip
		{
			get;
			set;
		}

		public AudioSource Source
		{
			get;
			private set;
		}

		public void Play()
		{
			SetState(ChannelState.fadeIn);
			fadeFrag = 0;
			ActualVolume = 0;
			Source.Play();
		}

		public void PlayOneShot (AudioClip clip, float volume)
		{
			SetState(ChannelState.oneShot);
			OneShotTimeOut = clip.length;
			Clip = clip;
			ActualVolume = Volume;
			Source.volume = ActualVolume;
			Source.clip = clip;
			Source.PlayOneShot(clip, volume);
		}

		public void Stop(bool immediately)
		{

			if(immediately)
				SetState(ChannelState.stopped);
			else
			{
				SetState(ChannelState.fadeOut);
				fadeFrag = 0;
			}

		}



		public void Update ()
		{
			if(State != ChannelState.oneShot)
			{
				Source.loop = Loops;
				Source.clip = Clip;
			}
			UpdateFromState();
			if(State != ChannelState.oneShot)
			{
				Source.volume = GetActualVolume();
			}
		}

		void SetState(ChannelState newState)
		{
			ChannelState oldState = State;
			State = newState;

			if(ChannelStateChanged != null)
				ChannelStateChanged(oldState,newState);
		}

		float GetActualVolume()
		{
			if(Mute)
				return 0;
			return ActualVolume;
		}

		void LerpActualVolume (float targetVolume, float duration)
		{
			if(duration == 0)
				ActualVolume = targetVolume;
			else
			{
				fadeFrag += Time.deltaTime;
				ActualVolume = Mathf.Lerp(ActualVolume, targetVolume, fadeFrag / duration); 
			}
		}

		void UpdateFromState()
		{
			switch(State)
			{
			case ChannelState.fadeIn: 
				LerpActualVolume(Volume, FadeDuration);
				if(Mathf.Epsilon > Mathf.Abs(ActualVolume-Volume))
				{
					ActualVolume = Volume;
					SetState(ChannelState.playing);

				}
				break;
			case ChannelState.fadeOut:
				LerpActualVolume(0, FadeDuration);
				if(Mathf.Epsilon > Mathf.Abs(ActualVolume))
				{
					ActualVolume = 0;
					SetState(ChannelState.stopped);
				}
				break;
			case ChannelState.playing:
				if(!Source.isPlaying)
				{
					if(Loops)
					{
						Source.Play();
						
						if(Loop != null)
							Loop();
					}
					else
						SetState(ChannelState.stopped);
				}

				ActualVolume = Volume;
				break;
			case ChannelState.stopped:
				ActualVolume = 0;
				if(Source.isPlaying)
				{
					Source.Stop();
				}
				break;
			case ChannelState.oneShot:
				OneShotTimeOut -= Time.deltaTime;
				if(OneShotTimeOut <= 0)
					SetState(ChannelState.stopped);
				break;
			}
		}

		#region IDisposable implementation

		public void Dispose ()
		{
			GameObject.Destroy(Source);
		}

		#endregion

	}

}