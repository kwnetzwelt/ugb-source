﻿using UnityEngine;
using System.Collections;
using UnityGameBase.Core.Extensions;

namespace UnityGameBase.Core.XUI
{
	public class AlphaFadeController : TransitionController
	{
		public float FadeTime {	set; get; }
		
		float targetAlpha = 0f;

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

			StopAllCoroutines();
            StartCoroutine(Fade(onDone));
		}
		
		public override void Hide(System.Action onDone)
		{
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
                    var delta = Mathf.Clamp(Time.deltaTime, 0f, .05f);
                    CanvasGroup.alpha = Mathf.MoveTowards(CanvasGroup.alpha, targetAlpha, speed * delta);
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
