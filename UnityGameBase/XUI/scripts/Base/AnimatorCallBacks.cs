using UnityEngine;
using System.Collections;

public class AnimatorCallBacks : StateMachineBehaviour
{
	public delegate void AnimatorCallBack(Animator animator,AnimatorStateInfo stateInfo,int layerIndex) ;
	
	public event AnimatorCallBack stateEnter;
	public event AnimatorCallBack stateExit;
	

	public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		base.OnStateEnter(animator, stateInfo, layerIndex);		
		
		if (stateEnter != null)
			stateEnter(animator, stateInfo, layerIndex);
	}
	
	public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		base.OnStateExit(animator, stateInfo, layerIndex);
		
		if (stateExit != null)
			stateExit(animator, stateInfo, layerIndex);
	}
}
