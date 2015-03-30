using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace UnityGameBase.Core.XUI
{
/// <summary>
/// this class is used to Add or Get a BaseScreen
/// </summary>
	public class ScreenManager
	{
		Dictionary<string,BaseScreen> screens = new Dictionary<string, BaseScreen>();
		
		private static ScreenManager instance = new ScreenManager();
		public static ScreenManager Instance
		{
			get{ return instance;}
		}
				
		public void AddScreen(BaseScreen screen)
		{
			string name = screen.screenName;
			if (!this.screens.ContainsKey(name))
				this.screens.Add(name, screen);
			else
				Debug.LogError("Screen with name: " + name + " already exists!");
		}
				
		public T GetScreen<T>(string name = "Default") where T : BaseScreen
		{			
			if (this.screens.ContainsKey(name))
				return this.screens [name] as T;
				
			return null;
		}		
	}
}
