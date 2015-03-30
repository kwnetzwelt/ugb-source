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
			//color all texts		
			LocalizedTextComponent[] components = this.gameObject.GetComponentsInChildren<LocalizedTextComponent>();
			foreach (LocalizedTextComponent cmp in components)
			{
				Color temp = cmp.color;
				temp.a = val;
				cmp.color = temp;
			}
			
			//todo: maybe XUI image class if needed
			//color all images			
			UnityEngine.UI.Image[] images = this.gameObject.GetComponentsInChildren<UnityEngine.UI.Image>();
			
			foreach (UnityEngine.UI.Image cmp in images)
			{			
				Color temp = cmp.color;
				temp.a = val;
				cmp.color = temp;
			}
		}
	}
}
