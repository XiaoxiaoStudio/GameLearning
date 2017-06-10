using System;
using System.Collections;
using System.Collections.Generic;
using System.Security;
using UnityEngine;

public class SkillController : MonoBehaviour
{
    public List<Skill> SkillList = new List<Skill>();

    public TextAsset textAsset;

    private void Start()
    {
        textAsset = Resources.Load<TextAsset>("Config/Warrior");
        ParseXML(textAsset);
    }

    public List<Plugin> GetSkill(RoleMotionType type)
    {
        Skill skill = new Skill();
        foreach (var item in SkillList)
        {
            if (type == item.Type)
            {
                skill = item;
                return skill.pluginList;
            }
        }
        return skill.pluginList;
    }

    private void ParseXML(TextAsset textAsset)
    {
        SecurityElement element = SecurityElement.FromString(textAsset.text);
        SecurityElement SkillControllerElement = element.SearchForChildByTag("SkillController");
        ArrayList skillElements = SkillControllerElement.Children;
        for (int i = 0; i < skillElements.Count; i++)
        {
            SecurityElement tempElement = skillElements[i] as SecurityElement;
            if (tempElement == null)
                continue;
            Skill tempSkill = new Skill();
            tempSkill.Type = (RoleMotionType)Enum.Parse(typeof(RoleMotionType), tempElement.Attribute("Type"));
            ArrayList pluginElements = tempElement.Children;
            List<Plugin> pluginList = new List<Plugin>();
            for (int j = 0; j < pluginElements.Count; j++)
            {
                Plugin tempPlugin = new Plugin();
                SecurityElement tempElementNode = pluginElements[j] as SecurityElement;
                tempPlugin.Name = tempElementNode.Attribute("Name");
                tempPlugin.BeginTime = float.Parse(tempElementNode.Attribute("BeginTime"));
                tempPlugin.EndTime = float.Parse(tempElementNode.Attribute("EndTime"));
                tempPlugin.RootPath = tempElementNode.Attribute("RootPath");
                tempPlugin.Position = StrToVec3(tempElementNode.Attribute("Position"));
                tempPlugin.Scale = StrToVec3(tempElementNode.Attribute("Scale"));
                tempPlugin.EffectPath = tempElementNode.Attribute("EffectPath");
                tempPlugin.EffectName = tempElementNode.Attribute("EffectName");
                pluginList.Add(tempPlugin);
            }
            tempSkill.pluginList = pluginList;
            SkillList.Add(tempSkill);
        }
    }

    /// <summary>
    /// 字符串转Vector3
    /// </summary>
    /// <param name="str"></param>
    /// <returns></returns>
    public static Vector3 StrToVec3(string str)
    {
        if (string.IsNullOrEmpty(str))
        {
            return Vector3.zero;
        }
        string v = str.Substring(1, str.Length - 2);
        string[] values = v.Split(new string[] { "," }, StringSplitOptions.None);
        if (values.Length == 3)
        {
            return new Vector3(Convert.ToSingle(values[0]), Convert.ToSingle(values[1]), Convert.ToSingle(values[2]));
        }
        return Vector3.zero;
    }
}