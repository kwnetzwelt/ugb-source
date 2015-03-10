using System;

namespace UnityGameBase.Core.Globalization
{
	/// <summary>
	/// Localized string.
	/// </summary>
	public class LString
	{
		private string mKey;
		private string mTranslation;

		public static implicit operator LString(string pValue)
		{
			
			return new LString(pValue);
			
		}
		
		public static implicit operator string(LString pValue)
		{
			if(pValue == null)
				return "";
			return pValue.ToString();
		}
		
		public LString(string pKey)
		{
			mKey = pKey;
			if(Game.Instance == null)
			{
				mTranslation = pKey;
				return;
			}
			mTranslation = Game.Instance.gameLoca.GetText(mKey);
			
		}
		
		public override string ToString ()
		{
			return mTranslation;
		}
		
		public string[] Split(params char[] pSeparator)
		{
			return mTranslation.Split(pSeparator);
		}
		
		public string Params(params object[] pParams)
		{
			return String.Format(mTranslation,pParams);
		}
	}		
}