using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace UnityGameBase.Core.Utils
{
    public class BaseStateMachine
    {
        /// Resultcode as return value for all public statemachine methods.
        public enum ResultCode
        {
            StateDoesNotExist,
            StateAlreadyExists,
            StateAdded,
            StateActivated,
            StateTransitionFailed,
            StateDeleted,
            StateTransitionActive,
            StateTransitionActivated,
        }

        public delegate void StateChangedDelegate(BaseState previous, BaseState next);
        /// <summary>
        /// Occurs when a state has changed.
        /// </summary>
        public event StateChangedDelegate StateChanged;
        
        private Dictionary<string,BaseState> states = new Dictionary<string, BaseState>();
        private BaseState previousState = null;
        private BaseState activeState = null;
        private BaseState nextState = null;
        private bool isInTransition = false;
        private System.Action transitionReadyCallBack = null;
        
        /// <summary>
        /// Add a state to the statemachine.
        /// </summary>
        public ResultCode AddState(BaseState state)
        {
            if (state == null)
            {
                return ResultCode.StateDoesNotExist;
            }
            
            if (!this.states.ContainsKey(state.Name))
            {
                this.states.Add(state.Name, state);
                state.Statemachine = this;
                return ResultCode.StateAdded;
            }
            
            return ResultCode.StateAlreadyExists;
        }
        
        /// <summary>
        /// Remove a state and call the BaseState.End() method for this state if needed.
        /// </summary>
        public ResultCode RemoveState(string name, bool callEnd = false)
        {
            if (this.states.ContainsKey(name))
            {
                if (callEnd)
                {
                    this.states[name].End();
                }
                
                this.states.Remove(name);
                return ResultCode.StateDeleted;
            }
            
            return ResultCode.StateDoesNotExist;
        }
              
        /// <summary>
        /// Return the given state, or null.
        /// </summary>
        public BaseState GetState(string name)
        {
            if (this.states.ContainsKey(name))
            {
                return states[name];
            }
            return null;
        }
        
        /// <summary>
        /// Returns the active state, or null.
        /// </summary>
        public BaseState GetActiveState()
        {
            return this.activeState;
        }
        
        /// <summary>
        /// Returns the previous state, or null.
        /// </summary>
        public BaseState GetPreviousState()
        {
            return this.previousState;
        }
                
        /// <summary>
        /// Set the active state for updating and test before the transition conditions.
        /// </summary>
        public virtual ResultCode SetActiveState(string name, System.Action onDone = null)
        {
            if (this.isInTransition)
            {
                return ResultCode.StateTransitionActive;
            }
            
            this.transitionReadyCallBack = onDone;
            
            BaseState state = this.GetState(name);
            if (state == null)
            {
                return ResultCode.StateDoesNotExist;
            }
            
            if (this.activeState != null)
            {
                if (this.activeState.IsTransitionAllowed(state))
                {
                    return this.SwitchState(state);
                }
                else
                {
                    return ResultCode.StateTransitionFailed;
                }
            }
            
            // Switch state because no active state exists.
            return SwitchState(this.GetState(name));
        }
        
        /// <summary>
        /// Switch the state and call BaseState.Start(), BaseState.End() and set the previousState.
        /// </summary>
        protected virtual ResultCode SwitchState(BaseState next)
        {
            if (next == null)
            {
                return ResultCode.StateDoesNotExist;
            }
                                
            if (this.activeState != null)
            {
                this.nextState = next;
                this.isInTransition = true;
                this.activeState.End(StateEndCallBack);                  
                                
                return ResultCode.StateTransitionActivated;              
            }
            
            this.activeState = next;
            this.activeState.Start(StateStartCallBack);
            
            return ResultCode.StateTransitionActivated;
        }
        
        /// <summary>
        /// Update the current active state.
        /// </summary>
        public virtual void Update()
        {
            if (this.activeState != null && !this.isInTransition)
            {
                this.activeState.Update();
            }
        }
        
        /// <summary>
        /// Called when state BaseState.End() is ready.
        /// </summary>
        private void StateEndCallBack()
        {
            this.previousState = this.activeState;
            this.activeState = this.nextState;

            if (StateChanged != null)
            {
                StateChanged(this.previousState, this.activeState);
            }
            
            this.activeState.Start(StateStartCallBack);
        }             
        
        /// <summary>
        /// Called when state BaseState.Start() is ready.
        /// </summary>
        private void StateStartCallBack()
        {
            this.isInTransition = false;
            if (this.transitionReadyCallBack != null)
            {
                this.transitionReadyCallBack();
            }
        }      
    }
}
