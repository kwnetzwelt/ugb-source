#if UNITY_METRO && !UNITY_EDITOR

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace UnityGameBase.Core.IO
{
    public class WrappedIO
    {
        private string mPath;
        private string mContent;
        
        private Exception mException;


        public bool FileExists{ get; set; }

        public bool IsDone
        {
            get;
            private set;
        }

        public WrappedIO(string pPath)
        {
            this.mPath = pPath;
            this.IsDone = false;
        }

        public WrappedIO(string pPath, string pContent)
        {
            this.mPath = pPath;
            this.mContent = pContent;
            this.IsDone = false;
        }


        internal void Write()
        {
            WriteFileAsync();
        }

        internal void Load()
        {
            ReadFileAsync();
        }

		internal void Delete()
		{
			DeleteFileAsync();
		}

        internal void Exists()
        {
            ExistsAsync();
        }

        async void ReadFileAsync()
        {
            var folder = Storage.GetFolderFromConfiguration();
            try
            {
                StorageFile file = await folder.GetFileAsync(mPath);
                mContent = await FileIO.ReadTextAsync(file);

            }
            catch (Exception e)
            {
                mException = e;
            }
            
            IsDone = true;
        }

        async void ExistsAsync()
        {
            var folder = Storage.GetFolderFromConfiguration();

            if (await FileExistsAsync(folder, mPath))
            {
                FileExists = true;
            }
            else
            {
                FileExists = false;
            }

            IsDone = true;
        }


        async void WriteFileAsync()
        {
            var folder = Storage.GetFolderFromConfiguration();
            try
            {
                StorageFile file;
                if (await FileExistsAsync(folder, mPath))
                    file = await folder.GetFileAsync(mPath);
                else
                    file = await folder.CreateFileAsync(mPath);
                
                await FileIO.WriteTextAsync(file,mContent);
            }
            catch (Exception e)
            {
                mException = e;
            }
            IsDone = true;
        }

        async Task<bool> FileExistsAsync(StorageFolder pFolder, string pPath)
        {
            try
            {
                await pFolder.GetFileAsync(pPath);
                return true;
            }
            catch
            {
                return false;
            }
        }

		async void DeleteFileAsync()
		{
			var folder = Storage.GetFolderFromConfiguration();
			
			try
			{
				StorageFile file;
				file = await folder.GetFileAsync(mPath);
				
				await file.DeleteAsync();
				
			}
			catch (Exception e)
			{
				mException = e;
			}
			IsDone = true;
		}

        public string GetContent()
        {
            if (mException != null)
                throw mException;
            return mContent;
        }


    }
}
#endif