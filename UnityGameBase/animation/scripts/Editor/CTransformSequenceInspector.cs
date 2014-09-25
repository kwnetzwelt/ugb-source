using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(CTransformSequence))]
public class CTransformSequenceInspector : Editor {
	
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
			ts.transform.localPosition = ts.mPositionStart;
			ts.transform.localRotation = Quaternion.Euler( ts.mRotationStart );
		}
		tg = GUILayout.Toggle( ts.progress == 1,"End" ,"button");
		
		if(tg)
		{
			ts.progress = 1;
			ts.transform.localPosition = ts.mPositionEnd;
			ts.transform.localRotation = Quaternion.Euler( ts.mRotationEnd );
		}
		
		
		GUILayout.EndHorizontal();
		
		ts.mEasing = EditorGUILayout.CurveField("Easing", ts.mEasing);
		
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
			mTarget.mRotationEnd = mTarget.transform.localRotation.eulerAngles;
			mTarget.mPositionEnd = mTarget.transform.localPosition;
		}else
		{
			mTarget.mRotationStart = mTarget.transform.localRotation.eulerAngles;
			mTarget.mPositionStart = mTarget.transform.localPosition;
		}
		
		EditorUtility.SetDirty(mTarget);
	}
}
