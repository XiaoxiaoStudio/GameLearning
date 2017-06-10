using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public enum RoleMotionType
{
    None = -1,

    RMT_Idle = 0,

    RMT_Idle_2 = 6,

    RMT_PreAppear = 20,
    RMT_Appear = 21,
    RMT_PostAppear = 22,

    RMT_PreDisappear = 23,
    RMT_Disappear = 24,
    RMT_PostDisappear = 25,

    RMT_PreDie_1 = 26,
    RMT_Die_1 = 27,
    RMT_PostDie_1 = 28,

    RMT_PreDie_2 = 29,
    RMT_Die_2 = 30,
    RMT_PostDie_2 = 31,

    RMT_PreWalk = 32,
    RMT_Walk = 33,
    RMT_PostWalk = 34,

    RMT_PreRun = 35,
    RMT_Run = 36,
    RMT_PostRun = 37,

    RMT_PreAttack_1 = 60,
    RMT_Attack_1 = 61,
    RMT_PostAttack_1 = 62,

    RMT_PreAttack_2 = 63,
    RMT_Attack_2 = 64,
    RMT_PostAttack_2 = 65,

    RMT_PreAttack_3 = 66,
    RMT_Attack_3 = 67,
    RMT_PostAttack_3 = 68,

    RMT_PreAttack_4 = 69,
    RMT_Attack_4 = 70,
    RMT_PostAttack_4 = 71,

    RMT_PreAttack_5 = 72,
    RMT_Attack_5 = 73,
    RMT_PostAttack_5 = 74,

    RMT_PreBeAttack_1 = 100,
    RMT_BeAttack_1 = 101,
    RMT_PostBeAttack_1 = 102,

    RMT_PreBeAttack_2 = 103,
    RMT_BeAttack_2 = 104,
    RMT_PostBeAttack_2 = 105,

    RMT_PreKnockDown = 106,
    RMT_KnockDown = 107,
    RMT_PostKnockDown = 108,

    RMT_PreKnockDownStand = 109,
    RMT_KnockDownStand = 110,
    RMT_PostKnockDownStand = 111,

    RMT_PreSkill_1 = 141,
    RMT_Skill_1 = 142,
    RMT_PostSkill_1 = 143,

    RMT_PreSkill_2 = 144,
    RMT_Skill_2 = 145,
    RMT_PostSkill_2 = 146,

    RMT_PreSkill_3 = 147,
    RMT_Skill_3 = 148,
    RMT_PostSkill_3 = 149,

    RMT_PreSkill_4 = 150,
    RMT_Skill_4 = 151,
    RMT_PostSkill_4 = 152,

    RMT_PreSkill_5 = 153,
    RMT_Skill_5 = 154,
    RMT_PostSkill_5 = 155,

    RMT_PreVictory = 190,
    RMT_Victory = 191,
    RMT_PostVictory = 192,

    RMT_PreFail = 200,
    RMT_Fail = 201,
    RMT_PostFail = 202,
}

[Serializable]
public class Motion
{
    public RoleMotionType Type;
    public string ChipName;
    public WrapMode WrapMode;
    public float Speed;
    public bool Break;
    public float ExitTime;
    public RoleMotionType NextType;
}

public class Changeable
{
    public RoleMotionType Type;
    public int parameters;
}