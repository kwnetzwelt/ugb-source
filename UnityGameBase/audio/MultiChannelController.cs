using UnityEngine;
using System.Collections;
using System.Collections.Generic;
namespace UGB.audio
{
	/// <summary>
	/// The MultiChannelController is a GameComponent script, that enables you to play various sound effects or music clips 
	/// without the hassle of adding audio sources for them. 
	/// 
	/// You can derive from the MultiChannelController to add functionality or simple add it as a component and use it directly. 
	/// 
	/// To get started you have to initialize the controller by calling MultiChannelController::Init and provide the number of channels you want the controller to use. 
	/// Typically you create your controller with 3 or more channels. Once your controller is created, you can play any audio clip. 
	/// 
	/// There are two ways to play a clip. 
	/// 
	/// Continuous playback can be achieved by calling MultiChannelController::Play This will play the given audio clip on a free channel. 
	/// As a return value you get an instance of ChannelInfo. You can use this to stop the clip or to get the channel responsible for playing the clip. 
	/// 
	/// If what you want to do is play a simple effect, you should use MultiChannelController::PlaySoundEffect. This does not return any info and just plays the provided clip once on a free channel. 
	/// 
	/// A free channel is any channel not currently playing. If no channel is free, the controller will return the controller with the lowest volume. If there is still no channel found, the first channel will be used. 
	/// 
	/// </summary>
	public class MultiChannelController : GameComponent
	{
		[SerializeField]
		[HideInInspector]
		List<Channel>mChannels = new List<Channel>();
		bool mMute;
		float mFadeDuration = 0.5f;

		public void Init(int pChannelCount)
		{
			while(mChannels.Count < pChannelCount)
			{
				var ch = new Channel(this);
				mChannels.Add(ch);

			}

			while(mChannels.Count > pChannelCount)
			{
				mChannels[0].Dispose();
				mChannels.RemoveAt(0);
			}

			UpdateFadeDuration();
			UpdateMuteState();
		}

		public Channel currentChannel = null;

		/// <summary>
		/// Duration in seconds a clip uses to fade in. Sound Effects will not fade in. 
		/// </summary>
		/// <value>The duration of the fade.</value>
		public float fadeDuration
		{
			get
			{
				return mFadeDuration;
			}
			set
			{
				mFadeDuration = value;
			}
		}

		public bool mute
		{
			get
			{
				return mMute;
			}
			set
			{
				if(mMute != value)
				{
					mMute = value;
					UpdateMuteState();
				}
			}
		}

		/// <summary>
		/// Stops playback on the specified channel. 
		/// </summary>
		/// <param name="pChannel">channel.</param>
		/// <param name="pImmediately">If set to <c>true</c> stops the channel immediately. (no fading)</param>
		public void Stop(ChannelInfo pChannel, bool pImmediately)
		{
			pChannel.channel.Stop(pImmediately);
		}

		/// <summary>
		/// Play the specified audio clip. 
		/// </summary>
		/// <param name="pClip">audio clip.</param>
		/// <param name="pLoop">If set to <c>true</c> loops the audio clip.</param>
		/// <returns>A ChannelInfo instance to stop playback or access the channel. </returns>
		public virtual ChannelInfo Play(AudioClip pClip, bool pLoop)
		{
			var channel = GetFreeChannel();
			channel.clip = pClip;
			channel.loop = pLoop;
			channel.fadeDuration = fadeDuration;
			channel.Play();
			ChannelInfo ci = new ChannelInfo();
			ci.channel = channel;
			return ci;
		}

		/// <summary>
		/// Plays a short sound effect. Fading is disabled. It will not loop. 
		/// </summary>
		/// <param name="pClip">P clip.</param>
		public virtual void PlaySoundEffect(AudioClip pClip, float pVolume)
		{
			var channel = GetFreeChannel();
			channel.PlayOneShot(pClip, pVolume);

		}

		public IEnumerable<Channel> Channels
		{
			get
			{
				foreach(var ch in mChannels)
				{
					yield return ch;
				}
			}
		}

		public void Update()
		{
			foreach(var channel in mChannels)
			{
				channel.Update();
			}
		}

		/// <summary>
		/// returns a channel, that is currently stopped. If none found, returns the channel with minimal volume. For channels playing one shots, the channel which has the least time left will be chosen. 
		/// </summary>
		/// <returns>The free channel.</returns>
		Channel GetFreeChannel()
		{
			Channel chnl = null;
			float minVal = float.MaxValue;
			float ttl = float.MaxValue;
			foreach(var channel in mChannels)
			{
				if(channel.state == Channel.eChannelState.stopped)
				{
					chnl = channel;
					break;
				}
				if(channel.state == Channel.eChannelState.oneShot)
				{
					if(channel.oneShotTimeOut < ttl)
					{
						ttl = channel.oneShotTimeOut;
						chnl = channel;
					}
				}else if(channel.actualVolume < minVal)
				{
					minVal = channel.actualVolume;
					chnl = channel;
				}
			}
			return chnl;
		}
		
		void UpdateMuteState()
		{
			foreach(var channel in mChannels)
			{
				channel.mute = mute;
			}
		}
		void UpdateFadeDuration()
		{
			foreach(var channel in mChannels)
			{
				channel.fadeDuration = mFadeDuration;
			}
		}
	}
}
