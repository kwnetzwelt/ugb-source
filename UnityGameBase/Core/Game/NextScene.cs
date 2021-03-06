using UnityEngine;
using UnityEngine.SceneManagement;

namespace UnityGameBase.Core
{
	public class NextScene
	{
		protected string name = string.Empty;
		protected int id = -1;

		public bool IsLoadedLevel
		{
			get
			{
                if(name != string.Empty)
                { 
                    return SceneManager.GetActiveScene().name == name;
                }else{
                    return SceneManager.GetActiveScene().buildIndex == id;
                }
			}
		}

		protected bool prepared = true;
		public bool IsPrepared
		{
			get { return prepared; }
		}

		public NextScene(int sceneId)
		{
			id = sceneId;
		}

		public NextScene(string sceneName)
		{
			name = sceneName;
		}

		public virtual void Load()
		{
			if (name != string.Empty)
			{
                SceneManager.LoadScene(name,LoadSceneMode.Single);
			} else
			{
                SceneManager.LoadScene(id,LoadSceneMode.Single);
			}
		}

		virtual public AsyncOperation LoadAsync()
		{
			if (name != string.Empty)
            {
                return SceneManager.LoadSceneAsync(name,LoadSceneMode.Single);
            } else
            {
                return SceneManager.LoadSceneAsync(id,LoadSceneMode.Single);
			}
		}

		public override bool Equals(object obj)
		{
			if (obj == null || GetType() != obj.GetType())
			{
				return false;
			}

			var other = obj as NextScene;
			return (other.id == id && other.name == name);
		}

		public override int GetHashCode()
		{
			return id.GetHashCode() ^ name.GetHashCode();
		}

		public override string ToString()
		{
			return string.Format("[NextScene={0}]", (name != string.Empty) ? name : id.ToString());
		}
	}
}