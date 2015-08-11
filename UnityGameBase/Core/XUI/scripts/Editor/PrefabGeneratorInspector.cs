using UnityEngine;
using System.Collections;
using UnityEditor;
using UnityEngine.Serialization;
using UnityGameBase.Core.XUI;

namespace UnityGameBase.Core.XUI
{
	[CustomEditor(typeof(PrefabGenerator))]
	public class PrefabGeneratorInspector : UnityEditor.Editor
	{
		PrefabGenerator myTarget = null;
		bool includeInactive = true;

		public override void OnInspectorGUI()
		{
			myTarget = (PrefabGenerator)target; 


			includeInactive = GUILayout.Toggle(includeInactive, "Include Inactive");

			if (myTarget.folder == null || myTarget.folder == "")
			{

				if (GUILayout.Button("Save as", GUILayout.Height(50)))
				{
					SavePrefab(includeInactive);
				}
			}
			else
			{					
				if (GUILayout.Button("Apply", GUILayout.Height(50)))
				{
					UpdatePrefab(includeInactive);
				}
				GUILayout.Label("Path: " + myTarget.folder);
				if (GUILayout.Button("Save as"))
				{
					SavePrefab(includeInactive);
				}
			}
		}
	
		private void UpdatePrefab(bool includeInactive = false)
		{
			string path = myTarget.folder;
		
			if (path != "")
			{
				myTarget.CloneAndSave(path, myTarget.gameObject, includeInactive);
			}
		}
	
		private void SavePrefab(bool includeInactive = false)
		{
			string path = EditorUtility.SaveFilePanelInProject("Save Screen Prefab", myTarget.gameObject.name, "prefab", "save Screen");
		
			if (path != "")
			{
				myTarget.folder = path;
				myTarget.CloneAndSave(path, myTarget.gameObject, includeInactive);
			}
			else
				Debug.Log("Canceled Saving");
		}
	
	}
}
