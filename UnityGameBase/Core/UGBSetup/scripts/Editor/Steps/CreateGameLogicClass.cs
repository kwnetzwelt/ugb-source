using UnityEngine;
using System.Collections;
using System;
using System.IO;
using UnityEditor;
using UnityGameBase.Core;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Text;

namespace UnityGameBase.Core.Setup
{
	internal class CreateGameLogicClass : UGBSetupStep
	{
		
		
		public static string LogicClassFile()
		{
            
            return "Assets/scripts/" + LogicClassName() + ".cs";
		}

        public static string LogicClassName()
        {
            
            DirectoryInfo di = new DirectoryInfo(Application.dataPath);
            //string pattern = @"\W|_";
            //string[] result = Regex.Split( di.Parent.Name , pattern, RegexOptions.IgnoreCase);

            return CamelCase(di.Parent.Name);

        }

		public override string GetName ()
		{
			return "Create GameLogic class";
		}

		public override IEnumerator Run ()
		{
			if(!force)
			{
				System.Type type = GetLogicClassType();
				if(type != null)
				{
					Debug.Log("Logic Class exists. " + type + " Skipping. ");
					yield break;
				}
			}

			//
			// roughly similar to : http://answers.unity3d.com/questions/14367/how-can-i-wait-for-unity-to-recompile-during-the-e.html?page=1&pageSize=5&sort=votes
			//
            
            
            File.WriteAllText(LogicClassFile(), kClassContent.Replace("%CLASSNAME%", LogicClassName() ));

            AssetDatabase.ImportAsset(LogicClassFile());


		}

		System.Type GetLogicClassType()
		{
			foreach( var assembly in AppDomain.CurrentDomain.GetAssemblies())
			{
				foreach (var t in assembly.GetTypes())
				{
					if(t.Name == LogicClassName() && t.IsAssignableFrom(typeof(Game)))
						return t;
				}
			}
			return null;
		}

		const string kClassContent = @"using UnityEngine;
using UnityGameBase;

public class %CLASSNAME% : Game
{
    #region implemented abstract members of Game

    protected override void Initialize ()
    {
        throw new System.NotImplementedException ();
    }

    protected override void GameSetupReady ()
    {
        throw new System.NotImplementedException ();
    }

#endregion
}

";
	
        public static string CamelCase(string original)
        {
            if (string.IsNullOrEmpty(original))
            {
                return string.Empty;
            }
            bool flag = NoLowerCase(original);
            var builder = new StringBuilder();
            if (!IsSeparatorChar(original[0]))
            {
                builder.Append(char.ToUpper(original[0]));
            }
            for (int i = 1; i < original.Length; i++)
            {
                if (!IsSeparatorChar(original[i]))
                {
                    if (IsSeparatorChar(original[i - 1]))
                    {
                        builder.Append(char.ToUpper(original[i]));
                    }
                    else if (flag)
                    {
                        builder.Append(char.ToLower(original[i]));
                    }
                    else
                    {
                        builder.Append(original[i]);
                    }
                }
            }
            return builder.ToString();
        }

        private static bool IsSeparatorChar(char value)
        {
            return !char.IsLetterOrDigit(value);
        }

        private static bool NoLowerCase(string value)
        {
            foreach (char ch in value)
            {
                if (char.IsLower(ch))
                {
                    return false;
                }
            }
            return true;
        }
    }
}
 
