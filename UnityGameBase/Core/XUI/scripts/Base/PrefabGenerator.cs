using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityGameBase.Core.XUI;

namespace UnityGameBase.Core.XUI
{
/// <summary>
/// this component is used for saving the prefab without loosing the child prefab references
/// </summary>
    [System.Serializable]
    public class PrefabGenerator : MonoBehaviour
    {
        [SerializeField]
        public string
            folder = null;

        public void CloneAndSave(string path, GameObject root)
        {
#if UNITY_EDITOR
            //create a copy to keep the child prefab references
            GameObject copy = Instantiate(root) as GameObject;
            copy.name = root.name;

            //TODO maybe another point to do this, its not clear why it happened here... (for others except me)
            copy.GetComponent<WidgetManager>().CreateCollection();
			
            if(!File.Exists(path))
            {
                GameObject parent = new GameObject("parent");
                parent.AddComponent<XUIHierarchyViewer>();
                copy.transform.SetParent(parent.transform);

                DestroyImmediate(copy.GetComponent<PrefabGenerator>());
                
                //create a new prefab
                UnityEditor.PrefabUtility.CreatePrefab(path, parent);
                DestroyImmediate(parent);
            }
            else
            {
                GameObject prefab = UnityEditor.AssetDatabase.LoadAssetAtPath(path, typeof(GameObject)) as GameObject;
                GameObject parent = UnityEditor.PrefabUtility.InstantiatePrefab(prefab as GameObject) as GameObject;
								
                //delete all childs to replace it with the copy and keep in parent the prefab metadata
                while(parent.transform.childCount > 0)
                {
                    DestroyImmediate(parent.transform.GetChild(0).gameObject);
                }
												
                copy.transform.SetParent(parent.transform);

                DestroyImmediate(copy.GetComponent<PrefabGenerator>());
				
                //update an existing one				
                UnityEditor.PrefabUtility.ReplacePrefab(parent, prefab);
				
                DestroyImmediate(parent);
            }
#endif // UNITY_EDITOR
        }
    }

}

