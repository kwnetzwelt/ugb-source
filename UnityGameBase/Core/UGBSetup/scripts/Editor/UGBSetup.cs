using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;

namespace UnityGameBase.Core.Setup
{
    public class UGBSetup
    {
        public bool force
        {
            get;
            set;
        }

        List<UGBSetupStep> mSteps = new List<UGBSetupStep>();
        public UGBSetup()
        {
            mSteps.Add(new CreateFoldersStep());
            mSteps.Add(new CreateDefaultSceneStep());
            mSteps.Add(new CreateGameLogicClass());
            //mSteps.Add( new OpenLogicClassInMD() );
        }

        public float progress
        {
            get;
            private set;
        }

        public IEnumerable<string> Steps()
        {
            foreach(var step in mSteps)
            {
                yield return step.GetName();
            }
        }

        public IEnumerator<string> Run()
        {
            progress = 0;
            float frag = 1 / (float)mSteps.Count;
            foreach(var step in mSteps)
            {
                step.force = force;
                yield return step.GetName();
                progress += frag;
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


