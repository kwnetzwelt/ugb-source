using UnityEngine;
using System.Collections;
using UnityEditor;

namespace UGB.Animation
{
	[CustomEditor(typeof(CTransformSequence))]
	public class CTransformSequenceInspector : UnityEditor.Editor {
		
		bool mAdvancedEditing;
		CTransformSequence mTarget;
		public override void OnInspectorGUI()
		{
			CTransformSequence ts = target as CTransformSequence;
			
			if(ts == null)
				return;
			
			GUILayout.Label("Toggle Start / End Position to Edit");
			GUILayout.BeginHorizontal();
			bool tg = GUILayout.Toggle( ts.progress == 0, "Start","button");
			if(tg)
			{
				ts.progress = 0;
				ts.transform.localPosition = ts.positionStart;
				ts.transform.localRotation = Quaternion.Euler( ts.rotationStart );
			}
			tg = GUILayout.Toggle( ts.progress == 1,"End" ,"button");
			
			if(tg)
			{
				ts.progress = 1;
				ts.transform.localPosition = ts.positionEnd;
				ts.transform.localRotation = Quaternion.Euler( ts.rotationEnd );
			}
			
			
			GUILayout.EndHorizontal();
			
			ts.easing = EditorGUILayout.CurveField("Easing", ts.easing);
			
			ts.speed = EditorGUILayout.Slider("Speed", ts.speed,0.01f,50.0f);
			
			mAdvancedEditing = EditorGUILayout.BeginToggleGroup("Advanced",mAdvancedEditing);
			
			DrawDefaultInspector();
			
			EditorGUILayout.EndToggleGroup();
			
		}
		
		void OnSceneGUI()
		{
			if(Application.isPlaying)
				return;
			
			mTarget = target as CTransformSequence;
			
			if(mTarget.progress == 1)
			{
				mTarget.rotationEnd = mTarget.transform.localRotation.eulerAngles;
				mTarget.positionEnd = mTarget.transform.localPosition;
			}else
			{
				mTarget.rotationStart = mTarget.transform.localRotation.eulerAngles;
				mTarget.positionStart = mTarget.transform.localPosition;
			}
			
			EditorUtility.SetDirty(mTarget);
		}
	}
}