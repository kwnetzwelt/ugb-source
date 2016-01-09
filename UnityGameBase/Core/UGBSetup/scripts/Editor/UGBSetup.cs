using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using System;

namespace UnityGameBase.Core.Setup
{
    [Serializable]
    public class UGBSetup
    {
        [SerializeField]
        int currentStepIndex;

        [SerializeField]
        bool force;


        public bool Force
        {
            get { return force; }
            set { force = value; }
        }

        List<UGBSetupStep> mSteps = new List<UGBSetupStep>();
        public UGBSetup()
        {
            mSteps.Add(new CreateFoldersStep());
            mSteps.Add(new CreateDefaultSceneStep());
            mSteps.Add(new CreateGameLogicClass());
            mSteps.Add(new AttachGameLogic());
            //mSteps.Add( new OpenLogicClassInMD() );
        }

        [SerializeField]
        float progress;

        public float Progress
        {
            get {
                return progress;
            }

            private set {
                progress = value;
            }
        }

        public IEnumerable<string> Steps()
        {
            foreach(var step in mSteps)
            {
                yield return step.GetName();
            }
        }

        public void Reset()
        {
            currentStepIndex = 0;
            Progress = 0;
        }

        public IEnumerator<string> Resume()
        {
            currentStepIndex++;
            return Run();
        }

        public IEnumerator<string> Run()
        {
            float frag = 1 / (float)mSteps.Count;
            for (; currentStepIndex < mSteps.Count; currentStepIndex++)
            {
                var step = mSteps[currentStepIndex];
                step.force = Force;
                yield return step.GetName();
                Progress += frag;
                var ien = step.Run();

                while(ien.MoveNext())
                {
                    yield return step.GetName();
                }
            }
            yield return "Done.";
        }
    }
}


