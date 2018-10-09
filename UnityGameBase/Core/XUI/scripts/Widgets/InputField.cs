using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

namespace UnityGameBase.Core.XUI
{
	public class InputField : UnityEngine.UI.InputField, IWidget
	{
        public static bool IsTouchedByUser { get; private set; }

		/// <summary>
		/// Event. Fires when touch keyboard input for this input field is finished (e.g. by "Done" or "Cancel" button).
		/// String: message
		/// Bool: success (input has been finished via "Done").
		/// </summary>
		public event System.Action<string, bool> TouchKeyboardInputFinishedEvent;

        string lastString;

        #region MONO
        protected override void Start()
        {
            lastString = text;
        }

        void Update()
		{
            var changed = (m_Keyboard != null && m_Keyboard.done) || ((m_Keyboard == null || !m_Keyboard.active) && lastString != text);
            if (TouchKeyboardInputFinishedEvent != null && changed)
			{
				TouchKeyboardInputFinishedEvent.Invoke(m_Keyboard.text, !m_Keyboard.wasCanceled || lastString != text);
			}

            lastString = text;
		}
		#endregion

        public override void OnSelect(BaseEventData eventData)
        {
            base.OnSelect(eventData);

            IsTouchedByUser = true;
        }

        public override void OnSubmit(BaseEventData eventData)
        {
            base.OnSubmit(eventData);

            IsTouchedByUser = false;
        }

        public override void OnDeselect(BaseEventData eventData)
        {
            base.OnDeselect(eventData);

            IsTouchedByUser = false;
        }
	}
}