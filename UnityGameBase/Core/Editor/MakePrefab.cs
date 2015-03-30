using UnityEngine;
using UnityEditor;

namespace UnityGameBase.Core.Editor
{
	//Generate a prefab from the selection
	    //took out if and else statements cause the code wouldn't work I got it to work this way.
	public class MakePrefab : MonoBehaviour
	{
		[MenuItem("Assets/Prefab from selection")]
	    static void CreatePrefab()
	    {
	        GameObject[] selectedObjects = Selection.gameObjects;
	       
	        
	        foreach(var go in selectedObjects)
	        {
	            //string localPath = "Assets/" + name + ".prefab";
				string localPath = AssetDatabase.GetAssetPath(go);
				if(localPath.LastIndexOf('.') != -1)
					localPath = localPath.Substring(0,localPath.LastIndexOf("."));
				localPath += ".prefab";
				
				
				
				Debug.Log(localPath);
	            
			     
	            createNew (go , localPath); //creating a new prefab
	        }
	    }
	     
	    //Create a new prefab
	     
	    static void createNew(GameObject _selectedObjects, string _localPath)
	    {
	        UnityEngine.Object prefab = PrefabUtility.CreateEmptyPrefab(_localPath);
	        PrefabUtility.ReplacePrefab(_selectedObjects, prefab);
	        AssetDatabase.Refresh();
			
			
			
	    }
	}
}