using System;
using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using System.Reflection;

namespace UnityGameBase.Core.SceneMenu
{
	[InitializeOnLoad]
	public class SceneMenu
	{
		static SceneMenu()
		{
			//EditorApplication.update += 
			
			mInstance = new SceneMenu();
			EditorApplication.update += mInstance.OnUpdate;
			SceneView.onSceneGUIDelegate += mInstance.OnSceneGUI;
		}
		
		public static bool isVisible
		{
			get;
			private set;
		}
			 
		
		private static SceneMenu mInstance;
		
		const string kMenuTextField = "SceneMenuTextField";
		const int kWidth = 300;
		const int kHeight = 200;
		
		string mTextFieldContent = "";
		Vector2 mScrollPos = Vector2.zero;
		
		SceneMenuCommand mSelectedEntry;
		[SerializeField]
		public List<SceneMenuCommand>mMenuCommands;
		
		[NonSerialized]
		public List<SceneMenuCommand>mFilteredMenuCommands;
		

		bool mForceRedraw = false;
		bool mSceneMenuVisible = false;
		bool mLastCommandHadError = false;
		
		public SceneMenu()
		{
			if(mMenuCommands == null)
			{
				CreateMenuCommands();
			}
		}
		
		
		void OnUpdate()
		{
			if(mForceRedraw)
			{
				SceneView.currentDrawingSceneView.Repaint();
				mForceRedraw = false;
			}
			
			
		}
		
		
		public void OnSceneGUI(SceneView sceneView)	
		{
			
			if(Event.current != null && Event.current.type == EventType.keyDown)
			{
				OnKeyDown(Event.current.keyCode);
			}
			
			
			if(mSceneMenuVisible)
			{
				
				Handles.BeginGUI();
				
				
				
				GUI.skin = EditorGUIUtility.GetBuiltinSkin(EditorSkin.Inspector);
				GUI.color = new Color(0,0,0,0.8f);
				GUI.DrawTexture(new Rect((Screen.width - kWidth) / 2,120,kWidth,kHeight),EditorGUIUtility.whiteTexture);
				
				GUI.color = Color.white;
				
				GUILayout.BeginArea( new Rect((Screen.width - kWidth) / 2,120,kWidth,kHeight));
				
				GUILayout.Space(2);
				
				GUI.color = Color.white;
				GUILayout.BeginVertical(GUILayout.Width(kWidth));
				
				GUI.SetNextControlName(kMenuTextField);
				
				string filter = GUILayout.TextField(mTextFieldContent);
				
				if(filter != mTextFieldContent)
				{
					FilterCommands(filter);
					mTextFieldContent = filter;
				}
				
				
				if(mFilteredMenuCommands != null)
				{
					mScrollPos = GUILayout.BeginScrollView(mScrollPos);
					
					foreach(SceneMenuCommand c in mFilteredMenuCommands)
					{
						RenderMenuEntry(c);
					}
					
					GUILayout.EndScrollView();
				}
				
				
				
				GUILayout.EndVertical();
				GUILayout.EndArea();
				
				Handles.EndGUI();
				
				GUI.FocusControl(kMenuTextField);
				
			}
			
			
		}
		
		
		protected void OnKeyDown (KeyCode pKeyCode)
		{
				
			if(pKeyCode == KeyCode.Return && !mSceneMenuVisible)
			{
				OpenMenu();
			}
			
			if(pKeyCode == KeyCode.Escape && mSceneMenuVisible)
			{
				CloseMenu();
			}
			if(mSceneMenuVisible)
			{
				if(pKeyCode == KeyCode.UpArrow)
				{
					SelectNextMenuEntry();
					Event.current.Use();
				}
				if(pKeyCode == KeyCode.DownArrow)
				{
					SelectPreviousMenuEntry();
					Event.current.Use();
				}
				
				if(pKeyCode == KeyCode.Return)
				{
					ExecuteCommandAndClose();
					Event.current.Use();
				}
			}else
			{
				// iterate all registered commands
				Event ev = Event.current;
				foreach(SceneMenuCommand c in mMenuCommands)
				{
					if(c.WillHandle(ev))
					{
						Execute(c);
						break;
					}
				}
				
			}
		}
		void Execute(SceneMenuCommand pCommand)
		{
			
			pCommand.Execute();
			
			if(!mLastCommandHadError)
				SceneView.currentDrawingSceneView.ShowNotification(new GUIContent(pCommand.mName));
			
			mLastCommandHadError = false;
						
		}
		public static void ShowError(string pErrorText)
		{
			mInstance.mLastCommandHadError = true;
			SceneView.currentDrawingSceneView.ShowNotification(new GUIContent(pErrorText));
		}
		
