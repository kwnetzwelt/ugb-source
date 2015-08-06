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
				if(value != key)
				{
	                key = value;
	                ReCreate();
				}
            }
        }

		public override string text {
			get {
				return base.text;
			}
			set {
				base.text = value;
				useLocaFiles = false;
			}
		}

        protected override void OnEnable()
        {
            base.OnEnable();

            if(Game.Instance != null
                && useLocaFiles)
            {
				ReCreate();
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
				if(Application.isPlaying)
				{
					base.text = UGB.Loca.GetText(this.key);
				}
				else
				{
					base.text = LocalizationHelper.GetText(this.key);       
				}
            }
            
            //locaKeys = LocalizationHelper.GetKeys();
#else
            if (useLocaFiles)
            {
                base.text = UGB.Loca.GetText(this.key);
            }
#endif
        }
    }
}
