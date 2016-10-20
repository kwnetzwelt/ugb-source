using System;
using UnityEngine;
using UnityGameBase.Core.IO;

namespace UnityGameBase.Core.Savegame
{
    [ExecuteInEditMode]
    public class GameSaveGame : MonoBehaviour
    {
        protected virtual string GetSaveGamePath(int index)
        {
            return Application.persistentDataPath + System.IO.Path.DirectorySeparatorChar + index.ToString() + ".sav";
        }
        public enum SaveGameIOState
        {
            ready,
            saving,
            loading
        }
        public enum SaveGameSerializationResult {
            ok, 
            failedSerializing,
            ioError
        }

        public SaveGameIOState State
        {
            get;
            private set;
        }

        IOWorker currentIOWorker;
        #if !UNITY_WEBGL
        System.Threading.Thread currentThread;
        #endif

        #if UNITY_WEBGL
        SaveGameSerializationResult SaveInLocalStorage(string path, string data)
        {
            return SaveGameSerializationResult.ok;
        }

        private SaveGameSerializationResult LoadFromLocalStorage(string path, ref string data)
        {
            data = "";
            return SaveGameSerializationResult.ok;
        }

        #endif
        public virtual void Save(int index, System.Object saveData, System.Action<SaveGameSerializationResult> onDone)
        {
            if(State != SaveGameIOState.ready)
            {
                throw new System.InvalidOperationException("SaveGameSystem is not ready. ");
            }
            State = SaveGameIOState.saving;
            
            string data = "";
            var path = GetSaveGamePath(index);
            try
            {
                
                data = UnityEngine.JsonUtility.ToJson(saveData);
            }catch(Exception ex)
            {
                Debug.LogException(ex);
                onDone(SaveGameSerializationResult.failedSerializing);
                return;
            }

            #if UNITY_WEBGL
            var result = SaveInLocalStorage(path, data);
            onDone(result);
            #else

            currentIOWorker = new IOWorker();
            currentIOWorker.ioData = data;
            currentIOWorker.ioPath = path;
            currentIOWorker.onDone = (result) => {
                State = SaveGameIOState.ready;
                currentIOWorker = null;
                currentThread = null;
                onDone(result);
            };
            currentThread = new System.Threading.Thread( currentIOWorker.SaveData ); 
            currentThread.Start();            
            #endif
        }

        public virtual void Load<T>(int index, System.Action<SaveGameSerializationResult, T>onDone) where T : class
        {
            if(State != SaveGameIOState.ready)
            {
                throw new System.InvalidOperationException("SaveGameSystem is not ready. ");
            }
            State = SaveGameIOState.loading;
            string data = "";
            var path = GetSaveGamePath(index);
            
            #if UNITY_WEBGL
            
            var result = LoadFromLocalStorage(path, ref data);
            T parsedData = null;

            try
            {
                parsedData = UnityEngine.JsonUtility.FromJson<T>(data);
            }catch(Exception e)
            {
                Debug.LogException(e);
                result = SaveGameSerializationResult.failedSerializing;
            }
            
            onDone(result, parsedData);

            #else

            currentIOWorker = new IOWorker();
            currentIOWorker.ioData = data;
            currentIOWorker.ioPath = path;
            T loadedObject = null;
            currentIOWorker.onDone = (result) => {
                State = SaveGameIOState.ready;
                try
                {
                    loadedObject = currentIOWorker.Parse<T>();
                }catch(Exception e)
                {
                    UnityEngine.Debug.LogException(e);
                    result = SaveGameSerializationResult.failedSerializing;
                }
                currentIOWorker = null;
                currentThread = null;
                onDone(result, loadedObject);
            };
            currentThread = new System.Threading.Thread( currentIOWorker.LoadData ); 
            currentThread.Start();

            #endif
        }


        void OnDisable()
        {
            #if !UNITY_WEBGL
            if(currentThread != null && currentThread.IsAlive)
            {
                currentThread.Abort();
            }
            #endif
        }
        internal class IOWorker
        {
            public string ioData;
            public string ioPath;

            string loadedData = null;

            public System.Action<SaveGameSerializationResult> onDone;
            public void SaveData()
            {
                var io = new WrappedIO(ioPath, ioData);
                SaveGameSerializationResult result = SaveGameSerializationResult.ok;
                try
                {
                    io.Write();
                    
                    while(!io.IsDone)
                    {
                        System.Threading.Thread.Sleep(10);
                    }
                }catch(Exception e)
                {
                    Debug.LogException(e);
                    result = SaveGameSerializationResult.ioError;
                }

                onDone(result);
            }

            public void LoadData()
            {
                var io = new WrappedIO(ioPath, ioData);
                SaveGameSerializationResult result = SaveGameSerializationResult.ok;
                try
                {
                    io.Load();
                    
                    while(!io.IsDone)
                    {
                        UnityEngine.Debug.Log("wait");
                        System.Threading.Thread.Sleep(10);
                    }
                    
                    loadedData = io.GetContent();

                }catch(Exception e)
                {
                    Debug.LogException(e);
                    result = SaveGameSerializationResult.ioError;
                }

                onDone(result);
            }

            public T Parse<T> () where T : class
            {
                return UnityEngine.JsonUtility.FromJson<T>(loadedData);
            }
        } 
    }

}