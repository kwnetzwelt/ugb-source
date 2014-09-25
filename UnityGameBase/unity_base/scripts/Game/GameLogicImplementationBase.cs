using System;
using UGB.Savegame;

public abstract class GameLogicImplementationBase
{
	public GameLogicImplementationBase ()
	{
	}
	
	
	protected GameOptions GOptions{get { return Game.instance.mGameOptions;}}
	protected GameLogicImplementationBase GLogic{get { return Game.instance.gameLogicImplementation;}}
	protected GameStateManager GState{get { return Game.instance.mGameState;}}
	protected GamePlayer GPlayer{get { return Game.instance.mGamePlayer;}}
	protected GameMusic GMusic{get { return Game.instance.mGameMusic;}}
	protected GameLocalization GLoca{get { return Game.instance.mGameLoca;}}
	protected GamePause GPause{get { return Game.instance.mGamePause;}}
	protected GameInput GInput{get { return Game.instance.mGameInput;}}
	protected GameData GData{ get {return Game.instance.mGameData;} }
	
	
	/// <summary>
	/// Use this method to setup your environment before you load the first (visible) scene. Usually
	/// this is the place where languages, game states, events and player states are registered. 
	/// This method is called before any other game component is present. 
	/// Calling \link GameLogicImplementationBase::GState GState \endlink or \link GameLogicImplementationBase::GLoca GLoca \endlink will therefore result in a NullReferenceException. 
	/// If you need access to another component for your setup use \link GameLogicImplementationBase::GameSetupReady GameSetupReady \endlink.
	/// 
	/// \see SLanguages
	/// \see SGameEventType
	/// \see SPlayerState
	/// \see SGameState
	/// 
	/// </summary>
	public abstract void Start();




	/// <summary>
	/// All Game components are loaded and the game can commence. 
	/// Use this method to setup further components you will need later. 
	/// When you are ready with your last setup steps you would usually want to load the scene with index 1. 
	/// </summary>
	public abstract void GameSetupReady();
	
	
	
	/// <summary>
	/// When your \link GameLogicImplementationBase::GetCurrentGameState GetCurrentGameState \endlink returns a 
	/// value different from the most recent call, the game state manager will call this method to notify your game logic. 
	/// The old state and the now active state are given as parameters. 
	/// </summary>
	/// <param name='pOldState'>
	/// The old game state.
	/// </param>
	/// <param name='pCurrentGameState'>
	/// The now active game state.
	/// </param>
	public abstract void GameStateChanged (SGameState pOldState, SGameState pCurrentGameState);
	
	
	/// <summary>
	/// Your game logic implementation is required to always infere the current game state. 
	/// You return the current game state here. Usually the current game state directly relates to the currently loaded scene index. 
	/// </summary>
	/// <returns>
	/// The current game state.
	/// </returns>
	public abstract SGameState GetCurrentGameState ();

	/// <summary>
	/// Called by the game instance if Game::Restart is called. Return false to prevent restarting. 
	/// </summary>
	public abstract bool OnBeforeRestart ();
	
	/// <summary>
	/// Called by the game instance if Game::PauseGame is called. Return false to prevent pausing. 
	/// </summary>
	public abstract bool OnBeforePause();

}

