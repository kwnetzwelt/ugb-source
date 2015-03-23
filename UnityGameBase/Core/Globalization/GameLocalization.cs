using UnityEngine;
using System.Collections;
using System.Globalization;
using UnityGameBase.Core.Data;

#if UNITY_METRO && !UNITY_EDITOR
using System.Runtime.InteropServices;
using System;
using System.Collections.Generic;

#endif
namespace UnityGameBase.Core.Globalization
{
    /// <summary>
    /// Loads a LocaData instance. Contains the setting for the currently selected language. Returns translations.
    /// </summary>
    public class GameLocalization : MonoBehaviour
    {
        LocaData mData;
        public Languages mCurrentLanguage = Languages.Invalid;

        public Languages currentLanguage
        {
            get { return mCurrentLanguage; }
        }

        public void SetLanguage(int pLanguage)
        {
            mCurrentLanguage = pLanguage;
            Initialize();
        }

        public string GetText(string pKey, params object[] pParams)
        {
            if (mData != null)
            {
                return System.String.Format(mData.GetText(pKey), pParams);
            }
            return null;
        }

        public string GetText(string pKey)
        {
            if (mData != null)
            {
                return mData.GetText(pKey);
            }
            return null;
        }

        public string[] GetKeys()
        {
            if (mData != null)
            {
                return mData.GetKeys();
            }
            return null;
        }

        public void Initialize()
        {
            mData = LocaData.Load(GetCurrentLanguageShort());
        }
        
        public static string GetCurrentLanguageShort()
        {
            // getting system default language
            Languages defLanguageShort = GetSystemLanguageShort();
            
            // checking for language setting in playerprefs
            defLanguageShort = PlayerPrefs.GetInt(GameOptions.LanguageOption, defLanguageShort);

            // enumerating all langauges to find a match with the language set in the system
            foreach (int lang in Languages.Enumerate())
            {
                if ((Languages)lang == defLanguageShort)
                {
                    return defLanguageShort;
                }
            }
            
            // if none found return first language
            return Languages.First;
        }
        
        /// <summary>
        /// Gets the system language short for all cultures.
        /// </summary>
        /// <returns>
        /// The system language short.
        /// </returns>
        static string GetSystemLanguageShort()
        {
            UnityEngine.SystemLanguage lang = Application.systemLanguage;
            
            string sysLang = lang.ToString();
            
            foreach (CultureInfo ci in GetCultures())
            {
                if (ci.DisplayName == sysLang)
                {
                    return ci.Name;
                }
            }
            return "en";
            
        }

        static CultureInfo[] GetCultures()
        {
            #if UNITY_WEBGL
            return Game.webGLHelper.GetAllCultures();
            #elif UNITY_METRO && !UNITY_EDITOR
            return CultureHelper.GetCultures( CultureHelper.CultureTypes.AllCultures ).ToArray();
            #else
            return CultureInfo.GetCultures(CultureTypes.AllCultures);
            #endif
        }

    }



    #if UNITY_METRO && !UNITY_EDITOR


    public class LocalesRetrievalException : Exception
    {
        public LocalesRetrievalException(string message)
            : base(message)
        {
        }
    }

    public static class CultureHelper
    {
        #region Windows API
        
        private delegate bool EnumLocalesProcExDelegate(
            [MarshalAs(UnmanagedType.LPWStr)]String lpLocaleString,
            LocaleType dwFlags, int lParam);
        
        [DllImport(@"kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        private static extern bool EnumSystemLocalesEx(EnumLocalesProcExDelegate pEnumProcEx,
                                                       LocaleType dwFlags, int lParam, IntPtr lpReserved);
        
        private enum LocaleType : uint
        {
            LocaleAll = 0x00000000,             // Enumerate all named based locales
            LocaleWindows = 0x00000001,         // Shipped locales and/or replacements for them
            LocaleSupplemental = 0x00000002,    // Supplemental locales only
            LocaleAlternateSorts = 0x00000004,  // Alternate sort locales
            LocaleNeutralData = 0x00000010,     // Locales that are "neutral" (language only, region data is default)
            LocaleSpecificData = 0x00000020,    // Locales that contain language and region data
        }
        
        #endregion
        
        public enum CultureTypes : uint
        {
            SpecificCultures = LocaleType.LocaleSpecificData,
            NeutralCultures = LocaleType.LocaleNeutralData,
            AllCultures = LocaleType.LocaleWindows
        }
        
        public static List<CultureInfo> GetCultures(
            CultureTypes cultureTypes)
        {
            List<CultureInfo> cultures = new List<CultureInfo>();
            EnumLocalesProcExDelegate enumCallback = (locale, flags, lParam) =>
            {
                try
                {
                    cultures.Add(new CultureInfo(locale));
                }
                catch (CultureNotFoundException)
                {
                    // This culture is not supported by .NET (not happened so far)
                    // Must be ignored.
                }
                return true;
            };
            
            if (EnumSystemLocalesEx(enumCallback, (LocaleType)cultureTypes, 0,
                                    (IntPtr)0) == false)
            {
                int errorCode = Marshal.GetLastWin32Error();
                throw new LocalesRetrievalException("Win32 error " + errorCode +
                                                    " while trying to get the Windows locales");
            }
            
            // Add the two neutral cultures that Windows misses 
            // (CultureInfo.GetCultures adds them also):
            if (cultureTypes == CultureTypes.NeutralCultures ||
                cultureTypes == CultureTypes.AllCultures)
            {
                cultures.Add(new CultureInfo("zh-CHS"));
                cultures.Add(new CultureInfo("zh-CHT"));
            }
            
            return cultures;
        }
    }
    #endif
}