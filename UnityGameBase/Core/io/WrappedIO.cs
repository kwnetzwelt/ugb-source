using System;
using System.IO;

#if !UNITY_METRO || UNITY_EDITOR

namespace UnityGameBase.Core.IO
{
	/// <summary>
	/// A IO operation wrapper which allows for async io to operate while this class remains persistant. 
	/// </summary>
	public class WrappedIO
	{
		private string mPath;
		private string mContent;
		
		private Exception mException;

        public bool FileExists { get; set; }

		/// <summary>
		/// Returns true if any async operation is finished. 
		/// </summary>
		/// <value><c>true</c> if this instance is done; otherwise, <c>false</c>.</value>
		public bool IsDone
		{
			get;
			private set;
		}

		/// <summary>
		/// Don't use a constructor. Use Storage to implicitly create instances instead!
		/// </summary>
		/// <param name="pPath">P path.</param>
		public WrappedIO(string pPath)
		{
			this.mPath = pPath;
			this.IsDone = false;
		}
		/// <summary>
		/// Don't use a constructor. Use Storage to implicitly create instances instead!
		/// </summary>
		/// <param name="pPath">P path.</param>
		/// <param name="pContent">P content.</param>
        public WrappedIO(string pPath, string pContent)
        {
            this.mPath = pPath;
            this.mContent = pContent;
            this.IsDone = false;
        }
		
		
		internal void Write()
		{
			WriteFile();
		}
		
		internal void Load()
		{
			ReadFile();
		}
		
        internal void Exists()
        {
            System.IO.FileInfo fi = new FileInfo(mPath);
            FileExists = fi.Exists;
            IsDone = true;
        }
		internal void Delete()
		{
			#if !UNITY_WEBPLAYER
			System.IO.FileInfo fi = new FileInfo(mPath);
			try
			{
				fi.Delete ();
			}catch(Exception e)
			{
				mException = e;
			}
			IsDone = true;
			#else
			throw new MissingMethodException("FileInfo doesn't contain the delete method in WebplayerBuild.");
			#endif
		}
		void ReadFile()
		{
			
            //var folder = Storage.GetFolderFromConfiguration();
			try
			{
                using (System.IO.StreamReader reader = new StreamReader(mPath))
                {
                    mContent = reader.ReadToEnd();
                }

			}
			catch (Exception e)
			{
				mException = e;
			}
			
			IsDone = true;
			
			
		}
		
		void WriteFile()
		{
			//var folder = Storage.GetFolderFromConfiguration();
			try
			{
                using (System.IO.StreamWriter writer = new StreamWriter(mPath))
                {
                    writer.Write(mContent);
                }

			}
			catch (Exception e)
			{
				mException = e;
			}
			IsDone = true;
		}

		/// <summary>
		/// The content of the file, if any is read. 
		/// </summary>
		/// <exception cref="System::Exception">Thrown if an exception ocurred during file access. </exception>
		/// <returns>The content as a string.</returns>
		public string GetContent()
		{
			if (mException != null)
				throw mException;
			return mContent;
		}

	}
}

#endif