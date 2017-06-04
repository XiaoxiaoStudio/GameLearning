using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using System.Security;
using System.Collections;
using Mono.Xml;

namespace Air2000
{
    public class SEMonster : SEObject
    {
        public int ID;
        public float DelayAppear;
        public override void LoadDependAsset()
        {
            base.LoadDependAsset();
        }
        public override void ParseXML(SecurityElement element)
        {
            base.ParseXML(element);
            int.TryParse(element.Attribute("ID"), out ID);
            float.TryParse(element.Attribute("DelayAppear"), out DelayAppear);
        }

        public override SecurityElement GenerateXmlElement(SecurityElement element)
        {
            element.AddAttribute("ID", ID.ToString());
            element.AddAttribute("DelayAppear", DelayAppear.ToString());
            return base.GenerateXmlElement(element);
        }
    }
}
