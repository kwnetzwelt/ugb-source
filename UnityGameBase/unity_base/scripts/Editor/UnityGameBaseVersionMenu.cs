using UnityEngine;
using System.Collections;
using UnityEditor;
using UGB.Utils;
namespace UGB.Editor
{
	public class UnityGameBaseVersionMenu
	{
		
		[MenuItem("UGB/Unity Game Base Version " + UnityGameBaseVersion.kVersion,false, int.MaxValue)]
		public static void Version()
		{
			
		}
		
		[MenuItem("UBG/Unity Game Base Version " + UnityGameBaseVersion.kVersion, true)]
		public static bool ValidateVersion()
		{
			return false;
		}

	}
}