using System;
using System.Collections.Generic;
using UnityEngine;
using System.Security;
using Mono.Xml;
using System.Collections;

namespace Air2000
{
    public class SkillController : CharacterComponent
    {
        private List<Skill> m_Skills = new List<Skill>();
        private List<Skill> m_ActiveSkills = new List<Skill>();

        void OnSwapCommand(Commander.Command lastCommand, Commander.Command currentCommand)
        {
            if (lastCommand != null)
            {
                Skill skill = TryGetSkill(lastCommand.Type);
                if (skill != null && skill.Status == SkillStatus.Active && skill.StopMode == SkillStopMode.OnCommandEnd)
                {
                    skill.End();
                    m_ActiveSkills.Remove(skill);
                }
            }
            if (currentCommand != null)
            {
                Skill skill = TryGetSkill(currentCommand.Type);
                if (skill != null)
                {
                    if (skill.Status == SkillStatus.Inactive)
                    {
                        m_ActiveSkills.Add(skill);
                        skill.Begin();
                    }
                    else
                    {
                        if (skill.PlayMode == SkillPlayMode.StopCurrent)
                        {
                            skill.End();
                            m_ActiveSkills.Add(skill);
                            skill.Begin();
                        }
                    }
                }
            }
        }
        protected override void Update()
        {
            base.Update();
            if (m_ActiveSkills != null && m_ActiveSkills.Count > 0)
            {
                for (int i = 0; i < m_ActiveSkills.Count; i++)
                {
                    Skill skill = m_ActiveSkills[i];
                    if (skill == null) continue;
                    if (skill.IsFinish())
                    {
                        m_ActiveSkills.Remove(skill);
                        i--;
                        continue;
                    }
                    skill.Update();
                }
            }
        }
        public override void ParseXML(SecurityElement element, Character character)
        {
            base.ParseXML(element, character);
            ArrayList skillElements = element.Children;
            if (skillElements == null || skillElements.Count == 0)
            {
                return;
            }
            for (int i = 0; i < skillElements.Count; i++)
            {
                SecurityElement skillElement = skillElements[i] as SecurityElement;
                if (skillElement == null) continue;
                string triggerCommandStr = skillElement.Attribute("TriggerCommand");
                if (string.IsNullOrEmpty(triggerCommandStr)) continue;
                CharacterCommand triggerCommand = (CharacterCommand)CharacterSystemUtils.TryParseEnum<CharacterCommand>(triggerCommandStr);
                if (triggerCommand == CharacterCommand.None) continue;
                Skill skill = TryGetSkill(triggerCommand);
                if (skill != null) continue;
                skill = new Skill();
                skill.TriggerCommand = triggerCommand;
                skill.ParseXML(skillElement, this);
                AddSkill(skill);
            }
        }
        public override void OnCharacterInitialized(Character character)
        {
            base.OnCharacterInitialized(character);
            Commander.PostSwapCommand -= OnSwapCommand;
            Commander.PostSwapCommand += OnSwapCommand;
        }
        public Skill TryGetSkill(CharacterCommand command)
        {
            if (command == CharacterCommand.None)
            {
                return null;
            }
            if (m_Skills == null || m_Skills.Count == 0)
            {
                return null;
            }
            for (int i = 0; i < m_Skills.Count; i++)
            {
                Skill skill = m_Skills[i];
                if (skill == null) continue;
                if (skill.TriggerCommand == command)
                {
                    return skill;
                }
            }
            return null;
        }
        public bool AddSkill(Skill skill)
        {
            if (m_Skills == null) m_Skills = new List<Skill>();
            m_Skills.Add(skill);
            return true;
        }
    }
}
