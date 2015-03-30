using System;
using UnityEngine;

namespace UnityGameBase.Core.Utils
{
	public class UIHelpers
	{
		/// <summary>
		/// A 4x4 pixel sized texture with transparent(0,0,0,0) color. It is created once and can be used multiple times. 
		/// </summary>
		/// <value>The transparent texture.</value>
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

		/// <summary>
		/// A 4x4 pixel sized texture with white(1,1,1,1) color. It is created once and can be used multiple times. 
		/// </summary>
		/// <value>The white texture.</value>
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

		/// <summary>
		/// A 4x4 pixel sized texture with black (0,0,0,1) color. It is created once and can be used multiple times. 
		/// </summary>
		/// <value>The black texture.</value>
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
		
		

		/// <summary>
		/// Returns true if the current screen is considered large. It has more than 1200 pixels width. 
		/// </summary>
		/// <value><c>true</c> if large screen; otherwise, <c>false</c>.</value>
		public static bool LargeScreen
		{
			get 
			{
				return Screen.width > 1200;
			}
		}

		/// <summary>
		/// Returns the same as Screen.dpi unless it returns 0, then this returns 90 as a default value. 
		/// </summary>
		/// <value>The dpi.</value>
		public static float Dpi
		{
			get {
				float dpi = Screen.dpi;
				if(dpi != 0)
					return dpi;
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

		/// <summary>
		/// All four components of the rect (x,y,width and height) are multiplied by the given ratio. 
		/// The resulting rectangle is returned. 
		/// </summary>
		/// <returns>The rect.</returns>
		/// <param name="rect">Rect.</param>
		/// <param name="ratio">Ratio.</param>
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