using UnityEngine;
using System.Collections;

namespace UGBSetup
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