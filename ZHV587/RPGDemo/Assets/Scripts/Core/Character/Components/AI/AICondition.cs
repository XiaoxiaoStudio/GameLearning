using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mono.Xml;
using System.Security;
using System.Collections;
using UnityEngine;

namespace Air2000
{
    [Serializable]
    public class AICondition
    {
        public string Name;
        public virtual bool IsSatisfied(AIMachine machine, AITrigger trigger)
        {
            return false;
        }
        public virtual void ParseXML(SecurityElement element, AITrigger trigger) { }
    }
}
