using UnityEngine;

namespace UnityGameBase.Core.XUI
{
	public class Button : UnityEngine.UI.Button, IWidget
	{
		[Tooltip("Enable static custom button click event (ButtonClicked).")]
		[SerializeField]
		bool enableStaticEvent = true;

		[Tooltip("This id value can be used to differentiate between button types (e.g. for click sounds).\nIt is used for a static custom button click event (ButtonClicked).")]
		[SerializeField]
		int eventId = 0;

		/// <summary>
		/// Occurs when button clicked. This static event can be used to get any button click, e.g. for playing button sounds.
		/// Returns Button and eventId:int
		/// </summary>
		public static event System.Action<Button, int> ButtonClicked;

		protected override void Awake()
		{
			base.Awake();

			onClick.AddListener(OnButtonClicked);
		}
		
		void OnButtonClicked ()
		{
			if (enabled && enableStaticEvent && ButtonClicked != null)
			{
				ButtonClicked(this, eventId);
			}
		}
	}
}
