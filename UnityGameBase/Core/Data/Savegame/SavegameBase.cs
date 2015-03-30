using System;

namespace UnityGameBase.Core.Savegame
{
	public abstract class SavegameBase : ISavegame
	{
		protected SavegameBase ()
		{
			Metadata = new Metadata();
		}

		protected SavegameBase (Metadata pData)
		{
			Metadata = pData;
		}

		public void Initialize (string pData, Metadata pMetadata)
		{
			Metadata = pMetadata;
			Deserialize(pData);
		}

		#region ISavegame implementation

		public abstract string Serialize ();

		public abstract void Deserialize (string pSavegame);

		public virtual void UpdateMetadata ()
		{
			Metadata.Date = DateTime.Now;
		}

		public virtual void UpdateMetadata (Metadata pData)
		{
			Metadata = pData;
		}

		public virtual Metadata Metadata {
			get;
			protected set;
		}

		#endregion
	}
}

