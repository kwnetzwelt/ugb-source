using System;
using System.Collections.Generic;

namespace UnityGameBase.Core.Savegame
{
	public interface IIOProvider
	{
		void Read<T>(int ID, Action<SavegameData> OnComplete, Action<Exception, int> OnError) where T : ISavegame;
		void Write<T>(SavegameData pData, Action OnComplete, Action<Exception, int> OnError) where T : ISavegame;
		void Remove<T>(int ID, Action OnComplete, Action<Exception, int> OnError) where T : ISavegame;
		void ReadMetadata<T>(Action<List<Metadata>> OnComplete, Action<Exception, int> OnError) where T : ISavegame;
	}
}

