using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace UnityGameBase.Core.XUI
{
    /// <summary>
    /// this is the root screen component and the parent canvas of all UI elements for this screen 
    /// </summary>	
    public class BaseScreen : MonoBehaviour
    {
        public string screenName = "Default"; 		
        public GameObject screenPrefab = null;
				
        protected GameObject screenInstance = null;
        protected Transform root;
        protected WidgetManager widgetManager;
        protected TransitionController transitionController;
									
        public T GetWidget<T>(string _layer, string _name) where T : Component,IWidget
        {
            if(this.widgetManager == null)
            {
                return null;
            }
				
            WidgetData widgetData = this.widgetManager.GetWidget(_layer, _name);
			
            if(widgetData == null)
            {
                return null;
            }
			
			
            return widgetData.widgetObject.GetComponent<T>();
        }

        public virtual void Show(System.Action onDone = null)
        {
            //init the screen and load the instance if not exist
            if(this.screenInstance == null)
            {
                this.InitScreen();
            }
			
            //activate the screen and disable input until the screen transtion is ready
            this.root.gameObject.SetActive(true);
            this.EnableInput(false);
			
            this.transitionController.Show(() => 
            {
                this.EnableInput(true);
                
                if(onDone != null)
                {
                    onDone();
                }
            });
        }
		
		
        public virtual void Hide(System.Action onDone = null)
        {
            this.EnableInput(false);
            this.transitionController.Hide(() => 
            {                
                this.root.gameObject.SetActive(false);
                if(onDone != null)
                {
                    onDone();
                }
				
            });
        }

        public void SetScreenInstance(GameObject instance)
        {
            this.screenInstance = instance;
            this.InitScreen();
        }

        private void InitScreen()
        {
            if(this.screenPrefab == null)
            {
                Debug.LogError(this.name + " missing the prefab reference!");
                return;
            }
			
            if(this.screenInstance == null)
            {
                this.screenInstance = GameObject.Instantiate(this.screenPrefab);
            }
			
            //remove the parent object cause its only for prefab purposes
            GameObject tempParent = this.screenInstance;
            GameObject tempScreen = this.screenInstance.transform.GetChild(0).gameObject;
            
            
						
            //Remove the parent			
            tempScreen.transform.SetParent(null);
            Destroy(tempParent);
            this.screenInstance = tempScreen;
			
            this.screenInstance.transform.SetParent(this.transform);
            
            this.widgetManager = this.screenInstance.GetComponent<WidgetManager>();
			
            //initialize the transitioncontroller
            this.transitionController = this.screenInstance.GetComponent<ScreenTransition>().AddController(this.screenInstance);
            this.transitionController.Init(this.screenInstance);
			
            //deactivate the root object
            root = this.screenInstance.transform.GetChild(0);
            root.gameObject.SetActive(false);
        }
		
        protected void OnEnable()
        {
            //add this screen to screenManager
            ScreenManager.Instance.AddScreen(this);
        }
		
        public void EnableInput(bool enable)
        {
            //disable the raycast on the root object
            this.screenInstance.GetComponent<UnityEngine.UI.GraphicRaycaster>().enabled = enable;
        }

    }
}
