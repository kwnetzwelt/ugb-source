using UnityEngine;
using System.Collections;
using System;
using UnityGameBase.Core.Globalization;

namespace UnityGameBase.Core
{
	/// <summary>
	/// Contains code for setting game options such as language, sound/music enabled/disabled, 
	/// the Quality setting and if touch feedback should be visible
	/// </summary>
	public class GameOptions : MonoBehaviour
	{
		const string SoundOption = "OptSound";
		const string MusicOption = "OptMusic";
		const string QualityOption = "OptQuality";
		public const string LanguageOption = "OptLanguage";
		const string TouchFeedbackOption = "OptTouchFeedback";
		
		private bool optionsDialogVisible = false;

		
		
		bool musicOption;
		bool moundOption;
		int qualityLevel;
		int language;
		bool showTouchFeedback;

		public delegate void OnOptionChange();
		public event OnOptionChange OnAnyOptionChanged;
		public event OnOptionChange OnOptionDialogToggled;


		public bool IsOptionsDialogVisible
		{
			get
			{
				return optionsDialogVisible;
			}
			set
			{
				if(value != optionsDialogVisible)
				{
					optionsDialogVisible = value;
					if(OnOptionDialogToggled != null)
						OnOptionDialogToggled();
				}
				
			}
		}

		public int NextQuality
		{
			get {
				return (GetQuality() + 1) % QualitySettings.names.Length;
			}
		}
		public void SetQuality(int value)
		{
			if(qualityLevel != value)
			{
				PlayerPrefs.SetInt(QualityOption,value);
				PlayerPrefs.Save();
				UpdateValues();
			}
			
		}
		
		public int GetQuality()
		{
			return qualityLevel;
		}
		
		public Languages NextLanguage
		{
			
			get {
				int i = 0;
				foreach(int lang in Languages.Enumerate())
				{
					if(lang == UGB.Loca.currentLanguage)
					{
						break;
					}
				}
				
				i = (i + 1) % Languages.count;
				return (Languages)i;
			}
		}
		
		public Languages CurrentLanguage
		{
			get {
				return language;
			}
			set {
				PlayerPrefs.SetInt(LanguageOption,(int)value);
				PlayerPrefs.Save();
				UpdateValues();
			}
		}
		
		public bool IsSoundOn
		{
			get { return moundOption;}
			set {
				
				PlayerPrefs.SetInt(SoundOption,(value)? 1 : 0);
				PlayerPrefs.Save();
				UpdateValues();
			}
		}
		
		
		public bool IsMusicOn
		{
			get { return musicOption;}
			set {
				PlayerPrefs.SetInt(MusicOption,(value)? 1 : 0);
				PlayerPrefs.Save();
				UpdateValues();
			}
		}
		
		public bool ShowTouchFeedBack
		{
			get { return showTouchFeedback;}
			set {
				PlayerPrefs.SetInt(TouchFeedbackOption,(value)? 1: 0);
				PlayerPrefs.Save();
				UpdateValues();
			}
		}

		void Start()
		{
			UpdateValues();
		}
		protected virtual void UpdateValues()
		{
			moundOption = System.Convert.ToBoolean( PlayerPrefs.GetInt(SoundOption,1) );
			musicOption = System.Convert.ToBoolean( PlayerPrefs.GetInt(MusicOption,1) );
			qualityLevel = PlayerPrefs.GetInt(QualityOption,0);

			language = PlayerPrefs.GetInt(LanguageOption,Languages.First);

			showTouchFeedback = System.Convert.ToBoolean( PlayerPrefs.GetInt(TouchFeedbackOption,1));
			UGB.Loca.SetLanguage(language);
#if !UNITY_WEBGL
			QualitySettings.SetQualityLevel(qualityLevel);
#endif
			if(OnAnyOptionChanged != null)
				OnAnyOptionChanged();
		}

		
		

		
		

		
	}

}