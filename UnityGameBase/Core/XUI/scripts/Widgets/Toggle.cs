using UnityEngine;
using System.Collections;

namespace UnityGameBase.Core.XUI
{
	public class Toggle : UnityEngine.UI.Toggle, IWidget
	{
		[Tooltip("Enable static custom button click event (ButtonClicked).")]
		[SerializeField]
		bool enableStaticEvent = true;

		[Tooltip("This id value can be used to differentiate between button types (e.g. for click sounds).\nIt is used for a static custom button click event (ButtonClicked).")]
		[SerializeField]
		int eventId = 0;

		/// <summary>
		/// Occurs when button clicked. This static event can be used to get any button click, e.g. for playing button sounds.
		/// Returns Toggle instance, eventId:int and toggleOn/off
		/// </summary>
		public static event System.Action<Toggle, int, bool> ToggleClicked;

		protected override void Awake()
		{
			base.Awake();

			onValueChanged.AddListener(OnToggleClicked);
		}
		
		void OnToggleClicked (bool toggled)
		{
			if (enableStaticEvent && ToggleClicked != null)
			{
				ToggleClicked(this, eventId, toggled);
			}
		}
	}
}