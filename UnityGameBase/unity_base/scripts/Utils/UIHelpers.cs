using System;
using UnityEngine;

namespace UGB.Utils
{
	public class UIHelpers
	{
		public const float kActivatedAlpha = 0.3f;
		public const float kHoverAlpha = 0.1f;
		public const float kNormalAlpha = 0.05f;
		
		public static Texture2D whiteTexture
		{
			get {
				if(mWhiteTexture == null)
				{
					mWhiteTexture = new Texture2D(4,4);
					for(int x = 0;x < 4;x++)
						for(int y = 0;y < 4;y++)
							mWhiteTexture.SetPixel(x,y,Color.white);
					mWhiteTexture.Apply();
					mWhiteTexture.Compress(true);
				}
				return mWhiteTexture;
			}
		}
		private static Texture2D mWhiteTexture;

		public static Texture2D blackTexture
		{
			get {
				if(mBlackTexture == null)
				{
					mBlackTexture = new Texture2D(4,4);
					for(int x = 0;x < 4;x++)
						for(int y = 0;y < 4;y++)
							mBlackTexture.SetPixel(x,y,Color.black);
					mBlackTexture.Apply();
					mBlackTexture.Compress(true);
				}
				return mBlackTexture;
			}
		}
		private static Texture2D mBlackTexture;
		
		
		
		public static bool largeScreen
		{
			get 
			{
				return Screen.width > 1200;
			}
		}
		
		public static float dpi
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
		public static string GetScaledStyle (string pStyleName)
		{
			if(largeScreen)
				return pStyleName + "Tablet";
			return pStyleName + "Normal";
			
		}
		
		public static Rect ScaleRect(Rect pRect, float pRatio)
		{
			if(pRatio == 1)
				return pRect;
			pRect.x *= pRatio;
			pRect.y *= pRatio;
			pRect.width *= pRatio;
			pRect.height *= pRatio;
			return pRect;
		}
	}

}