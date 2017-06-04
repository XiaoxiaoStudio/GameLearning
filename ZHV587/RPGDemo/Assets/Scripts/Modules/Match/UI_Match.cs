using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Air2000
{
    public class UI_Match : Performer
    {
        public UILabel Countdown;
        private int m_TotalTime = 30;
        protected override void Awake()
        {
            base.Awake();
            UIHelper.SetButtonEvent(transform, "Btn_Back", OnClickBack);
        }

        //public override void Active()
        //{
        //    base.Active();
        //    SetNetworkEventListen();
        //    SendRequest<PBMessage.go_copy_list_request>((int)AccountMessage.GO_COPY_LIST_REQUEST, new PBMessage.go_copy_list_request() { battletype = 101 });
        //    CancelInvoke("CountdownFunc");
        //    m_TotalTime = 30;
        //    InvokeRepeating("CountdownFunc", 0f, 1f);
        //    PBMessage.go_copy_match_request req = new PBMessage.go_copy_match_request() { copyid = Constants.COPY_ID };
        //    SendRequest<PBMessage.go_copy_match_request>((int)AccountMessage.GO_COPY_MATCH_REQUEST, req);
        //}
        //public override void Inactive()
        //{
        //    base.Inactive();
        //    CancelInvoke("CountdownFunc");
        //    RemoveNetworkEventListen();
        //}
        private void CountdownFunc()
        {
            m_TotalTime -= 1;
            if (Countdown)
            {
                Countdown.text = "玩家匹配中......" + m_TotalTime + "秒";
            }
            if (m_TotalTime == 0)
            {
                CancelInvoke("CountdownFunc");
                Timeout();
            }
        }
        private void Timeout()
        {
            //DisplayToast("匹配超时");
            CloseView();
        }
        private void CloseView()
        {
            if (Constants.CURRENT_ROOM_INFO != null)
            {
                SendRequest<PBMessage.go_copy_leave_request>((int)AccountMessage.GO_COPY_LEAVE_REQUEST, new PBMessage.go_copy_leave_request() { copyid = Constants.COPY_ID, fbid = Constants.CURRENT_ROOM_INFO.fbid });
            }
            //ContextProvider.PauseActivity<MatchModule>();
        }
        #region Network
        private void SetNetworkEventListen()
        {
            NetManager.BindNetworkEvent((int)AccountMessage.GO_COPY_MATCH_RETURN, OnEnterMatch);
            NetManager.BindNetworkEvent((int)AccountMessage.GO_COPY_LOGIN_RETURN, OnPlayerEnterRoom);
            NetManager.BindNetworkEvent((int)AccountMessage.GO_COPY_ROOMINFO_RETURN, OnRefreshRoomInfo);
            NetManager.BindNetworkEvent((int)AccountMessage.GO_COPY_BEGIN_RETURN, OnBeginGame);
        }
        private void RemoveNetworkEventListen()
        {
            NetManager.UnbindNetworkEvent((int)AccountMessage.GO_COPY_MATCH_RETURN, OnEnterMatch);
            NetManager.UnbindNetworkEvent((int)AccountMessage.GO_COPY_LOGIN_RETURN, OnPlayerEnterRoom);
            NetManager.UnbindNetworkEvent((int)AccountMessage.GO_COPY_ROOMINFO_RETURN, OnRefreshRoomInfo);
            NetManager.UnbindNetworkEvent((int)AccountMessage.GO_COPY_BEGIN_RETURN, OnBeginGame);
        }
        private void SendRequest<T>(int id, T obj)
              where T : class, ProtoBuf.IExtensible
        {
            NetManager.SendNetPacket<T>(id, obj);
        }
        private void OnEnterMatch(Evt eventObj)
        {
            PBMessage.go_copy_amtch_return pak = NetManager.DeserializeNetPacket<PBMessage.go_copy_amtch_return>(eventObj);
            if (pak == null)
            {
                //DisplayToast("数据解析错误");
                CloseView();
            }
            else
            {
                Constants.CURRENT_ROOM_INFO = pak;
                //DisplayToast("成功开始匹配，fbid=" + pak.fbid + ", copyid=" + pak.copyid);
            }
        }
        private void OnPlayerEnterRoom(Evt eventObj)
        {
            PBMessage.go_copy_login_return pak = NetManager.DeserializeNetPacket<PBMessage.go_copy_login_return>(eventObj);
            if (pak == null)
            {
                //DisplayToast("数据解析错误");
            }
            else
            {
            }
        }
        private void OnRefreshRoomInfo(Evt eventObj)
        {
            PBMessage.go_copy_roominfo_return pak = NetManager.DeserializeNetPacket<PBMessage.go_copy_roominfo_return>(eventObj);
            if (pak == null)
            {
                //DisplayToast("数据解析错误");
            }
            else
            {
                PBMessage.go_copy_match_request m;
                PBMessage.go_copy_login_request req = new PBMessage.go_copy_login_request() { ID = Constants.CURRENT_ROOM_INFO.fbid, copyid = Constants.CURRENT_ROOM_INFO.copyid };
                SendRequest<PBMessage.go_copy_login_request>((int)AccountMessage.GO_COPY_BEGIN_REQUEST, req);
            }
        }
        private void OnBeginGame(Evt eventObj)
        {
            PBMessage.go_copy_begin_return pak = NetManager.DeserializeNetPacket<PBMessage.go_copy_begin_return>(eventObj);
            if (pak == null)
            {
                //DisplayToast("数据解析错误");
            }
            else
            {
                switch (pak.errorid)
                {
                    case 1:
                        //DisplayToast("游戏即将开始", 1.5f);
                        //ContextProvider.PauseActivity<MatchModule>();
                        //ContextProvider.StartScene<BattleScene>();
                        break;
                    case 2:
                        //DisplayToast("房间不存在", 1f);
                        break;
                    case 3:
                        //DisplayToast("你不是房主，无法开始游戏", 1f);
                        break;
                    case 4:
                        //DisplayToast("你不属于当前房间", 1f);
                        break;
                    case 5:
                        //DisplayToast("人数不足，无法开始游戏", 1f);
                        break;
                    default:
                        break;
                }
            }
        }
        #endregion

        private void OnClickBack(GameObject go)
        {
            CloseView();
        }
    }
}
