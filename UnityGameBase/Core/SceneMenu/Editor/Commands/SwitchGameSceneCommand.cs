using System;
using UnityEngine;
using UnityEditor;

namespace UnityGameBase.Core.SceneMenu.Commands
{
	public class SwitchGameSceneCommand : SceneMenuCommand
	{
	    const string kGameScenePathKey = "GameScenePath";
	    static string path = null;

	    public SwitchGameSceneCommand()
	    {
	        mName = "Switch To Game Scene";
	    }

	    public override void Execute()
	    {
	        OpenGameScene();
	    }

	    [MenuItem("UGB/Go To/Open Game Scene &G", false, 2)]
	    static void OpenGameScene()
	    {
	        if (!EditorPrefs.HasKey(kGameScenePathKey))
	        {
	            path = EditorUtility.OpenFilePanel("Path To Game Scene", Application.dataPath + "/scenes", "unity");

	            if (!string.IsNullOrEmpty(path))
	            {
	                path = path.Replace(Application.dataPath, "Assets");
	                EditorPrefs.SetString(kGameScenePathKey, path);
	            }
	            else
	            {
	                return;
	            }
	        }

	        if (string.IsNullOrEmpty(path))
	        {
	            path = EditorPrefs.GetString(kGameScenePathKey);
	        }

	        EditorApplication.SaveCurrentSceneIfUserWantsTo();
	        EditorApplication.OpenScene(path);
	    }


	    [MenuItem("UGB/Go To/Reset/Game Scene Path", false, 16)]
	    static void DeleteGameScenePath()
	    {
	        EditorPrefs.DeleteKey(kGameScenePathKey);
	        path = null;
	    }
	}
}