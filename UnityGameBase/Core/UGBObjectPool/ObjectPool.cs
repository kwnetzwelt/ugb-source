using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace UnityGameBase.Core.ObjPool
{
	public class ObjectPool
	{
		private List<ObjectPoolEntry> mListOfGameobjectStacks = new List<ObjectPoolEntry>();
		private Dictionary<int,int> mDictionaryOfObjectTypeIndex = new Dictionary<int, int>();
		private bool mWasNotEnough;
		private int mListIndex = -1;

		#region Create new objectpool entry
		/// <summary>
		/// Creates new entry in objectpool and registers the given object type. Instances are automatically created (pCount). 
		/// </summary>
		/// <param name="pPrefabToPool"> Prefab to pool.</param>
		/// <param name="pObjectType"> The object type of the prefab.</param>
		/// <param name="pCount"> Number of instances of the prefab, which are created automatically.</param>
		public void CreateNewObjectPoolEntry (GameObject pPrefabToPool, int pObjectType, int pCount)
		{
			mListIndex++;
			var newEntry = new ObjectPoolEntry();
			newEntry.Initialize(pPrefabToPool, mListIndex, pCount);
			mListOfGameobjectStacks.Add(newEntry);

			mDictionaryOfObjectTypeIndex.Add(pObjectType, mListIndex);
		}
		#endregion

		#region Return object on stack of entry
		/// <summary>
		/// Returns a given instance to the Object Pool. Use this when you don't need the instance any longer. 
		/// Default for hide in hierarchy is <c>true</c>.
		/// </summary>
		/// <param name="pObjectToPool"> The instance you want to put on the stack</param>
		/// <param name="pObjectType"> The object type of the instance you want to pool</param>
		/// <param name="pHideInHierarchy">If true the instance will not be visible in the hierarchy</param>
		public void ReturnObject (GameObject pObjectToPool, int pObjectType, bool pHideInHierarchy = true)
		{
			int ListIndex = 0;
			if(mDictionaryOfObjectTypeIndex.TryGetValue(pObjectType,out ListIndex))
			{
				pObjectToPool.SetActive(false);

				if(pHideInHierarchy)
				{
					pObjectToPool.hideFlags = HideFlags.HideInHierarchy;
				}

				mListOfGameobjectStacks [ListIndex].ReturnObject(pObjectToPool);
			}
			else
			{
				throw new KeyNotFoundException("Couldn't find " + pObjectToPool.name + " in dictionary.");
			}
		}
		#endregion

		#region Get object from stack of entry
		/// <summary>
		/// Returns an unused instance for the given object type. If none is present, creates a new instance for the object type. 
		/// </summary>
		/// <param name="pObjectType"> The object type to find</param>
		public GameObject GetInstance(int pObjectType)
		{
			int ListIndex = 0;
			GameObject ReturnedGo = null;
			if(mDictionaryOfObjectTypeIndex.TryGetValue(pObjectType, out ListIndex))
			{
				ReturnedGo = mListOfGameobjectStacks [ListIndex].GetInstance();
				ReturnedGo.SetActive(true);
			}
			else
			{
				throw new KeyNotFoundException("Couldn't find type# " + pObjectType + " in dictionary.");
			}

			ReturnedGo.hideFlags = HideFlags.None;
			return ReturnedGo;
		}
		#endregion

		#region Print the stats/information of all entrys
		/// <summary>
		/// Prints the stats of all entrys if game is running in editor.
		/// </summary>
		public void PrintStats()
		{
			if(Application.isEditor)
			{
				for(int i = 0; i < mListOfGameobjectStacks.Count; i++)
				{
					mListOfGameobjectStacks[i].PrintStats();
				}

			}
		}
		#endregion

		#region Reset stacks of all entrys
		/// <summary>
		/// Destroys all unused instances for all object types. 
		/// </summary>
		/// <param name="pResetStatsToo"> If set to true stats will be resetted too </param>
		public void ResetStacks(bool pResetStatsToo = false)
		{
			for(int i = 0; i < mListOfGameobjectStacks.Count; i++)
			{
				mListOfGameobjectStacks[i].ResetStack(pResetStatsToo);
			}
		}
		#endregion

		#region Reset stack of a specified entry
		/// <summary>
		/// Destroys all unused instances of a specific object type.
		/// </summary>
		/// <param name="pObjectType"> The ID of the entry </param>
		/// <param name="pResetStatsToo"> If set to true stats will be reseted too </param>
		public void ResetStackOfEntry(int pObjectType, bool pResetStatsToo = false)
		{
			int ListIndex = 0;
			if(mDictionaryOfObjectTypeIndex.TryGetValue(pObjectType, out ListIndex))
			{
				mListOfGameobjectStacks [ListIndex].ResetStack(pResetStatsToo);
			}
			else
			{
				throw new KeyNotFoundException("Couldn't find type# " + pObjectType + " in dictionary.");
			}
		}
		#endregion

		#region Reset the whole pool
		/// <summary>
		/// Resets the whole pool. Removes all object types (including stats) and destroys all unused instances. 
		/// </summary>
		public void ResetPool()
		{
			for(int i = 0; i < mListOfGameobjectStacks.Count; i++)
			{
				mListOfGameobjectStacks[i].ResetStack(true);
			}

			mListOfGameobjectStacks.Clear();
			mDictionaryOfObjectTypeIndex.Clear();
			mListIndex = -1;
	
		}
		#endregion
	}
}