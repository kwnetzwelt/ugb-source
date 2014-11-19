using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.Xml;

public class XmlLocaData
{
	public XmlLocaData ()
	{
	}
	
	public class XmlLocaDataEntry
	{
		public string mKey;
		
		// the following is needed to store the mText Property in a CData Section
		
		[XmlIgnore]
		public string mText;
		
		#if	!UNITY_METRO || UNITY_EDITOR
		public XmlCDataSection mTextXml {get; set;}
		#else		
		public string mTextXml {get; set;}
		#endif
		
		
		public void PreWrite()
		{
			#if !UNITY_METRO || UNITY_EDITOR
			mTextXml = new XmlDocument().CreateCDataSection(mText);
			#endif
		}
		public void PostRead()
		{
			#if	!UNITY_METRO || UNITY_EDITOR
			mText = mTextXml.Value;
			#else
			mText = mTextXml;
			#endif
		}
	}
	
	public string mLanguage = "";
	
	[XmlIgnore]
	public Dictionary<string,string>mData = new Dictionary<string, string>();
	
	public XmlLocaDataEntry[] mDataBuffer;
	
	/// <summary>
	/// Method executed prio to write xml serialization. Copies all entries from the dictionary to the buffer
	/// </summary>
	public void PreWrite()
	{
		int i = 0;
		
		mDataBuffer = new XmlLocaDataEntry[mData.Count];
		
		foreach(KeyValuePair<string,string>e in mData)
		{
			XmlLocaDataEntry xmle = new XmlLocaDataEntry();
			xmle.mKey = e.Key;
			xmle.mText = e.Value;
			xmle.PreWrite();
			mDataBuffer[i] = xmle;
			i++;
		}
	}
	/// <summary>
	/// Method executed after the instance is read from xml. Copies buffer entries to the dictionary
	/// </summary>
	public void PostRead()
	{
		
		mData = new Dictionary<string, string>();
		
		foreach(XmlLocaDataEntry e in mDataBuffer)
		{
			e.PostRead();	
			mData[e.mKey] = e.mText;
		}
		mDataBuffer = null;
	}
	
	
}

