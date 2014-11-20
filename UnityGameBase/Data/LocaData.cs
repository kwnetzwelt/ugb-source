using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;
using System.IO;
using System;
using System.Text;
using UGB.Utils;
using UGB.Globalization;


namespace UGB.Data
{
	
	/// <summary>
	/// Loca data. Contains loading code for localization files. Allows access to translated values. 
	/// </summary>
	public class LocaData
	{
		
		private LocaData()
		{
			
		}
		
		XmlLocaData mXmlData;
		
		public string GetText(Languages pLang,string pKey)
		{
			if(mXmlData.mData.ContainsKey(pKey))
				return mXmlData.mData[pKey];
			
			return "KNF:" + pKey;
		}
	#if UNITY_EDITOR
		public void AddText (string pKey, string pText)
		{
			mXmlData.mData[pKey] = pText;
		}
	#endif
		
		public static LocaData Load()
		{
			if(Application.isPlaying)
				return Load(Game.Instance.gameLoca.currentLanguage.ToString());
			
			return null;
		}
		public static LocaData Load(string pLanguageShort)
		{
			
			LocaData lData = new LocaData();
			
			try
			{
				TextAsset ta = Resources.Load( "loca/loca_" + pLanguageShort ) as TextAsset;
				MemoryStream ms = new MemoryStream(ta.bytes);
				
				XmlSerializer s = new XmlSerializer(typeof(XmlLocaData));
				XmlLocaData data = s.Deserialize(ms) as XmlLocaData;
				
				lData.mXmlData = data;
				
				lData.mXmlData.PostRead();
				
			}
			catch(Exception e)
			{
				Debug.LogWarning("Error loading loca file for requested language. " + e.Message);
				lData.mXmlData = new XmlLocaData();
				lData.mXmlData.mLanguage = pLanguageShort;
				
			}
			
			
			
			return lData;
		}
	#if UNITY_EDITOR
		public void Save()
		{
			if(Application.isPlaying)
			{
				Debug.LogError("Not available at runtime!");
				return;
			}
			
			mXmlData.PreWrite();
			string path = "Assets/Resources/loca/"; 
			
			UGBHelpers.MakeSureFolderExists(path);
			
			path += "loca_" + mXmlData.mLanguage + ".xml";
			
			Debug.Log("Writing Loca file to : " + path);
			
			XmlSerializer serializer = new XmlSerializer(typeof(XmlLocaData));
			
			
			TextWriter writer = new StreamWriter(path);
			serializer.Serialize(writer,mXmlData);
			
			writer.Flush();
			writer.Close();
			
		}
	#endif
		
		
	}

}