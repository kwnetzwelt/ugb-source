using UnityEngine;

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
				return (name != string.Empty) ? Application.loadedLevelName == name : Application.loadedLevel == id;
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
				Application.LoadLevel(name);
			} else
			{
				Application.LoadLevel(id);
			}
		}

		virtual public AsyncOperation LoadAsync()
		{
			if (name != string.Empty)
			{
				return Application.LoadLevelAsync(name);
			} else
			{
				return Application.LoadLevelAsync(id);
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