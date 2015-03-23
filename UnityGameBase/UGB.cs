using System;
using UnityGameBase.Core;
using UnityGameBase.Core.Audio;
using UnityGameBase.Core.Globalization;
using UnityGameBase.Core.Input;
using UnityGameBase.Core.ObjPool;
using UnityGameBase.Core.PlatformInterface;

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


        public static T GetGame<T>() where T : Game
        {
            if (Game.Instance != null)
            {
                return Game.Instance as T;
            }
            else
            {
                throw new Exception("No Game Instance found. Do you have a GameRoot in your scene?");
            }
        }

        private static Game GetGame()
        {
            if(Game.Instance != null)
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
