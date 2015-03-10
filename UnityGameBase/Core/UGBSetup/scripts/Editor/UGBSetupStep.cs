using UnityEngine;
using System.Collections;

namespace UnityGameBase.Core.Setup
{
	internal abstract class UGBSetupStep
	{
		public bool force
		{
			get;
			set;
		}
		public abstract string GetName();
		public abstract IEnumerator Run();
	}

}