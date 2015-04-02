using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace UnityGameBase.Core.XUI
{

/// <summary>
/// Shows a preview of all existing layer and widgets in the screen prefab
/// </summary>
    public class XUIHierarchyViewer : MonoBehaviour
    {
        public	Dictionary<string,List<WidgetData>> hierarchy = new Dictionary<string, List<WidgetData>>();
		
        public void UpdateHierarchy()
        {
		
            Transform rootScreen = this.transform.GetChild(0);
			
            WidgetManager manager = rootScreen.GetComponent<WidgetManager>();
			
            hierarchy.Clear();
		
            foreach(WidgetData data in manager.widgetContainer)
            {					
                if(hierarchy.ContainsKey(data.layerName))
                {
                    hierarchy[data.layerName].Add(data);
                }
                else
                {
                    hierarchy.Add(data.layerName, new List<WidgetData>(){data});
                }				
            }	
            
        }
    }	
}
