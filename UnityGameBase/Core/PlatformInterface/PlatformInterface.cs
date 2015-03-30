using System;
namespace UnityGameBase.Core.PlatformInterface
{
	/// <summary>
	/// Platform specific code interface. This Interface gives access to platform specific functionality and encapsulates all platform specific code. 
	/// To implement platform specific functionality use <see cref="UGB.PlatformInterface.IPlatformInterface"/>. 
	/// To decide which platform interface should be used when <see cref="UGB.PlatformInterface.PlatformInterfaceCreator"/>. 
	/// </summary>
	public class PlatformInterface
	{
		public PlatformInterface()
		{
		}

		static IPlatformInterface instance;

		/// <summary>
		/// Returns an instance of a platform interface. You have to determine which one will be returned 
		/// in your own PlatformInterfaceCreator.
		/// </summary>
		/// <value>The instance.</value>
		public static IPlatformInterface Instance
		{
			get
			{
				if(instance == null)
				{
					instance = CreateInstance();
				}
				return instance;
			}
		}

		static IPlatformInterface CreateInstance()
		{
			var types = Utils.UGBHelpers.GetTypesAssignableFrom<PlatformInterfaceCreator>();

			if(types.Count < 1)
			{
				throw new Exception(string.Format("NocClass found with base class {0}", typeof(PlatformInterfaceCreator).ToString() ));
			}
			if(types.Count > 1)
			{
				UnityEngine.Debug.LogWarning(string.Format("More than one class found with base class {0}",typeof(PlatformInterfaceCreator).ToString() ));
			}
			System.Type selectedType = types[0];

			UnityEngine.Debug.Log(string.Format("Creating PlatformInterface {0}",selectedType.ToString()));

			var creator = Activator.CreateInstance(selectedType) as PlatformInterfaceCreator;

			var platformInstance = creator.CreateInstance();

			return platformInstance;

		}

	}
}

