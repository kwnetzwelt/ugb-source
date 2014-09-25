using System;

namespace UGB.Savegame
{
	public interface ISavegame
	{
		Metadata Metadata
		{
			get;
		}

		string Serialize();
		void Deserialize(string pSavegame);
		void UpdateMetadata();
		void UpdateMetadata(Metadata pData);
		void Initialize (string pData, Metadata pMetadata);
	}
}

