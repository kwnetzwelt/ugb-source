using UnityEngine;
using System.Collections;

namespace UGB.Core.XUI
{
/// <summary>
/// this serializable class save a widget name, layername and object reference
/// </summary>
	[System.Serializable]
	public class WidgetData
	{
		[SerializeField]
		public string
			layerName;
		[SerializeField]
		public string
			widgetName;
		[SerializeField]
		public Transform
			widgetObject;
		
		public WidgetData(string layer, string name, Transform widget)
		{
			this.layerName = layer;
			this.widgetName = name;
			this.widgetObject = widget;
		}		
	}

}
