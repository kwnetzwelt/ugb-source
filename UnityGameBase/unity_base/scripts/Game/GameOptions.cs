using UnityEngine;
using System.Collections;
using System;


/// <summary>
/// Contains code for setting game options such as language, sound/music enabled/disabled, 
/// the Quality setting and if touch feedback should be visible
/// </summary>
public class GameOptions : GameComponent
{
	const string kOptSound = "OptSound";
	const string kOptMusic = "OptMusic";
	const string kOptQuality = "OptQuality";
	public const string kOptLanguage = "OptLanguage";
	const string kOptTouchFeedback = "OptTouchFeedback";
	
	private bool mOptionsDialogVisible = false;
	
	public bool isOptionsDialogVisible
	{
		get
		{
			return mOptionsDialogVisible;
		}
		set
		{
			if(value != mOptionsDialogVisible)
			{
				mOptionsDialogVisible = value;
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
		mSoundOption = System.Convert.ToBoolean( PlayerPrefs.GetInt(kOptSound,1) );
		mMusicOption = System.Convert.ToBoolean( PlayerPrefs.GetInt(kOptMusic,1) );
		mQualityLevel = PlayerPrefs.GetInt(kOptQuality,0);

		mLanguage = PlayerPrefs.GetInt(kOptLanguage,SLanguages.first);

		mShowTouchFeedback = System.Convert.ToBoolean( PlayerPrefs.GetInt(kOptTouchFeedback,1));
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
			PlayerPrefs.SetInt(kOptQuality,pInt);
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
			PlayerPrefs.SetInt(kOptLanguage,(int)value);
			PlayerPrefs.Save();
			UpdateValues();
		}
	}
	
	public bool isSoundOn
	{
		get { return mSoundOption;}
		set {
			
			PlayerPrefs.SetInt(kOptSound,(value)? 1 : 0);
			PlayerPrefs.Save();
			UpdateValues();
		}
	}
	
	
	public bool isMusicOn
	{
		get { return mMusicOption;}
		set {
			PlayerPrefs.SetInt(kOptMusic,(value)? 1 : 0);
			PlayerPrefs.Save();
			UpdateValues();
		}
	}
	
	public bool showTouchFeedBack
	{
		get { return mShowTouchFeedback;}
		set {
			PlayerPrefs.SetInt(kOptTouchFeedback,(value)? 1: 0);
			PlayerPrefs.Save();
			UpdateValues();
		}
	}
	
}

