#if UNITY_METRO && !UNITY_EDITOR

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnityGameBase.Core.IO
{
    
    public enum BridgeConfiguration
    {
        useLocalStorage = 1,
        useRoamingStorage
    }

    public class Storage
    {
        /// <summary>
        /// Depending on the configuration you choose the path you give will be prefixed with 
        /// the local or roaming storage location.
        /// </summary>
        public static BridgeConfiguration Configuration = BridgeConfiguration.useLocalStorage;

        public static string ContainerName
        {
            get;
            set;
        }

        public static WrappedIO Save(string pPath, string pContent)
        {
            WrappedIO io = new WrappedIO(pPath,pContent);
            io.Write();
            return io;
        }

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
			WrappedIO io = new WrappedIO(pPath);
			io.Delete();
			return io;
		}
        internal static Windows.Storage.StorageFolder GetFolderFromConfiguration()
        {
            if(Configuration == BridgeConfiguration.useLocalStorage)
                return Windows.Storage.ApplicationData.Current.LocalFolder;
            return Windows.Storage.ApplicationData.Current.RoamingFolder;
        }

        internal static Windows.Storage.ApplicationDataContainer GetSettingsContainerFromConfiguration()
        {
            if (Configuration == BridgeConfiguration.useLocalStorage)
                return Windows.Storage.ApplicationData.Current.LocalSettings;
            return Windows.Storage.ApplicationData.Current.RoamingSettings;
        }

        #region settings

        public static T GetSetting<T>(string pKey)
        {
            return (T)GetSetting(pKey);
        }

        public static void SetSetting<T>(string pKey, T pValue)
        {

            SetSetting(pKey, pValue);

        }

        internal static object GetSetting(string pKey)
        {
            var containerConfig = GetSettingsContainerFromConfiguration();
            var container = containerConfig.CreateContainer(ContainerName, Windows.Storage.ApplicationDataCreateDisposition.Always);

            if (container.Containers.ContainsKey(ContainerName))
            {
                return container.Containers[ContainerName].Values[pKey];

            }
            throw new Exception("Container could not be found/created: " + ContainerName);
        }

        internal static void SetSetting(string pKey, object pValue)
        {
            var containerConfig = GetSettingsContainerFromConfiguration();
            var container = containerConfig.CreateContainer(ContainerName, Windows.Storage.ApplicationDataCreateDisposition.Always);

            if (container.Containers.ContainsKey(ContainerName))
            {
                container.Containers[ContainerName].Values[pKey] = pValue;
            }
            else
            {
                throw new Exception("Container could not be found/created: " + ContainerName);
            }
        }

        #endregion
    }
}
#endif