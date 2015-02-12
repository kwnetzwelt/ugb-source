using UnityEngine;
using UnityEditor;
using System.Collections;

/// <summary>
/// A window to display the CodeWatchdog results in the Unity editor.
/// </summary>
public class WatchdogEditorWindow : EditorWindow
{
    public string summary;
    
    public string log;
    
    public Vector2 scrollPosition;
    
    void OnGUI()
    {
        scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);
        
        GUILayout.Label("CodeWatchdog Summary", EditorStyles.boldLabel);
        
        GUILayout.Label(summary, GUILayout.Width(800));
        
        // TODO: Make clickable, opening MonoDevelop at the specific line.
        
        GUILayout.Label("CodeWatchdog Log", EditorStyles.boldLabel);
        
        GUILayout.Label(log, GUILayout.Width(800));
        
        EditorGUILayout.EndScrollView();
        
        return;
    }
}
