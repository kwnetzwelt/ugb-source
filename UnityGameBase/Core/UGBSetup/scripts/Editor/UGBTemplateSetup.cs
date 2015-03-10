using UnityEngine;
using UnityEditor;
using System.IO;
using System.Linq;


namespace UnityGameBase.Core.Setup
{
	public class UGBTemplateSetup : UGBWindowBase
	{
		const string kFileName = "UGBTemplates.dll";
		const string kDllSourcePath = "Assets/packages/UnityGameBase/UGBSetup/mdTemplates/" + kFileName;

		[SerializeField]
		string mSelectedPath;

		Object dllSource;

		[MenuItem("UGB/Setup/MonoDevelop Templates")]
		public static void Init ()
		{
			var window = EditorWindow.GetWindow <UGBTemplateSetup> (true, "MonoDevelop Template Setup", true);
			window.minSize = new Vector2(320,300);
			window.maxSize = new Vector2(320,300);
			window.Focus ();


		}
		protected override void OnEnable()
		{
			base.OnEnable();

			dllSource = AssetDatabase.LoadAssetAtPath( kDllSourcePath, typeof(Object));
			if(dllSource == null)
			{
				Debug.LogError("Could not find Source dll: " + kDllSourcePath);
				Close();
			}
		}

		protected override void OnGUI ()
		{
			base.OnGUI();

			if(dllSource == null)
			{
				return;
			}
			GUILayout.BeginVertical();
			
			GUILayout.Label("Templates Setup", mTitleStyle);

			GUILayout.Label("This will copy a dll containing some templates for MonoDevelop's File=>New Dialog to your MonoDevelop Addin Folder. ", mTextStyle);
			
			GUILayout.Space(3);


			if(GUILayout.Button("Select MonoDevelop Executable"))
			{
				SelectMD();
			}

			GUILayout.Label(mSelectedPath);



			GUI.enabled = !string.IsNullOrEmpty( mSelectedPath );
			if(!ChkFileExists())
			{
				if(GUILayout.Button("Copy Templates to Addins Folder"))
				{
					Copy();
				}
			}

			GUI.enabled =true;

			GUILayout.FlexibleSpace();

			if(string.IsNullOrEmpty(mSelectedPath))
			{
				#if UNITY_EDITOR_OSX
				EditorGUILayout.HelpBox("Select your MonoDevelop App", MessageType.Info);
				#else
				EditorGUILayout.HelpBox("Select your MonoDevelop Executable", MessageType.Info);
				#endif
			}

			if(ChkFileExists())
			{
				EditorGUILayout.HelpBox(kFileName + " is already in place. ", MessageType.Info);
			}

			if(GUILayout.Button("Show Templates"))
			{

				EditorGUIUtility.PingObject( dllSource );
			}


			GUILayout.EndVertical();
		}

		void SelectMD()
		{
#if UNITY_EDITOR_OSX
			string path = EditorUtility.OpenFilePanel("Select MonoDevelop Executable", "/Applications", "app");

#else
			string path = EditorUtility.OpenFilePanel("Select MonoDevelop Executable", "", "exe");
#endif
			Debug.Log(path);
			mSelectedPath = path;

		}
		void Copy()
		{
			File.Copy(kDllSourcePath, TargetPath);
		}

		bool ChkFileExists()
		{
			return File.Exists(TargetPath);
		}
		string TargetPath
		{
			get
			{
				#if UNITY_EDITOR_OSX
				return mSelectedPath + "/Contents/MacOS/lib/monodevelop/AddIns/" + kFileName;
				#else
				DirectoryInfo di = new DirectoryInfo( mSelectedPath );

				return di.Parent.Parent.FullName + Path.DirectorySeparatorChar + "AddIns" + Path.DirectorySeparatorChar + kFileName;
				#endif
			}
		}
	}

}