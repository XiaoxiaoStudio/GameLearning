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
    public class AIState
    {
        public string Name;
        public bool ForceStop = false;
        public virtual void Begin(AIMachine machine) { }
        public virtual void Update(AIMachine machine) { }
        public virtual void End(AIMachine machine) { }
        public virtual void OnMachineAwake(AIMachine machine) { }
        public virtual void OnMachineStart(AIMachine machine) { }
        public virtual void OnMachineEnable(AIMachine machine) { }
        public virtual void OnMachineDisable(AIMachine machine) { }
        public virtual void OnMachineDestroy(AIMachine machine) { }
        public virtual void ParseXML(SecurityElement element, AIMachine machine)
        {
            if (element.Attribute("ForceStop") != null)
            {
                bool.TryParse(element.Attribute("ForceStop"), out ForceStop);
            }
            else
            {
                ForceStop = false;
            }
        }
    }
}
