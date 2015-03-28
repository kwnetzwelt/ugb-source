/// 
/// 
/// UNITY GAME BASE v1.0
/// 
/// More Info on development: https://bitbucket.org/kaiwegner/unity-game-base
/// Documentation and reference: https://bitbucket.org/kaiwegner/unity-game-base/wiki
/// 
/// contact: https://bitbucket.org/kaiwegner
/// 
/// 

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/*! \page base_lib_intro Introduction

\section purpose_sec Purpose 

The library provides tools and workflows as well as best practices to create
games using the Unity 3D Game-Engine. It will also provide a generic way to create games. 

\section install_sec Installation and Usage
To use the library simply copy the path "packages/" to your Unity3D project. 
To Get started see \ref getting_started
Version of the lib can be looked up here: UnityGameBaseVersionMenu.cs

*/

/*! \page base_lib_getting_started Getting started

\tableofcontents

\section step1 Step 1: Create a Unity Project
 
Create a Unity project at a location you wish to use. Don't choose any additional packages from your library. 
Once you are done, you should have a project completely void of scripts, textures, models or any other asset. 

Before you proceed make sure to switch the empty project to the meta-files format. To do that, chooose "Edit => Project Settings => Editor". In the "Version Control" section, choose "Meta files" as the mode for your project. 
And finally in the "Asset Serialization" section you should switch the mode to "Force Text". 

\section step2 Step 2: Adding the library to your project

To get the library, you will have to checkout the latest version at https://bitbucket.org/kaiwegner/ugb-source.git as a submodule to your Unity Project. 

It is usually added as a submodule in Assets/packages/UGB. 

Once the library is installed within your project and unity recompiled the source code, you should see a "UGB"-Menu Item appear. 
Beneath it you will find several settings options and tools. It also displays the version of the Unity Game Base in your project. 


\section step3 Step 3: Create your default scene

You can automatically setup your game to use the Unity Game Base by running our included Setup Wizard. 
To do so, simply import the contents of Assets/packages/UnityGameBase in your project and choose "UGB => Setup => Wizard".
It will display a window and run all steps needed to setup your project and get you started. 

The game library relies heavily on the Game singleton class. The usual project startup layout is as follows. 
You have a default scene, which resides in scenes/default.unity. It only contains one GameObject which is called something like "_Game" or "_Root". 
It has one component of type Game. 

The Game component provides access to all other main game components such as localization, input, game state and more. 
There are three important settings you should consider setting. The first is the Testing flag. 

This flag allows you to set this instance of the Game component to be a testing instance. 
You can read this setting at runtime and prevent scene changes or other state changes. The "_Game" object should be able to reside in every scene of your project and thus provide instant testability to level designers and artists. 

The last setting is the version number. It is used to version save game entries. Once a version entry with a lower version number than your game is loaded, you will get a callback and thus be able to upgrade the data within the save game entry. 

\section step4 Step 4: Setup your game

Create a folder named "scripts". You will put all your CS-files in this folder. 

Add a class to the Folder. It will be your main game logic class. It is usually called "GameLogic". 
It needs to derive from GameLogicImplementationBase and implement the abstract members of this class. 

To actually use the implementation add the UGB.GameLogicImplementationAttribute to the class. 

\see UGB.GameLogicImplementationBase
\see UGB.GameStateManager

 */

/// <summary>
/// Game Main Class. Please look at \ref base_lib_getting_started for further instructions on using it. 
/// </summary>
using UnityGameBase.Core.Extensions;
using UnityGameBase.Core.Input;
using UnityGameBase.Core.Audio;
using UnityGameBase.Core.Globalization;
using UnityGameBase.Core.Utils;
using UnityGameBase.Core;
using UnityGameBase.Core.ObjPool;
using UnityGameBase.WebGL;

namespace UnityGameBase
{
    [ScriptExecutionOrder(-1000)]
    public abstract class Game : MonoBehaviour
    {
        #region abstract methods

