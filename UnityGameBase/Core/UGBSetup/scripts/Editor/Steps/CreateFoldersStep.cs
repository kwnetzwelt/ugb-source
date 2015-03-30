using UnityEngine;
using System.Collections;
using UnityEditor;
using System.IO;

namespace UnityGameBase.Core.Setup
{
	internal class CreateFoldersStep : UGBSetupStep
	{
		public override string GetName ()
		{
			return "Create Folders";
		}
		public override IEnumerator Run ()
		{
			if(force || !HasFolder("Assets", "scenes"))
				CreateFolder("Assets", "scenes");
			else
				Debug.Log("Scenes folder exists. Skipping. ");

			if(force || !HasFolder("Assets", "scripts"))
				CreateFolder("Assets", "scripts");
			else
				Debug.Log("Scripts folder exists. Skipping. ");

			if(force || !HasFolder("Assets", "art"))
				CreateFolder("Assets", "art");
			else
				Debug.Log("Art folder exists. Skipping. ");
			yield return 0;
			
			AssetDatabase.Refresh();

			yield return 0;
		}

		bool HasFolder(params string[] pFolder)
		{
			string folder = string.Join("" + Path.DirectorySeparatorChar, pFolder);

			DirectoryInfo di = new DirectoryInfo(folder);

			return di.Exists;
		}

		void CreateFolder(params string[] pFolder)
		{
			string folder = string.Join("" + Path.DirectorySeparatorChar, pFolder);
			
			DirectoryInfo di = new DirectoryInfo(folder);

			if(di.Exists)
				di.Delete( true );

			di.Create();
		}
	}

}