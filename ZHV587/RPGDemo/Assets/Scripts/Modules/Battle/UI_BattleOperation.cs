using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Air2000
{
    public class UI_BattleOperation : Performer
    {
        protected override void Awake()
        {
            base.Awake();
            UIHelper.SetButtonEvent(transform, "Btn_Attack", OnClickAttack);
            UIHelper.SetButtonEvent(transform, "Btn_Skill_1", OnClickSkill_1);
            UIHelper.SetButtonEvent(transform, "Btn_Skill_2", OnClickSkill_2);
            UIHelper.SetButtonEvent(transform, "Btn_Skill_3", OnClickSkill_3);

        }
        //public override void Visiable(Activity<BattleModule, UI_BattleMain> context)
        //{
        //    base.Visiable(context);
        //}
        //public override void Invisiable(Activity<BattleModule, UI_BattleMain> context)
        //{
        //    base.Invisiable(context);
        //}
        private void OnClickAttack(GameObject go)
        {
            Character character = PlayerProvider.Hero.Character;
            if (character)
            {
                if (character.Commander.CurrentCommand != null)
                {
                    switch (character.Commander.CurrentCommand.Type)
                    {
                        case CharacterCommand.CC_Attack_1:
                            character.ExecuteCommand(CharacterCommand.CC_Attack_2);
                            break;
                        case CharacterCommand.CC_Attack_2:
                            character.ExecuteCommand(CharacterCommand.CC_Attack_3);
                            break;
                        case CharacterCommand.CC_Attack_3:
                            character.ExecuteCommand(CharacterCommand.CC_Attack_4);
                            break;
                        case CharacterCommand.CC_Attack_4:
                            character.ExecuteCommand(CharacterCommand.CC_Attack_1);
                            break;
                        default:
                            character.ExecuteCommand(CharacterCommand.CC_Attack_1);
                            break;
                    }
                }
                else
                {
                    character.ExecuteCommand(CharacterCommand.CC_Attack_1);
                }
            }
        }
        private void OnClickSkill_1(GameObject go)
        {
            PlayerProvider.Hero.Character.ExecuteCommand(CharacterCommand.CC_Skill_1);
        }
        private void OnClickSkill_2(GameObject go)
        {
            PlayerProvider.Hero.Character.ExecuteCommand(CharacterCommand.CC_Skill_2);
        }
        private void OnClickSkill_3(GameObject go)
        {
            PlayerProvider.Hero.Character.ExecuteCommand(CharacterCommand.CC_Skill_3);
        }
    }
}
