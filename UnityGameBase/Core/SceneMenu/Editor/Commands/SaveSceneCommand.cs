using System;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;


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
            EditorSceneManager.SaveOpenScenes();
		}
	}

}