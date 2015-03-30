using UnityEngine;
using System.Collections;

namespace UnityGameBase.Core.XUI
{

	public class AnimatedTransitionController : TransitionController
	{
		private Animator animator;
		private System.Action doneCallBack;
		
		public override void Init(GameObject rootObj)
		{
			base.Init(rootObj);
			
			this.animator = rootObj.GetComponent<Animator>();
			AnimatorCallBacks[] callBacks = this.animator.GetBehaviours<AnimatorCallBacks>();
			
			foreach (AnimatorCallBacks callback in callBacks)
			{
				callback.stateExit += OnDoneHide;
				callback.stateExit += OnDoneShow;
			}
		}
		
		public override void Show(System.Action onDone)
		{
			
			this.animator.SetTrigger("show");
			this.doneCallBack = onDone;
		}
				
		public override void Hide(System.Action onDone)
		{
			this.animator.SetTrigger("hide");
			this.doneCallBack = onDone;
		}
		
		private void OnDoneShow(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
			if (stateInfo.IsName("Show") && this.doneCallBack != null)
				this.doneCallBack();
		}
		
		private void OnDoneHide(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
			if (stateInfo.IsName("Hide") && this.doneCallBack != null)
				this.doneCallBack();
		}
	}
}
