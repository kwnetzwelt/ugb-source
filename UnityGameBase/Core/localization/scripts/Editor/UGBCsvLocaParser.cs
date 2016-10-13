using System;
using System.Collections.Generic;
using System.Xml;
using System.IO;

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


		IEnumerator<float> CSVWalker(string filename, System.Text.Encoding encoding)
		{
			List<List<string>> dataGrid = Mono.Csv.CsvFileReader.ReadAll(filename, System.Text.Encoding.UTF8, ';');
			var firstContentCell = ParseHeaderRow(dataGrid[0].ToArray(),filename);
			int progress = 0;
			foreach(var line in dataGrid)
			{
				
				if (line[0].StartsWith("//") || string.IsNullOrEmpty(line[firstContentCell]))
				{
					// this is a commented out line, we do nothing here
				}else
				{
					var locaEntry = new CLocaEntry(mLangCount);
					locaEntry.mKey = line[firstContentCell];
					locaEntry.mDescription = line[firstContentCell + 1];
					for(int i = firstContentCell + 2, j = 0; j<mLangCount; i++, j++)
					{
						locaEntry.mTranslations[j] = line[i];
					}
					mLocaEntries.Add(locaEntry);
					progress++;
				}
				yield return progress / (float)dataGrid.Count;
			}
		}

		public void Parse(string pFilePath)
		{
        	Clear();
			
			
			//using (CsvReader csvReader = new CsvReader(new StreamReader(pFilePath), true))
			{
                try
                {
					var walker = CSVWalker(pFilePath, System.Text.Encoding.UTF8);
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
				UnityEngine.Debug.LogError(header.Length);
				throw new DataMisalignedException(string.Format("CSV at {0} is not a valid Loca file.", pFilePath));
			}
			for(int i = firstContentCell + 2; i < header.Length; i++)
			{
				if(!string.IsNullOrEmpty(header[i]))
					mLanguages.Add(header[i]);
			}
			mLangCount = mLanguages.Count;

			return firstContentCell;
		}

			
	}
}