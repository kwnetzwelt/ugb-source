using System;
using System.Collections.Generic;
using System.Xml;
using System.IO;

namespace UnityGameBase.Core.Localization
{
	/// <summary>
	/// Parses Microsoft Excel XML files for loca entries. 
	/// </summary>
	public class UGBXmlLocaParser : UGBLocaParser
	{
		const string kElementRow = "Row";
		const string kElementCell = "Cell";
		const string kElementData = "Data";
		const string kCommentStart ="//";
		const string kHeaderRowFirstCellContent = "textkey";

		const int kKeyCellIndex = 2;
		const int kDescriptionCellIndex = 3;
		const int kFirstLanguageCellIndex = 4;
		
		int mRowIndex;
		int mCellIndex;
		bool mInData;
		bool mIsInHeaderRow;
		int mLangCount;
		CLocaEntry mCurrentEntry;
		
		XmlReader mXmlReader;
		List<CLocaEntry> mLocaEntries = new List<CLocaEntry>();
		List<string> mLanguages = new List<string>();
		
		
		public void Clear()
		{
			mLocaEntries = new List<CLocaEntry>();
			mRowIndex = 0;
			mCellIndex = 0;
			mLangCount = 0;
			mInData = false;
			mIsInHeaderRow = false;
			mCurrentEntry = null;
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
			
			mXmlReader = XmlReader.Create(pFilePath);
				
			while(mXmlReader.Read())
			{
				switch(mXmlReader.NodeType)
				{
					case XmlNodeType.Element:
						ParseStartElement(); break;
					case XmlNodeType.EndElement:
						ParseEndElement(); break;
					case XmlNodeType.Text:
						ParseText(); break;
				}
			}
		}

		
		bool IsComment(string pValue)
		{
			if(pValue.StartsWith(kCommentStart) )
				return true;
			return false;
		}


		void ParseStartElement()
		{
			switch(mXmlReader.LocalName)
			{
				case kElementRow: 
					ParseStartRow(); break;
				case kElementCell:
					ParseStartCell(); break;
				case kElementData:
					ParseStartData(); break;
			}
		}
		
		
		void ParseStartRow()
		{
			mRowIndex++;
			mCellIndex = 0;
		}


		void ParseStartCell()
		{
			mCellIndex++;
			string customIndex = mXmlReader.GetAttribute("ss:Index");
			if(customIndex != null)
			{
				mCellIndex = System.Convert.ToInt32(customIndex);
			}
		}


		void ParseStartData()
		{
			mInData = true;
		}
		

		void ParseEndElement()
		{
			switch(mXmlReader.Name)
			{
				case kElementRow: 
					ParseEndRow(); break;
				case kElementCell:
					ParseEndCell(); break;
				case kElementData:
					ParseEndData(); break;
			}
		}
		
		void ParseEndRow()
		{
			if(mIsInHeaderRow)
			{
				UnityEngine.Debug.Log("Langs: " + mLangCount);
				mIsInHeaderRow = false;
			}
		}
		
		void ParseEndCell()
		{
		}
		
		void ParseEndData()
		{
			mInData = false;
		}
		
		void ParseHeaderRow()
		{
			mLangCount = System.Math.Max(mLangCount, mCellIndex - kFirstLanguageCellIndex -1);
				
			if(mCellIndex >= kFirstLanguageCellIndex)
			{
				mLanguages.Add(mXmlReader.Value);
			}
		}
		
		void ParseText()
		{
			if(mInData)
			{
				if(IsComment(mXmlReader.Value))
					return;
			
				if(mIsInHeaderRow)
				{
					ParseHeaderRow();
					return;
				}

				if(mCellIndex == kKeyCellIndex)
				{
					if(mLangCount == 0)
					{
						// If the language count is not yet set, test if this is the header row. 
						if(mXmlReader.Value == kHeaderRowFirstCellContent)
						{
							mIsInHeaderRow = true;
							return;
						}
					}	
				}
				
				if(mCellIndex == kKeyCellIndex)
				{
					// create new entry
					mCurrentEntry = new CLocaEntry(mLangCount);
					mCurrentEntry.mKey = mXmlReader.Value;
					mLocaEntries.Add(mCurrentEntry);
				}
				else if(mCellIndex == kDescriptionCellIndex)
				{
					// Handle Description
					if(mCurrentEntry != null)
						mCurrentEntry.mDescription = mXmlReader.Value;
				}
				else
				{
					if(mCurrentEntry != null)
					{
						if((mCellIndex - kFirstLanguageCellIndex) < mLangCount)
							mCurrentEntry.mTranslations[mCellIndex - kFirstLanguageCellIndex] = mXmlReader.Value;
					}
				}
			}
		}
	}
}