using System;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace UnityGameBase.Core.Setup
{
    internal class AttachGameLogic : UGBSetupStep
    {
        #region implemented abstract members of UGBSetupStep
        public override string GetName()
        {
            return "Attach GameLogic to default scene";
        }
        public override System.Collections.IEnumerator Run()
        {
            yield return GetName();

            var scene = EditorSceneManager.GetActiveScene();
            GameObject go = new GameObject();
            go.name = CreateDefaultSceneStep.kGameRoot;
            var logicType = GetGameLogicType();
            if (logicType != null)
            {
                go.AddComponent(logicType);
            }
            EditorSceneManager.SaveScene(scene, CreateDefaultSceneStep.kScene);

        }
        #endregion

        System.Type GetGameLogicType()
        {
            var className = CreateGameLogicClass.LogicClassName();

            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                foreach (var t in assembly.GetTypes())
                {
                    if (t.Name == className)
                    {
                        return t;
                    }
                }
            }
            return null;
    
        }

    }
}

