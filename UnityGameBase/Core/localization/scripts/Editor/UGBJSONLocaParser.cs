using System;
using System.Collections.Generic;
using System.Xml;
using System.IO;
using UnityGameBase.Core.Utils;
using System.Collections;
using UnityEditor;
using UnityEngine;
using UnityGameBase.Core.Globalization;
using System.Linq;

namespace UnityGameBase.Core.Localization
{
	/// <summary>
	/// Parses CSV files for loca entries. 
	/// </summary>
	public class UGBJSONLocaParser : UGBLocaParser
	{
        [Serializable]
        public class LocaJSON
        {
            public List<string> languages = new List<string>();
            public List<TranslationJSON> keys = new List<TranslationJSON>();

            [Serializable]
            public class TranslationJSON
            {
                public string key = "";
                public string description = "";
                public List<string> translations = new List<string>();
                public TranslationJSON()
                {

                }

                public TranslationJSON(string key, params string[] translations)
                {
                    this.key = key;
                    this.translations = new List<string>(translations);
                }
            }
        }

        [MenuItem("UGB/Loca/Create Example JSON")]
        public static void CreateExampleJSonLoca()
        {
            var path = EditorUtility.SaveFilePanelInProject("Create Empty JSON Localization", "loca", "json","Save");
            LocaJSON lj = new LocaJSON();
            lj.languages.Add("en");
            lj.keys.Add(new LocaJSON.TranslationJSON("btn_quit", "Quit") { description = "A Button to quit the game" });
            var content = JsonUtility.ToJson(lj, true);
            File.WriteAllText(path, content);
            AssetDatabase.Refresh();

            var locaAsset = AssetDatabase.LoadAssetAtPath<TextAsset>(path);
            var labels = new List<string>(AssetDatabase.GetLabels(locaAsset));

            if (!labels.Contains(GameLocalization.UGBLocaLabel))
            {
                labels.Add(GameLocalization.UGBLocaLabel);
                AssetDatabase.SetLabels(locaAsset, labels.ToArray());
                AssetDatabase.Refresh();
            }

            AssetDatabase.ImportAsset(path);
        }

        List<CLocaEntry> mLocaEntries = new List<CLocaEntry>();
		List<string> mLanguages = new List<string>();
		int mLangCount;

		public void Clear()
		{
			mLocaEntries = new List<CLocaEntry>();
			mLangCount = 0;
		}

		public List<string> GetLanguages()
		{
			return mLanguages;
		}

		public List<CLocaEntry> GetEntries()
		{
			return mLocaEntries;
		}


		IEnumerator<float> JSONWalker(LocaJSON loca)
		{
			int progress = 0;
            mLangCount = loca.languages.Count;
            mLanguages = loca.languages;
			foreach(var translation in loca.keys)
			{
				
				var locaEntry = new CLocaEntry(mLangCount);
				locaEntry.mKey = translation.key;
				locaEntry.mDescription = translation.description;
                locaEntry.mTranslations = translation.translations.ToArray();
				mLocaEntries.Add(locaEntry);
				progress++;
				
				yield return progress / (float)loca.keys.Count;
			}
		}

		public void Parse(string pFilePath)
		{
        	Clear();
			
			
            try
            {
                var content = File.ReadAllText(pFilePath);
                var lj = JsonUtility.FromJson<LocaJSON>(content);

                var walker = JSONWalker(lj);
				while(walker.MoveNext())
				{
					var canceled = UnityEditor.EditorUtility.DisplayCancelableProgressBar("Importing Loca from " + pFilePath, walker.Current.ToString("p1"), walker.Current);
					if(canceled)
					{
						UnityEngine.Debug.Log("Canceled");
						break;
					}
					//UnityEngine.Debug.Log("Adding key " + walker.Current);
				}
				UnityEditor.EditorUtility.ClearProgressBar();
					
            }catch(Exception e)
            {
                UnityEngine.Debug.LogException(e);
            }
		}	
	}
}