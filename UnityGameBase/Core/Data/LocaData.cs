using System.Xml.Serialization;
using UnityEngine;
using System.IO;
using System;
using System.Text;
using System.Xml;

namespace UnityGameBase.Core.Data
{

    /// <summary>
    /// Loca data. Contains loading code for localization files. Allows access to translated values. 
    /// </summary>
    public class LocaData
    {
        public static string targetFolder = Application.dataPath + "/Resources/loca"; 
        static string editorPath = targetFolder + "/loca_{0}.xml";
        static string resourcesPath = "loca/loca_{0}";
        XmlLocaData xmlData = new XmlLocaData();

        /// <summary>
        /// Factory Method to create a new instance of LocaData
        /// </summary>
        /// <param name="languageShort">Language short.</param>
        public static LocaData Create(string languageShort)
        {
            var lData = new LocaData();
            lData.xmlData = new XmlLocaData();
            lData.xmlData.language = languageShort;
            return lData;
        }

        /// <summary>
        /// Do not use directly! Use Factory Method instead
        /// </summary>
        private LocaData()
        {
        }

        public string GetText(string key)
        {
            if (String.IsNullOrEmpty(key))
            {
                return "Empty Key!";
            }

            if (xmlData.data.ContainsKey(key))
            {
                return xmlData.data[key] ?? "Null Value!";
            }

            return "KNF:" + key;
        }

        public string[] GetKeys()
        {
            string[] keys = new string[xmlData.data.Keys.Count];
            xmlData.data.Keys.CopyTo(keys, 0);
            return keys;
        }

        public static LocaData Load()
        {
            if (Application.isPlaying)
            {
                return Load(Game.Instance.gameLoca.currentLanguage.ToString());
            }
            return null;
        }

        public static LocaData Load(string languageShort)
        {
            LocaData lData = LocaData.Create(languageShort);
            string path = string.Format(resourcesPath, languageShort);
            try
            {
                // iOS related crash with connected Xcode (EXC_BAD_ACCESS) - check for file 'exists'
                TextAsset ta = Resources.Load<TextAsset>(path);
                if (ta == null)
                {
                    throw new FileNotFoundException("File not found: " + path);
                }

                MemoryStream ms = new MemoryStream(ta.bytes);

                XmlSerializer s = new XmlSerializer(typeof(XmlLocaData));
                XmlLocaData data = s.Deserialize(ms) as XmlLocaData;

                lData.xmlData = data;

                lData.xmlData.PostRead();

            }
            catch (Exception e)
            {
                Debug.LogWarning("Error loading loca file for requested language. " + e.Message);

            }
            return lData;
        }

        #region Editor

        #if UNITY_EDITOR

        public void AddText(string key, string value)
        {
            xmlData.data[key] = value;
        }

        public static LocaData LoadFromEditor(string languageShort)
        {
            LocaData lData = LocaData.Create(languageShort);
            string path = string.Format(editorPath, languageShort);

            try
            {
                FileInfo file = new FileInfo(path);
                if (!file.Exists)
                {
                    Debug.LogWarning("Localization: File not found: " + file.FullName);
                    return lData;
                }

                XmlReaderSettings settings = new XmlReaderSettings();
                settings.CloseInput = true;     // Critical setting due to path violation on OSX system
                using (XmlReader reader = XmlReader.Create(path, settings))
                {
                    XmlSerializer s = new XmlSerializer(typeof(XmlLocaData));
                    XmlLocaData data = s.Deserialize(reader) as XmlLocaData;
                    reader.Close();
                    lData.xmlData = data;
                    lData.xmlData.PostRead();
                }
            }
            catch (Exception e)
            {
                Debug.LogWarning("Error loading loca file for requested language. " + e.Message);
                lData.xmlData = new XmlLocaData();
                lData.xmlData.language = languageShort;
            }

            return lData;
        }

        public void Save()
        {
            if (Application.isPlaying)
            {
                Debug.LogError("Not available at runtime!");
                return;
            }

            xmlData.PreWrite();

            string path = string.Format(editorPath, xmlData.language);
            FileInfo fileInfo = new FileInfo(path);
            if (!fileInfo.Directory.Exists)
            {
                fileInfo.Directory.Create();
            }
            
            Debug.Log("Writing Loca file to : " + path);
            
            XmlSerializer serializer = new XmlSerializer(typeof(XmlLocaData));
            
            try
            {
                XmlWriterSettings settings = new System.Xml.XmlWriterSettings();
                settings.CloseOutput = true;
                settings.Encoding = Encoding.UTF8;
                settings.OmitXmlDeclaration = true;
                settings.Indent = true;
                settings.NewLineOnAttributes = true;
                using (XmlWriter writer = XmlWriter.Create(File.CreateText(path), settings))
                {
                    serializer.Serialize(writer, xmlData);
                    writer.Close();
                }
            }
            catch (IOException e)
            {
                Debug.LogException(e);
            }

            UnityEditor.AssetDatabase.Refresh();
        }
        #endif
        #endregion
    }
}