using UnityEngine;
using System.Collections.Generic;
using System.Globalization;
using UnityGameBase.Core.Data;
using System.IO;
using System.Linq;

namespace UnityGameBase.Core.XUI
{
    public static class LocalizationHelper
    {
        public static LocaData locaData;
		
        private static string[] allLanguagesNames;
        public static string[] AllLanguagesNames
        {
            get
            {
                if (allLanguagesNames == null || allLanguagesNames.Length == 0)
                    return null;
				
                return allLanguagesNames;
            }
            set{ allLanguagesNames = value;}
        }
		
        private static string currentLanguage;
        public static string CurrentLanguage
        {
            get
            {
                if (currentLanguage == null || currentLanguage == "")
                    return "No Language Available!";
					
                return currentLanguage;
            }
            set
            {
                if (currentLanguage != value)
                {
                    currentLanguage = value;				
                    Refresh();
                }
				
            }
        }
			
        public static void Refresh()
        {
            #if UNITY_EDITOR
            AllLanguagesNames = GetAllAvailableLanguages();	
            if (currentLanguage == null || currentLanguage == "")
                currentLanguage = "de";
			
            locaData = LocaData.LoadFromEditor(currentLanguage + ".xml");
            #endif
        }
#if UNITY_EDITOR
        public static string[] GetAllAvailableLanguages()
        {
            string path = Application.dataPath + "/Resources/loca/";
            string[] result = new string[0];
            try
            {
                result = Directory.GetFiles(path, "*.xml");//.Select(name => GetFileShortName(path)).ToArray();
            } catch
            {
                Debug.Log("No LocaFiles available");
                return null;
            }
		
            return AbstractFileNames(result);
        }
	
        private static string[] AbstractFileNames(string[] _files)
        {
            List<string> result = new List<string>();
            foreach (string file in _files)
            {
                result.Add(GetFileShortName(file));
            }
            return result.ToArray();
        }
	
        private static string GetFileShortName(string _path)
        {
            string result = Path.GetFileName(_path);
            result = result.Remove(0, 5);
            result = result.Replace(".xml", "");
            return result;
        }
#endif
        public static string GetText(string _key)
        {
            if (currentLanguage == "")
                return "no language Selected";
				
            if (locaData == null)
                Refresh();
				
            return 	locaData.GetText(_key);
        }
		
        public static string[] GetKeys()
        {
            if (locaData == null)
                LocalizationHelper.Refresh();
                                        
            return locaData.GetKeys();
        }
		
        public static string[] GetMatchingKeys(string key, string[] allKeys)
        {
            List<string> result = new List<string>();
            foreach (string str in allKeys)
            {
                if (str.StartsWith(key))
                {
                    result.Add(str);
                }
            }
            return result.ToArray();
        }
    }
}
