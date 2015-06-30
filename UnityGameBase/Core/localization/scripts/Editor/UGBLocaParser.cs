using System;
using System.Collections.Generic;

namespace UnityGameBase.Core.Localization
{
	public class CLocaEntry
	{
		public string mKey;
		public string mDescription;
		public string[] mTranslations;

		public CLocaEntry(int pLanguageCount)
		{
			mTranslations = new string[pLanguageCount];
		}
	}


	public interface UGBLocaParser
	{	
		void Clear();
		
		List<string> GetLanguages();
		
		List<CLocaEntry> GetEntries();
		
		void Parse(string pFilePath);
	}
}