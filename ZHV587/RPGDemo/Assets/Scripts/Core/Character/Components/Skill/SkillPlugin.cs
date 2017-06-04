using System;
using System.Collections.Generic;
using System.Security;
using Mono.Xml;
using System.Collections;

namespace Air2000
{
    public enum SkillPluginStatus
    {
        Inactive,
        Active,
    }

    public delegate void PostSkillPluginBeginDelegate(object plugin);
    public delegate void PostSkillPluginUpdateDelegate(object plugin);
    public delegate void PostSkillPluginEndDelegate(object plugin);

    public class SkillPlugin
    {
        public string ClassName;
        public float BeginTime;
        public float EndTime;

        public event PostSkillPluginBeginDelegate PostBegin;
        public event PostSkillPluginUpdateDelegate PostUpdate;
        public event PostSkillPluginEndDelegate PostEnd;

        public SkillController Controller;
        public Skill Skill;
        public Character Character;
        public SkillPluginStatus Status = SkillPluginStatus.Inactive;
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
        public virtual bool Begin()
        {
            if (BeginTime <= EndTime && EndTime <= 0)
            {
                Status = SkillPluginStatus.Inactive;
                return false;
            }
            Status = SkillPluginStatus.Active;
            if (PostBegin != null)
            {
                PostBegin(this);
            }
            return true;
        }
        public virtual void Update()
        {
            if (PostUpdate != null)
            {
                PostUpdate(this);
            }
        }
        public virtual void End()
        {
            Status = SkillPluginStatus.Inactive;
            if (PostEnd != null)
            {
                PostEnd(this);
            }
        }
        public virtual bool IsFinish(float currentTime)
        {
            if (EndTime <= currentTime && Status == SkillPluginStatus.Active)
            {
                return true;
            }
            return false;
        }
        public virtual void ParseXML(SecurityElement element, Skill skill)
        {
            Skill = skill;
            Controller = skill.Controller;
            Character = skill.Character;
            float.TryParse(element.Attribute("BeginTime"), out BeginTime);
            float.TryParse(element.Attribute("EndTime"), out EndTime);
        }
    }
}
