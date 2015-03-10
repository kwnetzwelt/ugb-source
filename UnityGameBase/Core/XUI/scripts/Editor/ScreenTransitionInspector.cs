using UnityEngine;
using System.Collections;
using UnityEditor;
using UnityEngine.Serialization;


namespace UnityGameBase.Core.XUI
{
	[CustomEditor(typeof(ScreenTransition))]
	public class ScreenTransitionInspector : UnityEditor.Editor
	{
	
		public override void OnInspectorGUI()
		{
			ScreenTransition myTarget = (ScreenTransition)target;
			
			if (myTarget == null)
				return;
		
			//first selection of transitionType
			myTarget.transitionType = (ScreenTransition.TransitionType)EditorGUILayout.EnumPopup(myTarget.transitionType);
			
			
			switch (myTarget.transitionType)
			{
				case ScreenTransition.TransitionType.AlphaFading:
					{
						myTarget.alphaTransitionTime = EditorGUILayout.Slider(myTarget.alphaTransitionTime, 0f, 3f);
					}
					;
					break;
				case ScreenTransition.TransitionType.Animator:
					{}
					;
					break;
				case ScreenTransition.TransitionType.ColorPlane:
					{}
					;
					break;
			}
		}
	}
}