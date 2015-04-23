using System;
using UnityEngine;

namespace UnityGameBase.Core.Animation.Tweener
{
    [Serializable]
    public class FloatTweener : BaseTweener<float>
    {
        public override float GetValue()
        {
            return Mathf.Lerp(this.from, this.to, GetDelta());
        }
    }
}
