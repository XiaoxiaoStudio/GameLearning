using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security;
using Mono.Xml;
using System.Collections;
using Air2000;
using UnityEngine;

public class SkillEffectPlugin : SkillPlugin
{
    public string RootPath;
    public string EffectPath;
    public string EffectName;
    public Vector3 Position;
    public Vector3 Rotation;
    public Vector3 Scale;

    private GameObject m_EffectObj;

    public override bool Begin()
    {
        bool b = base.Begin();
        if (b == false)
        {
            return b;
        }
        else
        {
            if (m_EffectObj == null)
            {
                m_EffectObj = CharacterEffectProvider.Get(EffectPath, EffectName);
            }
            if (m_EffectObj == null)
            {
                return false;
            }
            if (RootPath == "WorldSpace")
            {
                m_EffectObj.transform.SetParent(null);
                m_EffectObj.transform.localScale = Scale;
                m_EffectObj.transform.position = Position;
                m_EffectObj.transform.eulerAngles = Rotation;
                m_EffectObj.SetActive(false);
                Helper.SetLayer(m_EffectObj, LAYER.Player.ToString());
                m_EffectObj.SetActive(true);
            }
            else if (string.IsNullOrEmpty(RootPath))
            {
                m_EffectObj.transform.SetParent(Character.transform);
                m_EffectObj.transform.localScale = Scale;
                m_EffectObj.transform.localPosition = Position;
                m_EffectObj.transform.localEulerAngles = Rotation;
                m_EffectObj.SetActive(false);
                Helper.SetLayer(m_EffectObj, LAYER.Player.ToString());
                m_EffectObj.SetActive(true);
            }
            else
            {
                Transform body = Character.GetBodyTransform();
                if (body)
                {
                    Transform tempParent = body.Find(RootPath);
                    if (tempParent != null)
                    {
                        m_EffectObj.transform.SetParent(tempParent);
                        m_EffectObj.transform.localScale = Scale;
                        m_EffectObj.transform.localPosition = Position;
                        m_EffectObj.transform.localEulerAngles = Rotation;
                        m_EffectObj.SetActive(false);
                        Helper.SetLayer(m_EffectObj, LAYER.Player.ToString());
                        m_EffectObj.SetActive(true);
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            return true;
        }

    }
    public override void End()
    {
        base.End();
        if (m_EffectObj)
        {
            CharacterEffectProvider.Pool(m_EffectObj);
            m_EffectObj = null;
        }
    }
    public override void ParseXML(SecurityElement element, Skill skill)
    {
        base.ParseXML(element, skill);
        RootPath = element.Attribute("RootPath");
        EffectPath = element.Attribute("EffectPath");
        EffectName = element.Attribute("EffectName");
        Position = Helper.StrToVec3(element.Attribute("Position"));
        Rotation = Helper.StrToVec3(element.Attribute("Rotation"));
        Scale = Helper.StrToVec3(element.Attribute("Scale"));

        if (m_EffectObj != null)
        {
            CharacterEffectProvider.Pool(m_EffectObj);
        }
        CharacterEffectProvider.Register(EffectPath, EffectName);
    }
}
