using UnityEngine;
using System.Collections;
using UnityGameBase.Core.Extensions;

namespace UnityGameBase.Core.XUI
{
	public class AlphaFadeController : TransitionController
	{
		public float FadeTime {	set; get; }
		
		float targetAlpha = 0f;
        bool isRunning = false;

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
            if(isRunning)
            {
                Debug.LogError("AlphaFadeController::Show: AlphaFade is still Running!! ");
            }
            isRunning = true;
			CanvasGroup.alpha = 0f;
			targetAlpha = 1f;

			StopAllCoroutines();
            StartCoroutine(Fade(onDone));
		}
		
		public override void Hide(System.Action onDone)
		{
            if(isRunning)
            {
                Debug.LogError("AlphaFadeController::Hide: AlphaFade is still Running!! ");
            }
            isRunning = true;
			targetAlpha = 0f;
		
			StopAllCoroutines();
			StartCoroutine(Fade(onDone));
		}

        IEnumerator Fade (System.Action onDone)
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
            if (onDone != null)
			{
                onDone();
			}
		}
		
		public void SetAlpha(float val)
		{
			CanvasGroup.alpha = val;
		}
	}
}
