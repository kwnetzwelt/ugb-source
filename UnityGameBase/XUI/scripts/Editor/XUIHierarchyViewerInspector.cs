using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

namespace UGB.XUI
{
/// <summary>
/// Custom inspector shows all layers and widgets
/// </summary>
	[CustomEditor(typeof(XUIHierarchyViewer))]
	public class XUIHierarchyViewerInspector : UnityEditor.Editor
	{
		
		public override void OnInspectorGUI()
		{
			base.OnInspectorGUI();
			
			XUIHierarchyViewer myTarget = (XUIHierarchyViewer)target;
			
			//TODO call not on each tick
			myTarget.UpdateHierarchy();
			
			EditorGUILayout.LabelField("Layer", "Widget", GUILayout.Height(20));
			
			foreach (string layer in myTarget.hierarchy.Keys)
			{					
				foreach (string key in myTarget.hierarchy[layer])
				{
					EditorGUILayout.LabelField(layer, key);
				}
			}			
		}		
	}
}
