using System;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace UnityGameBase.Core.Savegame
{
	public class XmlObjectSerializer
	{
		public static string ObjectToString(Object obj)
		{
			var serializer = new XmlSerializer(obj.GetType());
			var memoryStream = new MemoryStream();
			
			// serialized to utf-8 without BOM
			var streamWriter = new StreamWriter(memoryStream, new System.Text.UTF8Encoding(false));
			
			serializer.Serialize(streamWriter, obj);
			var array = memoryStream.ToArray();
			return System.Text.Encoding.UTF8.GetString( array,0, array.Length );
		}
		
		public static T StringToType<T>(string s) where T : class
		{
			XmlSerializer serializer = new XmlSerializer(typeof(T));
			StringReader sr = new StringReader(s);
			XmlReader reader = XmlReader.Create(sr);
			return serializer.Deserialize(reader) as T;
		}
	}
}