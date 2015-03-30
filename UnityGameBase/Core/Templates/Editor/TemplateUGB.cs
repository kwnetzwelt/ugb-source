using UnityEngine;
using UnityEditor;
using System.Collections;


namespace UnityGameBase.Core.Templates
{
    public class TemplateUGB : BaseTemplate
    {
        [MenuItem("Assets/Add/UGBClass" )]
        public static void AddssItem()
        {
            Create(new TemplateUGB());
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

public class " + name + @" : GameComponent
{
    public void Start ()
    {
        
    }
    
    public void Update()
    {
    
    }
}

";            
            }
        }
    }
}