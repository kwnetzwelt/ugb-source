using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UnityGameBase.Core.XUI
{
    public class ScrollRect : UnityEngine.UI.ScrollRect
    {
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
            base.OnScroll(data);
                  
            IsScrolling = true;
            ResetOnMovementFinish();
        }

        public override void OnBeginDrag(PointerEventData eventData)
        {
            base.OnBeginDrag(eventData);

            IsScrolling = true;
            StopAllCoroutines();
        }

        public override void OnEndDrag(PointerEventData eventData)
        {
            base.OnEndDrag(eventData);
            ResetOnMovementFinish();
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
