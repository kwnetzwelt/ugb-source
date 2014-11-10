using System;
using UnityEngine;
using System.IO;
using System.Collections.Generic;
using System.Collections;
using System.Reflection;

#if UNITY_METRO && !UNITY_EDITOR	
using Windows.Storage;
using System.Linq;
#endif
namespace UGB.Utils
{
	public class UGBHelpers
	{
		public static void LogStackTrace (string pText)
		{
			System.Diagnostics.Debug.Assert(false, pText);
			
		}
		
		public static T CreateComponentIfNotExists<T> (MonoBehaviour pTarget) where T : Component
		{
			T comp = pTarget.gameObject.GetComponent<T>();
			if(comp != null)
				return comp;
			
			return pTarget.gameObject.AddComponent<T>();
		}
		
		public static T CreateComponentIfNotExists<T> (GameObject pTarget) where T : Component
		{
			T comp = pTarget.GetComponent<T>();
			if(comp != null)
				return comp;
			
			return pTarget.AddComponent<T>();
		}
		
		public static EGenericPlatform platform
		{
			get {
				
	#if UNITY_ANDROID || UNITY_IPHONE || UNITY_METRO || UNITY_WEBPLAYER
				return EGenericPlatform.Mobile;
	#elif UNITY_STANDALONE_WIN || UNITY_STANDALONE_OSX
				return EGenericPlatform.Desktop;
	#endif
			}
		}
		
		public static bool CheckFileExists(string pPath)
		{			
	#if !UNITY_METRO || UNITY_EDITOR
			FileInfo fi = new FileInfo(pPath);
			return fi.Exists;
	#else
			return true;
	#endif
		}
		
		public static bool MakeSureFolderExists(string pPath)
		{
	#if !UNITY_METRO || UNITY_EDITOR		
			DirectoryInfo di = new DirectoryInfo(pPath);
			if(!di.Exists)
				di.Create();
			return true;
	#else
			return true;
	#endif
		}
		public static bool onMobilePlatform 
		{
			get {return RuntimePlatform.Android == Application.platform || RuntimePlatform.IPhonePlayer == Application.platform;}
		}
		
		public static T GetResource<T>(string pPath) where T : UnityEngine.Object
		{
			return (T)Resources.Load(pPath , typeof(T));
		}
		
		public static void RandomizeList( IList pList)  
		{  
		    System.Random rng = new System.Random();  
		    int n = pList.Count;  
		    while (n > 1) {  
		        n--;  
		        int k = rng.Next(n + 1);  
		        var value = pList[k];  
		        pList[k] = pList[n];  
		        pList[n] = value;  
		    }  
			
		}
		
		public static IEnumerable<T> GetRandom<T>(List<T> pList)
		{
			System.Random random = new System.Random();
		    List<T> copy = new List<T>(pList);
		
		    while (copy.Count > 0)
		    {
		        int index = random.Next(copy.Count);
		        yield return copy[index];
		        copy.RemoveAt(index);
		    }	
		}
		public static Transform FindInChildren(Transform pParent, string pName)
		{
			
			foreach(Transform child in pParent)
			{
				if(child.name == pName)
					return child;
				Transform childSearch = FindInChildren(child, pName);
				
				if(childSearch != null)
					return childSearch;
			}
			
			return null;
		}
		
		public static Vector3 ConstrainZ (Vector3 pVector, int pZ)
		{
			pVector.z = pZ;
			return pVector;
		}
		
		public static void Encapsulate(Transform pTransform, ref Bounds pBounds)
		{
			Renderer r = pTransform.GetComponent<Renderer>();
			if(r != null)
				pBounds.Encapsulate( r.bounds );
			
			foreach(Transform t in pTransform)
			{
				Encapsulate(t, ref pBounds);
			}
		}

		public static void SetLayerRecursively(Transform pParent, int pLayer)
		{
			pParent.gameObject.layer = pLayer;
			
			foreach(Transform t in pParent)
			{
				SetLayerRecursively(t,pLayer);
			}
		}

		#region reflection helpers

		public static List<System.Type> GetTypesWithAttribute<T>()
		{
			var searchType = typeof(T);

#if UNITY_METRO && !UNITY_EDITOR
			var currentAssembly = searchType.GetTypeInfo().Assembly;

			var outList = new List<Type>();
			
			var types = currentAssembly.DefinedTypes; 
			foreach(var t in types)
			{
				if(t.GetCustomAttribute(searchType) != null)
				{
					outList.Add(t.AsType());
				}
			}
			return outList;
#else
			var list = new List<System.Type>();

			foreach( var assembly in System.AppDomain.CurrentDomain.GetAssemblies())
			{
				foreach(System.Type type in assembly.GetTypes()) {
					if (type.GetCustomAttributes(searchType, true).Length > 0) {
						list.Add(type);
					}
				}
			}
			return list;
#endif
		}

		public static List<System.Type> GetTypesAssignableFrom<T>()
		{
			var list = new List<System.Type>();
			var searchType = typeof(T);

#if UNITY_METRO && !UNITY_EDITOR
			var currentAssembly = searchType.GetTypeInfo().Assembly;
			
			var outList = new List<Type>();
			
			var types = currentAssembly.DefinedTypes;
			foreach (var t in types)
			{
				if (t.ImplementedInterfaces.Contains(searchType))
					outList.Add(t.AsType());
			}
			return outList;
#else

			foreach( var assembly in System.AppDomain.CurrentDomain.GetAssemblies())
			{
				foreach(System.Type type in assembly.GetTypes()) {
					if (searchType.IsAssignableFrom(type) && type != searchType) {
						list.Add(type);
					}
				}
			}
			return list;
#endif
		}

		#endregion

	}

}