using System;
using UnityEditor;
using UnityEngine;
namespace UnityGameBase.Core.SceneMenu.Commands
{
	public class DeleteSelectedCommand : SceneMenuCommand
	{
		public DeleteSelectedCommand ()
		{
			mName = "Delete Selected";
			mKeyCode = KeyCode.Delete;
			mModifiers = EventModifiers.FunctionKey;
		}
		public override void Execute ()
		{

			GameObject go = Selection.activeGameObject;
			Undo.DestroyObjectImmediate(go);

		}
	}

}