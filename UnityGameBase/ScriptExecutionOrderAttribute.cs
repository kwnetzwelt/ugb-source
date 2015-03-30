using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UnityGameBase
{
    public class ScriptExecutionOrderAttribute : Attribute
    {
        public int value;
        public ScriptExecutionOrderAttribute(int id)
        {
            this.value = id;
        }
    }
}
