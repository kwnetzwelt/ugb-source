using System;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;

#if UNITY_METRO && !UNITY_EDITOR
using Windows.System.Threading;
#else
using System.Threading;
#endif

namespace UGB.Utils
{
	public class ThreadingBridge : MonoBehaviour
	{
		public static void Initialize ()
		{

			GameObject go = new GameObject("_ThreadingBridge");
			go.AddComponent<ThreadingBridge>();
			DontDestroyOnLoad(go);

		}

		static Queue<System.Action> mTodo = new Queue<System.Action>();



		void Update()
		{
			if(mTodo.Count > 0)
			{
				StartCoroutine( Dequeue());
			}
		}


		IEnumerator Dequeue()
		{
			System.Action action = mTodo.Dequeue();

			yield return 0;

			action();
		}


		#region public interface

		public static void Dispatch(System.Action pAction)
		{
			mTodo.Enqueue(pAction);
		}

		public static void ExecuteThreaded(System.Action pAction)
		{
	#if UNITY_METRO && !UNITY_EDITOR
			ThreadPool.RunAsync( (source) => { pAction(); });
	#else
			ThreadPool.QueueUserWorkItem( new WaitCallback((state) => {pAction();}) );
	#endif
		}

		#endregion
	}

}