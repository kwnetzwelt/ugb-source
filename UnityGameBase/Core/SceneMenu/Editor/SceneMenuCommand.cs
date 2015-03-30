using System;
using UnityEngine;
using UnityEditor;
namespace UnityGameBase.Core.SceneMenu
{
	public abstract class SceneMenuCommand
	{
		public SceneMenuCommand ()
		{
		}
		
		public string mName;
		public KeyCode mKeyCode = KeyCode.None;
		public EventModifiers mModifiers;
		
		public abstract void Execute();
		
		public static void Execute<T>() where T : SceneMenuCommand, new()
		{
			T i = new T();
			i.Execute();
		}
		public string GetFormattedShortCut ()
		{
			int[] values = (int[])System.Enum.GetValues(typeof(EventModifiers));
			string outString = "";
			foreach(int v in values)
			{
				if(v == (int)EventModifiers.FunctionKey)
					continue;
			
				if(((int)mModifiers & v ) != 0)
				{
					outString = outString + "+" + Enum.GetName(typeof(EventModifiers),v);
				}
			}
			if(outString.Length > 0)
				return outString.Substring(1) + "+" + mKeyCode;
			
			return mKeyCode.ToString();
			
		}
		
		public static GameObject CreateAndSelect(string pPath)
		{
			
			GameObject prefab = AssetDatabase.LoadAssetAtPath(pPath,typeof(GameObject)) as GameObject;
			GameObject instance = PrefabUtility.InstantiatePrefab(prefab) as GameObject;
			
			if(instance == null)
			{
				Debug.LogError("Could not instantiate requested prefab: " + pPath);
				return null;
			}
			
			instance.transform.position = Vector3.zero;
			
			EditorUtility.SetDirty(instance);
			
			prefab = null;
#if UNITY_5_0
			EditorUtility.UnloadUnusedAssetsImmediate();
#else
			EditorUtility.UnloadUnusedAssets();
#endif
			
			
			
			
			
		
			GameObject parent = Selection.activeGameObject;
			if(parent != null)
			{
				PrefabType parentPrefabType = PrefabUtility.GetPrefabType(parent);
				if(parentPrefabType != PrefabType.None)
					instance.transform.parent = parent.transform;
			}
			Selection.activeGameObject = instance;
			return instance;
		}

		public bool WillHandle (Event pEvent)
		{
			if(mKeyCode == KeyCode.None)
				return false;
			if(pEvent.keyCode == mKeyCode)
				if(pEvent.modifiers == mModifiers)
					return true;
			
			return false;
		}
	}

}