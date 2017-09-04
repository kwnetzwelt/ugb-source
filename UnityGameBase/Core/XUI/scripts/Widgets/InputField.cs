using UnityEngine;
using System.Collections;

namespace UnityGameBase.Core.XUI
{
	public class InputField : UnityEngine.UI.InputField, IWidget
	{
		/// <summary>
		/// Event. Fires when touch keyboard input for this input field is finished (e.g. by "Done" or "Cancel" button).
		/// String: message
		/// Bool: success (input has been finished via "Done").
		/// </summary>
		public event System.Action<string, bool> TouchKeyboardInputFinishedEvent;

		#region MONO
		void Update()
		{
			if(TouchKeyboardInputFinishedEvent != null && m_Keyboard != null && m_Keyboard.done)
			{
				TouchKeyboardInputFinishedEvent.Invoke(m_Keyboard.text, !m_Keyboard.wasCanceled);
			}
		}
		#endregion
	}
}