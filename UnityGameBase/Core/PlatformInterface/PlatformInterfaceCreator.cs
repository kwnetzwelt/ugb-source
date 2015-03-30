using System;
namespace UnityGameBase.Core.PlatformInterface
{

	/// <summary>
	/// Abstract base class for platform interface creator. Implement your own creating policy in your custom (game specific) PlatformInterfaceCreator. 
	/// You can decide which PlatformInterface to create (and how) in your own PlatformInterfaceCreator. 
	/// 
	/// There is no default behaviour. Using PlatformInterface.Instance results in an implicit call to your own factory (which needs to derive from PlatformInterfaceCreator).
	/// </summary>
	public abstract class PlatformInterfaceCreator
	{
		/// <summary>
		/// Called implicitely by the PlatformInterface.Instance getter. 
		/// Implement your platform decision here. 
		/// </summary>
		/// <returns>The instance.</returns>
		public abstract IPlatformInterface CreateInstance();
	}
}

