using System.Security;
using Air2000;
using UnityEngine;

public class SkillBeAttackEffectPlugin : SkillPlugin
{
    public string RootPath;
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
            if (Character.Enemy == null)
            {
                return false;
            }
            string effectDirectory = "Effect/SkillEffect/Common/";
            string effectname = string.Empty;
            if (Character.Enemy.Profession == Profession.Warrior)
            {
                effectname += "warriorHit";
            }
            else if (Character.Enemy.Profession == Profession.Archer)
            {
                effectname += "archerHit";
            }
            if (m_EffectObj == null)
            {
                m_EffectObj = CharacterEffectProvider.Get(effectDirectory, effectname);
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
                GTimer.In(0.8f, InactiveEffect, m_EffectObj);
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
                GTimer.In(0.8f, InactiveEffect, m_EffectObj);

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
                        GTimer.In(0.8f, InactiveEffect, m_EffectObj);

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
    private void InactiveEffect(object obj)
    {
        GameObject go = obj as GameObject;
        if (go)
        {
            CharacterEffectProvider.Pool(go);
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
        Position = Helper.StrToVec3(element.Attribute("Position"));
        Rotation = Helper.StrToVec3(element.Attribute("Rotation"));
        Scale = Helper.StrToVec3(element.Attribute("Scale"));

        if (m_EffectObj != null)
        {
            CharacterEffectProvider.Pool(m_EffectObj);
        }
    }
}
