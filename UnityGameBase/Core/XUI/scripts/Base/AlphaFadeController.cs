using UnityEngine;
using System.Collections;
using UnityGameBase.Core.Extensions;

namespace UnityGameBase.Core.XUI
{
	public class AlphaFadeController : TransitionController
	{
		public float FadeTime {	set; get; }
		
		float targetAlpha = 0f;
		System.Action doneCallBack;

		CanvasGroup cg = null;
		CanvasGroup CanvasGroup 
		{
			get 
			{
				if (cg == null)
				{
					cg = this.AddComponentIfNotExists<CanvasGroup>();
				}
				return cg;
			}
		}

		public override void Show(System.Action onDone)
		{
			CanvasGroup.alpha = 0f;
			targetAlpha = 1f;
			doneCallBack = onDone;

			StopAllCoroutines();
			StartCoroutine(Fade());
		}
		
		public override void Hide(System.Action onDone)
		{
			targetAlpha = 0f;
			doneCallBack = onDone;
		
			StopAllCoroutines();
			StartCoroutine(Fade());
		}

		IEnumerator Fade ()
		{
			yield return new WaitForEndOfFrame();

			if (FadeTime > 0f)
			{
				var speed = 1f / FadeTime;
				while (!Mathf.Approximately(CanvasGroup.alpha, targetAlpha))
				{
					CanvasGroup.alpha = Mathf.MoveTowards(CanvasGroup.alpha, targetAlpha, speed * Time.deltaTime);
					yield return new WaitForEndOfFrame();
				}
			}

			CanvasGroup.alpha = targetAlpha;
			if (doneCallBack != null)
			{
				doneCallBack();
				doneCallBack = null;
			}
		}
		
		public void SetAlpha(float val)
		{
			CanvasGroup.alpha = val;
		}
	}
}
