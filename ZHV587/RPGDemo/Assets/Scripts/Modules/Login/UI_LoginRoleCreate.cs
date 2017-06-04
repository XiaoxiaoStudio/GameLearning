using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Air2000
{
    public class UI_LoginRoleCreate : Performer
    {
        public UIInput NickName;
        public List<UIToggle> RadioButtons = new List<UIToggle>();
        public GameObject WarriorModel;
        public GameObject ArcherModel;
        public Profession CurrentSelectedProfession;
        protected override void Awake()
        {
            base.Awake();
            UIHelper.SetButtonEvent(transform, "ArchoredUI/LeftTop/Btn_Back", OnClickBack);

            UIHelper.SetButtonEvent(transform, "ArchoredUI/Right/Btn_Warrior", OnClickWarrior);
            UIHelper.SetButtonEvent(transform, "ArchoredUI/Right/Btn_Archer", OnClickArcher);

            UIHelper.SetButtonEvent(transform, "ArchoredUI/Right/Btn_Confirm", OnClickConfirm);
        }
        //public override void Visiable(Activity<LoginModule, LoginActivityPerformer> context)
        //{
        //    base.Visiable(context);
        //    if (RadioButtons != null && RadioButtons.Count > 0)
        //    {
        //        for (int i = 0; i < RadioButtons.Count; i++)
        //        {
        //            if (i == 0)
        //            {
        //                RadioButtons[i].value = true;
        //            }
        //            else
        //            {
        //                RadioButtons[i].value = false;
        //            }
        //        }
        //    }
        //    if (WarriorModel)
        //    {
        //        WarriorModel.SetActive(true);
        //    }
        //    CurrentSelectedProfession = Profession.Warrior;
        //    NetManager.BindNetworkEvent((int)AccountMessage.GO_ACCOUNT_UPDATE_ROLE_RETURN, OnResponseUpdateRoleInfo);
        //    LoginScenePerformer.Instance.SetLookAt(LoginScenePerformer.Instance.CameraLookAtWarrior);
        //    LoginScenePerformer.Instance.CurrentCharacter = LoginScenePerformer.Instance.Warrior;
        //}
        private void OnResponseUpdateRoleInfo(Evt eventObj)
        {
            PBMessage.go_login_updateRoleInfoReturn pak = NetManager.DeserializeNetPacket<PBMessage.go_login_updateRoleInfoReturn>(eventObj);
            if (pak == null)
            {
                //ActivityPerformer.DisplayToast("协议解析错误");
            }
            else
            {
                switch (pak.errorid)
                {
                    case 1:
                        //ActivityPerformer.DisplayToast("创建 " + pak.name + " 成功", 2);
                        PlayerProvider.HeroInfo.profesion = pak.pression;
                        PlayerProvider.HeroInfo.name = pak.name;
                        PlayerProvider.Instance.CreateHero();
                        //ContextProvider.StartScene<CityScene>();
                        break;
                    case 2:
                        //ActivityPerformer.DisplayToast("名字有非法字符");
                        break;
                    case 3:
                        //ActivityPerformer.DisplayToast("名字已经被占用");
                        break;
                    case 4:
                        //ActivityPerformer.DisplayToast("职业不存在");
                        break;
                    default:
                        break;
                }
            }
        }
        //public override void Invisiable(Activity<LoginModule, LoginActivityPerformer> context)
        //{
        //    base.Invisiable(context);
        //    NetManager.UnbindNetworkEvent((int)AccountMessage.GO_ACCOUNT_UPDATE_ROLE_RETURN, OnResponseUpdateRoleInfo);
        //}
        private void OnClickConfirm(GameObject go)
        {
            if (NickName == null)
            {
                //ActivityPerformer.DisplayToast("系统错误");
                return;
            }
            if (string.IsNullOrEmpty(NickName.value))
            {
                //ActivityPerformer.DisplayToast("请输入昵称");
                return;
            }
            PBMessage.go_login_updateRoleInfoRequest req = new PBMessage.go_login_updateRoleInfoRequest();
            //req.name = Encoding.Unicode.GetString(Encoding.Default.GetBytes(NickName.value));
            req.name = NickName.value;
            req.pression = (int)CurrentSelectedProfession;
            NetManager.SendNetPacket<PBMessage.go_login_updateRoleInfoRequest>((int)AccountMessage.GO_ACCOUNT_UPDATE_ROLE_REQUEST, req);
        }
        private void OnClickBack(GameObject go)
        {
            //ActiveFragment(typeof(UI_LoginMenu));
        }
        private void OnClickWarrior(GameObject go)
        {
            CurrentSelectedProfession = Profession.Warrior;
            if (WarriorModel)
            {
                WarriorModel.SetActive(true);
            }
            if (ArcherModel)
            {
                ArcherModel.SetActive(false);
            }
        }
        private void OnClickArcher(GameObject go)
        {
            CurrentSelectedProfession = Profession.Archer;
            if (ArcherModel)
            {
                ArcherModel.SetActive(true);
            }
            if (WarriorModel)
            {
                WarriorModel.SetActive(false);
            }
        }
    }
}
