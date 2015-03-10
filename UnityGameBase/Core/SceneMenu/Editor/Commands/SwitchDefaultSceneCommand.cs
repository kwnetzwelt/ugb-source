using System;
using UnityEngine;
using UnityEditor;

namespace UnityGameBase.Core.SceneMenu.Commands
{

	public class SwitchDefaultSceneCommand : SceneMenuCommand
	{
	    const string kDefaultScenePathKey = "DefaultScenePath";
	    static string path = null;

	    public SwitchDefaultSceneCommand()
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