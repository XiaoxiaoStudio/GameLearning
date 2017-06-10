using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill
{
    public RoleMotionType Type;
    public List<Plugin> pluginList;

    public Skill()
    {
        pluginList = new List<Plugin>();
    }
}

public class Plugin
{
    public string Name;
    public float BeginTime;
    public float EndTime;
    public string RootPath;
    public Vector3 Position;
    public Vector3 Scale;
    public string EffectPath;
    public string EffectName;
}