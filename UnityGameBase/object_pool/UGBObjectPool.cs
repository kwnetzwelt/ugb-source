using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UGBObjectPool
{
	
	const int kHiddenLayerIndex = 3;
	
	private static Dictionary<int,CUGBObjectPoolEntry> mDefinitions = new Dictionary<int, CUGBObjectPoolEntry>();
	public static Dictionary<int,Stack<GameObject>> mDictionary = new Dictionary<int, Stack<GameObject>>();
	
	private static GameObject mDefinitionHolder;
	private static GameObject mInstanceHolder;
	
	private static void Init()
	{
		GameObject go = new GameObject("_ObjectPool");
		GameObject.DontDestroyOnLoad(go);
		
		mDefinitionHolder = new GameObject("Definitions");
		mInstanceHolder = new GameObject("Instances");
		
		mDefinitionHolder.transform.parent = go.transform;
		mInstanceHolder.transform.parent = go.transform;
		mInstanceHolder.SetActive(false);
		
	}
	
	public static void AddObjectDefinition(GameObject pPrototype, int pTypeIndex)
	{
		if(mDefinitionHolder == null)
		{
			Init ();
		}
		
		if(mDefinitions.ContainsKey(pTypeIndex))
		{
			// remove entry first
			var oldEntry = mDefinitions[pTypeIndex];
			mDefinitions.Remove(pTypeIndex);
			GameObject.Destroy( oldEntry );
			
			while(mDictionary[pTypeIndex].Count > 0)
			{
				var go = mDictionary[pTypeIndex].Pop();
				GameObject.Destroy(go);
			}
			mDictionary.Remove(pTypeIndex);
		}
		
		GameObject poolEntryGO = new GameObject("ObjectPoolEntry");
		var entry = poolEntryGO.AddComponent<CUGBObjectPoolEntry>();
		
		entry.pType = pTypeIndex;
		entry.mPrototype = pPrototype;
		pPrototype.transform.parent = entry.transform;
		
		mDefinitions.Add(pTypeIndex,entry);
		entry.gameObject.SetActive(false);
		
		entry.transform.parent = mDefinitionHolder.transform;
		
		mDictionary.Add(pTypeIndex,new Stack<GameObject>());
	}
	
	public static GameObject GetObjectInstance(int pTypeIndex)
	{
		if(mDictionary[pTypeIndex].Count == 0)
		{
			return CreateInstance(pTypeIndex);
		}
	
		GameObject go = mDictionary[pTypeIndex].Pop();
		go.transform.parent = null;
		return go;
	}
	
	private static GameObject CreateInstance(int pType)
	{
		GameObject ins = GameObject.Instantiate(mDefinitions[pType].mPrototype) as GameObject;
		return ins;
	}
		
	
	public static void ReturnObjectInstance(GameObject pInstance, int pTypeIndex)
	{
		if(mDictionary.ContainsKey(pTypeIndex))
		{
			mDictionary[pTypeIndex].Push(pInstance);
			pInstance.transform.parent = mInstanceHolder.transform;
		}
		else
		{
			GameObject.Destroy(pInstance);
		}
	}
	
	
}

public class CUGBObjectPoolEntry : MonoBehaviour
{
	public GameObject mPrototype;
	public int pType;
}