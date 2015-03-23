using UnityEngine;
using System.Collections;
using System.Collections.Generic;
namespace UnityGameBase.Core.audio
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
	public class MultiChannelController : MonoBehaviour
	{
		[SerializeField]
		[HideInInspector]
		List<Channel>channels = new List<Channel>();
		bool mute;
		float fadeDuration = 0.5f;

		/// <summary>
		/// Initialize this instance with a given amount of channels to be used for audio playback. 
		/// </summary>
		/// <param name="channelCount">P channel count.</param>
		public void Init(int channelCount)
		{
			while(channels.Count < channelCount)
			{
				var ch = new Channel(this);
				channels.Add(ch);

			}

			while(channels.Count > channelCount)
			{
				channels[0].Dispose();
				channels.RemoveAt(0);
			}

			UpdateFadeDuration();
			UpdateMuteState();
		}

		/// <summary>
		/// Duration in seconds a clip uses to fade in. Sound Effects will not fade in. 
		/// </summary>
		/// <value>The duration of the fade.</value>
		public float FadeDuration
		{
			get
			{
				return fadeDuration;
			}
			set
			{
				fadeDuration = value;
			}
		}

		public bool Mute
		{
			get
			{
				return mute;
			}
			set
			{
				if(mute != value)
				{
					mute = value;
					UpdateMuteState();
				}
			}
		}

		/// <summary>
		/// Stops playback on the specified channel. 
		/// </summary>
		/// <param name="channel">channel.</param>
		/// <param name="immediately">If set to <c>true</c> stops the channel immediately. (no fading)</param>
		public void Stop(ChannelInfo channel, bool immediately)
		{
			channel.Channel.Stop(immediately);
		}

		/// <summary>
		/// Play the specified audio clip. 
		/// </summary>
		/// <param name="clip">audio clip.</param>
		/// <param name="loop">If set to <c>true</c> loops the audio clip.</param>
		/// <returns>A ChannelInfo instance to stop playback or access the channel. </returns>
		public virtual ChannelInfo Play(AudioClip clip, bool loop)
		{
			var channel = GetFreeChannel();
			channel.Clip = clip;
			channel.Loops = loop;
			channel.FadeDuration = FadeDuration;
			channel.Play();
			ChannelInfo ci = new ChannelInfo();
			ci.Channel = channel;
			return ci;
		}

		/// <summary>
		/// Plays a short sound effect. Fading is disabled. It will not loop. 
		/// </summary>
		/// <param name="clip">P clip.</param>
		public virtual void PlaySoundEffect(AudioClip clip, float volume)
		{
			var channel = GetFreeChannel();
			channel.PlayOneShot(clip, volume);

		}

		public IEnumerable<Channel> Channels
		{
			get
			{
				foreach(var ch in channels)
				{
					yield return ch;
				}
			}
		}

		public void Update()
		{
			foreach(var channel in channels)
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
			foreach(var channel in channels)
			{
				if(channel.State == Channel.ChannelState.stopped)
				{
					chnl = channel;
					break;
				}
				if(channel.State == Channel.ChannelState.oneShot)
				{
					if(channel.OneShotTimeOut < ttl)
					{
						ttl = channel.OneShotTimeOut;
						chnl = channel;
					}
				}else if(channel.ActualVolume < minVal)
				{
					minVal = channel.ActualVolume;
					chnl = channel;
				}
			}
			return chnl;
		}
		
		void UpdateMuteState()
		{
			foreach(var channel in channels)
			{
				channel.Mute = Mute;
			}
		}
		void UpdateFadeDuration()
		{
			foreach(var channel in channels)
			{
				channel.FadeDuration = fadeDuration;
			}
		}
	}
}
