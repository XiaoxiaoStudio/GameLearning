using System;
using System.Collections.Generic;
using System.Security;
using Mono.Xml;
using System.Collections;

namespace Air2000
{
    public enum FlyerPluginStatus
    {
        Inactive,
        Active,
    }

    public delegate void PostFlyerPluginBeginDelegate(object plugin);
    public delegate void PostFlyerPluginUpdateDelegate(object plugin);
    public delegate void PostFlyerPluginEndDelegate(object plugin);
    [Serializable]

    public class FlyerPlugin
    {
        public string ClassName;
        public float BeginTime;
        public float EndTime;

        public event PostFlyerPluginBeginDelegate PostBegin;
        public event PostFlyerPluginUpdateDelegate PostUpdate;
        public event PostFlyerPluginEndDelegate PostEnd;
        public FlyerPluginStatus Status = FlyerPluginStatus.Inactive;

        public Flyer Flyer;
        public FlyerController Controller
        {
            get
            {
                return Flyer.Controller;
            }
        }

        public Character Character
        {
            get
            {
                return Flyer.Character;
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
        public virtual bool Begin()
        {
            if (BeginTime <= EndTime && EndTime <= 0)
            {
                Status = FlyerPluginStatus.Inactive;
                return false;
            }
            Status = FlyerPluginStatus.Active;
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
            Status = FlyerPluginStatus.Inactive;
            if (PostEnd != null)
            {
                PostEnd(this);
            }
        }
        public virtual bool IsFinish(float currentTime)
        {
            if (EndTime <= currentTime && Status == FlyerPluginStatus.Active)
            {
                return true;
            }
            return false;
        }
        public virtual void ParseXML(SecurityElement element, Flyer flyer)
        {
            Flyer = flyer;
            float.TryParse(element.Attribute("BeginTime"), out BeginTime);
            float.TryParse(element.Attribute("EndTime"), out EndTime);
        }
    }
}
