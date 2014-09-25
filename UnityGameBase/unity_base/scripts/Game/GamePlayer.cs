using UnityEngine;
using System.Collections;
using System;

/// <summary>
/// Contains reference to the PlayerTransform and PlayerController instance. Dispatches playerstatechanges. 
/// </summary>
public class GamePlayer : GameComponent
{
	public delegate void OnPlayerStateChangedDelegate(SPlayerState _old, SPlayerState _new);
	public event OnPlayerStateChangedDelegate OnPlayerStateChanged;
	
	public SPlayerState playerState
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
	
	
	
	public void SetPlayerState(SPlayerState pNewState)
	{
		if(pNewState == playerState)
			return;
		Debug.Log("PlayerState: " + playerState + " > " + pNewState);
	
		SPlayerState old = playerState;
		playerState = pNewState;
		if(OnPlayerStateChanged != null)
			OnPlayerStateChanged(old,pNewState);
	}
	public void SetPlayerController(IPlayerController pControllerInstance)
	{
		mPlayerController = pControllerInstance;
	}
}

