using UnityEngine;
using System.Collections;

namespace UnityGameBase.Core.Animation
{
    [ExecuteInEditMode()]
    public class TransformSequence : MonoBehaviour
    {
        public Vector3 rotationStart;
        public Vector3 rotationEnd;
        public Vector3 positionStart;
        public Vector3 positionEnd;
		
        public AnimationCurve easing = new AnimationCurve(new Keyframe(0, 0), new Keyframe(1, 1));
		
        public float progress;
        public float speed;
		
        float reverse;
        public bool IsReverse
        {
            get
            {
                return reverse != 1;
            }
        }
		
        [SerializeField]
        public bool IsPlaying
        {
            get;
            private set;
        }
		
        public void Play()
        {
            Play(false);
        }
		
        public void Play(bool reverse)
        {
            this.reverse = (reverse) ? -1 : 1;
            IsPlaying = true;	
        }
		
        public void Pause()
        {
            IsPlaying = false;
        }
		
        void Update()
        {
            if (IsPlaying)
            {
                if (progress > 1 || progress < 0)
                {
                    // implement loop mode if wanted
                    IsPlaying = false;
                    progress = Mathf.Clamp01(progress);
                }
                else
                {
                    progress += Time.deltaTime * reverse * speed;
                }
				
                this.transform.localPosition = Vector3.Lerp(positionStart, positionEnd, easing.Evaluate(progress));
                this.transform.localRotation = Quaternion.Lerp(
					Quaternion.Euler(rotationStart)
					, Quaternion.Euler(rotationEnd)
					, progress);
				
            }
        }
    }
}