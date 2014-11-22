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
		
		public PlayerState PlayerState
		{
			get;
			private set;
		}
		
		private Transform playerInstance;
		private IPlayerController playerController;

		/// <summary>
		/// Returns the player transform. The controller transform
		/// is preferred if set.
		/// </summary>
		/// <value>The player transform.</value>
		public Transform PlayerTransform
		{
			get
			{ 
				if (playerController != null) 
					return playerController.transform;
				else if (playerInstance != null)
					return playerInstance;
				return null;
			}
		}
		public IPlayerController PlayerController
		{
			get { return playerController; }
		}

		public void SetPlayerState(PlayerState newState)
		{
			if (newState == PlayerState)
				return;

			Debug.Log(string.Format("PlayerState changed: {0} > {1}", PlayerState, newState));

			PlayerState oldState = PlayerState;
			PlayerState = newState;
			if (OnPlayerStateChanged != null)
				OnPlayerStateChanged(oldState, newState);
		}

		public void SetController(IPlayerController controller)
		{
			playerController = controller;
		}

		/// <summary>
		/// Sets the new controller and destroys the previous player transform.
		/// </summary>
		/// <param name="controller">Controller.</param>
		/// <param name="destroy">If set to <c>true</c> destroy.</param>
		public void SetController(IPlayerController controller, bool destroy)
		{
			if (destroy)
			{
				RemoveController();
			}
			playerController = controller;
		}

		/// <summary>
		/// Removes the controller and destroys the transform.
		/// </summary>
		public void RemoveController()
		{
			GameObject.Destroy(playerController.transform.gameObject);
			playerController = null;
		}

		/// <summary>
		/// Sets the players transform.
		/// </summary>
		/// <param name="player">PlayerInstance.</param>
		public void SetTransform(Transform player)
		{
			playerInstance = player;
		}

		public void SetTransform(Transform player, bool destroy)
		{
			if (destroy)
			{
				RemoveTransform();
			}
			playerInstance = player;
		}

		public void RemoveTransform()
		{
			GameObject.Destroy(playerInstance.gameObject);
			playerInstance = null;
		}
	}
}