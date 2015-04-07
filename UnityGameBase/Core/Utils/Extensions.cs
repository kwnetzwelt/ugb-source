using UnityEngine;
using System.Collections.Generic;
namespace UnityGameBase.Core.Extensions
{
    public static class Extensions
    {
		#region GameObject/MonoBehaviour
        /// <summary>
        /// Tries to get a component of the given type on the GameObject of the given MonoBehaviour. If none exists adds a component to the GameObject. 
        /// </summary>
        /// <returns>The existing or added component.</returns>
        /// <param name="target">target.</param>
        /// <typeparam name="T">Component</typeparam>
        public static T AddComponentIfNotExists<T>(this MonoBehaviour target) where T : Component
        {
            return AddComponentIfNotExists<T>(target.gameObject);
        }
		
        /// <summary>
        ///  Tries to get a component of the given type on the given GameObject. If none exists adds a component to the GameObject. 
        /// </summary>
        /// <returns>The existing or added component.</returns>
        /// <param name="target">target.</param>
        /// <typeparam name="T">Component</typeparam>
        public static T AddComponentIfNotExists<T>(this GameObject target) where T : Component
        {
            T comp = target.GetComponent<T>();
            if(comp != null)
            {
                return comp;
            }
			
            return target.AddComponent<T>();
        }

        public static T FindInParents<T>(this GameObject go) where T : Component
        {
            if(go == null)
            {
                return null;
            }
            var comp = go.GetComponent<T>();
            if(comp != null)
            {
                return comp;
            }
            
            Transform t = go.transform.parent;
            while(t != null && comp == null)
            {
                comp = t.gameObject.GetComponent<T>();
                t = t.parent;
            }
            return comp;
        }
        
		#endregion
        
        /// <summary>
        /// Returns a random element from the list
        /// </summary>
        /// <returns>Random Element</returns>
        /// <param name="target">Target.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public static T GetRandom<T>(this List<T> target)
        {
            if(target.Count == 0)
            {
                return default(T);
            }
            
            int rand = Random.Range(0, target.Count);
            return target[rand];
        }
        
        public static T GetRandomAndRemove<T>(this List<T> target)
        {
            if(target.Count == 0)
            {
                return default(T);
            }
            
            int rand = Random.Range(0, target.Count);
            
            T result = target[rand];
            target.Remove(result);
            return result;
        }
        
        public static void Shuffle<T>(this List<T> list)
        {
            System.Random rng = new System.Random();  
            int n = list.Count;  
            while(n > 1)
            {  
                n--;  
                int k = rng.Next(n + 1);  
                var value = list[k];  
                list[k] = list[n];  
                list[n] = value;  
            }
        }
        
        public static GameObject FindGameObject(this GameObject target, string name)
        {
            GameObject[] allObjects = GameObject.FindObjectsOfType<GameObject>();
            foreach(GameObject obj in allObjects)
            {
                if(obj.name == name)
                {
                    return obj;
                }                
            }
            return null;
        }
    }
}