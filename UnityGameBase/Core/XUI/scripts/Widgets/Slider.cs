using UnityEngine.EventSystems;

namespace UnityGameBase.Core.XUI
{
	public class Slider : UnityEngine.UI.Slider, IWidget
	{
        public static bool IsTouchedByUser { get; private set; }

        public override void OnSelect(BaseEventData eventData)
        {
            base.OnSelect(eventData);

            IsTouchedByUser = true;
        }

        public override void OnDeselect(BaseEventData eventData)
        {
            base.OnDeselect(eventData);

            IsTouchedByUser = false;
        }
    }
}