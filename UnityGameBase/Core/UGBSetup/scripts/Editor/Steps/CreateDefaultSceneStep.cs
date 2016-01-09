using UnityEngine;
using System.Collections;
using UnityEditor;
using System.IO;
using UnityGameBase.Core;
using UnityEditor.SceneManagement;

namespace UnityGameBase.Core.Setup
{
    internal class CreateDefaultSceneStep : UGBSetupStep
    {
        public const string kScene = "Assets/scenes/default.unity";
        public const string kGameRoot = "_GameRoot";

        public override string GetName()
        {
            return "Create default scene";
        }

        public override IEnumerator Run()
        {
            if(!force)
            {
                FileInfo fi = new FileInfo(kScene);
                if(fi.Exists)
                {
                    Debug.Log("Default scene exists. Skipping. ");
                    yield break;
                }
            }

            EditorSceneManager.NewScene(NewSceneSetup.EmptyScene);


        }

    }

}