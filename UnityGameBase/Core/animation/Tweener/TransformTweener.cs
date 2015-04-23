using System;
using UnityEngine;

namespace UnityGameBase.Core.Animation.Tweener
{
    [Serializable]
    public class TransformTweener
    {
        private VectorTweener positionTweener = new VectorTweener();
        private VectorTweener scaleTweener = new VectorTweener();
        private QuaternionTweener rotationTweener = new QuaternionTweener();
        private bool localTransform = false;

        public AnimationCurve TweenAnimationCurve = null;

        public void TweenTo(Vector3 position, Vector3 scale, Quaternion rotation, float duration, Transform currentTransform, bool localTransform)
        {
            this.localTransform = localTransform;
            positionTweener.TweenTo(localTransform ? currentTransform.localPosition : currentTransform.position, position, duration);
            scaleTweener.TweenTo(currentTransform.localScale, scale, duration);
            rotationTweener.TweenTo(localTransform ? currentTransform.localRotation : currentTransform.rotation, rotation, duration);

            positionTweener.curve = TweenAnimationCurve;
            scaleTweener.curve = TweenAnimationCurve;
            rotationTweener.curve = TweenAnimationCurve;
        }

        public bool IsTweening
        {
            get
            {
                return positionTweener.IsTweening || rotationTweener.IsTweening || scaleTweener.IsTweening;
            }
        }

        public void SetValues(Transform transform)
        {
            if (localTransform)
            {
                transform.localPosition = positionTweener.GetValue();
                transform.localRotation = rotationTweener.GetValue();
            }
            else
            {
                transform.position = positionTweener.GetValue();
                transform.rotation = rotationTweener.GetValue();
            }
            transform.localScale = scaleTweener.GetValue();
        }
    }
}
