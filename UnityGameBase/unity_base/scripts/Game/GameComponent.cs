using System;
using UnityEngine;
public class GameComponent : MonoBehaviour
{
	protected GameOptions GOptions{get { return Game.instance.mGameOptions;}}
	protected GameLogicImplementationBase GLogic{get { return Game.instance.gameLogicImplementation;}}
	protected GameStateManager GState{get { return Game.instance.mGameState;}}
	protected GamePlayer GPlayer{get { return Game.instance.mGamePlayer;}}
	protected GameMusic GMusic{get { return Game.instance.mGameMusic;}}
	protected GameLocalization GLoca{get { return Game.instance.mGameLoca;}}
	protected GamePause GPause{get { return Game.instance.mGamePause;}}
	protected GameInput GInput{get { return Game.instance.mGameInput;}}
	protected GameData GData{ get {return Game.instance.mGameData;} }
	
	public bool handleDesktopInput
	{
		get {
			return Application.isEditor || 
				( Application.platform != RuntimePlatform.Android &&
					Application.platform != RuntimePlatform.IPhonePlayer);
					
		}
	}
	
	/// <summary>
	/// Gets or sets a value indicating whether this instance is paused. GameComponents receive the pause state by implementing OnPauseGame(bool _value)
	/// </summary>
	/// <value>
	/// <c>true</c> if this instance is paused; otherwise, <c>false</c>.
	/// </value>
	public bool isPaused{ get; set;}
	
	public bool isDestroyed
	{
		get;
		protected set;
	}
	
	protected virtual void OnDestroy()
	{
		isDestroyed = true;
	}

}

