using System;
using UnityEngine;

namespace UnityGameBase.Core.Animation.Tweener
{
    [Serializable]
    public class QuaternionTweener : BaseTweener<Quaternion>
    {
        public override Quaternion GetValue()
        {
            return Quaternion.Lerp(this.from, this.to, GetDelta());
        }
    }
}
