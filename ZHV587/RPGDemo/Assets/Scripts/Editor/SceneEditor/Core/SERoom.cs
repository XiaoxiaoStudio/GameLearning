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
    public class SERoom : SEObject
    {
#if UNITY_EDITOR 
        [UnityEditor.MenuItem("GameObject/Create Room (SERoom) ", false, 0)]
        public static void CreateSERoom()
        {
            new GameObject("SERoom").AddComponent<SERoom>().OnCreate();
        }
#endif
        public static SERoom Instance;
        public int ID;
        public string UnityLevel;


        public SECamera Camera;
        public SELight Light;
        public SEBirthPoint BirthPoint;
        public SETerrain Terrain;
        public SEMonsterWave MonsterWave;

        public override void OnCreate()
        {
            base.OnCreate();
            Instance = this;
        }
        protected override void Awake()
        {
            base.Awake();
            Instance = this;
        }
        protected override void OnDestroy()
        {
            base.OnDestroy();
            if (Instance == this) { Instance = null; }
        }
        public void InitAllElements()
        {

        }
        public override void ParseXML(SecurityElement element)
        {
            base.ParseXML(element);
            int.TryParse(element.Attribute("ID"), out ID);
            UnityLevel = element.Attribute("UnityLevel");
        }
        public override SecurityElement GenerateXmlElement(SecurityElement element)
        {
            if (element == null)
            {
                element = new SecurityElement("SERoom");
            }
            element.AddAttribute("ID", ID.ToString());
            element.AddAttribute("UnityLevel", UnityLevel);
            return base.GenerateXmlElement(element);
        }
    }
}