        /// <summary>
        /// Use this method to setup your environment before you load the first (visible) scene. Usually
        /// this is the place where languages, game states, events and player states are registered. 
        /// This method is called before any other game component is present. 
        /// Calling \link GameLogicImplementationBase::GState GState \endlink or \link GameLogicImplementationBase::GLoca GLoca \endlink will therefore result in a NullReferenceException. 
        /// If you need access to another component for your setup use \link GameLogicImplementationBase::GameSetupReady GameSetupReady \endlink.
        /// 
        /// \see Languages
        /// \see SGameEventType
        /// \see SPlayerState
        /// \see SGameState
        /// 
        /// </summary>
        protected abstract void Initialize();

        /// <summary>
        /// All Game components are loaded and the game can commence. 
        /// Use this method to setup further components you will need later. 
        /// When you are ready with your last setup steps you would usually want to load the scene with index 1. 
        /// </summary>
        protected abstract void GameSetupReady();

        #endregion

        public bool testing;

        public static Game Instance
        {
            get;
            private set;
        }
              
		
        // Game Components
        public GameOptions gameOptions;
        public GameMusic gameMusic;
        public GameLocalization gameLoca;
        public GameInput gameInput;
        public SceneTransition sceneTransition;
        
        public ObjectPool objectPool;

        public int version;
		
        bool initialized = false;
        
        public static WebGLPlatformHelper webGLHelper;

        IEnumerator InitializeInternal()
        {
            initialized = true;
            Instance = this;
            DontDestroyOnLoad(this);

            ThreadingBridge.Initialize();

            // for webgl we cannot use custom attributes to find classes, so we rely on a delegate to create the logic instance for us. 

            Initialize();

            #if UNITY_WEBGL
            webGLHelper = this.AddComponentIfNotExists<WebGLPlatformHelper>();          
            #endif
            
            gameOptions = this.AddComponentIfNotExists<GameOptions>();
            gameMusic = this.AddComponentIfNotExists<GameMusic>();
            gameLoca = this.AddComponentIfNotExists<GameLocalization>();
            gameLoca.Initialize();
            gameInput = this.AddComponentIfNotExists<GameInput>();
            sceneTransition = this.AddComponentIfNotExists<SceneTransition>();
            
           
            
            objectPool = new ObjectPool();

            yield return new WaitForEndOfFrame();

            GameSetupReady();
        }
		
        void OnEnable()
        {
            if((testing && !Application.isEditor) || (Application.isEditor && testing && Instance != null))
            {
				
					
                GameObject.Destroy(this.gameObject);
                Debug.Log("Destroyed Test Game Instance");
                return;
            }
            
            if(Instance != null)
            {
                GameObject.Destroy(this);
                return;
            }

            if(!initialized)
            {
                StartCoroutine(InitializeInternal());
            }
        }
		
        public void QuitGame()
        {
			GameObject.Destroy(this.gameObject);
        }
		
		
        /// <summary>
        /// If game logic approves this will destroy all game objects and then load level 1. 
        /// This can be used to force a reload of all visual elements. 
        /// \see GameLogicImplementationBase::OnBeforeRestart
        /// </summary>
        public void Restart()
        {
            UnityEngine.Object[] allGameObjects = FindObjectsOfType(typeof(GameObject)); 
            foreach(GameObject go in allGameObjects)
                GameObject.Destroy(go);
            Application.LoadLevel(1);
        }

        /// <summary>
        /// If game logice approves this will pause the game and send the OnPauseGame message to all GameObjects. 
        /// This does not change the GameComponent::isPaused flag. 
        /// \see GameLogicImplementationBase::OnBeforePause
        /// </summary>
        /// <param name="pValue">If set to <c>true</c> p value.</param>
        public void PauseGame(bool pValue)
        {
            UnityEngine.Object[] objects = FindObjectsOfType(typeof(GameObject));
			
            foreach(GameObject go in objects)
            {
                go.SendMessage("OnPauseGame", pValue, SendMessageOptions.DontRequireReceiver);
            }
        }
		
        public static void SetTargetFramerate(int pFrames)
        {
            if(Application.targetFrameRate != pFrames)
            {
                Application.targetFrameRate = pFrames;
            }
        }
    }
}