using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace UnityGameBase.Core.ObjPool
{

	internal class ObjectPoolEntry
	{
		private GameObject mPrefab = null;
		private int mMaxParallelObjects;
		internal int mEntryID;
		private Stack<GameObject> mStackOfPooledObjects = new Stack<GameObject>();

		#region CreateStackObject
		/// <summary>
		/// Fills the stack with new instantiated gameobjects(mPrefab), updates the
		/// maximum amount of gameobjects at once and returns true if it's done.
		/// <param name="pAmountOfInstances" > The amount of instances to instantiate</param>
		/// </summary>
		private bool CreateStackObject(int pAmountOfInstances)
		{
			for(int i = 0; i < pAmountOfInstances; i++)
			{	
				GameObject newInstance = GameObject.Instantiate(mPrefab) as GameObject;

				newInstance.name = mPrefab.name;

				mStackOfPooledObjects.Push(newInstance);

				newInstance.SetActive(false);
				newInstance.hideFlags = HideFlags.HideInHierarchy;
			}

			UpdateMaxParallelObjects();
			return true;
		}
		#endregion

		#region Initialize entry
		/// <summary>
		/// Sets the ID and the prefab of this entry for further 
		/// instantiations and calls the method to fill the stack.
		/// <param name="pPrefabToPool" > The prefab in the stack</param>
		/// <param name="pObjectType" > Objecttype-ID</param>
		/// <param name="pCount" > Amount of objects to instantiate</param>
		/// </summary>
		internal void Initialize(GameObject pPrefabToPool, int pObjectType, int pCount)
		{
			mEntryID = pObjectType;
			mPrefab = pPrefabToPool;
			CreateStackObject(pCount);
		}
		#endregion

		#region Put object on stack
		/// <summary>
		/// Push the given object on top of the stack and update the
		/// maximum amount of parallel objects at once.
		/// <param name="ObjectToFillIn" > The object to push</param>
		/// </summary>
		internal void ReturnObject(GameObject pObjectToFillIn)
		{
			mStackOfPooledObjects.Push(pObjectToFillIn);
			UpdateMaxParallelObjects();
		}
		#endregion

		#region Take object from stack
		/// <summary>
		/// Checks if either an gameobject is still in the stack or if a
		/// new gameobject is needed and returns the gameobject.
		/// </summary>
		internal GameObject GetInstance()
		{
			GameObject lastGO = null;
			if(mStackOfPooledObjects.Count > 0)
			{
				UpdateMaxParallelObjects();
				lastGO =  mStackOfPooledObjects.Pop();
			}
			else
			{
				// Stack is empty, create a new object
				if (CreateStackObject(1))
				{
					lastGO =  mStackOfPooledObjects.Pop();
				}
			}
			return lastGO;
		}

		internal GameObject GetInstance(bool pDebugMessage)
		{
			GameObject lastGO = null;
			if(mStackOfPooledObjects.Count > 0)
			{
				UpdateMaxParallelObjects();
				lastGO =  mStackOfPooledObjects.Pop();
			}
			else
			{
				// Stack is empty, create a new object
				if (CreateStackObject(1))
				{
					if(Application.isEditor && pDebugMessage)
					{
						Debug.Log("Instantiation shouldn't happen anymore");
					}
					
					lastGO =  mStackOfPooledObjects.Pop();
				}
			}
			return lastGO;
		}
		#endregion

		#region Update maximum amount of parallel obejcts at once
		/// <summary>
		/// Updates the amount of maxium parallel gameobjects at once of this type.
		/// </summary>
		private void UpdateMaxParallelObjects()
		{
			if(mStackOfPooledObjects.Count > mMaxParallelObjects)
			{
				mMaxParallelObjects = mStackOfPooledObjects.Count;
			}
		}
		#endregion

		#region Reset stack
		/// <summary>
		/// Resets the stack of this entry and the stats if wanted.
		/// </summary>
		/// <param name="pResetStatsToo">If set to <c>true</c> p reset stats too.</param>
		internal void ResetStack(bool pResetStatsToo)
		{
			ResetStack ();

			if(pResetStatsToo)
			{
				mMaxParallelObjects = 0;
			}
		}

		/// <summary>
		/// Resets the stack of this entry and the stats if wanted. Default for Stat-reset is false.
		/// </summary>
		/// <param name="pResetStatsToo">If set to <c>true</c> p reset stats too.</param>
		internal void ResetStack()
		{
			
			foreach (GameObject stackedGo in mStackOfPooledObjects)
			{
				GameObject.Destroy(stackedGo);
			}
			
			mStackOfPooledObjects.Clear();
		}
		#endregion

		#region Print stats/information about stack
		/// <summary>
		/// Prints stats/information of this entry.
		/// </summary>
		internal void PrintStats()
		{

			Debug.Log("----------------- " + "Information about stackentry [" + mEntryID + "]" + " -----------------\n" +
					  "Pooled object: " + mPrefab.name + "; \n" +
			          "Current size of stack: " + mStackOfPooledObjects.Count + "; \n" +
			          "Amount of maximum parallel objects: " + mMaxParallelObjects + "; \n" +
			          "Amount of current parallel objects: " + (mMaxParallelObjects - mStackOfPooledObjects.Count) + ";\n");
		}
		#endregion
	}
}