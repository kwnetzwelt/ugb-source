using System;
using System.IO;
using UnityEditor;
using UnityEngine;
using System.Collections;
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
        
//        Debug.Log(string.Format("OnPostprocessAllAssets({0}, {1}, {2}, {3})",
//                                importedAssets.Length,
//                                deletedAssets.Length,
//                                movedAssets.Length,
//                                movedFromAssetsPaths.Length));
    
    
        // http://stackoverflow.com/a/59250/1132250
        //
        string[] changedFiles = new string[importedAssets.Length
                                           + movedAssets.Length
                                           + movedFromAssetsPaths.Length];
                                           
        Array.Copy(importedAssets,
                   changedFiles,
                   importedAssets.Length);
        
        Array.Copy(movedAssets,
                   0,
                   changedFiles,
                   importedAssets.Length,
                   movedAssets.Length); 
        
        // REMOVE
        //
        string changed = "1234";
        
        StringBuilder log = new StringBuilder();
        
        CamelCaseCSharpWatchdog cswd = new CamelCaseCSharpWatchdog();
        
        cswd.Init();
        
        cswd.woff += (string message) => {log.AppendLine(message);};
        
        for (int i = 0; i < changedFiles.Length; i++)
        {
            string filename = changedFiles[i];
            
            if (filename.EndsWith(".cs"))
            {
                log.AppendLine("\nChecking " + filename);
                
                cswd.Check(filename);
            }
        }
        
        WatchdogEditorWindow w = (WatchdogEditorWindow)EditorWindow.GetWindow(typeof(WatchdogEditorWindow));
        
        w.summary = cswd.Summary();
        
        w.log = log.ToString();
        
        w.title = "CodeWatchdog Results";
        
        w.minSize = new Vector2(500, 500);
        
        w.Show();
        
        cswd = null;
        
        return;
    }
}
