using UnityEngine;
using UnityEditor;
using System.Collections;
using CodeWatchdog;
using System.IO;

// Thanks to http://unity3d.com/learn/tutorials/modules/intermediate/editor/menu-items

/// <summary>
/// Main and context menu to run CodeWatchdog on scripts.
/// </summary>
public class CodeWatchdogMenu : MonoBehaviour
{
    [MenuItem("XOZ/Run CodeWatchdog")]
    static void RunWatchdogOnScripts()
    {
        CamelCaseCSharpWatchdog cswd = new CamelCaseCSharpWatchdog();
        
        cswd.Init();
        
        cswd.woff += Debug.LogError;
        
        // TODO: Offer configuration for directories and files to include / exclude.
        //
        foreach (string path in Directory.GetFiles(Path.Combine("Assets", "scripts"), "*.cs", SearchOption.AllDirectories))
        {
            Debug.Log("Checking " + path);
            
            cswd.Check(path);
        }
        
        WatchdogEditorWindow w = ScriptableObject.CreateInstance<WatchdogEditorWindow>();
        
        w.displayText = cswd.Summary();
        
        w.title = "CodeWatchdog Results";
        
        w.minSize = new Vector2(500, 500);
        
        w.Show();
        
        return;
    }
}

/// <summary>
/// A window to display the CodeWatchdog results in the Unity editor.
/// </summary>
public class WatchdogEditorWindow : EditorWindow
{
    public string displayText;
    
    void OnGUI()
    {
        GUILayout.Label("CodeWatchdog Results", EditorStyles.boldLabel);
        
        GUILayout.Label(displayText, GUILayout.Width(500));
        
        return;
    }
}
