using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace UGB.XUI
{

/// <summary>
/// Shows a preview of all existing layer and widgets in the screen prefab
/// </summary>
	public class XUIHierarchyViewer : MonoBehaviour
	{
		public	Dictionary<string,List<string>> hierarchy = new Dictionary<string, List<string>>();
		
		public void UpdateHierarchy()
		{
		
			Transform rootScreen = this.transform.GetChild(0);
			
			WidgetManager manager = rootScreen.GetComponent<WidgetManager>();
			
			hierarchy.Clear();
		
			foreach (WidgetData data in manager.widgetContainer)
			{					
				if (hierarchy.ContainsKey(data.layerName))
				{
					hierarchy [data.layerName].Add(data.widgetName);
				}
				else
				{
					hierarchy.Add(data.layerName, new List<string>(new string[]{data.widgetName}));
				}				
			}	
		}
	}	
}
