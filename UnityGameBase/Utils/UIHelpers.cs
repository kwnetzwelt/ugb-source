using System;
using UnityEngine;

namespace UGB.Utils
{
	public class UIHelpers
	{
		public static Texture2D TransparentTexture {
			get {
				if(transparentTexture == null)
				{
					transparentTexture = new Texture2D(4,4);
					for(int x = 0;x < 4;x++)
						for(int y = 0;y < 4;y++)
							transparentTexture.SetPixel(x,y,new Color(0,0,0,0));
					transparentTexture.Apply();
					transparentTexture.Compress(true);
				}
				return transparentTexture;
			}
		}
		private static Texture2D transparentTexture;
		
		public static Texture2D WhiteTexture
		{
			get {
				if(whiteTexture == null)
				{
					whiteTexture = new Texture2D(4,4);
					for(int x = 0;x < 4;x++)
						for(int y = 0;y < 4;y++)
							whiteTexture.SetPixel(x,y,Color.white);
					whiteTexture.Apply();
					whiteTexture.Compress(true);
				}
				return whiteTexture;
			}
		}
		private static Texture2D whiteTexture;

		public static Texture2D BlackTexture
		{
			get {
				if(blackTexture == null)
				{
					blackTexture = new Texture2D(4,4);
					for(int x = 0;x < 4;x++)
						for(int y = 0;y < 4;y++)
							blackTexture.SetPixel(x,y,Color.black);
					blackTexture.Apply();
					blackTexture.Compress(true);
				}
				return blackTexture;
			}
		}
		private static Texture2D blackTexture;
		
		
		
		public static bool LargeScreen
		{
			get 
			{
				return Screen.width > 1200;
			}
		}
		
		public static float Dpi
		{
			get {
				if(Screen.dpi != 0)
					return Screen.dpi;
				return 90;
			}
		}

		/// <summary>
		/// Gets the scaled style name according to screen size and resolution. Appends Retina,Tablet,Normal or Desktop
		/// </summary>
		/// <returns>
		/// The scaled style.
		/// </returns>
		/// <param name='pStyleName'>
		/// _style name.
		/// </param>
		public static string GetScaledStyle (string styleName)
		{
			if(LargeScreen)
				return styleName + "Tablet";
			return styleName + "Normal";
			
		}
		
		public static Rect ScaleRect(Rect rect, float ratio)
		{
			if(ratio == 1)
				return rect;
			rect.x *= ratio;
			rect.y *= ratio;
			rect.width *= ratio;
			rect.height *= ratio;
			return rect;
		}
	}

}