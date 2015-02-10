using UnityEngine;
using UnityEditor;
using System.Collections;
using CodeWatchdog;
using System.IO;

// http://unitypatterns.com/customizing-the-editor-part-3-inspectors-editors/

// TODO: Custom icon based on error count would be nice. See http://forum.unity3d.com/threads/custom-scriptableobject-icons-thumbnail.256246/

[CustomEditor(typeof(MonoScript))]
public class CodeWatchdogInspector : Editor
{
    string LastFileViewed = "";
    
    string LastCheckErrors = "";
    
    string LastCheckSummary = "";
    
    string LastFileContent = "";
    
    public override void OnInspectorGUI()
    {
		// We really do *not* want to check the file on every OnInspectorGUI() call.
		//
		if (LastFileViewed != Selection.activeObject.name)
		{
			CamelCaseCSharpWatchdog cswd = new CamelCaseCSharpWatchdog();
			
			cswd.Init();
			
			LastCheckErrors = "";
			
			cswd.woff += (string message) => {LastCheckErrors += message + "\n";};
			
			string path = AssetDatabase.GetAssetPath(Selection.activeObject);
			
			cswd.Check(path);
			
			LastCheckSummary = cswd.Summary();
			
			cswd = null;
			
			LastFileViewed = Selection.activeObject.name;
			
			LastFileContent = File.ReadAllText(AssetDatabase.GetAssetPath(Selection.activeObject));
		}
		
		GUILayout.Label("CodeWatchdog Results", EditorStyles.boldLabel);
		
		GUILayout.Label(LastCheckSummary);
		
		GUILayout.Label("File Content", EditorStyles.boldLabel);
		
		GUILayout.Label(LastFileContent);
		
		if (LastCheckErrors != "")
		{
			GUILayout.Label("CodeWatchdog Errors", EditorStyles.boldLabel);
			
			GUILayout.Label(LastCheckErrors);
		}
		
		
		//		DrawDefaultInspector();
		
		return;
	}
}
