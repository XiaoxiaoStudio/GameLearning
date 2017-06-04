using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Air2000
{
    public class UI_BattleCharacterDebug : Performer
    {
        public UILabel MotionTag;
        public UILabel AnimationCrossfaderTag;
        public UILabel CommandTag;
        public UILabel AITag;

        private float m_LastSwapMotionTime = -1f;
        private float m_CurrentMotionTime;
        private string m_MotionTag;

        private float m_LastSwapAnimationTime = -1f;
        private float m_CurrentAnimationTime;
        private string m_AnimationTag;

        private float m_LastSwapCommandTime = -1f;
        private float m_CurrentCommandTime;
        private string m_CommandTag;

        private float m_LastSwapAIStateTime = -1f;
        private float m_CurrentAIStateTime;
        private string m_AITag;

        protected override void Awake()
        {
            base.Awake();
            Character character = PlayerProvider.Hero.Character;
            if (character == null)
            {
                //InActiveFragment();
                return;
            }
            character.MotionMachine.PostSwapMotion += OnSwapMotion;
            character.AnimationCrossfader.PostSwapAnimation += OnSwapAnimation;
            character.Commander.PostSwapCommand += OnSwapCommand;
            character.AIMachine.PostSwapState += OnSwapAIState;
        }
        private void OnSwapMotion(Motion lastMotion, Motion currentMotion)
        {
            string str = string.Empty;
            if (lastMotion == null)
            {
                str = "Null->";
            }
            else
            {
                str = lastMotion.Type.ToString() + "->";
            }

            if (currentMotion == null)
            {
                str += "Null";
            }
            else
            {
                str += currentMotion.Type.ToString() + ",";
            }
            if (m_LastSwapMotionTime == -1f)
            {
                str += "Cost:0";
            }
            else
            {
                float costTime = Time.realtimeSinceStartup - m_LastSwapMotionTime;
                string costTimeStr = string.Format("{0:0.00}", costTime);
                str += "Cost:" + costTimeStr;
            }
            m_MotionTag = str;
            m_CurrentMotionTime = 0;
            m_LastSwapMotionTime = Time.realtimeSinceStartup;
        }
        private void OnSwapAnimation(string lastClip, string currentClip)
        {
            string str = string.Empty;
            if (string.IsNullOrEmpty(lastClip))
            {
                str = "Null->";
            }
            else
            {
                str = lastClip + "->";
            }

            if (string.IsNullOrEmpty(currentClip))
            {
                str += "Null";
            }
            else
            {
                str += currentClip + ",";
            }
            if (m_LastSwapAnimationTime == -1f)
            {
                str += "Cost:0";
            }
            else
            {
                float costTime = Time.realtimeSinceStartup - m_LastSwapAnimationTime;
                string costTimeStr = string.Format("{0:0.00}", costTime);
                str += "Cost:" + costTimeStr;
            }
            m_AnimationTag = str;
            m_CurrentAnimationTime = 0;
            m_LastSwapAnimationTime = Time.realtimeSinceStartup;
        }
        private void OnSwapCommand(Commander.Command lastCommand, Commander.Command currentCommand)
        {
            string str = string.Empty;
            if (lastCommand == null)
            {
                str = "Null->";
            }
            else
            {
                str = lastCommand.Type.ToString() + "->";
            }

            if (currentCommand == null)
            {
                str += "Null";
            }
            else
            {
                str += currentCommand.Type.ToString() + ",";
            }
            if (m_LastSwapCommandTime == -1f)
            {
                str += "Cost:0";
            }
            else
            {
                float costTime = Time.realtimeSinceStartup - m_LastSwapCommandTime;
                string costTimeStr = string.Format("{0:0.00}", costTime);
                str += "Cost:" + costTimeStr;
            }
            m_CommandTag = str;
            m_CurrentCommandTime = 0;
            m_LastSwapCommandTime = Time.realtimeSinceStartup;
        }
        private void OnSwapAIState(AIState lastState, AIState currentState)
        {
            string str = string.Empty;
            if (lastState == null)
            {
                str = "Null->";
            }
            else
            {
                str = lastState.Name + "->";
            }

            if (currentState == null)
            {
                str += "Null";
            }
            else
            {
                str += currentState.Name + ",";
            }
            if (m_LastSwapAIStateTime == -1f)
            {
                str += "Cost:0";
            }
            else
            {
                float costTime = Time.realtimeSinceStartup - m_LastSwapAIStateTime;
                string costTimeStr = string.Format("{0:0.00}", costTime);
                str += "Cost:" + costTimeStr;
            }
            m_AITag = str;
            m_CurrentAIStateTime = 0;
            m_LastSwapAIStateTime = Time.realtimeSinceStartup;
        }

        protected override void Update()
        {
            base.Update();
            m_CurrentMotionTime += Time.deltaTime;
            m_CurrentAnimationTime += Time.deltaTime;
            m_CurrentCommandTime += Time.deltaTime;
            m_CurrentAIStateTime += Time.deltaTime;

            if (MotionTag && string.IsNullOrEmpty(m_MotionTag) == false)
            {
                string timeStr = string.Format("{0:0.00}", m_CurrentMotionTime);
                MotionTag.text = m_MotionTag + ",CurrentTime:" + timeStr;
            }

            if (AnimationCrossfaderTag && string.IsNullOrEmpty(m_AnimationTag) == false)
            {
                string timeStr = string.Format("{0:0.00}", m_CurrentAnimationTime);
                AnimationCrossfaderTag.text = m_AnimationTag + ",CurrentTime:" + timeStr;
            }

            if (CommandTag && string.IsNullOrEmpty(m_CommandTag) == false)
            {
                string timeStr = string.Format("{0:0.00}", m_CurrentCommandTime);
                CommandTag.text = m_CommandTag + ",CurrentTime:" + timeStr;
            }

            if (AITag && string.IsNullOrEmpty(m_AITag) == false)
            {
                string timeStr = string.Format("{0:0.00}", m_CurrentAIStateTime);
                AITag.text = m_AITag + ",CurrentTime:" + timeStr;
            }
        }
    }
}
