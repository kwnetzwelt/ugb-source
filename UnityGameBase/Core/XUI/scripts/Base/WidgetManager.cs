using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace UnityGameBase.Core.XUI
{
/// <summary>
/// this class save create all widgetData and keep them in the prefab
/// </summary>
    [System.Serializable]
    public class WidgetManager : MonoBehaviour
    {
        [SerializeField,HideInInspector]
        public List<WidgetData>
            widgetContainer = new List<WidgetData>();
		
							
        /// <summary>
        /// Gets the widget by layer and name
        /// </summary>
        /// <returns>The widget.</returns>
        /// <param name="_layer">_layer.</param>
        /// <param name="_name">_name.</param>
        public WidgetData GetWidget(string _layer, string _name)
        {
            foreach(WidgetData widgetData in widgetContainer)
            {
                if(widgetData.layerName == _layer && widgetData.widgetName == _name)
                {
                    return widgetData;
                }
            }
				
            //Debug.Log("Widget with name: " + _name + " dont exists in layer: " + _layer);
            return null;			
        }
		
        /// <summary>
        /// Create a collection with all IWidget Gameobjects
        /// </summary>
        public void CreateCollection()
        {		
			
            RectTransform[] transForms = this.gameObject.GetComponentsInChildren<RectTransform>();
			
            widgetContainer.Clear();
			
            foreach(RectTransform t in transForms)
            {
                IWidget widget = (IWidget)t.GetComponent(typeof(IWidget));
				
                if(widget != null)
                {
                    Component c = t.GetComponent(typeof(IWidget));
                    
                    string typeName = c.GetType().ToString();
                
                    WidgetData widgetData = new WidgetData(t.parent.name, t.name, t, typeName);
				
					
                    if(this.ExistLayerAndName(t.parent.name, t.name))
                    {		
                        Debug.LogError("Widget with name: " + t.name + " already exists in layer: " + t.parent.name);
                    }
                    else
                    {
                        widgetContainer.Add(widgetData);					
                        Debug.Log("Add Widget: " + widgetData.layerName + ", " + widgetData.widgetName);
                    }
                }
            }				
        }
        
        public void RemoveWidget(WidgetData widgetData)
        {
            if(this.widgetContainer.Contains(widgetData))
            {
                this.widgetContainer.Remove(widgetData);
            }
        }
        
        public void RemoveWidget(string layerName, string widgetName)
        {
            for(int i=0; i<this.widgetContainer.Count; i++)
            {
                WidgetData widgetData = this.widgetContainer[i];
                if(widgetData.layerName == layerName && widgetData.widgetName == widgetName)
                {
                    this.widgetContainer.RemoveAt(i);
                    break;
                }
            }
        }
        
        public void AddWidget(WidgetData widgetData)
        {
            if(this.ExistLayerAndName(widgetData.layerName, widgetData.widgetName))
            {       
                Debug.LogError("Widget with name: " + widgetData.widgetName + " already exists in layer: " + widgetData.layerName);
            }
            else
            {
                this.widgetContainer.Add(widgetData);
            }                
        }
		
        public void AddWidget(string layerName, string widgetName, Transform widget, string type)
        {
            if(this.ExistLayerAndName(layerName, widgetName))
            {       
                Debug.LogError("Widget with name: " + widgetName + " already exists in layer: " + layerName);
            }
            else
            {
                this.widgetContainer.Add(new WidgetData(layerName, widgetName, widget, type));
            }
        }
        
        public bool ExistLayerAndName(string _layer, string _name)
        {
            foreach(WidgetData widgetData in widgetContainer)
            {
                if(widgetData.layerName == _layer && widgetData.widgetName == _name)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
