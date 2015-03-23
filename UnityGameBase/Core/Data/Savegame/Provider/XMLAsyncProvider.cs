using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using System.Xml;
using UnityEngine;

namespace UnityGameBase.Core.Savegame
{
	public class XMLAsyncProvider : MonoBehaviour, IIOProvider
	{
		static string mPath = UnityEngine.Application.persistentDataPath + "/saveGames/";
		static string mFilenameTemplate = "saveGame_{0}.xml";
		static string mMetadataFilename = "metadata.xml";

		[XmlArrayAttribute("SaveGames")]
		List<Metadata> mMetadataList;

		public void Start()
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
					StartCoroutine(ReadSavegameAsync(source, ID, OnComplete));
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

			StartCoroutine(WriteMetadataAsync<T>(mPath + mMetadataFilename, null));

			string target = mPath + mFilenameTemplate;
			target = string.Format(target.ToString(), pData.Metadata.Id);

			StartCoroutine(WriteSavegameAsync(target, pData.Savegame, OnComplete));
		}

		public void Remove<T> (int ID, Action OnComplete, Action<Exception, int> OnError) where T : ISavegame
		{
			if(RemoveMetadata(ID))
			{
				// update meta file
				StartCoroutine(WriteMetadataAsync<T>(mPath + mMetadataFilename, null));
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
				StartCoroutine(ReadMetadataAsync<T>(source, OnComplete));
			else
				StartCoroutine(WriteMetadataAsync<T>(source, d => OnComplete(mMetadataList)));
		}

		#endregion

		IEnumerator WriteSavegameAsync(string pTarget, string pData, Action OnComplete)
		{
			var progress = IO.Storage.Save(pTarget, pData);
			while(!progress.IsDone)
				yield return 0;

			if(OnComplete != null)
				OnComplete();
		}

		IEnumerator WriteMetadataAsync<T>(string pTarget, Action<T> OnComplete) where T : ISavegame
		{
			string content = XmlObjectSerializer.ObjectToString(mMetadataList);
			var progress = IO.Storage.Save(pTarget, content);
			while(!progress.IsDone)
				yield return 0;
			
			if(OnComplete != null)
				OnComplete(default(T));
		}

		IEnumerator ReadSavegameAsync(string pSource, int ID, Action<SavegameData> OnComplete)
		{
			var progress = IO.Storage.Load(pSource);
			
			while(!progress.IsDone)
				yield return true;

			Metadata metadata = GetSavegameMetadata(ID);
			if(metadata == null)
				throw new KeyNotFoundException("Metadata not found for ID: " + ID);

			SavegameData sgData = new SavegameData(metadata, progress.GetContent());

			if(OnComplete != null)
				OnComplete(sgData);
		}

		IEnumerator ReadMetadataAsync<T>(string pSource, Action<List<Metadata>> OnComplete) where T : ISavegame
		{
			var progress = IO.Storage.Load(pSource);
			
			while(!progress.IsDone)
				yield return true;

			mMetadataList = XmlObjectSerializer.StringToType<List<Metadata>>(progress.GetContent()) as List<Metadata>;
			
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
}

