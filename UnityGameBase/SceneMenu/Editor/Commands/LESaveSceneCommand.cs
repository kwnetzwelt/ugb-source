using System;
using UnityEngine;
using UnityEditor;
namespace UGB.SceneMenu.Commands
{
	public class LESaveSceneCommand : LESceneMenuCommand
	{
		public LESaveSceneCommand ()
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