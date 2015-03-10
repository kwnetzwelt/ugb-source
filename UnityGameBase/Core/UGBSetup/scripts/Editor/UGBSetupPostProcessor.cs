using UnityEngine;
using System.Collections;
using System;
using System.IO;
using System.Reflection;
using UnityEditor;
using UnityGameBase.Core;

namespace UnityGameBase.Core.Setup
{
	public class UGBSetupPostProcessor : AssetPostprocessor
	{
		private static string className = "GameInitializer";
		private static string classPath = "Assets/Scripts/" + className + ".cs";
		private static string gameClassPath = "UnityGameBase/Game/Game.cs";
        
		static void OnPostprocessAllAssets(string[] imported, string[] deleted, string[] moved, string[] movedFromAssetPath)
		{
			foreach (string asset in imported)
			{
				//copy the GameInitializer if not exit when Game was imported
				if (asset.Contains(gameClassPath))
				{
					Debug.Log("Imported: " + asset + " try to create the GameInitializer.cs");
					CreateGameInitializer();
				}
			}
		}
    
		private static void CreateGameInitializer()
		{            
			System.Type type = GetGameInitializerType();
			if (type != null)
			{
				Debug.Log("Logic Class exists. " + type + " Skipping. ");
				return;
			}          
        
			File.WriteAllText(classPath, kClassContent);        
			AssetDatabase.ImportAsset(classPath);        
		}  

		public static System.Type GetGameInitializerType()
		{
			foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
			{
				foreach (var t in assembly.GetTypes())
				{
					if (t.Name == className)
					{
						return t;
					}
				}
			}
			return null;
		}

		private static string kClassContent = @"using UnityEngine;
using UnityGameBase.Core;


public class GameInitializer : MonoBehaviour
{
    public bool testing = false;

    public void Start ()
    {
        var cmp = this.gameObject.AddComponent<Game>();
        cmp.testing = testing;
    }
}
";
	}
}