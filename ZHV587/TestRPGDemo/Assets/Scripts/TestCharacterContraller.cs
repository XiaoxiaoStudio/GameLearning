using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCharacterContraller : MonoBehaviour
{
    public CharacterController characterController;

    private void Start()
    {
        characterController = this.GetComponent<CharacterController>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            characterController.ExecuteAnimation(RoleMotionType.RMT_Idle);
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            characterController.ExecuteAnimation(RoleMotionType.RMT_Walk);
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            characterController.ExecuteAnimation(RoleMotionType.RMT_Run);
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            characterController.ExecuteAnimation(RoleMotionType.RMT_Attack_1);
        }
        if (Input.GetKeyDown(KeyCode.T))
        {
            characterController.ExecuteAnimation(RoleMotionType.RMT_Attack_2);
        }
        if (Input.GetKeyDown(KeyCode.Y))
        {
            characterController.ExecuteAnimation(RoleMotionType.RMT_Attack_3);
        }
        if (Input.GetKeyDown(KeyCode.U))
        {
            characterController.ExecuteAnimation(RoleMotionType.RMT_Attack_4);
        }
        if (Input.GetKeyDown(KeyCode.I))
        {
            characterController.ExecuteAnimation(RoleMotionType.RMT_Skill_1);
        }
        if (Input.GetKeyDown(KeyCode.O))
        {
            characterController.ExecuteAnimation(RoleMotionType.RMT_Skill_2);
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            characterController.ExecuteAnimation(RoleMotionType.RMT_Skill_3);
        }
    }

    private void OnGUI()
    {
        if (GUI.Button(new Rect(50, 100, 150, 30), "RMT_Idle Q"))
        {
            characterController.ExecuteAnimation(RoleMotionType.RMT_Idle);
        }
        if (GUI.Button(new Rect(50, 150, 150, 30), "RMT_Walk W"))
        {
            characterController.ExecuteAnimation(RoleMotionType.RMT_Walk);
        }
        if (GUI.Button(new Rect(50, 200, 150, 30), "RMT_Run E"))
        {
            characterController.ExecuteAnimation(RoleMotionType.RMT_Run);
        }
        if (GUI.Button(new Rect(50, 250, 150, 30), "RMT_Attack_1 R"))
        {
            characterController.ExecuteAnimation(RoleMotionType.RMT_Attack_1);
        }
        if (GUI.Button(new Rect(50, 300, 150, 30), "RMT_Attack_2 T"))
        {
            characterController.ExecuteAnimation(RoleMotionType.RMT_Attack_2);
        }
        if (GUI.Button(new Rect(50, 350, 150, 30), "RMT_Attack_3 Y"))
        {
            characterController.ExecuteAnimation(RoleMotionType.RMT_Attack_3);
        }
        if (GUI.Button(new Rect(50, 400, 150, 30), "RMT_Attack_4 U"))
        {
            characterController.ExecuteAnimation(RoleMotionType.RMT_Attack_4);
        }
        if (GUI.Button(new Rect(50, 450, 150, 30), "RMT_Skill_1 I"))
        {
            characterController.ExecuteAnimation(RoleMotionType.RMT_Skill_1);
        }
        if (GUI.Button(new Rect(50, 500, 150, 30), "RMT_Skill_2 O"))
        {
            characterController.ExecuteAnimation(RoleMotionType.RMT_Skill_2);
        }
        if (GUI.Button(new Rect(50, 550, 150, 30), "RMT_Skill_3 P"))
        {
            characterController.ExecuteAnimation(RoleMotionType.RMT_Skill_3);
        }
    }
}