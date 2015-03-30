using System;
using UnityEditor;
using UnityEngine;
namespace UnityGameBase.Core.SceneMenu.Commands
{
	public class PlayDefaultSceneCommand : SceneMenuCommand
	{
	    const string kDefaultScenePathKey = "DefaultScenePath";

		public PlayDefaultSceneCommand()
	    {
	        mName = "Play Default Scene";
	    }
	    public override void Execute()
	    {
	        PlayDefaultScene();
	    }

	    [MenuItem("UGB/Go To/Play Default Scene &P",false,0)]
	    static void PlayDefaultScene()
	    {
	        string path = null;

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

	        EditorApplication.SaveScene();
	        EditorApplication.OpenScene(path);
	        EditorApplication.isPlaying = true;
	    }
	}
}