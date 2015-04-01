using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

namespace UnityGameBase.Core.XUI
{
/// <summary>
/// Custom inspector shows all layers and widgets
/// </summary>
    [CustomEditor(typeof(XUIHierarchyViewer))]
    public class XUIHierarchyViewerInspector : UnityEditor.Editor
    {
        GUIStyle darkStyle;
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            
            if(darkStyle == null)
            {
                darkStyle = new GUIStyle();
                darkStyle.normal.background = UnityGameBase.Core.Utils.UIHelpers.BlackTexture;
            }
            
            XUIHierarchyViewer myTarget = (XUIHierarchyViewer)target;
			
            //TODO call not on each tick
            myTarget.UpdateHierarchy();
			
            EditorGUILayout.BeginVertical("box");
            EditorGUILayout.BeginHorizontal("toolbar");
            GUI.skin.GetStyle("PreLabel").fontStyle = FontStyle.Bold;
            EditorGUILayout.LabelField("Name<Type>", "Layer", GUILayout.Height(20));
            GUI.skin.GetStyle("PreLabel").fontStyle = FontStyle.Normal;
            EditorGUILayout.EndHorizontal();
            
            bool odd = false;
            foreach(string layer in myTarget.hierarchy.Keys)
            {					
                foreach(WidgetData data in myTarget.hierarchy[layer])
                {
                    odd = !odd;
                    GUILayout.BeginHorizontal(odd ? "AnimationRowEven" : "AnimationRowOdd");
                    string key = layer + ", " + data.widgetName;
                    if(data.widgetType != "" && data.widgetType != null)
                    {
                        string toolTipShort = data.widgetType.Substring(data.widgetType.LastIndexOf(".") + 1);
                        EditorGUILayout.LabelField(new GUIContent(data.widgetName + "<" + toolTipShort + ">", data.widgetType), new GUIContent(layer));
                        
                    }
                    else
                    {                    
                        EditorGUILayout.LabelField(key + ", Undefinded please reapply prefab!");
                    }
                    GUILayout.EndHorizontal();
                }
            }			
            EditorGUILayout.EndVertical();
        }		
    }
}
