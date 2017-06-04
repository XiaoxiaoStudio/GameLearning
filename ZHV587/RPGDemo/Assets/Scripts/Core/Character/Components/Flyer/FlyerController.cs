using System;
using System.Collections.Generic;
using UnityEngine;
using System.Security;
using Mono.Xml;
using System.Collections;

namespace Air2000
{
    public class FlyerController : CharacterComponent
    {
        private List<Flyer> m_Flyers = new List<Flyer>();
        private List<Flyer> m_ActiveFlyers = new List<Flyer>();

        protected override void Update()
        {
            base.Update();
            if (m_ActiveFlyers != null && m_ActiveFlyers.Count > 0)
            {
                for (int i = 0; i < m_ActiveFlyers.Count; i++)
                {
                    Flyer flyer = m_ActiveFlyers[i];
                    if (flyer == null) continue;
                    if (flyer.IsFinish())
                    {
                        m_ActiveFlyers.Remove(flyer);
                        i--;
                        continue;
                    }
               //     flyer.Update();
                }
            }
        }
        public override void ParseXML(SecurityElement element, Character character)
        {
            base.ParseXML(element, character);
            ArrayList flyerElements = element.Children;
            if (flyerElements == null || flyerElements.Count == 0)
            {
                return;
            }
            for (int i = 0; i < flyerElements.Count; i++)
            {
                SecurityElement flyerElement = flyerElements[i] as SecurityElement;
                if (flyerElement == null) continue;
                string flyerName = flyerElement.Attribute("Name");
                if (string.IsNullOrEmpty(flyerName)) continue;
                Flyer flyer = TryGetFlyer(flyerName);
                if (flyer != null) continue;
                flyer = new Flyer();
                flyer.Name = flyerName;
                flyer.ParseXML(flyerElement, this);
                AddFlyer(flyer);
            }
        }
        public override void OnCharacterInitialized(Character character)
        {
            base.OnCharacterInitialized(character);
        }
        public Flyer TryGetFlyer(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return null;
            }
            if (m_Flyers == null || m_Flyers.Count == 0)
            {
                return null;
            }
            for (int i = 0; i < m_Flyers.Count; i++)
            {
                Flyer flyer = m_Flyers[i];
                if (flyer == null) continue;
                if (flyer.Name == name)
                {
                    return flyer;
                }
            }
            return null;
        }
        public bool AddFlyer(Flyer flyer)
        {
            if (m_Flyers == null) m_Flyers = new List<Flyer>();
            m_Flyers.Add(flyer);
            return true;
        }
        public bool LauncherFlyer(string name)
        {
            Flyer flyer = TryGetFlyer(name);
            if (flyer == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
