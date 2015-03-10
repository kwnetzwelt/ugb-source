using UnityEngine;
using System.Collections;
using UnityEditor;

namespace UnityGameBase.Core.Animation
{
    [CustomEditor(typeof(TransformSequence))]
    public class TransformSequenceInspector : UnityEditor.Editor
    {
        bool advancedEditing;
        TransformSequence currentTarget;
        public override void OnInspectorGUI()
        {
            TransformSequence ts = target as TransformSequence;
			
            if (ts == null)
            {
                return;
            }
			
            GUILayout.Label("Toggle Start / End Position to Edit");
            GUILayout.BeginHorizontal();
            bool tg = GUILayout.Toggle(ts.progress == 0, "Start", "button");
            if (tg)
            {
                ts.progress = 0;
                ts.transform.localPosition = ts.positionStart;
                ts.transform.localRotation = Quaternion.Euler(ts.rotationStart);
            }
            tg = GUILayout.Toggle(ts.progress == 1, "End", "button");
			
            if (tg)
            {
                ts.progress = 1;
                ts.transform.localPosition = ts.positionEnd;
                ts.transform.localRotation = Quaternion.Euler(ts.rotationEnd);
            }
			
			
            GUILayout.EndHorizontal();
			
            ts.easing = EditorGUILayout.CurveField("Easing", ts.easing);
			
            ts.speed = EditorGUILayout.Slider("Speed", ts.speed, 0.01f, 50.0f);
			
            advancedEditing = EditorGUILayout.BeginToggleGroup("Advanced", advancedEditing);
			
            DrawDefaultInspector();
			
            EditorGUILayout.EndToggleGroup();
        }
		
        void OnSceneGUI()
        {
            if (Application.isPlaying)
            {
                return;
            }
			
            currentTarget = target as TransformSequence;
			
            if (currentTarget.progress == 1)
            {
                currentTarget.rotationEnd = currentTarget.transform.localRotation.eulerAngles;
                currentTarget.positionEnd = currentTarget.transform.localPosition;
            }
            else
            {
                currentTarget.rotationStart = currentTarget.transform.localRotation.eulerAngles;
                currentTarget.positionStart = currentTarget.transform.localPosition;
            }
			
            EditorUtility.SetDirty(currentTarget);
        }
    }
}