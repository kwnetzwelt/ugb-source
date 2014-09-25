using UnityEngine;
using UnityEditor;
using System;

namespace UGB
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
					mBigLogo = (Texture2D) AssetDatabase.LoadAssetAtPath("Assets/packages/UnityGameBase/Editor/Gizmos/ugb_logo_256.png", typeof(Texture2D));
				}
				return mBigLogo;
			}
		}

		protected GUIStyle mTitleStyle;
		protected GUIStyle mTextStyle;
		protected GUIStyle mBulletPointStyle;

		void CreateStyles()
		{
			try
			{

				mTitleStyle = EditorStyles.boldLabel;
				mTextStyle = EditorStyles.label;
				mTextStyle.wordWrap = true;
				mBulletPointStyle = EditorStyles.foldout;

			}catch
			{
				Debug.LogError("could not get editor styles. ");
			}
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
