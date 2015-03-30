using System;
using System.Collections.Generic;

namespace UnityGameBase.Core.Utils
{
	public class GenericStateMachine
	{
			

		//
		// Provides basic state handling for components
		//


		struct SStateEntry
		{
			public string name;
			public OnStateChange OnEnterState;
			public OnStateChange OnLeaveState;
		}

		List<SStateEntry> mUIStates = new List<SStateEntry>();

		SStateEntry mCurrentUIState = new SStateEntry ();
		public string CurrentState
		{
			get
			{
				return mCurrentUIState.name;
			}
		}
		public delegate void OnStateChange( System.Action pOnDoneCbk = null );
		
		public void AddState(string pStateName, OnStateChange pOnEnterState, OnStateChange pOnLeaveState)
		{
			SStateEntry e = new SStateEntry ();
			e.name = pStateName;
			e.OnEnterState = pOnEnterState;
			e.OnLeaveState = pOnLeaveState;

			mUIStates.Add (e);
		}

		public void GoToState(string pStateName, System.Action pOnDoneCbk = null)
		{
			SStateEntry newState = GetUIState (pStateName);

			if(mCurrentUIState.name != null && mCurrentUIState.OnLeaveState != null)
			{
				mCurrentUIState.OnLeaveState( () => { 
				
					mCurrentUIState = newState;
					newState.OnEnterState( pOnDoneCbk );
				
				});
			}else
			{
				mCurrentUIState = newState;
				newState.OnEnterState( pOnDoneCbk );
			}
		}

		SStateEntry GetUIState(string pName)
		{
			foreach (var e in mUIStates)
				if (e.name == pName)
					return e;

			throw new IndexOutOfRangeException("The given state could not be found. Have you added the state? Was looking for: " + pName);

		}
	}

}