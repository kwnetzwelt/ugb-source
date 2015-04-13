using UnityEngine;
using UnityEditor;
using System.Collections;

namespace UnityGameBase.Core.XUI
{
/// <summary>
/// currently not used, later for contextmenu in sceneView
/// </summary>
	[InitializeOnLoad]
	public class XUISceneContextMenu
	{
		static Vector2 mousePosition = Vector2.zero;
		
		static bool showMenu = false;
		
		
		static void OnScene(SceneView scene)
		{
            //if (Event.current.type == EventType.MouseDown)
            //{
            //    // right click
            //    if (Event.current.button == 1)
            //    {
            //        Event.current.Use();
            //        mousePosition = Event.current.mousePosition;
            //        showMenu = true;
            //    }
            //    else if (Event.current.type == EventType.MouseUp)
            //    {
            //        showMenu = false;
            //    }
				
            //    if (Event.current.button == 0)
            //    {
            //        showMenu = false;
            //    }
            //}
			
            ////if (mouseWorldPos != Vector3.zero)
            //if (showMenu)
            //{
            //    //	DrawMenu();
            //}
		}
		
		static void DrawMenu()
		{
			Rect r = new Rect(mousePosition.x, mousePosition.y, 200, 400);
			GUILayout.BeginArea(r, "XUI Menu", style);
			if (GUILayout.Button("Button"))
			{
				//EditorHelper.AddButton(Vector2.zero);
			}
			GUILayout.EndArea();
		}
		
		static void CloseMenu()
		{
		
		}
		
		static Vector3 GetWorldPosition(Vector2 mousePosition)
		{
			Ray r = HandleUtility.GUIPointToWorldRay(mousePosition);
			RaycastHit hit;
			Physics.Raycast(r, out hit);
			return hit.point; // if there is no hit > Vec3.zero
		}
		
		static XUISceneContextMenu()
		{
			SceneView.onSceneGUIDelegate += OnScene;
		}
		
		~XUISceneContextMenu()
		{
			SceneView.onSceneGUIDelegate -= OnScene;
		}
		
		static GUIStyle contextStyle;
		static GUIStyle style
		{
			get
			{
				if (contextStyle == null)
				{
					contextStyle = new GUIStyle();
					contextStyle.normal.background = new Texture2D(1, 1);
					Color32[] clrs = new Color32[1];
					clrs [0] = GetColor(0x222c36cc);
					contextStyle.normal.background.SetPixels32(clrs);
					contextStyle.normal.background.Apply();
					contextStyle.padding = new RectOffset(2, 2, 2, 2);
				}
				return contextStyle;
			}
		}
		
		static Color32 GetColor(long pColorRGBA)
		{
			byte r = (byte)((pColorRGBA & 0xff000000) >> 24);
			byte g = (byte)((pColorRGBA & 0xff0000) >> 16);
			byte b = (byte)((pColorRGBA & 0xff00) >> 8);
			byte a = (byte)((pColorRGBA & 0xff));
			
			Color32 c = new Color32(r, g, b, a);
			return c;
		}
	}
}
