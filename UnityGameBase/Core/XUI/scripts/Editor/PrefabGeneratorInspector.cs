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

		public override void OnInspectorGUI()
		{
			myTarget = (PrefabGenerator)target; 


			if (myTarget.folder == null || myTarget.folder == "")
			{

				if (GUILayout.Button("Save as", GUILayout.Height(50)))
				{
					SavePrefab();
				}
			}
			else
			{					
				if (GUILayout.Button("Apply", GUILayout.Height(50)))
				{
					UpdatePrefab();
				}
				GUILayout.Label("Path: " + myTarget.folder);
				if (GUILayout.Button("Save as"))
				{
					SavePrefab();
				}
			}
		}
	
		private void UpdatePrefab()
		{
			string path = myTarget.folder;
		
			if (path != "")
			{
				myTarget.CloneAndSave(path, myTarget.gameObject);
			}
		}
	
		private void SavePrefab()
		{
			string path = EditorUtility.SaveFilePanelInProject("Save Screen Prefab", myTarget.gameObject.name, "prefab", "save Screen");
		
			if (path != "")
			{
				myTarget.folder = path;
				myTarget.CloneAndSave(path, myTarget.gameObject);
			}
			else
				Debug.Log("Canceled Saving");
		}
	
	}
}
