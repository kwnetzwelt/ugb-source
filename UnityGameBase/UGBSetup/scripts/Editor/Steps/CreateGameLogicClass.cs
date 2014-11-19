using UnityEngine;
using System.Collections;
using System;
using System.IO;
using UnityEditor;
using UGB;

namespace UGBSetup
{
	internal class CreateGameLogicClass : UGBSetupStep
	{
		const string kClassName = "GameLogic";
		const string kClassPath = "Assets/scripts/" + kClassName + ".cs";

		public static string LogicClassFile()
		{
			return kClassPath;
		}

		public override string GetName ()
		{
			return "Create GameLogic class";
		}

		public override IEnumerator Run ()
		{
			if(!force)
			{
				System.Type type = GetLogicClassType();
				if(type != null)
				{
					Debug.Log("Logic Class exists. " + type + " Skipping. ");
					yield break;
				}
			}

			//
			// roughly similar to : http://answers.unity3d.com/questions/14367/how-can-i-wait-for-unity-to-recompile-during-the-e.html?page=1&pageSize=5&sort=votes
			//

			File.WriteAllText( kClassPath, kClassContent);

			AssetDatabase.ImportAsset( kClassPath );


		}

		System.Type GetLogicClassType()
		{
			foreach( var assembly in AppDomain.CurrentDomain.GetAssemblies())
			{
				foreach (var t in assembly.GetTypes())
				{
					if(t.Name == kClassName && t.IsAssignableFrom(typeof(GameLogicImplementationBase)))
						return t;
				}
			}
			return null;
		}

		const string kClassContent = @"using UnityEngine;
[GameLogicImplementation()]
public class GameLogic : GameLogicImplementationBase
{
	#region implemented abstract members of GameLogicImplementationBase

	public override void Start ()
	{
		throw new System.NotImplementedException ();
	}

	public override void GameSetupReady ()
	{
		throw new System.NotImplementedException ();
	}

	public override void GameStateChanged (SGameState pOldState, SGameState pCurrentGameState)
	{
		throw new System.NotImplementedException ();
	}

	public override SGameState GetCurrentGameState ()
	{
		throw new System.NotImplementedException ();
	}

	public override bool OnBeforeRestart ()
	{
		throw new System.NotImplementedException ();
	}

	public override bool OnBeforePause ()
	{
		throw new System.NotImplementedException ();
	}

	#endregion


}

";
	}

}