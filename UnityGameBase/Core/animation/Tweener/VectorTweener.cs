using System;
using UnityEngine;

namespace UnityGameBase.Core.Animation.Tweener
{
    [Serializable]
    public class VectorTweener : BaseTweener<Vector3>
    {
        public override Vector3 GetValue()
        {
            return Vector3.Lerp(this.from, this.to, GetDelta());
        }
    }
}
