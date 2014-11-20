using UnityEngine;
using System.Collections;
using System;

namespace UGB.Player
{
	/// <summary>
	/// Contains reference to the PlayerTransform and PlayerController instance. Dispatches playerstatechanges. 
	/// </summary>
	public class GamePlayer : GameComponent
	{
		public delegate void PlayerStateChanged(PlayerState oldState,PlayerState newState);
		public event PlayerStateChanged OnPlayerStateChanged;
		
		public PlayerState playerState
		{
			get;
			private set;
		}
		
		public Transform mPlayerPrefab;
		
		private Transform mPlayerInstance;
		private IPlayerController mPlayerController;
		
		public Transform playerTransform
		{
			get { if(mPlayerController != null) return mPlayerController.transform; else return null;}
		}
		public IPlayerController playerController
		{
			get { return mPlayerController; }
		}
		
		
		
		public void SetPlayerState(PlayerState pNewState)
		{
			if(pNewState == playerState)
				return;
			Debug.Log("PlayerState: " + playerState + " > " + pNewState);
		
			PlayerState old = playerState;
			playerState = pNewState;
			if(OnPlayerStateChanged != null)
				OnPlayerStateChanged(old,pNewState);
		}
		public void SetPlayerController(IPlayerController pControllerInstance)
		{
			mPlayerController = pControllerInstance;
		}
	}
}