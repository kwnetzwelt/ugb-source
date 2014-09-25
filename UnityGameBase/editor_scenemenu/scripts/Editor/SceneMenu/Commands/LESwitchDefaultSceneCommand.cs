using System;
using UnityEngine;
using UnityEditor;

namespace UGB.SceneMenu.Commands
{

	public class LESwitchDefaultSceneCommand : LESceneMenuCommand
	{
	    const string kDefaultScenePathKey = "DefaultScenePath";
	    static string path = null;

	    public LESwitchDefaultSceneCommand()
	    {
	        mName = "Switch To Default Scene";
	    }

	    public override void Execute()
	    {
	        OpenDefaultScene();
	    }

	    [MenuItem("UGB/Go To/Open Default Scene &D", false, 1)]
	    static void OpenDefaultScene()
	    {
	        if (!EditorPrefs.HasKey(kDefaultScenePathKey))
	        {
	            path = EditorUtility.OpenFilePanel("Path To Default Scene", Application.dataPath + "/scenes", "unity");

	            if (!string.IsNullOrEmpty(path))
	            {
	                path = path.Replace(Application.dataPath, "Assets");
	                EditorPrefs.SetString(kDefaultScenePathKey, path);
	            }
	            else
	            {
	                return;
	            }
	        }

	        if (string.IsNullOrEmpty(path))
	        {
	            path = EditorPrefs.GetString(kDefaultScenePathKey);
	        }

	        EditorApplication.SaveCurrentSceneIfUserWantsTo();
	        EditorApplication.OpenScene(path);
	    }

	    [MenuItem("UGB/Go To/Reset/Default Scene Path", false, 15)]
	    static void DeleteDefaultScenePath()
	    {
	        EditorPrefs.DeleteKey(kDefaultScenePathKey);
	        path = null;
	    }
	}
}