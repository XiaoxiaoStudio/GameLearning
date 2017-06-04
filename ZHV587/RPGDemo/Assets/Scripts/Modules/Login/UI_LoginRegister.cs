using System;
using System.Collections.Generic;
using UnityEngine;

namespace Air2000
{
    public class UI_LoginRegister : Performer
    {
        public UIInput Account;
        public UIInput Password;
        public UIInput ConfirmPassword;
        protected override void Awake()
        {
            base.Awake();

            UIHelper.SetButtonEvent(transform, "Panel/Btn_Register/button", OnClickRegister);
            UIHelper.SetButtonEvent(transform, "Panel/Btn_BackToLoginView", OnClickBackToLoginView);
        }
        //public override void Visiable(Activity<LoginModule, LoginActivityPerformer> context)
        //{
        //    base.Visiable(context);
        //    NetManager.BindNetworkEvent((int)AccountMessage.GO_ACCOUNT_LOGIN_RETURN, OnRegisterReturn);
        //}
        //public override void Invisiable(Activity<LoginModule, LoginActivityPerformer> context)
        //{
        //    base.Invisiable(context);
        //    NetManager.UnbindNetworkEvent((int)AccountMessage.GO_ACCOUNT_LOGIN_RETURN, OnRegisterReturn);
        //}
        private void OnRegisterReturn(Evt obj)
        {
            PBMessage.go_login_return result = NetManager.DeserializeNetPacket<PBMessage.go_login_return>(obj);
            if (result == null)
            {
                return;
            }
            switch (result.errorid)
            {
                case 1:
                    //ActivityPerformer.DisplayToast("注册成功");
                    PlayerProvider.Instance.CreateHero(result.roleiddata);
                    //ActiveFragment<Frag_Login_RoleCreateView>();
                    break;
                case 2:
                    //ActivityPerformer.DisplayToast("该账号已经存在");
                    break;
                case 3:
                    //ActivityPerformer.DisplayToast("帐号含有特殊字符");
                    break;
                case 4:
                    //ActivityPerformer.DisplayToast("密码错误");
                    break;
                case 5:
                    //ActivityPerformer.DisplayToast("该账号已经存在");
                    break;
                default:
                    break;
            }
        }
        private void OnClickRegister(GameObject go)
        {
            if (Account == null || Password == null || ConfirmPassword == null)
            {
                //ActivityPerformer.DisplayToast("系统错误");
                return;
            }
            if (string.IsNullOrEmpty(Account.value))
            {
                //ActivityPerformer.DisplayToast("请输入用户名");
                return;
            }
            if (string.IsNullOrEmpty(Password.value))
            {
                //ActivityPerformer.DisplayToast("请输入密码");
                return;
            }
            if (ConfirmPassword.value != Password.value)
            {
                //ActivityPerformer.DisplayToast("两次输入的密码不一致，请重新输入");
                return;
            }
            NetManager.SendNetPacket<PBMessage.go_login_request>((int)AccountMessage.GO_ACCOUNT_LOGIN_REQUEST, new PBMessage.go_login_request()
            {
                logintype = 1,
                account = Account.value,
                pwd = Password.value
            });

        }
        private void OnClickBackToLoginView(GameObject go)
        {
            //ActiveFragment(typeof(UI_Login));
        }

        private void OnClickBackToMenu(GameObject go)
        {
            //ActiveFragment(typeof(UI_LoginMenu));
        }
    }
}
