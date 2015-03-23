using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityGameBase;


namespace UnityGameBase
{
    public abstract class GameComponent<T> : MonoBehaviour where T : Game
    {
        public T Game 
        {
            get
            {
                return UGB.GetGame<T>();
            }
        }
    }
}
