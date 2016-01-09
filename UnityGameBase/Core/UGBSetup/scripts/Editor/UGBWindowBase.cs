using UnityEngine;
using UnityEditor;
using System;

namespace UnityGameBase.Core.Setup
{
	public class UGBWindowBase : EditorWindow
	{
		static Texture2D mBigLogo;
		protected static Texture2D bigLogo
		{
			get
			{
				if(mBigLogo == null)
				{
					var guids = AssetDatabase.FindAssets("ugb_logo_256 t:Texture2D");
					if(guids.Length > 0)
					{
						mBigLogo = (Texture2D) AssetDatabase.LoadAssetAtPath(AssetDatabase.GUIDToAssetPath(guids[0]), typeof(Texture2D));
					}else
					{
						mBigLogo = Utils.UIHelpers.TransparentTexture;
					}
				}
				return mBigLogo;
			}
		}

        protected GUIStyle mTitleStyle = new GUIStyle();
        protected GUIStyle mTextStyle = new GUIStyle();
        protected GUIStyle mBulletPointStyle = new GUIStyle();

		void CreateStyles()
		{
			try
			{

				mTitleStyle = EditorStyles.boldLabel;
				mTextStyle = EditorStyles.label;
				mTextStyle.wordWrap = true;
				mBulletPointStyle = EditorStyles.foldout;

            }catch {
            }
            // we don't care about errors here. Some Unity Versions throw exceptions here at some stage, but this will be resolved anyway. 

		}

		protected virtual void OnEnable ()
		{
			CreateStyles();
		}

		protected virtual void OnDisable()
		{
			mBigLogo = null;
		}

		protected virtual void OnGUI()
		{
			
			GUI.color = new Color(1,1,1,0.3f);
			GUI.DrawTexture( new Rect( position.width - 160, position.height - 160, 256, 256), bigLogo);
			GUI.color = Color.white;
		}
	}
}
