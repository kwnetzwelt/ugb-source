using UnityEngine;
using System.Collections;
using UnityEditor;
using System;
using System.Collections.Generic;


namespace UnityGameBase.Core
{



    [InitializeOnLoad]
    public class BlenderShortCuts
    {
	
        static BlenderShortCuts mInstance;
	
        SceneView mSceneView;
	
        void OnUpdate()
        {
        }

        Dictionary<KeyCode,System.Action<Event>> shortCuts;
        bool init = false;
        void OnEnable()
        {
            init = true;
            shortCuts = new Dictionary<KeyCode, Action<Event>>(){
			{ KeyCode.Keypad7, ViewTop },
			{ KeyCode.Keypad1, ViewFront },
			{ KeyCode.Keypad3, ViewRight },
			{ KeyCode.Keypad5, ViewOrthoToggle },

			{ KeyCode.Keypad4, ViewToLeft },
			{ KeyCode.Keypad6, ViewToRight },
			{ KeyCode.Keypad8, ViewToTop },
			{ KeyCode.Keypad2, ViewToBottom },

			{ KeyCode.X, Delete}
			
			
		};
        }

        void OnSceneGUI(SceneView sceneView)
        {
            if(!init)
            {
                OnEnable();
            }
            mSceneView = sceneView;
            if(Event.current != null && Event.current.type == EventType.keyDown)
            {
                foreach(var kv in shortCuts)
                {
                    if(kv.Key == Event.current.keyCode)
                    {
                        kv.Value(Event.current);
                        Event.current.Use();
                    }
                }
            }
        }


        static BlenderShortCuts()
        {
            mInstance = new BlenderShortCuts();
		
            EditorApplication.update += mInstance.OnUpdate;
            SceneView.onSceneGUIDelegate += mInstance.OnSceneGUI;
        }


        void Delete(Event e)
        {
            var obj = Selection.activeGameObject;
            Undo.DestroyObjectImmediate(obj);
        }
	#region view
        void ViewTop(Event e)
        {
		
            var obj = GetCurrentTransform();
            mSceneView.LookAtDirect(obj.transform.position,
		                        Quaternion.LookRotation(Vector3.down));
        }

        void ViewFront(Event e)
        {
            var obj = GetCurrentTransform();
            mSceneView.LookAtDirect(obj.transform.position,
		                        Quaternion.LookRotation(Vector3.forward));
        }

        void ViewRight(Event e)
        {
            var obj = GetCurrentTransform();
            mSceneView.LookAtDirect(obj.transform.position,
		                        Quaternion.LookRotation(Vector3.right));
        }
        void ViewOrthoToggle(Event e)
        {
            mSceneView.orthographic = ! mSceneView.orthographic;
        }

        void ViewToLeft(Event e)
        {
            Quaternion q = mSceneView.rotation;
            q = Quaternion.Euler(q.eulerAngles - new Vector3(0, -15, 0));
            mSceneView.rotation = q;
        }

        void ViewToRight(Event e)
        {
            Quaternion q = mSceneView.rotation;
            q = Quaternion.Euler(q.eulerAngles + new Vector3(0, -15, 0));
            mSceneView.rotation = q;
        }

        void ViewToTop(Event e)
        {
            Quaternion q = mSceneView.rotation;
            q = Quaternion.Euler(q.eulerAngles - new Vector3(-15, 0, 0));
            mSceneView.rotation = q;
        }

        void ViewToBottom(Event e)
        {
            Quaternion q = mSceneView.rotation;
            q = Quaternion.Euler(q.eulerAngles + new Vector3(-15, 0, 0));

            mSceneView.rotation = q;
        }

        Transform GetCurrentTransform()
        {
            var obj = Selection.activeGameObject;
            if(obj != null)
            {
                return obj.transform;
            }
            return mSceneView.camera.transform;
        }
	#endregion
    }
}