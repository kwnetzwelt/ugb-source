using System;
using System.IO;
using UnityEditor;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using CodeWatchdog;
using System.Text;

/// <summary>
/// Hook into asset postprocessing, running CodeWatchdog on .cs files.
/// </summary>
public class CodeWatchdogUpdateCheck : AssetPostprocessor
{
    static void OnPostprocessAllAssets(string[] importedAssets,
                                       string[] deletedAssets,
                                       string[] movedAssets,
                                       string[] movedFromAssetsPaths)
    {
        // TODO: Offer configuration for directories and files to include / exclude.
        // TODO: Offer configuration to log an error / warning to the Unit log when files pass below a configurable threshold.
        
//        Debug.Log(string.Format("OnPostprocessAllAssets({0}, {1}, {2}, {3})",
//                                importedAssets.Length,
//                                deletedAssets.Length,
//                                movedAssets.Length,
//                                movedFromAssetsPaths.Length));
    
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
        
        List<string> changedFiles = new List<string>();
        
        changedFiles.AddRange(importedAssets);
        changedFiles.AddRange(movedAssets);
        
        StringBuilder log = new StringBuilder();
        
        CamelCaseCSharpWatchdog cswd = new CamelCaseCSharpWatchdog();
        
        cswd.Init();
        
        cswd.woff += (string message) => {
                log.AppendLine(message);
            };
        
        foreach (string filename in changedFiles)
        {
            if (filename.EndsWith(".cs"))
            {
                if (WatchdogEditorWindow.debug)
                {
                    Debug.Log(string.Format("Checking '{0}' ({1})",
                                            Path.GetFileName(filename),
                                            filename));
                }
                
                log.AppendLine(string.Format("\nChecking '{0}' ({1})",
                                             Path.GetFileName(filename),
                                             filename));
                
                cswd.Check(filename);
            }
        }
        
        WatchdogEditorWindow.instance.Summary = cswd.Summary();
        
        WatchdogEditorWindow.instance.Log = log.ToString();
        
        cswd = null;
        
        return;
    }
}
