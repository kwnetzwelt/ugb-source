using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace UnityGameBase.Core.Utils
{
    public abstract class BaseState : IDisposable
    {   
        //the statemachine which owned the state
        public BaseStateMachine Statemachine{ get; set; }
    
        //constructor to use    
        public BaseState(string name)
        {
            this.name = name;
        }
        
        //set default constructor to private to forbid access
        private BaseState()
        {
        
        }
        
        //important abstract members for statehandling
        public abstract void Start(System.Action onDone = null);
        public abstract void End(System.Action onDone = null);    
        public abstract void Update();
        public virtual void Dispose()
		{

		}
        //if desired you can overwrite it and do some transition conditions
        public virtual bool IsTransitionAllowed(BaseState target)
        {
            return true;
        }      
        
        //name property to determine this state
        private string name = "";
        public string Name
        {
            get{ return name;}
        }
    }
}
