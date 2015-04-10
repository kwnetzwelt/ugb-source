using System;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;

#if UNITY_METRO && !UNITY_EDITOR
using Windows.System.Threading;
#elif !UNITY_WEBGL
using System.Threading;
#endif

namespace UnityGameBase.Core.Utils
{
	public class ThreadingBridge : MonoBehaviour
	{
		/// <summary>
		/// Initialize the threading bridge. This is called automatically by the UGB.Game class. 
		/// </summary>
		public static void Initialize ()
		{

			GameObject go = new GameObject("_ThreadingBridge");
			go.AddComponent<ThreadingBridge>();
			DontDestroyOnLoad(go);

		}

		static Queue<System.Action> todo = new Queue<System.Action>();



		void Update()
		{
			if(todo.Count > 0)
			{
				StartCoroutine( Dequeue());
			}
		}


		IEnumerator Dequeue()
		{
			System.Action action = todo.Dequeue();

			yield return 0;

			action();
		}


		#region public interface

		/// <summary>
		/// Enqueue some work that will be done on the main thread during the next update. 
		/// </summary>
		/// <param name="action">The action to be executed. </param>
		public static void Dispatch(System.Action action)
		{
			todo.Enqueue(action);
		}

		/// <summary>
		/// Enqueue some work to be executed in a separate thread. 
		/// </summary>
		/// <param name="action">The action to be executed. </param>
		public static void ExecuteThreaded(System.Action action)
		{
#if UNITY_WEBGL
			throw new NotSupportedException("Threading is not supported on this platform. ");
#elif UNITY_METRO && !UNITY_EDITOR
#pragma warning disable 4014
			ThreadPool.RunAsync( (source) => { action(); });
#pragma warning restore
#else
			ThreadPool.QueueUserWorkItem( new WaitCallback((state) => {action();}) );
#endif
		}

		#endregion
	}

}