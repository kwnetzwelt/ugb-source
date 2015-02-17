using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace UGB.Utils
{
    public class BaseStateMachine
    {
        //Resultcode to determine the problems or not
        public enum ResultCode
        {
            StateNotExists,
            StateAlreadyExists,
            StateAdded,
            StateActivated,
            StateTransitionFailed,
            StateDeleted,
        }
        
        private Dictionary<string,BaseState> states = new Dictionary<string, BaseState>();        
        private BaseState previousState = null;
        private BaseState activeState = null;
        
        //add a state to the statemachine
        public ResultCode AddState(BaseState state)
        {
            if(state == null)
            {
                return ResultCode.StateNotExists;
            }
            
            if(!this.states.ContainsKey(state.Name))
            {
                this.states.Add(state.Name, state);
                state.Statemachine = this;
                return ResultCode.StateAdded;
            }
            
            return ResultCode.StateAlreadyExists;
        }
        
        //remove a state and call the end method for this state if needed
        public ResultCode RemoveState(string name, bool callEnd = false)
        {
            if(this.states.ContainsKey(name))
            {
                if(callEnd)
                {
                    this.states[name].End();
                }
                
                this.states.Remove(name);
                return ResultCode.StateDeleted;
            }
            
            return ResultCode.StateNotExists;
        }
        
        //returns the given state or null
        public BaseState GetState(string name)
        {
            if(this.states.ContainsKey(name))
            {
                return states[name];
            }
            return null;
        }
        
        //returns the given state or null
        public BaseState GetPreviousState()
        {
            return this.previousState;
        }
                
        //set the active state for updating and test before that the transition conditions
        public virtual ResultCode SetActiveState(string name)
        {
            BaseState state = this.GetState(name);
            if(state == null)
            {
                return ResultCode.StateNotExists;
            }
            
            if(this.activeState != null)
            {
                if(this.activeState.IsTransitionAllowed(state))
                {
                    return this.SwitchState(state);
                }
                else
                {
                    return ResultCode.StateTransitionFailed;
                }
            }   
            //switch state because no active state exist
            return SwitchState(this.GetState(name));
        }
        
        //switch the state and call Start,End and set the previousState
        protected virtual ResultCode SwitchState(BaseState next)
        {
            if(next == null)
            {
                return ResultCode.StateNotExists;
            }
                                
            if(this.activeState != null)
            {
                this.activeState.End();
                this.previousState = this.activeState;
            }
            
            this.activeState = next;
            this.activeState.Start();           
            
            return ResultCode.StateActivated;
        }
        
        //update the current active state
        public virtual void Update()
        {
            if(this.activeState != null)
            {
                this.activeState.Update();
            }
        }
        
    }
}
