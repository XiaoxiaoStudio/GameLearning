using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Air2000
{
    public class CharacterCommandTest : MonoBehaviour
    {
        public Character Character;
        void Awake()
        {
            if (Character == null)
            {
                Character = GetComponent<Character>();
            }
        }
        void OnGUI()
        {
            if (Character == null || Character.MotionMachine == null) return;
            if (GUI.Button(new Rect(10, 10, 150, 30), "CC_Idle"))
            {
                Character.ExecuteCommand(CharacterCommand.CC_Idle);
            }
            if (GUI.Button(new Rect(10, 50, 150, 30), "CC_Walk"))
            {
                Character.ExecuteCommand(CharacterCommand.CC_Walk);
            }
            if (GUI.Button(new Rect(10, 90, 150, 30), "CC_Run"))
            {
                Character.ExecuteCommand(CharacterCommand.CC_Run);
            }
            if (GUI.Button(new Rect(10, 130, 150, 30), "CC_Attack_1"))
            {
                Character.ExecuteCommand(CharacterCommand.CC_Attack_1);
            }
            if (GUI.Button(new Rect(10, 170, 150, 30), "CC_Attack_2"))
            {
                Character.ExecuteCommand(CharacterCommand.CC_Attack_2);
            }
            if (GUI.Button(new Rect(10, 210, 150, 30), "CC_Attack_3"))
            {
                Character.ExecuteCommand(CharacterCommand.CC_Attack_3);
            }
            if (GUI.Button(new Rect(10, 250, 150, 30), "CC_Attack_4"))
            {
                Character.ExecuteCommand(CharacterCommand.CC_Attack_4);
            }
            if (GUI.Button(new Rect(10, 290, 150, 30), "CC_Skill_1"))
            {
                Character.ExecuteCommand(CharacterCommand.CC_Skill_1);
            }
            if (GUI.Button(new Rect(10, 330, 150, 30), "CC_Skill_2"))
            {
                Character.ExecuteCommand(CharacterCommand.CC_Skill_2);
            }
            if (GUI.Button(new Rect(10, 370, 150, 30), "CC_Skill_3"))
            {
                Character.ExecuteCommand(CharacterCommand.CC_Skill_3);
            }
            if (GUI.Button(new Rect(10, 410, 150, 30), "CC_KnokDown"))
            {
                Character.ExecuteCommand(CharacterCommand.CC_KnockDown);
            }
            if (GUI.Button(new Rect(10, 450, 150, 30), "CC_KnockDownStand"))
            {
                Character.ExecuteCommand(CharacterCommand.CC_KnockDownStand);
            }
            if (GUI.Button(new Rect(10, 490, 150, 30), "CC_BeAttack_1"))
            {
                Character.ExecuteCommand(CharacterCommand.CC_BeAttack_1);
            }
            if (GUI.Button(new Rect(10, 530, 150, 30), "CC_Die_1"))
            {
                Character.ExecuteCommand(CharacterCommand.CC_Die_1);
            }
            //if (GUI.Button(new Rect(170, 530, 150, 30), "Inactive"))
            //{
            //    Character.gameObject.SetActive(false);
            //    //Character.ExecuteCommand(CharacterCommand.CC_Display);
            //}
            //if (GUI.Button(new Rect(330, 530, 150, 30), "Active"))
            //{
            //    Character.gameObject.SetActive(true);
            //    //Character.ExecuteCommand(CharacterCommand.CC_Display);
            //}
            //if (GUI.Button(new Rect(10, 570, 150, 30), "Overlap"))
            //{
            //    Character.ExecuteCommand(CharacterCommand.CC_Overlap);
            //}
            //if (GUI.Button(new Rect(10, 610, 150, 30), "EnableStencilTest"))
            //{
            //    Character.EnableStencilTest();
            //}
            //if (GUI.Button(new Rect(10, 650, 150, 30), "DisableStencilTest"))
            //{
            //    Character.DisableStencilTest();
            //}
        }
    }
}
