using UnityEngine;
using System.Collections;
using UnityGameBase.Core.PlatformInterface;

 

namespace UnityGameBase.Core.PlatformInterface.Example
{
	public class PlatformInterfaceDemo : MonoBehaviour {
#if UGB_PI_DEMO
		void Start () {
			PlatformInterface.Instance.EnterImmersiveMode();
		}
#endif
	}
	#if UGB_PI_DEMO
	public class PlatformCreator : PlatformInterfaceCreator
	{
		public override IPlatformInterface CreateInstance()
		{
	#if UNITY_ANDROID
			return new AndroidPlatformInterface();
	#else
			return new IOSPlatformInferface();
	#endif
		}
	}


	//
	// Platform specific code (game specific)
	//



	#if UNITY_ANDROID
	public class AndroidPlatformInterface : IPlatformInterface
	{
		
		
		#region IPlatformInterface implementation
		
		public void EnterImmersiveMode()
		{
			Debug.Log("Android EnterImmersiveMode");
		}
		
		
		
		public void LeaveImmersiveMode()
		{
			Debug.Log("Android EnterImmersiveMode");
		}
		
		#endregion
		
		
	}

	#endif




	public class IOSPlatformInferface : IPlatformInterface
	{
		#region IPlatformInterface implementation

		public void EnterImmersiveMode()
		{
			//dummy (not supported on this platform)
		}



		public void LeaveImmersiveMode()
		{
			//dummy (not supported on this platform)
		}

		#endregion


	}
#endif
}
//
// Example: Interface additions
//
#if UGB_PI_DEMO
namespace UnityGameBase.Core.PlatformInterface
{
	public partial interface IPlatformInterface
	{
		void EnterImmersiveMode();
		void LeaveImmersiveMode();
	}
}
#endif