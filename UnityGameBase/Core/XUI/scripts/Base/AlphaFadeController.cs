using UnityEngine;
using System.Collections;

namespace UnityGameBase.Core.XUI
{
	public class AlphaFadeController : TransitionController
	{
		private float fadeTime;
		public float FadeTime
		{
			set{ fadeTime = value;}
			get{ return fadeTime;}
		}
		
		private float currentTime = 0;
		private System.Action doneCallBack;

		protected override void Awake()
		{
			base.Awake();

			CanvasGroup canvasGroup = this.gameObject.GetComponent<CanvasGroup>();
			if (canvasGroup == null)
			{
				Debug.LogWarning("Screen " + gameObject.name + " has no CanvasGroup attached. Will be attached now, but you should add the CanvasGroup to your prefab.");
				gameObject.AddComponent<CanvasGroup>();
			}
		}

		public override void Show(System.Action onDone)
		{
			currentTime = 0;
			doneCallBack = onDone;
			InvokeRepeating("FadeIn", 0f, 0.05f);
		}
		
		public override void Hide(System.Action onDone)
		{
			currentTime = 0;
			doneCallBack = onDone;
			InvokeRepeating("FadeOut", 0f, 0.05f);
		}
		
		private void FadeIn()
		{
			currentTime += 0.05f;
			float curAlpha = currentTime / fadeTime;
			
			if (curAlpha >= 1)
			{
				curAlpha = 1;
				this.SetAlpha(1);
				this.CancelInvoke("FadeIn");
				doneCallBack();
			}				
			this.SetAlpha(curAlpha);		
		}
			
		private void FadeOut()
		{
			currentTime += 0.05f;
			float curAlpha = 1 - (currentTime / fadeTime);
			
			if (curAlpha <= 0)
			{
				curAlpha = 0;
				this.SetAlpha(0);
				this.CancelInvoke("FadeOut");
				doneCallBack();
			}				
			this.SetAlpha(curAlpha);		
		}
		
		public void SetAlpha(float val)
		{
			CanvasGroup canvasGroup = this.gameObject.GetComponent<CanvasGroup>();
			canvasGroup.alpha = val;
		}
	}
}
