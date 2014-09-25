using System;
using UnityEditor;
using UnityEngine;

namespace UGBSetup
{
	[InitializeOnLoad]
	public class UGBEditorUtils
	{
		static EditorApplication.HierarchyWindowItemCallback hierarchyItemCallback;
		static RectOffset mOffset = new RectOffset(150,15,2,2);
		private static Texture2D mIcon;
		private static Texture2D UGBIcon
		{
			get
			{
				if( mIcon == null )
				{
					try
					{
						mIcon = (Texture2D) AssetDatabase.LoadAssetAtPath("Assets/packages/UnityGameBase/Editor/Gizmos/ugb_logo_256.png", typeof(Texture2D));
						//mIcon = (Texture2D)Resources.Load( "ugb_logo_256" );
					}catch
					{

					}
				}
				return mIcon;
			}
		}

		static UGBEditorUtils()
		{
			hierarchyItemCallback = new EditorApplication.HierarchyWindowItemCallback( DrawHierarchyIcon );

			EditorApplication.hierarchyWindowItemOnGUI = (EditorApplication.HierarchyWindowItemCallback)Delegate.Combine( EditorApplication.hierarchyWindowItemOnGUI, hierarchyItemCallback );
			EditorApplication.update += OnEditorUpdate;
		}

		private static void OnEditorUpdate()
		{
			if(EditorApplication.isCompiling)
			{
				mIcon = null;
			}
		}

		private static void DrawHierarchyIcon( int instanceID, Rect selectionRect )
		{
			if(EditorApplication.isCompiling)
				return;

			if( UGBIcon == null )
			{
				return;
			}
			
			GameObject gameObject = EditorUtility.InstanceIDToObject( instanceID ) as GameObject;
			if( gameObject == null )
				return;
			
			var view = gameObject.GetComponent<Game>();
			if( view == null )
				return;

			if(!view.mTesting)
				GUI.color = new Color(0.5f,1,0,.2f);
			else
				GUI.color = new Color(1,1,0,.2f);

			GUI.Box(mOffset.Add( selectionRect ),"");


			GUI.color = Color.white;
			Rect rect = new Rect( selectionRect.x + selectionRect.width - 32f, selectionRect.y - 6, 32f, 32f );
			GUI.DrawTexture( rect, UGBIcon );


		}
	}
}

