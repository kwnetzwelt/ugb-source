using UnityEngine;
using System.Collections;
using UnityEditor;
using System.IO;


namespace UGBSetup
{
	internal class CreateDefaultSceneStep : UGBSetupStep
	{
		const string kScene = "Assets/scenes/default.unity";
		public const string kGameRoot = "_GameRoot";

		public override string GetName ()
		{
			return "Create default scene";
		}

		public override IEnumerator Run ()
		{
			if(!force)
			{
				FileInfo fi = new FileInfo( kScene );
				if(fi.Exists)
				{
					Debug.Log("Default scene exists. Skipping. ");
					yield break;
				}
			}

			if(EditorApplication.isUpdating) {
				Debug.Log("Wait");
				yield return 0;
			}
			EditorApplication.NewScene();

			ClearScene();

			GameObject go = new GameObject();
			go.name = kGameRoot;
			go.AddComponent<Game>();

			EditorApplication.SaveScene( kScene );
		}

		void ClearScene()
		{
			var gos = GameObject.FindObjectsOfType<GameObject>();
			foreach(var go in gos)
			{
				if(go != null)
					GameObject.DestroyImmediate(go);
			}

		}

	}

}