using UnityEngine;
using System.Collections;

[ExecuteInEditMode()]
public class CTransformSequence : MonoBehaviour
{
	public Vector3 mRotationStart;
	public Vector3 mRotationEnd;
	public Vector3 mPositionStart;
	public Vector3 mPositionEnd;
	
	public AnimationCurve mEasing = new AnimationCurve( new Keyframe(0,0), new Keyframe(1,1));
	
	public float progress;
	public float speed;
	
	float mReverse;
	public bool isReverse
	{
		get
		{
			return mReverse != 1;
		}
	}
	
	[SerializeField]
	public bool isPlaying
	{
		get;
		private set;
	}
	
	
	public void Play()
	{
		Play(false);
	}
	
	
	public void Play(bool pReverse)
	{
		mReverse = (pReverse)? -1 : 1;
		isPlaying = true;	
	}
	
	
	public void Pause()
	{
		isPlaying = false;
	}
	
	
	void Update ()
	{
		if(isPlaying)
		{
			if(progress > 1 || progress < 0)
			{
				// implement loop mode if wanted
				isPlaying = false;
				progress = Mathf.Clamp01( progress );
			}else
			{
				progress += Time.deltaTime * mReverse * speed;
			}
			
			this.transform.localPosition = Vector3.Lerp(mPositionStart,mPositionEnd, mEasing.Evaluate( progress ));
			this.transform.localRotation = Quaternion.Lerp(
				Quaternion.Euler( mRotationStart )
				,Quaternion.Euler( mRotationEnd )
				, progress);
			
		}
	}
}

