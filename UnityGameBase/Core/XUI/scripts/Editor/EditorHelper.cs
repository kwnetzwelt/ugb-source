using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEditor;

namespace UnityGameBase.Core.XUI
{
	/// <summary>
	/// this class Create the XUI prefabs and link to the XUI contextMenu
	/// </summary>
	public class EditorHelper
	{
		//---------------------------------------------------------------------------------------------------------
		[MenuItem( "GameObject/XUI/Button",false,0 )]
		public static void AddButton()
		{
			AddChild("XUI_Button", Vector2.zero);
		}
		//---------------------------------------------------------------------------------------------------------
		[MenuItem( "GameObject/XUI/Panel",false,0 )]
		public static void AddPanel()
		{			
			GameObject prefab = PrefabUtility.InstantiatePrefab(Resources.Load("XUI_Panel")) as GameObject;
			
			RectTransform t = (RectTransform)prefab.transform;
			t.sizeDelta = Vector2.zero;
			t.position = Vector2.zero;
			AddChild(prefab);
		}
		//---------------------------------------------------------------------------------------------------------
		[MenuItem( "GameObject/XUI/Slider",false,0 )]
		public static void AddSlider()
		{
			AddChild("XUI_Slider");
		}
		//---------------------------------------------------------------------------------------------------------
		[MenuItem( "GameObject/XUI/Text",false,0 )]
		public static void AddText()
		{
			AddChild("XUI_Text");
		}
		//---------------------------------------------------------------------------------------------------------
		[MenuItem( "GameObject/XUI/Scrollbar",false,0 )]
		public static void AddScrollbar()
		{
			AddChild("XUI_Scrollbar");
		}
		//---------------------------------------------------------------------------------------------------------
		[MenuItem( "GameObject/XUI/InputField",false,0 )]
		public static void AddInputField()
		{
			AddChild("XUI_InputField");
		}
		//---------------------------------------------------------------------------------------------------------
		[MenuItem( "GameObject/XUI/Image",false,0 )]
		public static void AddImage()
		{
			AddChild("XUI_Image");
		}
		//---------------------------------------------------------------------------------------------------------
		[MenuItem( "GameObject/XUI/RawImage",false,0 )]
		public static void AddRawImage()
		{
			AddChild("XUI_RawImage");
		}
		//---------------------------------------------------------------------------------------------------------
		[MenuItem( "GameObject/XUI/Toggle",false,0 )]
		public static void AddToggle()
		{
			AddChild("XUI_Toggle");
		}
		//---------------------------------------------------------------------------------------------------------
		[MenuItem( "GameObject/XUI/ToggleGroup",false,0 )]
		public static void AddToggleGroup()
		{
			AddChild("XUI_ToggleGroup");
		}
		//---------------------------------------------------------------------------------------------------------
		[MenuItem( "GameObject/XUI/Canvas",false,0 )]
		public static void AddCanvas()
		{
			AddChild("XUI_Canvas");
		}
		//---------------------------------------------------------------------------------------------------------
		[MenuItem( "GameObject/XUI/Screen",false,0 )]
		public static void AddScreen()
		{
			//todo: maybe as prefabInstance
			GameObject prefab = GameObject.Instantiate(Resources.Load("XUI_Screen")) as GameObject;		
			prefab.transform.position = Vector3.zero;	
		}
		//---------------------------------------------------------------------------------------------------------
		private static void AddChild(GameObject _child)
		{
			GameObject obj = GetParent();
			
			if (obj != null)
			{						
				
				_child.transform.SetParent(obj.transform, false);
				
				EditorUtility.SetDirty(_child);			
			}
			else
				Debug.Log("No Screen-Root found!");
		}
		//---------------------------------------------------------------------------------------------------------
		private static void AddChild(string _child, Vector2 _pos = default(Vector2))
		{
			GameObject obj = GetParent();
		
			if (obj != null)
			{		
				GameObject prefab = PrefabUtility.InstantiatePrefab(Resources.Load(_child)) as GameObject;
				
				prefab.transform.position = Vector3.zero;
				prefab.transform.SetParent(obj.transform, false);
			
				EditorUtility.SetDirty(prefab);			
			}
			else
				Debug.Log("No Screen-Root found!");
		}
		//---------------------------------------------------------------------------------------------------------
		private static GameObject GetParent()
		{
			if (Selection.activeGameObject != null)
				return Selection.activeGameObject;

			return GetScreenRoot();
		}
		//---------------------------------------------------------------------------------------------------------
		private static GameObject GetScreenRoot()
		{
			GameObject[] sceneObj = GameObject.FindObjectsOfType<GameObject>();
		
			GameObject root = null;
			foreach (GameObject obj in sceneObj)
			{
				Debug.Log(obj.name);
			
				PrefabGenerator temp = obj.GetComponentInChildren<PrefabGenerator>();
			
				if (temp != null)
				{
					root = temp.gameObject;
				}
			}
			return root;
		}
		//---------------------------------------------------------------------------------------------------------
	}
}
