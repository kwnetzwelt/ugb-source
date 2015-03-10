using System;
using UnityGameBase.Core;
using UnityGameBase.Core.Audio;
using UnityGameBase.Core.Globalization;
using UnityGameBase.Core.Input;
using UnityGameBase.Core.ObjPool;
using UnityGameBase.Core.PlatformInterface;
using UnityGameBase.Core.Player;

namespace UnityGameBase
{
    public static class UGB
    {
        public static GameInput Input
        {
            get
            {
                return GetGame().gameInput;
            }
        }

        public static GameOptions Options
        {
            get
            {
                return GetGame().gameOptions;
            }
        }

        public static GameStateManager GameState
        {
            get
            {
                return GetGame().gameState;
            }
        }

        public static GamePlayer Player
        {
            get
            {
                return GetGame().gamePlayer;
            }
        }

        public static GameMusic Music
        {
            get
            {
                return GetGame().gameMusic;
            }
        }

        public static GameLocalization Loca
        {
            get
            {
                return GetGame().gameLoca;
            }
        }

        public static GameData Storage
        {
            get
            {
                return GetGame().gameData;
            }
        }

        public static IPlatformInterface Platform
        {
            get
            {
                return PlatformInterface.Instance;
            }
        }

        public static ObjectPool Pool
        {
            get
            {
                return GetGame().objectPool;
            }
        }

        public static T GetGameLogic<T>() where T : GameLogicImplementationBase
        {
            return GetGame().CurrentGameLogic as T;
        }

        private static Game GetGame()
        {
            if (Game.Instance != null)
            {
                return Game.Instance;
            }
            else
            {
                throw new Exception("No Game Instance found. Do you have a GameRoot in your scene?");
            }
        }
    }
}
