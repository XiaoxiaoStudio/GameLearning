using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Air2000
{
    public class UI_Main : Performer
    {
        protected override void Awake()
        {
            base.Awake();

            UIHelper.SetButtonEvent(transform, "AnchoredUI/CenterBottom/Btn_Bag/Button", OnClickBag);
            UIHelper.SetButtonEvent(transform, "AnchoredUI/CenterBottom/Btn_Skill/Button", OnClickSkill);
            UIHelper.SetButtonEvent(transform, "AnchoredUI/CenterBottom/Btn_Pet/Button", OnClickPet);

            UIHelper.SetButtonEvent(transform, "AnchoredUI/RightBottom/Btn_Battle", OnClickBattle);

            UIHelper.SetButtonEvent(transform, "AnchoredUI/LeftCenter/Btn_Mail", OnClickMail);
            UIHelper.SetButtonEvent(transform, "AnchoredUI/LeftCenter/Btn_Ranklist", OnClickRanklist);
            UIHelper.SetButtonEvent(transform, "AnchoredUI/LeftCenter/Btn_Activity", OnClickActivity);
            UIHelper.SetButtonEvent(transform, "AnchoredUI/LeftCenter/Btn_Mall", OnClickMall);

            UIHelper.SetButtonEvent(transform, "AnchoredUI/LeftTop/HeadPanel/head", OnClickHeadArea);

            UIHelper.SetButtonEvent(transform, "AnchoredUI/CenterTop/Btn_Tired/add", OnClickTiredAdd);
            UIHelper.SetButtonEvent(transform, "AnchoredUI/CenterTop/Btn_Diamond/add", OnClickDiamondAdd);
            UIHelper.SetButtonEvent(transform, "AnchoredUI/CenterTop/Btn_Gold/add", OnClickGoldAdd);

            UIHelper.SetButtonEvent(transform, "AnchoredUI/RightTop/Btn_Logout/button", OnClickLogout);
        }
        //public override void Active()
        //{
        //    base.Active();
        //    //Character mainRole = PlayerProvider.Hero.Character;
        //    //if (mainRole)
        //    //{
        //    //    mainRole.transform.position = LoginScenePerformer.Instance.MainRoleStand.position;
        //    //    mainRole.transform.rotation = LoginScenePerformer.Instance.MainRoleStand.rotation;
        //    //    LoginScenePerformer.Instance.SetLookAt(LoginScenePerformer.Instance.CameraLookAtMainRole);
        //    //}
        //    //else
        //    //{
        //    //    PlayerProvider.Hero.PostCreateCharacter += OnCreateMainRole;
        //    //}
        //    ActiveFragment<Frag_Main_RankList>();
        //    ActiveFragment<Frag_Main_RoleInfo>();
        //}
        private void OnCreateMainRole(Player player, Character character)
        {
            player.PostCreateCharacter -= OnCreateMainRole;
            if (character)
            {
                if (CitySceneRoleDisplay.Instance)
                {
                    CitySceneRoleDisplay.Instance.SetupRoleModel(character);
                }
            }
        }
        private void OnClickSetting(GameObject go)
        {

        }
        private void OnClickBackToMenu(GameObject go)
        {
            //ContextProvider.StartActivity<LoginModule>(Context);
        }
        private void OnClickRanklist(GameObject go)
        {
            //ContextProvider.StartActivity<RanklistActivity>(Context);
        }
        private void OnClickBag(GameObject go)
        {
            //ContextProvider.StartActivity<BagModule>(Context);
        }
        private void OnClickSkill(GameObject go)
        {
            //DisplayToast("暂未开放");
        }
        private void OnClickPet(GameObject go)
        {
            //DisplayToast("暂未开放");
        }
        private void OnClickShop(GameObject go)
        {
            //DisplayToast("暂未开放");
        }
        private void OnClickPVPMode(GameObject go)
        {
            //ActiveFragment<Frag_Main_GameRoom>();
        }
        private void OnClickBattle(GameObject go)
        {
            //ContextProvider.StartActivity<MatchModule>();
            //ActiveFragment<Frag_Main_GameRoom>();
        }
        private void OnClickMail(GameObject go)
        {
            //DisplayToast("暂未开放");
        }
        private void OnClickActivity(GameObject go)
        {
            //DisplayToast("暂未开放");
        }
        private void OnClickMall(GameObject go)
        {
            //DisplayToast("暂未开放");
        }
        private void OnClickHeadArea(GameObject go)
        {
            //DisplayToast("我的等级: " + PlayerProvider.HeroInfo.level);
        }
        private void OnClickTiredAdd(GameObject go)
        {
            //DisplayToast("我的体力: " + PlayerProvider.HeroInfo.physical);
        }
        private void OnClickDiamondAdd(GameObject go)
        {
            //DisplayToast("我的钻石: " + PlayerProvider.HeroInfo.gold);
        }
        private void OnClickGoldAdd(GameObject go)
        {
            //DisplayToast("我的金币: " + PlayerProvider.HeroInfo.gold);
        }
        private void OnClickLogout(GameObject go)
        {
            PlayerProvider.Instance.Logout();
            //StartScene<LoginScene>(ContextProvider.CurrentScene);
        }
    }
}
