using System;

namespace UnityGameBase.Core.Savegame
{
	public class Metadata
	{
		public DateTime Date;
		public string 	Name = "";
		public Byte[] 	Screenshot = null;
		public int 		Version = 0;
		public int 		Id = 0;

		public Metadata()
		{
			Date = DateTime.Now;
		}

		public Metadata(Metadata pCopyData)
		{
			Date 		= pCopyData.Date;
			Name 		= pCopyData.Name;
			Screenshot 	= pCopyData.Screenshot;
			Version 	= pCopyData.Version;
			Id 			= pCopyData.Id;
		}
	}
}