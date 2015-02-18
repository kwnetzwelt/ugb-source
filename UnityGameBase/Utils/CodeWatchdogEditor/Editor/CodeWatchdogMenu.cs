using UnityEngine;
using UnityEditor;
using System.Collections;
using CodeWatchdog;
using System.IO;
using System.Text;

// Thanks to http://unity3d.com/learn/tutorials/modules/intermediate/editor/menu-items

/// <summary>
/// Menu to open a CodeWatchdog report window and run CodeWatchdog on scripts.
/// </summary>
public class CodeWatchdogMenu : MonoBehaviour
{
    [MenuItem("UGB/CodeWatchdog/Open Window")]
    static void ShowWatchdogWindow()
    {
        if (WatchdogEditorWindow.instance == null)
        {
            WatchdogEditorWindow.OpenWindow();
        }
        
        return;
    }
    
    [MenuItem("UGB/CodeWatchdog/Run")]
    static void RunWatchdogOnScripts()
    {
        // If there is no window open, there is nothing
        // to display, and no reason to do a costly parse.
        //
        if (WatchdogEditorWindow.instance == null)
        {
            if (WatchdogEditorWindow.debug)
            {
                Debug.Log(string.Format("WatchdogEditorWindow.instance == '{0}'",
                                        WatchdogEditorWindow.instance));
            }
            
            return;
        }
        
        StringBuilder log = new StringBuilder();
        
        CamelCaseCSharpWatchdog cswd = new CamelCaseCSharpWatchdog();
        
        cswd.Init();
        
        cswd.woff += (string message) => {
                log.AppendLine(message);
            };
        
        // TODO: Offer configuration for directories and files to include / exclude.
        //
        foreach (string path in Directory.GetFiles(Path.Combine("Assets", "scripts"), "*.cs", SearchOption.AllDirectories))
        {
            if (WatchdogEditorWindow.debug)
            {
                Debug.Log(string.Format("Checking '{0}' ({1})",
                                        Path.GetFileName(path),
                                        path));
            }
            
            log.AppendLine(string.Format("\nChecking '{0}' ({1})",
                                         Path.GetFileName(path),
                                         path));
            
            cswd.Check(path);
        }
        
        WatchdogEditorWindow.instance.Summary = cswd.Summary();
        
        WatchdogEditorWindow.instance.Log = log.ToString();
        
        return;
    }
}
