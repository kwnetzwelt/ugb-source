using UnityEngine;
using System.Collections;
using UnityEditor;
using UGB.Data;

namespace UGB.Localization
{
	public class UGBLocaPostProcessor : AssetPostprocessor 
	{
		
		const string kLocaPath = "Resources/loca/";

		static void OnPostprocessAllAssets( string[] pImportedAssets
			,string[] pDeletedAssets
			,string[] pMovedAssets
			,string[] pMovedFromAssetsPaths)
		{
			foreach( string imported in pImportedAssets)
			{
				if(imported.EndsWith(".xml") && imported.Contains("loca"))
					ImportLocaFile(imported);
			}
		}
		
		
		
		static void ImportLocaFile(string pPath)
		{
			UGBLocaParser parser = new UGBLocaParser();
			parser.Parse(pPath);
			
			
			// now that loca is parsed, generating loca files. 
			var languages = parser.GetLanguages();
			
			for(int i = 0;i<languages.Count;i++)
			{
				// Load Loca for language
				LocaData ld = LocaData.Load(languages[i]);
				
				// Add all parsed keys to the loca file
				foreach(UGBLocaParser.CLocaEntry e in parser.GetEntries())
				{
					if(i > 0 && i < e.mTranslations.Length && e.mTranslations[i] != null)
						ld.AddText(e.mKey,e.mTranslations[i]);
				}
				
				// Save Loca file
				ld.Save();
			}
			
			AssetDatabase.Refresh();
		}
	}
}