using UnityEngine;
using System.Collections;
using UnityEditor;
using UnityGameBase.Core.XUI;

/// <summary>
/// this class shows an extra inputfield for the loca key and a toggle to use it
/// </summary>
[CustomEditor(typeof(LocalizedTextComponent))]
public class LocalizedTextInspector : Editor
{
    int selectedIndex = 0;
    int selectedLanguageIndex = 0;
    
    string[] matchingKeys = null;
    string searchKey = "";
    Vector2 scrollPos = Vector2.zero;
    Rect scrollviewRect;
    
    public override void OnInspectorGUI()
    {        	
        base.OnInspectorGUI();
		
        if(target == null)
        {
            return;
        }
        
            
        LocalizedTextComponent myTarget = (LocalizedTextComponent)target;
	
        EditorGUILayout.BeginHorizontal();
        myTarget.useLocaFiles = EditorGUILayout.Toggle("Use Loca", myTarget.useLocaFiles);
        DrawLanguageSelection();
        
        EditorGUILayout.EndHorizontal();		
        
                        
        if(myTarget.useLocaFiles)
        {
            //GUI.SetNextControlName("LocaKeys");
            
            //  GUILayout.BeginHorizontal(EditorStyles.toolbar);
            //  GUILayout.FlexibleSpace();
            
            matchingKeys = LocalizationHelper.GetMatchingKeys(searchKey, LocalizationHelper.GetKeys());       
            
            if(matchingKeys.Length == 0)
            {
                matchingKeys = LocalizationHelper.GetKeys();
            }
            
            UpdateKeyControl();
            
            EditorGUILayout.LabelField("Loca Key: ", myTarget.Key);
            
        
            GUI.SetNextControlName("Textfield");
            
            string tempSearchKey = searchKey;
            searchKey = EditorGUILayout.TextField("Search Key: ", searchKey);
            if(searchKey != tempSearchKey)
            {
                selectedIndex = 0;
            }
        
            
            if(GUI.GetNameOfFocusedControl() == "Textfield" || Event.current.type == EventType.Layout)
            {            
                scrollPos = EditorGUILayout.BeginScrollView(scrollPos);
                
//                float yOffset = lastRectangle.yMax;
                
                int index = 0;
                foreach(string val in matchingKeys)
                {           
                    GUI.color = Color.white;
                    if(index == selectedIndex)
                    {
                        GUI.color = Color.green;
                        EditorGUILayout.LabelField(" ", val, GUILayout.Height(15));
                        myTarget.Key = val;
                    }
                    else
                    {
                        
                        EditorGUILayout.LabelField(" ", val, GUILayout.Height(15));
                    }
                    index++;
                }
                EditorGUILayout.EndScrollView();
                scrollviewRect = GUILayoutUtility.GetLastRect();                
            }
        }
        EditorUtility.SetDirty(myTarget);        
    }
    
    private void UpdateKeyControl()
    {
        Event e = Event.current;
        
       
        if(e.type == EventType.keyDown)
        {
            if(e.keyCode == KeyCode.DownArrow)
            {               
                selectedIndex += 1;
            }
            else if(e.keyCode == KeyCode.UpArrow)
                {
                    selectedIndex -= 1;
                }
                else if(e.keyCode == KeyCode.Return)
                    {
                        searchKey = matchingKeys[selectedIndex];
                        GUI.FocusControl("");   
                    }   
            selectedIndex = Mathf.Clamp(selectedIndex, 0, matchingKeys.Length);
            
            scrollPos.y = (selectedIndex * 16) + scrollviewRect.height;
        }      
        
    }    
    
    private void DrawLanguageSelection()
    {
        if(GUILayout.Button("Refresh Loca"))
        {
            LocalizationHelper.Refresh();
        }
        
        //switch language
        if(LocalizationHelper.AllLanguagesNames != null)
        {            
            int tempIndex = selectedLanguageIndex;
            selectedLanguageIndex = EditorGUILayout.Popup(selectedLanguageIndex, LocalizationHelper.AllLanguagesNames);
            
            LocalizationHelper.CurrentLanguage = LocalizationHelper.AllLanguagesNames[selectedLanguageIndex];          
            if(tempIndex != selectedLanguageIndex)
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

