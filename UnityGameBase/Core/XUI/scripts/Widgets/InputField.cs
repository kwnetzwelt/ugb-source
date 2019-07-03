using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

namespace UnityGameBase.Core.XUI
{
	public class InputField : UnityEngine.UI.InputField, IWidget
	{
        static InputField isTouchedByUser = null;
        public static bool IsTouchedByUser { get { return isTouchedByUser != null; } }

		/// <summary>
		/// Event. Fires when touch keyboard input for this input field is finished (e.g. by "Done" or "Cancel" button).
		/// String: message
		/// Bool: success (input has been finished via "Done").
		/// </summary>
		public event System.Action<string, bool> TouchKeyboardInputFinishedEvent;

        #region MONO

        protected override void OnDisable()
        {
            base.OnDisable();

            if (isTouchedByUser == this)
                isTouchedByUser = null;
        }

        void Update()
		{
			if(TouchKeyboardInputFinishedEvent != null && m_Keyboard != null && m_Keyboard.status == TouchScreenKeyboard.Status.Done)
				TouchKeyboardInputFinishedEvent.Invoke(m_Keyboard.text, m_Keyboard.status != TouchScreenKeyboard.Status.Canceled);
		}

		#endregion

        public override void OnSelect(BaseEventData eventData)
        {
            base.OnSelect(eventData);

            isTouchedByUser = this;
        }

        public override void OnSubmit(BaseEventData eventData)
        {
            base.OnSubmit(eventData);

            if (isTouchedByUser == this)
                isTouchedByUser = null;
        }

        public override void OnDeselect(BaseEventData eventData)
        {
            base.OnDeselect(eventData);

            if (isTouchedByUser == this)
                isTouchedByUser = null;
        }
    }
}