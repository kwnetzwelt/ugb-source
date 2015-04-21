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

        public void TweenTo(Vector3 position, Vector3 scale, Quaternion rotation, float duration, Transform currentTransform)
        {
            positionTweener.TweenTo(currentTransform.position, position, duration);
            scaleTweener.TweenTo(currentTransform.localScale, scale, duration);
            rotationTweener.TweenTo(currentTransform.rotation, rotation, duration);
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
            transform.position = positionTweener.GetValue();
            transform.localScale = scaleTweener.GetValue();
            transform.rotation = rotationTweener.GetValue();
        }
    }
}
