using System;
using System.Collections.Generic;

namespace UnityGameBase.Core.Savegame
{
	/// <summary>
	/// Provides a list of savegame metadata and IO functionality for their corresponding savegames. 
	/// </summary>
	public class SavegameController<T> where T : ISavegame, new()
	{
		IIOProvider mProvider;
		public bool isBusy { get; private set; }

		private N Create<N>()
		{
			if(OnCreate != null)
				OnCreate();

			return Activator.CreateInstance<N>();
		}

		#region public events
		public delegate void LoadComplete<C>(C t);
		public event LoadComplete<T> OnLoadComplete;

		public delegate void CreateSavegame();
		public event CreateSavegame OnCreate;
		
		public delegate void SaveComplete();
		public event SaveComplete OnSaveComplete;
		
		public delegate void RemoveComplete();
		public event RemoveComplete OnRemoveComplete;
		
		public delegate void MetadataList<L>(List<Metadata> l);
		public event MetadataList<List<Metadata>> OnMetadataList;
		#endregion

		#region public interface
		public SavegameController(IIOProvider pIOProvider)
		{
			isBusy = false;
			mProvider = pIOProvider;
		}
		
		public void Save(T sg)
		{
			if(isBusy)
				return;

			isBusy = true;

			sg.UpdateMetadata();

			mProvider.Write<T>(new SavegameData(sg.Metadata, sg.Serialize()), OnProviderSaved, OnError);
		}

		public void Load(int id)
		{
			if(isBusy)
				return;

			isBusy = true;

			mProvider.Read<T>(id, OnProviderLoaded, OnError);
		}

		public void Remove(int id)
		{
			if(isBusy)
				return;

			isBusy = true;

			mProvider.Remove<T>(id, OnProviderRemoved, OnError); 
		}

		public void List()
		{
			if(isBusy)
				return;
			
			isBusy = true;

			mProvider.ReadMetadata<T>(OnProviderListed, OnError);
		}
		#endregion
		
		#region internal callbacks
		private void OnProviderLoaded(SavegameData savegameData)
		{
			isBusy = false;

			T savegame = new T();
			savegame.Initialize(savegameData.Savegame, savegameData.Metadata);

			if(OnLoadComplete != null)
				OnLoadComplete(savegame);
		}

		private void OnProviderSaved()
		{
			isBusy = false;

			if(OnSaveComplete != null)
				OnSaveComplete();
		}
		
		private void OnProviderRemoved()
		{
			isBusy = false;

			if(OnRemoveComplete != null)
				OnRemoveComplete();

			List();
		}

		private void OnProviderListed(List<Metadata> metdadata)
		{
			isBusy = false;

			if(OnMetadataList != null)
				OnMetadataList(metdadata);
		}

		private void OnError (Exception e, int ID)
		{
			isBusy = false;
			
			if(e.GetType() == typeof(System.IO.FileNotFoundException))
			{
				T savegame = Create<T>();
				savegame.Metadata.Id = ID;
				Save(savegame);
				Load(ID);
			}
			else
				throw e;
		}
		#endregion
	}
}