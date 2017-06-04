using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using System.Security;
#if UNITY_EDITOR
using UnityEditor;
#endif
using Air2000;
using Mono.Xml;

[Serializable]
public class EffectPlugin : MotionPlugin
{
    public override string DisplayName
    {
        get
        {
            return "[Effect] ";
        }
    }
    public EffectRoot Root;
    public string RootPath;
    public override void OnBegin(Air2000.Motion motion)
    {
        base.OnBegin(motion);
        if (Root == null)
        {
            CurrentStatus = Status.StopOnNextFrame; return;
        }
        Root.PlayEffect();
    }
    public override void OnUpdate(Air2000.Motion motion)
    {
        base.OnUpdate(motion);
    }
    public override void OnEnd(Air2000.Motion motion)
    {
        base.OnEnd(motion);
        if (Root != null)
        {
            Root.StopEffect();
        }
    }
    public override void OnTimerBegin()
    {
        base.OnTimerBegin();
        if (Root != null)
        {
            Root.PlayEffect();
        }
    }

    public override void OnTimerEnd()
    {
        base.OnTimerEnd();
        if (Root != null)
        {
            Root.StopEffect();
        }
    }
    public override void OnMachineDisable(MotionMachine machine, Air2000.Motion motion)
    {
        base.OnMachineDisable(machine, motion);
        if (Root != null)
        {
            //Root.StopEffect();
        }
    }

#if UNITY_EDITOR
    public override void DisplayEditorView(Air2000.Motion motion)
    {
        base.DisplayEditorView(motion);
        GUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Root", GUILayout.Width(140));
        Root = (EffectRoot)EditorGUILayout.ObjectField(Root, typeof(EffectRoot), true);
        GUILayout.EndHorizontal();
    }
    public override void OnPreReplaceAnimation(Air2000.Motion motion)
    {
        base.OnPreReplaceAnimation(motion);
        if (Root != null && motion != null && motion.Character!= null && motion.Character.GetBodyTransform() != null)
        {
            RootPath = CharacterSystemUtils.GenerateRootPath(motion.Character.GetBodyTransform(), Root.transform);
        }
    }
    public override void OnReplacedAnimation(Air2000.Motion motion)
    {
        base.OnReplacedAnimation(motion);
        if (motion != null && motion.Character != null && motion.Character.GetBodyTransform() != null && string.IsNullOrEmpty(RootPath) == false)
        {
            Transform effectRootTran = motion.Character.GetBodyTransform().Find(RootPath);
            if (effectRootTran != null)
            {
                EffectRoot com = effectRootTran.GetComponent<EffectRoot>();
                if (com)
                {
                    Root = com;
                }
                else
                {
                    Debug.LogError("EffectPlugin::OnReplacedAnimation: error cuased by null EffectRoot component attach to " + effectRootTran.name + " (GameObject)");
                }
            }
            else
            {
                Debug.LogError("EffectPlugin::OnReplacedAnimation: error cuased by null transform at " + RootPath);
            }
        }
    }
    public override void UpdateDependency(Air2000.Motion motion)
    {
        base.UpdateDependency(motion);
        if (motion != null)
        {

        }
    }
    public override void ParseXML(SecurityElement element, Air2000.Motion motion)
    {
        base.ParseXML(element, motion);
        RootPath = element.Attribute("RootPath");
        Transform tran = motion.Machine.Character.transform.Find(RootPath);
        if (tran)
        {
            Root = tran.GetComponent<EffectRoot>();
        }
    }
#endif
}
