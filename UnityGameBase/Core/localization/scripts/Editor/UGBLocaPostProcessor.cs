using UnityEditor;
using UnityGameBase.Core.Data;
using System.Collections.Generic;

namespace UnityGameBase.Core.Localization
{
    public class UGBLocaPostProcessor : AssetPostprocessor
    {
        public static bool ProcessLoca { get; set; }

        static void OnPostprocessAllAssets(
            string[] pImportedAssets,
            string[] pDeletedAssets,
            string[] pMovedAssets,
            string[] pMovedFromAssetsPaths)
        {
            // only do this if triggered from outside - Jenkins has IOExceptions on creating the XML's.
            // now to something completely different:
            if (ProcessLoca)
            {
                CreateLoca(pImportedAssets);
            }

            ProcessLoca = false;
        }

        public static void CreateLoca(params string[] pImportedAssets)
        {
            List<string> locaAssets = new List<string>(AssetDatabase.FindAssets(UnityGameBase.Core.Globalization.GameLocalization.UGBLocaSourceFilter));
            
            // no files containing loca found. 
            if (locaAssets.Count == 0)
                return;

            // assets are only the guids. So we need to parse the path to compare them. 
            List<string> locaSources = new List<string>();
            foreach (var assetGuid in locaAssets)
            {
                locaSources.Add(AssetDatabase.GUIDToAssetPath(assetGuid));
            }

            foreach (string imported in pImportedAssets)
            {
                if (!locaSources.Contains(imported))
                    continue;
                
                UGBLocaParser parser = null;
                if (imported.EndsWith(".xml"))
                {
                    parser = new UGBXmlLocaParser();
                    parser.Parse(imported);
                }
                else if (imported.EndsWith(".csv"))
                {
                    parser = new UGBCsvLocaParser();
                    parser.Parse(imported);
                }
                else if(imported.EndsWith(".json"))
                {
                    parser = new UGBJSONLocaParser();
                    parser.Parse(imported);
                }
                else
                {
                    continue;
                }

                // now that loca is parsed, generating loca files. 
                var languages = parser.GetLanguages();
                float progress = 0;
                float max = languages.Count * parser.GetEntries().Count;

                for (int i = 0; i < languages.Count; i++)
                {
                    // Load Loca for language
                    LocaData ld = LocaData.LoadFromEditor(languages[i]);
                    
                    // Add all parsed keys to the loca file
                    foreach (CLocaEntry e in parser.GetEntries())
                    {
                        if (i < e.mTranslations.Length)
                            ld.AddText(e.mKey, e.mTranslations[i]);
                        progress++;
                        
                    }
                    EditorUtility.DisplayProgressBar("Syncing Loca " + imported, (progress / max).ToString("p1"), progress / max);
                    // Save Loca file
                    ld.Save();
                }
                EditorUtility.ClearProgressBar();
            }
        }
    }
}