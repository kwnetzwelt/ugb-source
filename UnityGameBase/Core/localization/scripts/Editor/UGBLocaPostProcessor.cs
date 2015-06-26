using UnityEngine;
using System.Collections;
using UnityEditor;
using UnityGameBase.Core.Data;

namespace UnityGameBase.Core.Localization
{
	public class UGBLocaPostProcessor : AssetPostprocessor 
	{
		static void OnPostprocessAllAssets( 
		        string[] pImportedAssets,
				string[] pDeletedAssets,
				string[] pMovedAssets,
				string[] pMovedFromAssetsPaths)
		{
			foreach( string imported in pImportedAssets)
			{
				if(!imported.Contains("loca"))
					continue;

				UGBLocaParser parser = null;
				if(imported.EndsWith(".xml"))
				{
					parser = new UGBXmlLocaParser();
					parser.Parse(imported);
				}
				else if(imported.EndsWith(".csv"))
				{
					parser = new UGBCsvLocaParser();
					parser.Parse(imported);
				}
				else {
					continue;
				}

				// now that loca is parsed, generating loca files. 
				var languages = parser.GetLanguages();
				
				for(int i = 0;i<languages.Count;i++)
				{
					// Load Loca for language
					LocaData ld = LocaData.Load(languages[i]);
					
					// Add all parsed keys to the loca file
					foreach(CLocaEntry e in parser.GetEntries())
					{
						if(i < e.mTranslations.Length)
							ld.AddText(e.mKey, e.mTranslations[i]);
					}
					
					// Save Loca file
					ld.Save();
				}
				
				AssetDatabase.Refresh();
			}
		}

	}
}