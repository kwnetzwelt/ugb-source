
namespace UnityGameBase.Core.Savegame
{
	public sealed class SavegameData {

		public Metadata Metadata
		{
			get;
			private set;
		}

		public string Savegame
		{
			get;
			private set;
		}

		private SavegameData()
		{

		}

		public SavegameData(Metadata pMetadata, string pSavegame)
		{
			Metadata = pMetadata;
			Savegame = pSavegame;
		}

	}
}