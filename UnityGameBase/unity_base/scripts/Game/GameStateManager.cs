using System;
using UnityEngine;


/// <summary>
/// Provides a public event which is triggered whenever the game state changes. Polls the current game state from the game logic. 
/// Is used by custom components to retrieve the current game state GameStateManager::currentGameState. Access an instance of this Component via GameComponent::GState. 
/// \see GameLogicImplementationBase::GetCurrentGameState
/// </summary>
public class GameStateManager : GameComponent
{
	
	public delegate void GameStateChangedDelegate(SGameState pOld, SGameState pNew);

	/// <summary>
	/// This event is called, whenever the GameLogicImplementationBase::GetCurrentGameState is different to its most recent value. 
	/// </summary>
	public event GameStateChangedDelegate OnGameStateChanged;
	
	void Start()
	{
		currentGameState = SGameState.invalid;
	}

	/// <summary>
	/// The definitive current game state. Set indirectly by polling GameLogicImplementationBase::GetCurrentGameState. 
	/// </summary>
	public SGameState currentGameState
	{
		get;
		private set;
	}
	
	
	void Update()
	{
		SGameState oldState = currentGameState;
		currentGameState = GetCurrentGameState();
		
		if(oldState != currentGameState)
		{
			
			Debug.Log("Old Game State: " + oldState + " > " + currentGameState);
			
			GLogic.GameStateChanged(oldState, currentGameState);
			
			if(OnGameStateChanged != null)
			{
				OnGameStateChanged(oldState, currentGameState);
			}
			
		}
	}
	SGameState GetCurrentGameState()
	{
		SGameState newState = Game.instance.gameLogicImplementation.GetCurrentGameState();
		return newState;
		
	}
	
}

