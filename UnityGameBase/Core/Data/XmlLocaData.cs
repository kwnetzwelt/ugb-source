using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.Xml;

public class XmlLocaData
{
    public XmlLocaData()
    {
    }
	
    public class XmlLocaDataEntry
    {
        public string key;
		
        // the following is needed to store the text Property in a CData Section
        [XmlIgnore]
        public string
            text;
		
		#if	!UNITY_METRO || UNITY_EDITOR
        public XmlCDataSection TextXml { get; set; }
		#else		
        public string TextXml {get; set;}
		#endif
		
		
        public void PreWrite()
        {
            #if !UNITY_METRO || UNITY_EDITOR
            TextXml = new XmlDocument().CreateCDataSection(text);
            #endif
        }
        public void PostRead()
        {
            #if	!UNITY_METRO || UNITY_EDITOR
            text = TextXml.Value;
            #else
            text = TextXml;
            #endif
        }
    }
	
    public string language = "";
	
    [XmlIgnore]
    public Dictionary<string,string>
        data = new Dictionary<string, string>();
	
    
    public XmlLocaDataEntry[]
        dataBuffer = new XmlLocaDataEntry[0];
	
    /// <summary>
    /// Method executed prio to write xml serialization. Copies all entries from the dictionary to the buffer
    /// </summary>
    public void PreWrite()
    {
        int i = 0;
		
        dataBuffer = new XmlLocaDataEntry[data.Count];
		
        foreach(KeyValuePair<string,string>e in data)
        {
            XmlLocaDataEntry xmle = new XmlLocaDataEntry();
            xmle.key = e.Key;
            xmle.text = e.Value;
            xmle.PreWrite();
            dataBuffer[i] = xmle;
            i++;
        }
    }
    /// <summary>
    /// Method executed after the instance is read from xml. Copies buffer entries to the dictionary
    /// </summary>
    public void PostRead()
    {
        data = new Dictionary<string, string>();
		
        foreach(XmlLocaDataEntry e in dataBuffer)
        {
            e.PostRead();	
            data[e.key] = e.text;
        }
        dataBuffer = null;
    }	
}
