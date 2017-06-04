using System;
using System.Collections.Generic;
using System.Security;
using Mono.Xml;
using System.Collections;
using UnityEngine;

namespace Air2000
{
    public enum FlyerPlayMode
    {
        StopCurrent,
        DontPlayWhenBusy,
    }
    public enum FlyerStopMode
    {
        OnCommandEnd,
        DontStopOnCommandEnd,
    }
    public enum FlyerStatus
    {
        Inactive,
        Active,
    }
    public delegate void PostFlyerBeginDelegate(Flyer flyer);
    public delegate void PostFlyerUpdateDelegate(Flyer flyer);
    public delegate void PostFlyerEndDelegate(Flyer flyer);
    public class Flyer : MonoBehaviour
    {
        public string Name;
        public int ID;
        public AnimationCurve MoveCurve = new AnimationCurve() { keys = new Keyframe[] { new Keyframe() { time = 0, value = 1 }, new Keyframe() { time = 10, value = 1 } } };

        public FlyerPlayMode PlayMode;
        public FlyerPlayMode StopMode;
        public FlyerStatus Status = FlyerStatus.Inactive;

        public event PostFlyerBeginDelegate PostBegin;
        public event PostFlyerUpdateDelegate PostUpdate;
        public event PostFlyerEndDelegate PostEnd;

        public FlyerController Controller;
        private List<FlyerPlugin> m_Plugins = new List<FlyerPlugin>();
        private List<FlyerPlugin> m_WaittingPlayPlugins = new List<FlyerPlugin>();
        private List<FlyerPlugin> m_ActivePlugins = new List<FlyerPlugin>();
        private float m_CurrentTime;
        public Character Character
        {
            get
            {
                return Controller.Character;
            }
        }
        public Player Player
        {
            get
            {
                if (Character == null)
                {
                    return null;
                }
                return Character.Player;
            }
        }
        private void Awake()
        {
            //if(PluginsType != null && PluginsType)
        }
        public void Begin()
        {
            Status = FlyerStatus.Active;
            if (m_WaittingPlayPlugins == null) m_WaittingPlayPlugins = new List<FlyerPlugin>();
            m_WaittingPlayPlugins.Clear();
            m_WaittingPlayPlugins.AddRange(m_Plugins);
            m_CurrentTime = 0;
            if (PostBegin != null)
            {
                PostBegin(this);
            }
        }
        private void Update()
        {
            m_CurrentTime += Time.deltaTime;

            for (int i = 0; i < m_WaittingPlayPlugins.Count; i++)
            {
                FlyerPlugin plugin = m_WaittingPlayPlugins[i];
                if (plugin == null) continue;
                if (plugin.Status == FlyerPluginStatus.Active)
                {
                    m_WaittingPlayPlugins.Remove(plugin);
                    i--;
                    continue;
                }
                if (m_CurrentTime >= plugin.BeginTime && m_CurrentTime < plugin.EndTime)
                {
                    if (plugin.Begin())
                    {
                        m_ActivePlugins.Add(plugin);
                    }
                }
            }
            for (int i = 0; i < m_ActivePlugins.Count; i++)
            {
                FlyerPlugin plugin = m_ActivePlugins[i];
                if (plugin == null) continue;
                if (plugin.IsFinish(m_CurrentTime))
                {
                    m_ActivePlugins.Remove(plugin);
                    plugin.End();
                    i--;
                    continue;
                }
                plugin.Update();
            }
            if (PostUpdate != null)
            {
                PostUpdate(this);
            }
        }
        public void End()
        {
            Status = FlyerStatus.Inactive;
            if (PostEnd != null)
            {
                PostEnd(this);
            }
        }
        public bool IsFinish()
        {
            if (m_WaittingPlayPlugins.Count == 0 && m_ActivePlugins.Count == 0)
            {
                return true;
            }
            return false;
        }
        public void AddPlugin(FlyerPlugin plugin)
        {
            if (m_Plugins == null)
            {
                m_Plugins = new List<FlyerPlugin>();
            }
            m_Plugins.Add(plugin);
        }
        public void ParseXML(SecurityElement element, FlyerController controller)
        {
            Controller = controller;

            int.TryParse(element.Attribute("ID"), out ID);
            PlayMode = (FlyerPlayMode)CharacterSystemUtils.TryParseEnum<FlyerPlayMode>(element.Attribute("PlayMode"));
            StopMode = (FlyerPlayMode)CharacterSystemUtils.TryParseEnum<FlyerPlayMode>(element.Attribute("StopMode"));

            ArrayList pluginElements = element.Children;
            if (pluginElements == null || pluginElements.Count == 0)
            {
                return;
            }
            for (int i = 0; i < pluginElements.Count; i++)
            {
                SecurityElement pluginElement = pluginElements[i] as SecurityElement;
                if (pluginElement == null) continue;
                string className = pluginElement.Attribute("Name");
                if (string.IsNullOrEmpty(className)) continue;
                Type pluginType = Type.GetType(className);
                if (pluginType == null) continue;
                FlyerPlugin pluginObj = pluginType.Assembly.CreateInstance(pluginType.FullName) as FlyerPlugin;
                if (pluginObj == null) continue;
                pluginObj.ClassName = className;
                pluginObj.ParseXML(pluginElement, this);
                AddPlugin(pluginObj);
            }
        }
    }
}
