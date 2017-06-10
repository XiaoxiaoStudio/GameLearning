using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    public SkillController skillController;
    public Commander commander;
    public MotionsAnimator motionsAnimator;

    private void Start()
    {
        skillController = transform.GetComponentInChildren<SkillController>();
        commander = transform.GetComponentInChildren<Commander>();
        motionsAnimator = transform.GetComponentInChildren<MotionsAnimator>();
    }

    public void ExecuteAnimation(RoleMotionType type)
    {
        commander.Execute(new Commander.Command(motionsAnimator, type, GetDefauteType(type), GetPlugin(type)));
    }

    private RoleMotionType GetDefauteType(RoleMotionType type)
    {
        foreach (var item in motionsAnimator.Motions)
        {
            if (item.Type == type)
            {
                return item.NextType;
            }
        }
        return RoleMotionType.RMT_Idle;
    }

    private Plugin GetPlugin(RoleMotionType type)
    {
        if (skillController.GetSkill(type).Count > 0)
        {
            return skillController.GetSkill(type)[0];
        }
        return null;
    }
}