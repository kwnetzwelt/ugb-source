using UnityEngine;
using System.Collections;

namespace UnityGameBase.Core.XUI
{

	[System.Serializable]
	public class ScreenTransition : MonoBehaviour
	{
		public TransitionController AddController(GameObject obj)
		{
			switch (this.transitionType)
			{
				case ScreenTransition.TransitionType.AlphaFading:
					{
						AlphaFadeController controller = obj.AddComponent<AlphaFadeController>();
						controller.FadeTime = alphaTransitionTime;
						return (TransitionController)controller;					
					}
				case ScreenTransition.TransitionType.Animator:
					{
						AnimatedTransitionController controller = obj.AddComponent<AnimatedTransitionController>();
						return (TransitionController)controller;		
					}
				case ScreenTransition.TransitionType.ColorPlane:
					{}
					break;
			}
			
			return null;
		}
	
	
		public enum TransitionType
		{
			AlphaFading,
			Animator,
			ColorPlane,
		}
		
		[SerializeField]
		public TransitionType
			transitionType = TransitionType.AlphaFading;

		[SerializeField]
		public float
			alphaTransitionTime = 1;
		
	}
}