using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Air2000
{
    public class UI_Login : Performer
    {
        public UIInput Account;
        public UIInput Password;
        protected override void Awake()
        {
            base.Awake();
            UIHelper.SetButtonEvent(transform, "Login/Btn_Login/button", OnClickLogin);
            UIHelper.SetButtonEvent(transform, "Login/Btn_Register/button", OnClickRegister);
            UIHelper.SetButtonEvent(transform, "Login/Btn_BackToMenu", OnClickBackToMenu);
        }
        //public override void Visiable(Activity<LoginModule, LoginActivityPerformer> context)
        //{
        //    base.Visiable(context);
        //    NetManager.BindNetworkEvent((int)AccountMessage.GO_ACCOUNT_LOGIN_RETURN, OnLoginReturn);
        //}
        //public override void Invisiable(Activity<LoginModule, LoginActivityPerformer> context)
        //{
        //    base.Invisiable(context);
        //    NetManager.UnbindNetworkEvent((int)AccountMessage.GO_ACCOUNT_LOGIN_RETURN, OnLoginReturn);
        //}
        private void OnLoginReturn(Evt obj)
        {
            PBMessage.go_login_return result = NetManager.DeserializeNetPacket<PBMessage.go_login_return>(obj);
            if (result == null)
            {
                return;
            }
            switch (result.errorid)
            {
                case 1:
                    //ActivityPerformer.DisplayToast("登录成功");
                    //PlayerProvider.Instance.CreateHero(result.roleiddata);
                    //ContextProvider.StartScene<CityScene>();
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
        private void OnClickLogin(GameObject go)
        {
            if (Account == null || Password == null)
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
            NetManager.SendNetPacket<PBMessage.go_login_request>((int)AccountMessage.GO_ACCOUNT_LOGIN_REQUEST, new PBMessage.go_login_request()
            {
                logintype = 2,
                account = Account.value,
                pwd = Password.value
            });
        }
        private void OnClickRegister(GameObject go)
        {
            //ActiveFragment<Frag_Login_RegisterView>();
        }
        private void OnClickBackToMenu(GameObject go)
        {
            //ActiveFragment<Frag_Login_MenuView>();
        }
    }
}
