using UnityEngine;
using UnityEditor;
using System.Collections;


namespace UnityGameBase.Core.Templates
{
    public class TemplateCSharp : BaseTemplate
    {
    
        [MenuItem("Assets/Add/Empty c#" )]
        public static void AdddItem()
        {
            Create(new TemplateCSharp());
        }
        
        public override string fileType
        {
            get
            {
                return ".cs";
            }            
        }
    
        public override string content
        {
            get
            {
                return 
                
@"using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityGameBase.Core;
using UnityGameBase.Core.Extensions;


public class " + name + @" 
{
   
}

";            
            }
        }
    }
}