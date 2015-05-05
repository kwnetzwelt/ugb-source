using UnityEngine;
using System.Collections;
using System.Globalization;
using System.Runtime.InteropServices;

namespace UnityGameBase.WebGL
{
    public class WebGLPlatformHelper : MonoBehaviour
    {
#if !UNITY_METRO
        public System.Globalization.CultureInfo[] GetAllCultures()
        {
            string webglCulture = string.Empty;
            #if UNITY_WEBGL && !UNITY_EDITOR 
		webglCulture = DetectBrowserLanguage();
            #endif
            Debug.Log("Detected browser language: " + webglCulture);
            CultureInfo[] c = new CultureInfo[1]
		{
			CultureInfo.GetCultureInfo((webglCulture != string.Empty) ? webglCulture : "en-US")
		};
            return c;
        }
	

	#if UNITY_WEBGL
        [DllImport("__Internal")]
        private static extern string DetectBrowserLanguage();

        void Awake()
        {
            Game.webGLHelper = this;
        }

	#else
    void Awake()
    {
        GameObject.Destroy(this);
    }
	#endif
#endif
    }
}
