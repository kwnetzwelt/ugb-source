using UnityEngine;
using System.Collections;
using System;

namespace UGB.audio
{
	public class Channel : IDisposable
	{
		MultiChannelController controller;
		float fadeFrag;

		/// <summary>
		/// If this channel plays a one shot, this resembles the time this channel will remain in this state. 
		/// </summary>
		public float oneShotTimeOut
		{
			get;
			private set;
		}
		public enum eChannelState
		{
			oneShot,
			stopped,
			playing,
			fadeIn,
			fadeOut
		}
		

		public Channel (MultiChannelController multiChannelController)
		{
			controller = multiChannelController;
			volume = 1;
			fadeDuration = 0.5f;
			source = controller.gameObject.AddComponent<AudioSource>();
			source.loop = false;
			state = eChannelState.stopped;
		}
		/// <summary>
		/// whenever the channel state changes, this event is called. It provides the old and new state as a parameter. 
		/// </summary>
		public event System.Action<eChannelState,eChannelState> OnChannelStateChanged;
		public event System.Action OnLoop;

		public float fadeDuration
		{
			get;
			set;
		}

		public eChannelState state
		{
			get;
			private set;
		}

		public float actualVolume
		{
			get;
			private set;
		}

		public float volume
		{
			get;
			set;
		}

		public bool isPlaying
		{
			get
			{
				return source.isPlaying;
			}
		}

		public bool mute
		{
			get;
			set;
		}

		public bool loop
		{
			get
			{
				return source.loop;
			}

			set
			{
				source.loop = value;
			}
		
		}

		public AudioClip clip
		{
			get;
			set;
		}

		public AudioSource source
		{
			get;
			private set;
		}

		public void Play()
		{
			SetState(eChannelState.fadeIn);
			fadeFrag = 0;
			actualVolume = 0;
			source.Play();
		}

		public void PlayOneShot (AudioClip pClip, float pVolume)
		{
			SetState(eChannelState.oneShot);
			oneShotTimeOut = pClip.length;
			clip = pClip;
			actualVolume = volume;
			source.volume = actualVolume;
			source.clip = pClip;
			source.PlayOneShot(pClip, pVolume);
		}

		public void Stop(bool pImmediately)
		{

			if(pImmediately)
				SetState(eChannelState.stopped);
			else
			{
				SetState(eChannelState.fadeOut);
				fadeFrag = 0;
			}

		}



		public void Update ()
		{
			if(state != eChannelState.oneShot)
			{
				source.loop = loop;
				source.clip = clip;
			}
			UpdateFromState();
			if(state != eChannelState.oneShot)
			{
				source.volume = GetActualVolume();
			}
		}

		void SetState(eChannelState pNewState)
		{
			eChannelState oldState = state;
			state = pNewState;

			if(OnChannelStateChanged != null)
				OnChannelStateChanged(oldState,pNewState);
		}

		float GetActualVolume()
		{
			if(mute)
				return 0;
			return actualVolume;
		}

		void LerpActualVolume (float pTargetVolume, float pDuration)
		{
			if(pDuration == 0)
				actualVolume = pTargetVolume;
			else
			{
				fadeFrag += Time.deltaTime;
				actualVolume = Mathf.Lerp(actualVolume, pTargetVolume, fadeFrag / pDuration); 
			}
		}

		void UpdateFromState()
		{
			switch(state)
			{
			case eChannelState.fadeIn: 
				LerpActualVolume(volume, fadeDuration);
				if(Mathf.Epsilon > Mathf.Abs(actualVolume-volume))
				{
					actualVolume = volume;
					SetState(eChannelState.playing);

				}
				break;
			case eChannelState.fadeOut:
				LerpActualVolume(0, fadeDuration);
				if(Mathf.Epsilon > Mathf.Abs(actualVolume))
				{
					actualVolume = 0;
					SetState(eChannelState.stopped);
				}
				break;
			case eChannelState.playing:
				if(!source.isPlaying)
				{
					if(loop)
					{
						source.Play();
						
						if(OnLoop != null)
							OnLoop();
					}
					else
						SetState(eChannelState.stopped);
				}

				actualVolume = volume;
				break;
			case eChannelState.stopped:
				actualVolume = 0;
				if(source.isPlaying)
				{
					source.Stop();
				}
				break;
			case eChannelState.oneShot:
				oneShotTimeOut -= Time.deltaTime;
				if(oneShotTimeOut <= 0)
					SetState(eChannelState.stopped);
				break;
			}
		}

		#region IDisposable implementation

		public void Dispose ()
		{
			GameObject.Destroy(source);
		}

		#endregion

	}

}