		void RenderMenuEntry(SceneMenuCommand pCommand)
		{
			GUILayout.BeginHorizontal();
			if(pCommand == mSelectedEntry)
			{
				GUI.color = new Color(0.2f,0.6f,1.0f,1.0f);
			}else
			{
				GUI.color = Color.white;
			}
			
			GUILayout.Label(pCommand.mName,EditorStyles.whiteLabel);
			
			GUILayout.FlexibleSpace();
			
			GUILayout.Label(pCommand.GetFormattedShortCut(),EditorStyles.whiteLabel);
			
			GUILayout.EndHorizontal();
			
		}
		void OpenMenu()
		{
			mSceneMenuVisible = true;
			isVisible = mSceneMenuVisible;
			mSelectedEntry = null;
			GUI.FocusControl(kMenuTextField);
		}
		void CloseMenu()
		{
			mSceneMenuVisible = false;
			isVisible = mSceneMenuVisible;
			mForceRedraw = true;
		}
		void ExecuteCommandAndClose()
		{
			if(mSelectedEntry != null && mFilteredMenuCommands.IndexOf(mSelectedEntry) != -1)
			{
				Execute(mSelectedEntry);
				CloseMenu();
				
			}
		}
		void SelectPreviousMenuEntry()
		{
			if(mFilteredMenuCommands.Count == 0)
				return;
			
			if(!mFilteredMenuCommands.Contains( mSelectedEntry ))
			{
				mSelectedEntry = mFilteredMenuCommands[0];
				return;
			}
			
			int idx = mFilteredMenuCommands.IndexOf(mSelectedEntry) + 1;
			mSelectedEntry = mFilteredMenuCommands[ idx % mFilteredMenuCommands.Count ];
			
		}
		void SelectNextMenuEntry()
		{
			if(mFilteredMenuCommands.Count == 0)
				return;
			
			if(!mFilteredMenuCommands.Contains( mSelectedEntry ))
			{
				mSelectedEntry = mFilteredMenuCommands[0];
				return;
			}
			
			int idx = mFilteredMenuCommands.IndexOf(mSelectedEntry) - 1 + mFilteredMenuCommands.Count;
			mSelectedEntry = mFilteredMenuCommands[ idx % mFilteredMenuCommands.Count];
		}
		void FilterCommands(string pFilter)
		{
			pFilter = pFilter.ToLower();
			mFilteredMenuCommands = new List<SceneMenuCommand>();
			foreach(var c in mMenuCommands)
			{
				if(c.mName.ToLower().StartsWith(pFilter))
					mFilteredMenuCommands.Add(c);
			}
			if(mFilteredMenuCommands.Count > 0)
				mSelectedEntry = mFilteredMenuCommands[0];
			else
				mSelectedEntry = null;
		}
		
		void CreateMenuCommands()
		{
			mMenuCommands = new List<SceneMenuCommand>();
		
			
			foreach(Type t in GetTypesSubclassOf(typeof(SceneMenuCommand)))
			{
				mMenuCommands.Add( Activator.CreateInstance( t ) as SceneMenuCommand);
			}

			
			FilterCommands("");
			
			
		}
		
		static IEnumerable<Type> GetTypesSubclassOf(System.Type pParentType)
		{
			Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
		
			foreach(Assembly a in assemblies)
	    		foreach(Type type in a.GetTypes())
				{
					if(!type.IsAbstract && type.IsClass && type.IsSubclassOf(pParentType))
					{
						yield return type;
					}
			   	}
		}

		
	}

}