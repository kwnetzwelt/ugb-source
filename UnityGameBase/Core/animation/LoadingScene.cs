using System;
using UnityEngine;

namespace UnityGameBase.Core.Animation
{
    /// <summary>
    /// Component for use with LoadingSceneController. 
    /// <see cref="LoadingSceneController"/>
    /// </summary>
    public class LoadingScene : MonoBehaviour
    {
        public AnimationClip inAnimation;
        public AnimationClip loopAnimation;
        public AnimationClip outAnimation;

        public Camera loadingCamera;

        void Awake()
        {
            GameObject.DontDestroyOnLoad(this.gameObject);
            if (loadingCamera != null)
            {
                GameObject.DontDestroyOnLoad(loadingCamera.gameObject);
            }
        }
    }
}