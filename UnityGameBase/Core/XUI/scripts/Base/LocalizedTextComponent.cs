using UnityEngine;
using System.Collections;
using UnityGameBase;

namespace UnityGameBase.Core.XUI
{
    /// <summary>
    /// this class represents an UI Text and manage the localization Stuff
    /// </summary>
    [System.Serializable]
    public class LocalizedTextComponent : UnityEngine.UI.Text, IWidget
    {
        [SerializeField, HideInInspector]
        public bool
            useLocaFiles = false;
        private object[] mParams;

        //private string[] locaKeys = null;

        public object[] Params
        {
            get { return mParams; }
            set
            {
                mParams = value;
                ReCreate();
            }
        }

        [SerializeField, HideInInspector]
        private string
            key = "Key Here";
        public string Key
        {
            get { return key; }
            set
            {
                key = value;
                ReCreate();
            }
        }

        protected override void OnEnable()
        {
            base.OnEnable();

            if(Game.Instance != null
                && useLocaFiles)
            {
                this.text = Game.Instance.gameLoca.GetText(this.key);
            }


            if(this.text == "")
            {
                this.text = "NAME HERE";
            }

        }

        protected void HandleLocaChanged()
        {
            //TODO ingame loca change
            ReCreate();
        }

        private string FormatText(string _key, params object[] _params)
        {           
            return System.String.Format(_key, _params);            
        }

        public void ReCreate()
        {
            //in editor show only the translated text 
#if UNITY_EDITOR
            if(useLocaFiles)
            {
                this.text = LocalizationHelper.GetText(this.key);                
            }
            
            //locaKeys = LocalizationHelper.GetKeys();
#else
            if (useLocaFiles)
            {
                this.text = UGB.Loca.GetText(this.key);
            }
#endif
        }
    }
}
