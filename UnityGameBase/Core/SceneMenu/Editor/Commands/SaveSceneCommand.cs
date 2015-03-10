using System;
using UnityEngine;
using UnityEditor;
namespace UnityGameBase.Core.SceneMenu.Commands
{
	public class SaveSceneCommand : SceneMenuCommand
	{
		public SaveSceneCommand ()
		{
			mName = "Save Scene";
			mModifiers = UnityEngine.EventModifiers.Alt;
			mKeyCode = UnityEngine.KeyCode.S;
		}
		public override void Execute ()
		{
			EditorApplication.SaveScene();
		}
	}

}