using UnityEngine;
using UnityEditor;
using System.Collections;
using System.IO;
using System;

public class WebGLAssetPostProcessor : AssetPostprocessor
{
    static string relativePath = "Editor/WebGLAssetPostProcessor.cs"; 
    static string[] triggers = new string[3]
    {
        "Editor/WebGLAssetPostProcessor.cs",
        "WebGL/CultureDetection_jslib.txt",
        "WebGL/WebGLPlatformHelper.cs"
    };

    static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
    {
        foreach(string asset in importedAssets)
        {
            CheckCultureDetection(asset);
        }        
    }
    
    private static void CheckCultureDetection(string asset)
    {
        bool startToCopy = false;
        
        foreach(string trigger in triggers)
        {
            if(asset.Contains(trigger))
            {
                startToCopy = true;
                break;
            }
        }
        
        if(startToCopy)
        {
            string targetPath = Application.dataPath + "/Plugins/WebGL/UgbCultureDetection.jslib";
            string sourcePath = asset.Replace(relativePath, "CultureDetection_jslib.txt");
            
            if(!File.Exists(targetPath))
            {                
                AssetDatabase.Refresh();
            
                TextAsset cultureJsLib = (TextAsset)AssetDatabase.LoadAssetAtPath(sourcePath, typeof(TextAsset)) as TextAsset; 
                
                if(cultureJsLib != null)
                {
                    string directory = Path.GetDirectoryName(targetPath);
                    Directory.CreateDirectory(directory);
                    File.WriteAllBytes(targetPath, cultureJsLib.bytes);
                }
                else
                {
                    throw new Exception("File: " + sourcePath + " not found!");
                }
            }
        }    
    }
}
