using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Air2000
{
    public class UI_LoginMenu : Performer
    {
        protected override void Awake()
        {
            UIHelper.SetButtonEvent(transform, "Panel/Btn_SinglePlayer/button", OnClickSinglePlayer);
            UIHelper.SetButtonEvent(transform, "Panel/Btn_MultiPlayer/button", OnClickMultiPlayer);
            UIHelper.SetButtonEvent(transform, "Panel/Btn_About/button", OnClickAbout);
            base.Awake();
        }
        //public override void Visiable(Activity<LoginModule, LoginActivityPerformer> context)
        //{
        //    base.Visiable(context);
        //    PlayerProvider.Instance.Logout();
        //    if (LoginScenePerformer.Instance)
        //    {
        //        LoginScenePerformer.Instance.SetLookAt(LoginScenePerformer.Instance.CameraLookAtMenu);
        //    }
        //}
        private void OnClickSinglePlayer(GameObject go)
        {
            //ActivityPerformer.DisplayToast("暂未开放，请选择联网模式");
            return;
            Constants.IsSinglePlayer = true;
            SceneManager.Goto((int)SceneName.Battle);
        }
        private void OnClickMultiPlayer(GameObject go)
        {
            if (m_ConnectProgressDialog)
            {
                m_ConnectProgressDialog.Close();
            }
            //m_ConnectProgressDialog = ActivityPerformer.DisplayProgress("正在连接服务器", 30f, true);
            ConnectToLogicServer();
        }
        private void OnClickAbout(GameObject go)
        {
            //ActivityPerformer.DisplayToast("Copyright © 2014-2016 Air2000 Studio.", 2.0f);
        }

        private ProgressDialog m_ConnectProgressDialog;
        private void ConnectToLogicServer()
        {
            NetManager.ConnectTo((int)ServerType.Logic, Constants.SERVER_ADDRESS, Constants.SERVER_PORT,  
               delegate (NetConnection connection,object param)
                {
                    if (m_ConnectProgressDialog)
                    {
                        m_ConnectProgressDialog.Close();
                        //ActivityPerformer.DisplayToast("成功连接至服务器");
                        //ActiveFragment<UI_Login>();
                    }
                },
               delegate (NetConnection connection,  object param)
                {
                    if (m_ConnectProgressDialog)
                    {
                        m_ConnectProgressDialog.Close();
                    }
                    //ActivityPerformer.DisplayToast("与服务器连接异常");
                    //if (ContextProvider.CurrentScene != null && ContextProvider.CurrentScene.GetType() != typeof(LoginScene))
                    //{
                    //    ContextProvider.StartScene<LoginScene>();
                    //}
                },null,null
            );
        }
    }
}
