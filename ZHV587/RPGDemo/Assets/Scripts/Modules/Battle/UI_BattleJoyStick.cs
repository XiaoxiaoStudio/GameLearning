using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Air2000
{
    public class UI_BattleJoyStick : Performer
    {
        public Character ControlledCharacter;
        public float MoveSpeed = 6f;
        public Transform Cursor;
        private Vector3 m_DragStartPos;
        private readonly float m_CursorRadius = 45;
        private bool m_IsPress = false;
        private Vector3 m_CursorOriginalWorldPos;
        private Quaternion m_LastRotation = Quaternion.identity;

        protected override void Awake()
        {
            base.Awake();
            if (Cursor)
            {
                m_CursorOriginalWorldPos = Cursor.transform.position;
            }
        }
        protected override void Start()
        {
            base.Start();
        }
        //public override void Visiable(Activity<BattleModule, BattleActivityPerformer> context)
        //{
        //    base.Visiable(context);
        //}
        //public override void Invisiable(Activity<BattleModule, BattleActivityPerformer> context)
        //{
        //    base.Invisiable(context);
        //    ControlledCharacter = null;
        //}
        void OnHeroCreateCharacter(Player player, Character character)
        {
            player.PostCreateCharacter -= OnHeroCreateCharacter;
            ControlledCharacter = character;
            if (ControlledCharacter != null)
            {
                enabled = true;
            }
            else { enabled = false; }
        }
        void OnPress(bool isPress)
        {
            this.m_IsPress = isPress;
            if (m_IsPress == false)
            {
                Cursor.localPosition = Vector2.zero;
                if (ControlledCharacter != null && ControlledCharacter.Commander.CurrentCommand != null && ControlledCharacter.Commander.CurrentCommand.Type != CharacterCommand.CC_Idle)
                {
                    ControlledCharacter.ExecuteCommand(CharacterCommand.CC_Idle);
                }
            }
            else
            {
                m_DragStartPos = UICamera.currentTouch.pos;
            }
        }
        protected override void Update()
        {
            if (ControlledCharacter == null)
            {
                ControlledCharacter = PlayerProvider.Hero.Character;
            }
            if (ControlledCharacter == null)
            {
                return;
            }
            if (m_IsPress)
            {
                Vector3 mousePos = UICamera.lastTouchPosition;

                // Update cursor position
                Vector3 dir = m_DragStartPos - mousePos;
                if (dir != Vector3.zero)
                {
                    if (dir.sqrMagnitude > m_CursorRadius * m_CursorRadius)
                    {
                        dir = dir.normalized * m_CursorRadius;
                    }
                    Cursor.localPosition = -dir;
                }

                if (ControlledCharacter)
                {
                    // Calculate new direction and apply it to character.
                    Vector3 cursorPlanePos = Constants.UICamera.cachedCamera.WorldToScreenPoint(m_CursorOriginalWorldPos);
                    m_CursorOriginalWorldPos.z = 0;
                    mousePos.z = 0;
                    Vector3 planeDirection = mousePos - cursorPlanePos;
                    planeDirection.Normalize();

                    Vector3 worldDirection = new Vector3(planeDirection.x, 0, planeDirection.y);
                    Quaternion rotation = Quaternion.identity;
                    //if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
                    //{
                    //    rotation = Quaternion.LookRotation(worldDirection);
                    //}
                    //else
                    //{
                    //    rotation = Quaternion.LookRotation(-worldDirection);
                    //}
                    rotation = Quaternion.LookRotation(-worldDirection);
                    if (rotation != m_LastRotation || m_LastRotation == Quaternion.identity)
                    {
                        // Send sync msg.
                        m_LastRotation = rotation;
                    }
                    else
                    {
                        // Do not send sync msg.
                    }

                    // ControledCharacter.WorldRotation = rotation.eulerAngles;
                    // Calculater new point and apply it to character.
                    Vector3 newTargetPoint = ControlledCharacter.transform.TransformPoint(Vector3.forward * MoveSpeed * Time.deltaTime);
                    Vector3 moveOffset = ControlledCharacter.transform.forward * MoveSpeed * Time.deltaTime;
                    ControlledCharacter.PostCharacterTransform(newTargetPoint, rotation.eulerAngles);
                    //ControledCharacter.transform.position = newTargetPoint;
                    if (ControlledCharacter.Commander.CurrentCommand != null && ControlledCharacter.Commander.CurrentCommand.Type != CharacterCommand.CC_Walk)
                    {
                        ControlledCharacter.ExecuteCommand(CharacterCommand.CC_Walk);
                    }
                }
            }
        }
    }
}
