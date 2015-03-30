using System;
using System.Collections;
using System.Text;

#if !UNITY_METRO || UNITY_EDITOR

namespace UnityGameBase.Core.IO
{
	
	public enum BridgeConfiguration
	{
		useLocalStorage = 1,
		useRoamingStorage
	}

	/// <summary>
	/// Multiplatform storage wrapper. Can be used to read, write or delete files and to check if they exist. 
	/// </summary>
	public class Storage
	{
		/// <summary>
		/// Depending on the configuration you choose the path you give will be prefixed with 
		/// the local or roaming storage location.
		/// </summary>
		public static BridgeConfiguration Configuration = BridgeConfiguration.useLocalStorage;
		
		/// <summary>
		/// Save the given content to the given path destination. 
		/// On Windows Store Apps BridgeConfiguration is used to determine the path. 
		/// </summary>
		/// <param name="pPath">P path.</param>
		/// <param name="pContent">P content.</param>
		public static WrappedIO Save(string pPath, string pContent)
		{
			WrappedIO io = new WrappedIO(pPath,pContent);
            
			io.Write();
			return io;
		}

		/// <summary>
		/// Loads all the content from the given path. 
		/// On Windows Store Apps BridgeConfiguration is used to determine the path. 
		/// </summary>
		/// <param name="pPath">P path.</param>
		public static WrappedIO Load(string pPath)
		{
			WrappedIO io = new WrappedIO(pPath);
			io.Load();
			return io;
		}

        public static WrappedIO Exists(string pPath)
        {
            WrappedIO io = new WrappedIO(pPath);
            io.Exists();
            return io;
        }

		public static WrappedIO Delete(string pPath)
		{
			WrappedIO io = new WrappedIO (pPath);
			io.Delete ();
			return io;
		}

		
		internal static string GetFolderFromConfiguration()
		{
            return "";
		}
		
		internal static string GetSettingsContainerFromConfiguration()
		{
			return "";
			
		}
	}
}
#endif