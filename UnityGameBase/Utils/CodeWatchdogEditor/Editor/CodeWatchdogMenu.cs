using UnityEngine;
using UnityEditor;
using System.Collections;
using CodeWatchdog;
using System.IO;
using System.Text;

// Thanks to http://unity3d.com/learn/tutorials/modules/intermediate/editor/menu-items

/// <summary>
/// Main and context menu to run CodeWatchdog on scripts.
/// </summary>
public class CodeWatchdogMenu : MonoBehaviour
{
    [MenuItem("UGB/Run CodeWatchdog")]
    static void RunWatchdogOnScripts()
    {
        StringBuilder log = new StringBuilder();
        
        CamelCaseCSharpWatchdog cswd = new CamelCaseCSharpWatchdog();
        
        cswd.Init();
        
        cswd.woff += (string message) => {log.AppendLine(message);};
        
        // TODO: Offer configuration for directories and files to include / exclude.
        //
        foreach (string path in Directory.GetFiles(Path.Combine("Assets", "scripts"), "*.cs", SearchOption.AllDirectories))
        {
            log.AppendLine("\nChecking " + path);
            
            cswd.Check(path);
        }
        
        WatchdogEditorWindow w = (WatchdogEditorWindow)EditorWindow.GetWindow(typeof(WatchdogEditorWindow));
        
        w.summary = cswd.Summary();
        
        w.log = log.ToString();
        
        w.title = "CodeWatchdog Results";
        
        w.minSize = new Vector2(500, 500);
        
        w.Show();
        
        return;
    }
}
