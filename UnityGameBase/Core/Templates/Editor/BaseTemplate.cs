using UnityEngine;
using UnityEditor;
using System.Collections;
using System.IO;

namespace UnityGameBase.Core.Templates
{
    public abstract class BaseTemplate
    {               
        public string name { get; set; }    
        public abstract string content { get; }
        public abstract string fileType { get; }   
        
        protected static void Create(BaseTemplate template)
        {
            string curPath = AssetDatabase.GetAssetPath(Selection.activeObject);
            
            if(!System.IO.Directory.Exists(curPath))
            {
                Debug.LogWarning("Path is not a Folder");
                return;
            }
            
            Save(curPath, template.fileType, template);
        }        
        
        protected static void Save(string path, string fileType, BaseTemplate template)
        {
            string curPath = EditorUtility.SaveFilePanel("Save File", path, "template", "");
            if(curPath.Length > 0)
            {
                curPath += template.fileType;
                int index = curPath.LastIndexOf('/');
                template.name = curPath.Substring(index + 1);
                template.name = template.name.Replace(template.fileType, "");
                
                File.WriteAllText(curPath, template.content);
            }
            
            AssetDatabase.Refresh();
            
        } 
    }

}
