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
		public static void LogStackTrace(string text)
		{
			System.Diagnostics.Debug.Assert(false, text);
		}

		/// <summary>
		/// Returns true if the game is running on a mobile platform. 
		/// </summary>
		/// <value><c>true</c> if on mobile platform; otherwise, <c>false</c>.</value>
		public static bool OnMobilePlatform
		{
			get
			{
				return RuntimePlatform.WP8Player == Application.platform ||
					RuntimePlatform.Android == Application.platform ||
					RuntimePlatform.BlackBerryPlayer == Application.platform ||
					RuntimePlatform.IPhonePlayer == Application.platform;
			}
		}

		[System.Obsolete("You should use Resources.Load<T> instead.")]
		public static T GetResource<T>(string path) where T : UnityEngine.Object
		{
			return Resources.Load<T>(path);
		}

		/// <summary>
		/// Randomizes the given list by iterating all entries and moving them to a random location within the list. 
		/// </summary>
		/// <param name="list">List.</param>
		public static void RandomizeList(IList list)
		{  
			System.Random rng = new System.Random();  
			int n = list.Count;  
			while (n > 1)
			{  
				n--;  
				int k = rng.Next(n + 1);  
				var value = list [k];  
				list [k] = list [n];  
				list [n] = value;  
			}
		}

		/// <summary>
		/// Returns a random item in the given list. 
		/// </summary>
		/// <returns>The random.</returns>
		/// <param name="pList">P list.</param>
		/// <typeparam name="T">The 1st type parameter.</typeparam>
		public static IEnumerable<T> GetRandom<T>(List<T> list)
		{
			System.Random random = new System.Random();
			List<T> copy = new List<T>(list);
		
			while (copy.Count > 0)
			{
				int index = random.Next(copy.Count);
				yield return copy [index];
				copy.RemoveAt(index);
			}	
		}

		[System.Obsolete("You should use Transform.FindChild(name) instead.")]
		public static Transform FindInChildren(Transform parent, string name)
		{

			foreach (Transform child in parent)
			{
				if (child.name == name)
					return child;
				Transform childSearch = FindInChildren(child, name);
				
				if (childSearch != null)
					return childSearch;
			}
			
			return null;
		}
		/// <summary>
		/// Recursively encapsulates all renderers on a given transform and its children in the given bounds. 
		/// </summary>
		/// <param name="transform">Transform.</param>
		/// <param name="bounds">Bounds.</param>
		public static void Encapsulate(Transform transform, ref Bounds bounds)
		{
			Renderer r = transform.GetComponent<Renderer>();
			if (r != null)
				bounds.Encapsulate(r.bounds);
			
			foreach (Transform t in transform)
			{
				Encapsulate(t, ref bounds);
			}
		}
		/// <summary>
		/// Recursively moves the given transform and all its children to the given layer. 
		/// </summary>
		/// <param name="parent">Parent.</param>
		/// <param name="layer">Layer.</param>
		public static void SetLayerRecursively(Transform parent, int layer)
		{
			parent.gameObject.layer = layer;
			
			foreach (Transform t in parent)
			{
				SetLayerRecursively(t, layer);
			}
		}

		#region reflection helpers

		/// <summary>
		/// Returns a list of all types in the current assembly with a given attribute type on them.
		/// </summary>
		/// <returns>The types with attribute.</returns>
		/// <typeparam name="T">The 1st type parameter.</typeparam>
		public static List<Type> GetTypesWithAttribute<T>()
		{
			var searchType = typeof(T);
			var list = new List<Type>();
			var assemblies = GetAssembly<T>();
			foreach (var assembly in assemblies)
			{
				var types = GetAssemblyTypes(assembly);
				foreach (var t in types)
				{
					// Replaced the type.GetCustromAttributes approach with this reflection-context-only one.
					// The t.GetCA will try to create the custom attribute object which can lead to crashes e.g. in IL2CPP context.
					// Anyhow the CustomAttribute isn't supported for IL2CPP.
					var data = CustomAttributeData.GetCustomAttributes(t);
					foreach (var cad in data)
					{
						if (cad.Constructor.DeclaringType == searchType)
							list.Add(t);
					}
				}
			}
			return list;
		}

		private static Assembly[] GetAssembly<T>()
		{
			Assembly[] assembly;
			#if UNITY_METRO && !UNITY_EDITOR
			var ca = typeof(T).GetTypeInfo().Assembly;
			assembly = new Assembly[1]
			{
				ca
			};
			#else
			assembly = AppDomain.CurrentDomain.GetAssemblies();
			#endif
			return assembly;
		}

		private static Type[] GetAssemblyTypes(Assembly assembly)
		{
			Type[] types;
			#if UNITY_METRO && !UNITY_EDITOR
			typeInfos = assembly.DefinedTypes; //IEnumerable<TypeInfo>
			List<Type> justTypes = new List<Type>();
			foreach(var ti in typeInfos)
			{
				justTypes.Add(ti.AsType());
			}
			types = justTypes.ToArray();
			#else
			types = assembly.GetTypes();
			#endif
			return types;
		}

		/// <summary>
		/// Returns a list of all types in the current assembly, that are assignable from a given type. You can use that to get all types implementing an interface or inheriting from a given base type. 
		/// </summary>
		/// <returns>The types assignable from.</returns>
		/// <typeparam name="T">The 1st type parameter.</typeparam>
		public static List<Type> GetTypesAssignableFrom<T>()
		{
			var list = new List<Type>();
			var searchType = typeof(T);
			var assemblies = GetAssembly<T>();

			foreach (var assembly in assemblies)
			{
				var types = GetAssemblyTypes(assembly);
				foreach (var t in types)
				{
					#if UNITY_METRO && !UNITY_EDITOR
					if (t.ImplementedInterfaces.Contains(searchType))
					{
						list.Add(t);
					}
					#else
					if (searchType.IsAssignableFrom(t) && t != searchType)
					{
						list.Add(t);
					}
					#endif
				}
			}
			return list;
		}

		#endregion
	}

}