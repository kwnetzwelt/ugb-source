using UnityEngine;
using System.Collections;
using System.Globalization;

namespace UGB.WebGL
{
	/// <summary>
	/// A temporary interface, that needs to be implemented for each game to support the webGL platform.
	/// </summary>
	public interface IWebGLPlatformHelper
	{
		GameLogicImplementationBase InitLogic();
		CultureInfo[] GetAllCultures();
	}
}

