using System;
using UnityEngine;
using UnityGameBase.Core.Input;
using UnityGameBase.Core.Audio;
using UnityGameBase.Core.Player;
using UnityGameBase.Core.Globalization;

namespace UnityGameBase.Core
{
	/// <summary>
	/// Base Class for all custom components for your game. 
	/// Has some handy accessors to the game singleton components. 
	/// </summary>
	public class GameComponent : MonoBehaviour
	{
		protected GameOptions GOptions{get { return Game.Instance.gameOptions;}}
		protected GameLogicImplementationBase GLogic{get { return Game.Instance.CurrentGameLogic;}}
		protected GameStateManager GState{get { return Game.Instance.gameState;}}
		protected GamePlayer GPlayer{get { return Game.Instance.gamePlayer;}}
		protected GameMusic GMusic{get { return Game.Instance.gameMusic;}}
		protected GameLocalization GLoca{get { return Game.Instance.gameLoca;}}
		protected GamePause GPause{get { return Game.Instance.gamePause;}}
		protected GameInput GInput{get { return Game.Instance.gameInput;}}
		protected GameData GData{ get {return Game.Instance.gameData;} }

		
		/// <summary>
		/// Gets or sets a value indicating whether this instance is paused. GameComponents receive the pause state by implementing OnPauseGame(bool _value)
		/// </summary>
		/// <value>
		/// <c>true</c> if this instance is paused; otherwise, <c>false</c>.
		/// </value>
		public bool IsPaused{ get; set;}
		
		public bool IsDestroyed
		{
			get;
			protected set;
		}
		
		protected virtual void OnDestroy()
		{
			IsDestroyed = true;
		}

	}

}