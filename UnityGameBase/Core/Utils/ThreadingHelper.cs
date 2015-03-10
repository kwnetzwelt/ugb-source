using System;
using System.Threading;
#if UNITY_METRO && !UNITY_EDITOR

using System.Threading.Tasks;
using Windows.System.Threading;

#endif
namespace UnityGameBase.Core.Utils
{
    class ThreadingHelper
    {
        public delegate void ActionDelegate();
        public delegate void ThreadCallback(Object state);
		
		
		
        public static void Dispatch(System.Action callback)
        {
            if (callback == null)
                return;
			
			ThreadingBridge.Dispatch(() => { callback(); });
        }

        public static void CallThreadedFunction(Action action)
        {
			ThreadingBridge.ExecuteThreaded(() => { 
				try
				{
					action();
				}catch(Exception e)
				{
					ThreadingHelper.Dispatch(() => {
						
						throw e;
						
					});
				}
			});
        }

        public static void Sleep(int millisecondsTimeout)
        {
            // MS-EX: not supported for win-rt builds
            //Thread.Sleep(50);
            
            // Alternative:
            //new System.Threading.ManualResetEvent(false).WaitOne(millisecondsTimeout);

#if UNITY_METRO && !UNITY_EDITOR
			SleepWinRT(millisecondsTimeout);
#else
            // not yet implemented
			Thread.Sleep(millisecondsTimeout);
#endif
        }

#if UNITY_METRO && !UNITY_EDITOR
        private static async void SleepWinRT(int millisecondsTimeout)
        {
            await Task.Delay(millisecondsTimeout);
        }
#endif

        public static void QueueUserWorkItem(ThreadCallback callback)
        {
			ThreadingBridge.ExecuteThreaded(() => { callback(null); });
        }
    }
}
