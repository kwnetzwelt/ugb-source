using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using System.Xml;
using UnityGameBase.Core.IO;

namespace UnityGameBase.Core.Savegame
{
#if !UNITY_METRO
	public class XMLProvider : IIOProvider
	{
		static string mPath = UnityEngine.Application.persistentDataPath + "/saveGames/";
		static string mFilenameTemplate = "saveGame_{0}.xml";
		static string mMetadataFilename = "metadata.xml";

		[XmlArrayAttribute("SaveGames")]
		List<Metadata> mMetadataList;

		public XMLProvider()
		{
			mMetadataList = new List<Metadata>();
		}

		#region IIOProvider implementation

		public virtual void Read<T> (int ID, Action<SavegameData> OnComplete, Action<Exception, int> OnError) where T : ISavegame
		{
			string source = mPath + mFilenameTemplate;
			source = string.Format(source, ID);
			if(File.Exists(source))
			{
				try
				{
					ReadSavegame(source, ID, OnComplete);
				}
				catch(XmlException e)
				{
					if(OnError != null)
						OnError(e, ID);

					UnityEngine.Debug.LogError("Your XML was probably bad :" + source);
				}
			} else
			{
				if(OnError != null)
					OnError(new FileNotFoundException(source), ID);
			}

		}

		public virtual void Write<T> (SavegameData pData, Action OnComplete, Action<Exception, int> OnError) where T : ISavegame
		{
			if(!Directory.Exists(mPath))
				Directory.CreateDirectory(mPath);

			UpdateMetadataList(pData.Metadata);

			WriteMetadata<T>(mPath + mMetadataFilename, null);

			string target = mPath + mFilenameTemplate;
			target = string.Format(target.ToString(), pData.Metadata.Id);

			WriteSavegame(target, pData.Savegame, OnComplete);
		}

		public void Remove<T> (int ID, Action OnComplete, Action<Exception, int> OnError) where T : ISavegame
		{
			if(RemoveMetadata(ID))
			{
				// update meta file
				WriteMetadata<T>(mPath + mMetadataFilename, null);
			}

			string target = mPath + mFilenameTemplate;
			target = string.Format(target.ToString(), ID);

			try
			{
				File.Delete(target);
			} catch (Exception e)
			{
				if(OnError != null)
					OnError(e, ID);
			}

			if(!File.Exists(target))
			{
				if(OnComplete != null)
					OnComplete();
			}
		}

		public virtual void ReadMetadata<T> (Action<List<Metadata>> OnComplete, Action<Exception, int> OnError) where T : ISavegame
		{
			string source = mPath + mMetadataFilename;
			if(File.Exists(source))
				ReadMetadata<T>(source, OnComplete);
			else
				WriteMetadata<T>(source, d => OnComplete(mMetadataList));
		}

		#endregion

		void WriteSavegame(string pTarget, string pData, Action OnComplete)
		{
			var writer = new StreamWriter(pTarget);
			writer.Write(pData);
			writer.Flush();
			writer.Close();

			if(OnComplete != null)
				OnComplete();
		}

		void WriteMetadata<T>(string pTarget, Action<T> OnComplete) where T : ISavegame
		{
			var writer = new StreamWriter(pTarget);
			writer.Write(XmlObjectSerializer.ObjectToString(mMetadataList));
			writer.Flush();
			writer.Close();
			
			if(OnComplete != null)
				OnComplete(default(T));
		}

		void ReadSavegame(string pSource, int ID, Action<SavegameData> OnComplete)
		{
			var reader = new StreamReader(pSource);
			string result = reader.ReadToEnd();
			reader.Close();

			Metadata metadata = GetSavegameMetadata(ID);
			if(metadata == null)
				throw new KeyNotFoundException("Metadata not found for ID: " + ID);

			SavegameData sgData = new SavegameData(metadata, result);

			if(OnComplete != null)
				OnComplete(sgData);
		}

		void ReadMetadata<T>(string pSource, Action<List<Metadata>> OnComplete) where T : ISavegame
		{
			var reader = new StreamReader(pSource);
			string result = reader.ReadToEnd();
			reader.Close();

			mMetadataList = XmlObjectSerializer.StringToType<List<Metadata>>(result) as List<Metadata>;
			
			if(OnComplete != null)
				OnComplete(mMetadataList);
		}

		bool RemoveMetadata (int ID)
		{
			Metadata result = GetSavegameMetadata(ID);
			if(result != null)
				mMetadataList.Remove(result);

			return (result != null);
		}

		void UpdateMetadataList(Metadata data)
		{
			Metadata result = GetSavegameMetadata(data.Id);

			if(result == null)
				mMetadataList.Add(data);
			else
				result = new Metadata(data);
		}

		Metadata GetSavegameMetadata(int ID)
		{
			Metadata result = mMetadataList.Find( (m) => {
				return (m.Id == ID);
			});

			return result;
		}
	}
#endif
}

