using UnityEngine;
using UnityEditor;
using UnityGameBase.Core.Globalization;

using System.Collections;

namespace UnityGameBase.Core.XUI
{
    /// <summary>
    /// Contains logic for language selection in editor
    /// </summary>
    public class XUIEditor : EditorWindow
    {		
        private int selectedIndex = 0;
		
        [MenuItem ("UGB/XUIEditor")]
        static void Init()
        {
            // Get existing open window or if none, make a new one:
            XUIEditor window = (XUIEditor)EditorWindow.GetWindow(typeof(XUIEditor));
#if UNITY_5
			window.titleContent = new GUIContent("XUI Editor");
#else
            window.title = "XUI Editor";
#endif
        }
		
        void OnEnable()
        {
            //Reaload the locaFiles
			if(Application.isPlaying)
			{
				UGB.Loca.Initialize();
			}
			else
			{
				LocalizationHelper.Refresh();
			}
        }
		 
        void OnGUI()
        {
			if(Application.isPlaying)
			{
				DrawLanguageSelectionWhilePlaying();
			}
			else
			{
				DrawLanguageSelection();
			}

        }		
		
        /// <summary>
        /// Switch the current language and send it to all LocalizedTextComponents in active scene when Application is not playing
        /// </summary>
        private void DrawLanguageSelection()
        {
            if(GUILayout.Button("Refresh Localization"))
            {
                LocalizationHelper.Refresh();
            }
			
            //switch language
            if(LocalizationHelper.AllLanguagesNames != null)
            {
				
                int tempIndex = selectedIndex;
                selectedIndex = EditorGUILayout.Popup(selectedIndex, LocalizationHelper.AllLanguagesNames);
				
                LocalizationHelper.CurrentLanguage = LocalizationHelper.AllLanguagesNames[selectedIndex];			
                if(tempIndex != selectedIndex)
                {
                    //throw event to all listener
                    LocalizedTextComponent[] all = GameObject.FindObjectsOfType(typeof(LocalizedTextComponent)) as LocalizedTextComponent[];
					
                    foreach(LocalizedTextComponent comp in all)
                    {
                        if(comp == null)
                        {
                            continue;
                        }
						
                        comp.ReCreate();
                        EditorUtility.SetDirty(comp);
                    }
                }
            }
        }

		/// <summary>
		/// Switch the current language and send it to all LocalizedTextComponents in active scene when Application is playing
		/// Only for debugging
		/// </summary>
		private void DrawLanguageSelectionWhilePlaying()
		{
			if(GUILayout.Button("Refresh Localization"))
			{
				UGB.Loca.Initialize();
			}
			
			//switch language
			if(Languages.count > 0)
			{

				string[] languages = new string[Languages.count];

				for(int i = 0; i < Languages.count; i++)
				{
					Languages l = new Languages(i);
					languages[i] = l;
				}
				
				int tempIndex = selectedIndex;
				selectedIndex = EditorGUILayout.Popup(selectedIndex, languages);
				
				UGB.Loca.SetLanguage(selectedIndex);			
				if(tempIndex != selectedIndex)
				{
					//throw event to all listener
					LocalizedTextComponent[] all = GameObject.FindObjectsOfType(typeof(LocalizedTextComponent)) as LocalizedTextComponent[];
					
					foreach(LocalizedTextComponent comp in all)
					{
						if(comp == null)
						{
							continue;
						}
						
						comp.ReCreate();
						EditorUtility.SetDirty(comp);
					}
				}
			}
		}
    }
}