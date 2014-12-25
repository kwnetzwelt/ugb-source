using UnityEngine;

namespace UGB.Extensions
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
			if (comp != null)
				return comp;
			
			return target.AddComponent<T>();
		}
		#endregion
	}
}
