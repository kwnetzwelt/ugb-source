using UnityEngine;
using System.Collections;

namespace UGB
{
	/// <summary>
	/// A Text Mesh based text which is localized using the current language. 
	/// </summary>
	public class LocalizableText : GameComponent
	{
		public string locaKey;
		LString translation;
		TextMesh textMesh;
		GUIText textDisplay;
		// Use this for initialization
		void Start ()
		{
			
			translation = locaKey;
			textMesh = this.GetComponent<TextMesh>();
			textDisplay = this.GetComponent<GUIText>();
			if(textMesh != null)
			{
				textMesh.text = translation;
			}
			
			if(textDisplay != null)
			{
				textDisplay.text = translation;
			}
		}
		
	}

}