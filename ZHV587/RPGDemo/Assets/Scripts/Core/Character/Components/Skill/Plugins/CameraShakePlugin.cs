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

public class CameraShakePlugin : SkillPlugin
{
    public Vector3 Direction;
    public float Duration;
    public override bool Begin()
    {
        if (base.Begin() == false)
        {
            return false;
        }
        Transform cameraNode = SceneCameraController.Instance.MainCameraNode;
        if (cameraNode == null)
        {
            return false;
        }
        iTween.Stop(cameraNode.gameObject);
        Duration = EndTime - BeginTime;
        iTween.PunchPosition(cameraNode.gameObject, Direction, Duration);
        return true;
    }
    public override void Update()
    {
        base.Update();
    }
    public override void End()
    {
        base.End();
    }
    public override void ParseXML(SecurityElement element, Skill skill)
    {
        base.ParseXML(element, skill);
        Direction = Helper.StrToVec3(element.Attribute("Direction"));
    }
}
