using UnityEngine;
using UnityEditor;

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
            window.title = "XUI Editor";
        }
		
        void OnEnable()
        {
            //Reaload the locaFiles
            LocalizationHelper.Refresh();
        }
		 
        void OnGUI()
        {
            DrawLanguageSelection();			
        }		
		
        /// <summary>
        /// Switch the current language and send it to all LocalizedTextComponents in active scene
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
    }
}