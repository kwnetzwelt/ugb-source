using System;
using UnityEditorInternal;


namespace UnityGameBase.Core.Setup
{
	internal class OpenLogicClassInMD : UGBSetupStep
	{
		public OpenLogicClassInMD ()
		{
		}

		public override string GetName ()
		{
			return "Open GameLogic.cs in MonoDevelop";
		}

		public override System.Collections.IEnumerator Run ()
		{
			yield return 0;
			InternalEditorUtility.OpenFileAtLineExternal (CreateGameLogicClass.LogicClassFile(), 0);
			yield return 0;
		}
	}
}

