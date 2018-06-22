using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UnityGameBase.Core.XUI
{
    public class ScrollRect : UnityEngine.UI.ScrollRect
    {
        [SerializeField]
        bool enableUserInteraction = true;
        public bool EnableUserInteraction
        {
            get { return enableUserInteraction; }
            set { enableUserInteraction = value; }
        }

        public bool IsScrolling { get; private set; }

        WaitForEndOfFrame waitForEndOfFrame;
        Vector2 oldPosition;

        protected override void Start()
        {
            base.Start();

            waitForEndOfFrame = new WaitForEndOfFrame();
        }

        public override void OnScroll(PointerEventData data)
        {
            if (enableUserInteraction)
            {
                base.OnScroll(data);
                  
                IsScrolling = true;
                ResetOnMovementFinish();
            }
        }

        public override void OnBeginDrag(PointerEventData eventData)
        {
            if (enableUserInteraction)
            {
                base.OnBeginDrag(eventData);

                IsScrolling = true;
                StopAllCoroutines();
            }
        }

        public override void OnDrag(PointerEventData eventData)
        {
            if (enableUserInteraction)
            {
                base.OnDrag(eventData);
            }
        }

        public override void OnEndDrag(PointerEventData eventData)
        {
            if (enableUserInteraction)
            {
                base.OnEndDrag(eventData);
                ResetOnMovementFinish();
            }
        }

        void ResetOnMovementFinish()
        {
            StopAllCoroutines();
            StartCoroutine(ResetCoroutine());
        }

        IEnumerator ResetCoroutine()
        {
            do
            {
                oldPosition = normalizedPosition;
                yield return waitForEndOfFrame;
            }
            while (!Vector2.Equals(oldPosition, normalizedPosition));

            IsScrolling = false;
        }
    }
}
