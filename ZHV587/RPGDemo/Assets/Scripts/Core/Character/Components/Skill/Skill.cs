using System;
using System.Collections.Generic;
using System.Security;
using Mono.Xml;
using System.Collections;
using UnityEngine;

namespace Air2000
{
    public enum SkillPlayMode
    {
        StopCurrent,
        DontPlayWhenBusy,
    }

    public enum SkillStopMode
    {
        OnCommandEnd,
        DontStopOnCommandEnd,
    }

    public enum SkillStatus
    {
        Inactive,
        Active,
    }

    public delegate void PostSkillBeginDelegate(Skill skill);

    public delegate void PostSkillUpdateDelegate(Skill skill);

    public delegate void PostSkillEndDelegate(Skill skill);

    public class Skill
    {
        public CharacterCommand TriggerCommand;
        public int ID;

        public SkillPlayMode PlayMode;
        public SkillStopMode StopMode;
        public SkillStatus Status = SkillStatus.Inactive;

        public event PostSkillBeginDelegate PostBegin;

        public event PostSkillUpdateDelegate PostUpdate;

        public event PostSkillEndDelegate PostEnd;

        public SkillController Controller;
        private List<SkillPlugin> m_Plugins = new List<SkillPlugin>();
        private List<SkillPlugin> m_WaittingPlayPlugins = new List<SkillPlugin>();
        private List<SkillPlugin> m_ActivePlugins = new List<SkillPlugin>();
        private float m_CurrentTime;
        public Character Character;

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

        public void Begin()
        {
            Status = SkillStatus.Active;
            if (m_WaittingPlayPlugins == null) m_WaittingPlayPlugins = new List<SkillPlugin>();
            m_WaittingPlayPlugins.Clear();
            m_WaittingPlayPlugins.AddRange(m_Plugins);
            m_CurrentTime = 0;
            //Character.Player.FaceToEnemy(BattleScene.Instace.CurrentEnemy);
            Character.Player.FaceToEnemy(null);
            if (PostBegin != null)
            {
                PostBegin(this);
            }
        }

        public void Update()
        {
            m_CurrentTime += Time.deltaTime;
            if (TriggerCommand == CharacterCommand.CC_BeAttack_1)
            {
                int a = 1;
            }
            for (int i = 0; i < m_WaittingPlayPlugins.Count; i++)
            {
                SkillPlugin plugin = m_WaittingPlayPlugins[i];
                if (plugin == null) continue;
                if (plugin.Status == SkillPluginStatus.Active)
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
                SkillPlugin plugin = m_ActivePlugins[i];
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
            m_CurrentTime = 0;
            Status = SkillStatus.Inactive;
            if (m_ActivePlugins.Count > 0)
            {
                for (int i = 0; i < m_ActivePlugins.Count; i++)
                {
                    SkillPlugin plugin = m_ActivePlugins[i];
                    if (plugin == null) continue;
                    plugin.End();
                }
            }
            m_ActivePlugins.Clear();
            m_WaittingPlayPlugins.Clear();
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

        public void AddPlugin(SkillPlugin plugin)
        {
            if (m_Plugins == null)
            {
                m_Plugins = new List<SkillPlugin>();
            }
            m_Plugins.Add(plugin);
        }

        public void ParseXML(SecurityElement element, SkillController controller)
        {
            Controller = controller;
            Character = controller.Character;

            int.TryParse(element.Attribute("ID"), out ID);
            PlayMode = (SkillPlayMode)CharacterSystemUtils.TryParseEnum<SkillPlayMode>(element.Attribute("PlayMode"));
            StopMode = (SkillStopMode)CharacterSystemUtils.TryParseEnum<SkillStopMode>(element.Attribute("StopMode"));

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
                SkillPlugin pluginObj = pluginType.Assembly.CreateInstance(pluginType.FullName) as SkillPlugin;
                if (pluginObj == null) continue;
                pluginObj.ClassName = className;
                pluginObj.ParseXML(pluginElement, this);
                AddPlugin(pluginObj);
            }
        }
    }
}