using UnityEngine;
using UnityEditor;
using System.Collections;

/// <summary>
/// A window to display the CodeWatchdog results in the Unity editor.
/// </summary>
[System.Serializable]
public class WatchdogEditorWindow : EditorWindow
{
    // Thanks to http://blogs.unity3d.com/2012/10/25/unity-serialization/

    public static WatchdogEditorWindow instance;

    [SerializeField]
    string summary;
    
    public string Summary
    {
        get
        {
            return summary;
        }
        
        set
        {
            summary = value;
            
            Repaint();
        }
    }
    
    [SerializeField]
    string log;
    
    public string Log
    {
        get
        {
            return log;
        }
        
        set
        {
            log = value;
            
            Repaint();
        }
    }
    
    public static bool debug = false;
    
    Vector2 scrollPosition;
    
    /// <summary>
    /// Show the CodeWatchdog report window, and create it if it is not already there.
    /// </summary>
    public static void OpenWindow()
    {
        if (debug)
        {
            Debug.Log("WatchdogEditorWindow.OpenWindow()");
        }
        
        instance = (WatchdogEditorWindow)EditorWindow.GetWindow(typeof(WatchdogEditorWindow));
        
        instance.title = "CodeWatchdog";
        
        instance.Show();
        
        return;
    }
    
    void OnGUI()
    {
        scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);
        
        GUILayout.Label("CodeWatchdog Summary", EditorStyles.boldLabel);
        
        GUILayout.Label(Summary, GUILayout.Width(800));
        
        // TODO: Make clickable, opening MonoDevelop at the specific line.
        
        GUILayout.Label("CodeWatchdog Log", EditorStyles.boldLabel);
        
        GUILayout.Label(Log, GUILayout.Width(800));
        
        EditorGUILayout.EndScrollView();
        
        return;
    }
    
    void OnEnable()
    {
        if (debug)
        {
            Debug.Log("WatchdogEditorWindow.OnEnable()");
        }
        
        if (instance == null)
        {
            if (debug)
            {
                Debug.Log("Setting instance to " + GetInstanceID());
            }
            
            instance = this;
        }
        else
        {
            if (debug)
            {
                Debug.Log("instance already exists");
            }
        }
        
        return;
    }
    
    void OnDestroy()
    {
        if (debug)
        {
            Debug.Log("WatchdogEditorWindow.OnDestroy()");
            
            Debug.Log("Deleting instance");
        }
        
        instance = null;
    }
}
