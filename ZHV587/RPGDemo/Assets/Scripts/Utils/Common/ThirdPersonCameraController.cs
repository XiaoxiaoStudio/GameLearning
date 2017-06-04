using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Air2000
{
    public class ThirdPersonCameraController : MonoBehaviour
    {
        public Character Hero;
        public float WalkSpeed = 0.1f;
        public float Distance = 8f;
        private Vector3 m_CharacterHeight;

        public static ThirdPersonCameraController Instance { get; set; }
        void Awake()
        {
            if (Instance)
            {
                GameObject.DestroyImmediate(Instance);
            }
            Instance = this;
        }
        void OnDestroy()
        {
            Instance = null;
        }
        void OnEnable()
        {
            Hero = PlayerProvider.Hero.Character;
            if (Hero == null)
            {
                PlayerProvider.Hero.PostCreateCharacter += OnHeroCreateCharacter;
                enabled = false;
            }
        }
        void OnHeroCreateCharacter(Player player, Character character)
        {
            player.PostCreateCharacter -= OnHeroCreateCharacter;
            Hero = character;
            if (Hero != null)
            {
                m_CharacterHeight = Vector3.zero;
                m_CharacterHeight.y = Hero.Height;
                enabled = true;
            }
            else { enabled = false; }
        }
        void OnDisable()
        {

        }
        void Update()
        {
            if (Hero == null && PlayerProvider.Hero != null)
            {
                Hero = PlayerProvider.Hero.Character;
            }
            if (Hero == null)
            {
                return;
            }
            Vector3 oriPosition = Hero.transform.position;
            oriPosition += m_CharacterHeight;
            Vector3 direction = transform.forward;
            direction.Normalize();
            transform.position = oriPosition + direction * Distance * (-1.0f);


            return;

#if UNITY_EDITOR
            if (Hero != null)
            {
                if (Input.GetKeyDown("1"))
                {
                    Hero.ExecuteCommand(CharacterCommand.CC_Attack_1);
                    return;
                }
                if (Input.GetKeyDown("2"))
                {
                    Hero.ExecuteCommand(CharacterCommand.CC_Attack_2);
                    return;
                }
                if (Input.GetKeyDown("3"))
                {
                    Hero.ExecuteCommand(CharacterCommand.CC_Attack_3);
                    return;
                }
                if (Input.GetKeyDown("4"))
                {
                    Hero.ExecuteCommand(CharacterCommand.CC_Attack_4);
                    return;
                }
                if (Input.GetKeyDown("5"))
                {
                    Hero.ExecuteCommand(CharacterCommand.CC_Attack_5);
                    return;
                }
                if (Input.GetKeyDown(KeyCode.Z))
                {
                    Hero.ExecuteCommand(CharacterCommand.CC_Skill_1);
                    return;
                }
                if (Input.GetKeyDown(KeyCode.X))
                {
                    Hero.ExecuteCommand(CharacterCommand.CC_Skill_2);
                    return;
                }
                if (Input.GetKeyDown(KeyCode.C))
                {
                    Hero.ExecuteCommand(CharacterCommand.CC_Skill_2);
                    return;
                }
                if (Input.GetKeyDown("9"))
                {
                    Hero.ExecuteCommand(CharacterCommand.CC_Skill_4);
                    return;
                }

                if (Input.GetKey(KeyCode.A))
                {
                    float turn = -0.1f;
                    float x = Mathf.Sin(turn);
                    float z = Mathf.Cos(turn);
                    Vector3 targetDirection = new Vector3(x, 0.0f, z);
                    targetDirection = Hero.transform.TransformDirection(targetDirection);
                    Quaternion target = Quaternion.LookRotation(targetDirection);
                    Hero.transform.rotation = target;
                }
                else if (Input.GetKey(KeyCode.D))
                {
                    float turn = 0.1f;
                    float x = Mathf.Sin(turn);
                    float z = Mathf.Cos(turn);
                    Vector3 targetDirection = new Vector3(x, 0.0f, z);
                    targetDirection = Hero.transform.TransformDirection(targetDirection);
                    Quaternion target = Quaternion.LookRotation(targetDirection);
                    Hero.transform.rotation = target;
                }

                if (Input.GetKey(KeyCode.W))
                {
                    Vector3 newTargetPoint = Hero.transform.TransformPoint(Vector3.forward * WalkSpeed * Time.deltaTime);
                    Hero.transform.position = newTargetPoint;
                    if (Hero.Commander.CurrentCommand != null && Hero.Commander.CurrentCommand.Type != CharacterCommand.CC_Walk)
                    {
                        Hero.ExecuteCommand(CharacterCommand.CC_Walk);
                    }
                }
                else if (Input.GetKeyUp(KeyCode.W))
                {
                    Hero.ExecuteCommand(CharacterCommand.CC_Idle);
                }
            }

#endif

        }
    }
}
