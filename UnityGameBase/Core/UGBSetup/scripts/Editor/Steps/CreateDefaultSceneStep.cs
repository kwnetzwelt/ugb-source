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
        const string kScene = "Assets/scenes/default.unity";
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

            if(EditorApplication.isUpdating || EditorApplication.isCompiling)
            {
                Debug.Log("Wait");
                yield return 0;
            }
            
            var scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene);

            GameObject go = new GameObject();
            go.name = kGameRoot;
           
            System.Type type = UGBSetupPostProcessor.GetGameInitializerType();
            go.AddComponent(type);
           
            EditorSceneManager.SaveScene(scene, kScene);
        }

    }

}