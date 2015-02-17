using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace UGB.Utils
{
    public abstract class BaseState
    {   
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
        public abstract void Start();
        public abstract void End();    
        public abstract void Update();
        
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
