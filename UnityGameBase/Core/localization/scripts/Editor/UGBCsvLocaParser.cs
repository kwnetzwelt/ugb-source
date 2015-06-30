using System;
using System.Collections.Generic;
using System.Xml;
using System.IO;
using LumenWorks.Framework.IO.Csv;

namespace UnityGameBase.Core.Localization
{
	/// <summary>
	/// Parses CSV files for loca entries. 
	/// </summary>
	public class UGBCsvLocaParser : UGBLocaParser
	{
		const string kHeaderRowFirstCellContent = "textkey";

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

		public void Parse(string pFilePath)
		{
			Clear();
			using (CsvReader csvReader = new CsvReader(new StreamReader(pFilePath), true))
			{
				var firstContentCell = ParseHeaderRow(csvReader.GetFieldHeaders(), pFilePath);
				while(csvReader.ReadNextRecord()) 
				{
					if (csvReader[0].StartsWith("//") || string.IsNullOrEmpty(csvReader[firstContentCell]))
					{
						continue;
					}

					var locaEntry = new CLocaEntry(mLangCount);
					locaEntry.mKey = csvReader[firstContentCell];
					locaEntry.mDescription = csvReader[firstContentCell + 1];
					for(int i = firstContentCell + 2, j = 0; i < csvReader.FieldCount; i++, j++)
					{
						locaEntry.mTranslations[j] = csvReader[i];
					}
					mLocaEntries.Add(locaEntry);
				}
			}
		}
		
		int ParseHeaderRow(string[] header, string pFilePath)
		{
			var firstContentCell = -1;
			for(int i = 0; i < header.Length; i++) 
			{
				if (header[i] == kHeaderRowFirstCellContent) 
				{
					firstContentCell = i;
					break;
				}
			}
			if(firstContentCell == -1) 
			{
				throw new DataMisalignedException(string.Format("CSV at {0} is not a valid Loca file.", pFilePath));
			}
			for(int i = firstContentCell + 2; i < header.Length; i++)
			{
				mLanguages.Add(header[i]);
			}
			mLangCount = mLanguages.Count;

			return firstContentCell;
		}

			
	}
}