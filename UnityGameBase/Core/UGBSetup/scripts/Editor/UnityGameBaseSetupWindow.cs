using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Collections.Generic;
using System;
using UnityEditorInternal;

namespace UnityGameBase.Core.Setup
{
    public class UnityGameBaseSetupWindow : UGBWindowBase
    {


        bool mForce = false;
        bool mRunning = false;
        UGBSetup mSetup = new UGBSetup();
        IEnumerator<string> mEnumerator;

        [MenuItem("UGB/Setup/Wizard")]
        static void SetupWizard()
        {
            var window = EditorWindow.GetWindow<UnityGameBaseSetupWindow>(true, "Setup Wizard", true);
            window.position = new Rect(window.position.x + 50, window.position.y + 50, 320, 300);
            window.minSize = new Vector2(320, 300);
            window.maxSize = new Vector2(320, 300);
        }

        protected override void OnGUI()
        {
            base.OnGUI();
            GUILayout.BeginVertical();

            GUILayout.Label("Unity Game Base Setup Wizard", mTitleStyle);

            GUILayout.Label("This wizard will create initial project structure as well as a default scene. ", mTextStyle);

            GUILayout.Space(3);

            GUILayout.Label("Once these steps are complete, the default scene will be open. You should then change the 'MLogic' member of the 'Game' Component on the '_GameRoot' GameObject to point to 'GameLogic.cs' or any of your custom implementations. ", mTextStyle);
			
            GUILayout.Space(3);

            GUILayout.Label("The following steps will be walked through: ", mTextStyle);

            foreach(var step in mSetup.Steps())
            {
                GUILayout.BeginHorizontal();
                GUILayout.Space(15);
                GUILayout.Label(step, mBulletPointStyle);
                GUILayout.EndHorizontal();
            }

            GUILayout.Space(8);

            GUI.enabled = !mRunning;

            mForce = GUILayout.Toggle(mForce, "Overwrite existing");

            if(GUILayout.Button("Run Setup"))
            {
                DoSetup();
            }


			

        }
        void OnInspectorUpdate()
        {

            Repaint();

            if(mRunning)
            {
                if(mEnumerator.MoveNext())
                {
                    EditorUtility.DisplayProgressBar("UGB Setup Wizard", mEnumerator.Current, mSetup.progress);
                }
                else
                {
					
                    EditorUtility.ClearProgressBar();
                    mEnumerator.Dispose();

                    mEnumerator = null;
                    mSetup = new UGBSetup();
                    mRunning = false;
                }
            }

        }

		#region actually do the setup

        void DoSetup()
        {
            mRunning = true;
            mSetup.force = mForce;
            mEnumerator = mSetup.Run();
            
        }


		#endregion
    }
}