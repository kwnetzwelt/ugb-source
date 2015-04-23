using System;
using UnityEngine;

namespace UnityGameBase.Core.Animation.Tweener
{
    [Serializable]
    public abstract class BaseTweener<T>
    {
        protected T from;
        protected T to;
        protected float startTime = -1;
        protected float endTime = -1;

        // an optional animation curve to spice up the animation
        public AnimationCurve curve;

        public void TweenTo(T from, T to, float duration)
        {
            this.from = from;
            this.to = to;
            this.startTime = Time.time;
            this.endTime = this.startTime + duration;
        }

        public bool IsTweening
        {
            get
            {
                return this.startTime <= Time.time && Time.time <= this.endTime;
            }
        }

        protected float GetDelta()
        {
            if (IsTweening == false) return 1.0f;

            float t = (Time.time - this.startTime) / (this.endTime - this.startTime);
            t = Mathf.Clamp(t, 0, 1);
            if (curve != null)
                t = curve.Evaluate(t);
            return t;
        }

        public abstract T GetValue();
    }
}
