using UnityEngine;
using System.Collections;
using System;

namespace UGB
{
	/// <summary>
	/// Contains code for setting game options such as language, sound/music enabled/disabled, 
	/// the Quality setting and if touch feedback should be visible
	/// </summary>
	public class GameOptions : GameComponent
	{
		const string SoundOption = "OptSound";
		const string MusicOption = "OptMusic";
		const string QualityOption = "OptQuality";
		public const string LanguageOption = "OptLanguage";
		const string TouchFeedbackOption = "OptTouchFeedback";
		
		private bool optionsDialogVisible = false;
		
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
		
		void Start()
		{
			UpdateValues();
		}
		protected virtual void UpdateValues()
		{
			mSoundOption = System.Convert.ToBoolean( PlayerPrefs.GetInt(SoundOption,1) );
			mMusicOption = System.Convert.ToBoolean( PlayerPrefs.GetInt(MusicOption,1) );
			mQualityLevel = PlayerPrefs.GetInt(QualityOption,0);

			mLanguage = PlayerPrefs.GetInt(LanguageOption,SLanguages.first);

			mShowTouchFeedback = System.Convert.ToBoolean( PlayerPrefs.GetInt(TouchFeedbackOption,1));
			GLoca.SetLanguage(mLanguage);
			QualitySettings.SetQualityLevel(mQualityLevel);
			
			if(OnAnyOptionChanged != null)
				OnAnyOptionChanged();
		}
		
		
		bool mMusicOption;
		bool mSoundOption;
		int mQualityLevel;
		int mLanguage;
		bool mShowTouchFeedback;
		
		
		public delegate void OnOptionChange();
		public event OnOptionChange OnAnyOptionChanged;
		public event OnOptionChange OnOptionDialogToggled;
		
		
		
		public int nextQuality
		{
			get {
				return (GetQuality() + 1) % QualitySettings.names.Length;
			}
		}
		public void SetQuality(int pInt)
		{
			if(mQualityLevel != pInt)
			{
				PlayerPrefs.SetInt(QualityOption,pInt);
				PlayerPrefs.Save();
				UpdateValues();
			}
			
		}
		
		public int GetQuality()
		{
			return mQualityLevel;
		}
		
		public SLanguages nextLanguage
		{
			
			get {
				int i = 0;
				foreach(int lang in SLanguages.Enumerate())
				{
					if(lang == GLoca.currentLanguage)
					{
						break;
					}
				}
				
				i = (i + 1) % SLanguages.count;
				return (SLanguages)i;
			}
		}
		
		public SLanguages language
		{
			get {
				 return mLanguage;
			}
			set {
				PlayerPrefs.SetInt(LanguageOption,(int)value);
				PlayerPrefs.Save();
				UpdateValues();
			}
		}
		
		public bool isSoundOn
		{
			get { return mSoundOption;}
			set {
				
				PlayerPrefs.SetInt(SoundOption,(value)? 1 : 0);
				PlayerPrefs.Save();
				UpdateValues();
			}
		}
		
		
		public bool isMusicOn
		{
			get { return mMusicOption;}
			set {
				PlayerPrefs.SetInt(MusicOption,(value)? 1 : 0);
				PlayerPrefs.Save();
				UpdateValues();
			}
		}
		
		public bool showTouchFeedBack
		{
			get { return mShowTouchFeedback;}
			set {
				PlayerPrefs.SetInt(TouchFeedbackOption,(value)? 1: 0);
				PlayerPrefs.Save();
				UpdateValues();
			}
		}
		
	}

}