using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Air2000
{
    public class UI_Room : Performer
    {
        public GameObject ProgressView;
        public void ActiveProgress()
        {
            if (ProgressView)
            {
                ProgressView.SetActive(true);
            }
        }
        public void InactiveProgress()
        {
            if (ProgressView)
            {
                ProgressView.SetActive(false);
            }
        }

        public enum RoomType
        {
            PVP = 101,
            Team,
        }
        public GameObject ProgressBar;
        public RoomType CurrentRoomType = RoomType.PVP;
        public List<UIToggle> RadioButtons = new List<UIToggle>();

        public GameObject CreateRoomView;
        public GameObject RoomDetailView;
        public GameObject RoomListView;

        public UIInput NewRoomName;


        public UIGrid RoomList;
        public GameObject RoomListGrid;

        public UIGrid RoomDetailLeftList;
        public GameObject RoomDetailLeftListGrid;

        public UIGrid RoomDetailRightList;
        public GameObject RoomDetailRightListGrid;

        private Dictionary<GameObject, PBMessage.go_copy_roughroominfo> m_CurrentRoomInfos = new Dictionary<GameObject, PBMessage.go_copy_roughroominfo>();
        private Dictionary<GameObject, PBMessage.go_login_playerInfo> m_CurrentRoomPlayers = new Dictionary<GameObject, PBMessage.go_login_playerInfo>();

        private PBMessage.go_copy_roughroominfo m_CurrentSelectedRoomInfo;

        protected override void Awake()
        {
            base.Awake();
            UIHelper.SetButtonEvent(transform, "NavBar/Tab/PVP", OnClickPVPTab);
            UIHelper.SetButtonEvent(transform, "NavBar/Tab/Team", OnClickTeamTab);
            UIHelper.SetButtonEvent(transform, "NavBar/Tab/Create", OnClickCreateTab);
            UIHelper.SetButtonEvent(transform, "NavBar/Tab/Exit", OnClickExitTab);

            UIHelper.SetButtonEvent(transform, "RoomCreate/Btn_OK", OnRoomCreateViewClickConfirm);
            UIHelper.SetButtonEvent(transform, "RoomCreate/Btn_Back", OnRoomCreateViewClickBack);

            UIHelper.SetButtonEvent(transform, "RoomDetail/Btn_OK", OnRoomDetailViewClickConfirm);
            UIHelper.SetButtonEvent(transform, "RoomDetail/Btn_Back", OnRoomDetailViewClickBack);
        }

        public void Active()
        {
            CurrentRoomType = RoomType.PVP;
            if (RadioButtons != null && RadioButtons.Count > 0)
            {
                for (int i = 0; i < RadioButtons.Count; i++)
                {
                    if (i == 0)
                    {
                        RadioButtons[i].value = true;
                    }
                    else
                    {
                        RadioButtons[i].value = false;
                    }
                }
            }
            if (CreateRoomView)
            {
                CreateRoomView.SetActive(false);
            }
            if (RoomDetailView)
            {
                RoomDetailView.SetActive(false);
            }
            if (ProgressBar)
            {
                ProgressBar.SetActive(false);
            }
            if (RoomListView)
            {
                RoomListView.SetActive(true);
            }
            SetNetworkEventListen();
            SendRequest<PBMessage.go_copy_list_request>((int)AccountMessage.GO_COPY_LIST_REQUEST, new PBMessage.go_copy_list_request() { battletype = 101 });
        }
        public void Inactive()
        {
            RemoveNetworkEventListen();
        }

        #region Network
        private void SetNetworkEventListen()
        {
            NetManager.BindNetworkEvent((int)AccountMessage.GO_COPY_CREATE_RETURN, OnCreateRoomReturn);
            NetManager.BindNetworkEvent((int)AccountMessage.GO_COPY_LIST_RETURN, OnGotRoomlist);
            NetManager.BindNetworkEvent((int)AccountMessage.GO_COPY_ROOMINFO_RETURN, OnGotRoomDetailInfo);
            NetManager.BindNetworkEvent((int)AccountMessage.GO_COPY_LOGIN_RETURN, OnPlayerEnterRoom);
        }
        private void RemoveNetworkEventListen()
        {
            NetManager.UnbindNetworkEvent((int)AccountMessage.GO_COPY_CREATE_RETURN, OnCreateRoomReturn);
            NetManager.UnbindNetworkEvent((int)AccountMessage.GO_COPY_LIST_RETURN, OnGotRoomlist);
            NetManager.UnbindNetworkEvent((int)AccountMessage.GO_COPY_ROOMINFO_RETURN, OnGotRoomDetailInfo);
            NetManager.UnbindNetworkEvent((int)AccountMessage.GO_COPY_LOGIN_RETURN, OnPlayerEnterRoom);
        }
        private void SendRequest<T>(int id, T obj)
              where T : class, ProtoBuf.IExtensible
        {
            if (ProgressBar)
            {
                ProgressBar.SetActive(true);
            }
            NetManager.SendNetPacket<T>(id, obj);
        }
        private void OnCreateRoomReturn(Evt eventObj)
        {
            if (ProgressBar)
            {
                ProgressBar.SetActive(false);
            }
            PBMessage.go_copy_create_return pak = NetManager.DeserializeNetPacket<PBMessage.go_copy_create_return>(eventObj);
            if (pak == null)
            {
                //DisplayToast("数据解析错误");
            }
            else
            {
                //DisplayToast("创建房间成功，fbid=" + pak.fbid + ", copyid=" + pak.copyid + ", name=" + pak.sign);
            }
        }
        private void OnGotRoomDetailInfo(Evt eventObj)
        {
            if (ProgressBar)
            {
                ProgressBar.SetActive(false);
            }
            PBMessage.go_copy_roominfo_return pak = NetManager.DeserializeNetPacket<PBMessage.go_copy_roominfo_return>(eventObj);
            if (pak == null)
            {
                //DisplayToast("数据解析错误");
            }
            else
            {
                if (pak.fbid != m_CurrentSelectedRoomInfo.fbid)
                {
                    //DisplayToast("房间ID解析错误");
                }
                else
                {
                    DisplayRoomDetailView(pak);
                }
            }
        }
        private void OnGotRoomlist(Evt eventObj)
        {
            if (ProgressBar)
            {
                ProgressBar.SetActive(false);
            }
            PBMessage.go_copy_rouglist_return pak = NetManager.DeserializeNetPacket<PBMessage.go_copy_rouglist_return>(eventObj);
            if (pak == null)
            {
                //DisplayToast("数据解析错误");
            }
            else
            {
                if (pak.roomdata.Count == 0)
                {
                    //DisplayToast("当前没有房间，点击创建", 1.5f);
                }
                DisplayRoomList(RoomType.PVP, pak.roomdata);
            }
        }
        private void OnPlayerEnterRoom(Evt eventObj)
        {
            if (ProgressBar)
            {
                ProgressBar.SetActive(false);
            }
            PBMessage.go_copy_login_return pak = NetManager.DeserializeNetPacket<PBMessage.go_copy_login_return>(eventObj);
            if (pak == null)
            {
                //DisplayToast("数据解析错误");
            }
            else
            {
                //DisplayToast("进入房间成功，游戏将在5秒钟后开始", 1.5f);
            }
        }
        #endregion

        /// <summary>
        /// 显示房间列表
        /// </summary>
        /// <param name="type"></param>
        /// <param name="infos"></param>
        private void DisplayRoomList(RoomType type, List<PBMessage.go_copy_roughroominfo> infos)
        {
            if (type != CurrentRoomType)
            {
                return;
            }
            if (m_CurrentRoomInfos.Count > 0)
            {
                Dictionary<GameObject, PBMessage.go_copy_roughroominfo>.Enumerator it = m_CurrentRoomInfos.GetEnumerator();
                for (int i = 0; i < m_CurrentRoomInfos.Count; i++)
                {
                    it.MoveNext();
                    KeyValuePair<GameObject, PBMessage.go_copy_roughroominfo> kvp = it.Current;
                    if (kvp.Key)
                    {
                        GameObject.DestroyImmediate(kvp.Key);
                    }
                }
            }
            m_CurrentRoomInfos.Clear();
            if (RoomList != null && RoomListGrid != null
                && infos != null && infos.Count > 0)
            {
                for (int i = 0; i < infos.Count; i++)
                {
                    PBMessage.go_copy_roughroominfo info = infos[i];
                    if (info == null)
                    {
                        infos.RemoveAt(i);
                        i--;
                        continue;
                    }
                    GameObject item = GameObject.Instantiate(RoomListGrid) as GameObject;
                    if (item == null) continue;
                    UIHelper.SetButtonEvent(item.transform, OnClickRoomlistGrid);
                    UILabel titleLabel = item.transform.Find("TitleLabel").GetComponent<UILabel>();
                    if (titleLabel)
                    {
                        titleLabel.text = info.sign;
                    }
                    UILabel stateLabel = item.transform.Find("StateLabel").GetComponent<UILabel>();
                    if (stateLabel)
                    {
                        stateLabel.text = "当前在线: " + info.rolenum;
                    }

                    item.transform.SetParent(RoomList.transform);
                    item.transform.localPosition = Vector3.zero;
                    item.transform.localScale = Vector3.one;
                    item.SetActive(true);
                    RoomList.enabled = true;

                    m_CurrentRoomInfos.Add(item, info);

                }
            }
        }

        /// <summary>
        /// 显示房间详细信息的界面
        /// </summary>
        /// <param name="data"></param>
        private void DisplayRoomDetailView(PBMessage.go_copy_roominfo_return data)
        {
            if (RoomListView)
            {
                RoomListView.SetActive(false);
            }
            if (CreateRoomView)
            {
                CreateRoomView.SetActive(false);
            }
            if (RoomDetailView)
            {
                RoomDetailView.SetActive(true);
            }
            UILabel roomnameLable = RoomDetailView.transform.Find("RoomTitle").GetComponent<UILabel>();
            if (roomnameLable)
            {
                roomnameLable.text = m_CurrentSelectedRoomInfo.sign;
            }
            if (m_CurrentRoomPlayers.Count > 0)
            {
                Dictionary<GameObject, PBMessage.go_login_playerInfo>.Enumerator it = m_CurrentRoomPlayers.GetEnumerator();
                for (int i = 0; i < m_CurrentRoomPlayers.Count; i++)
                {
                    it.MoveNext();
                    KeyValuePair<GameObject, PBMessage.go_login_playerInfo> kvp = it.Current;
                    if (kvp.Key)
                    {
                        GameObject.DestroyImmediate(kvp.Key);
                    }
                }
            }
            m_CurrentRoomPlayers.Clear();
            for (int i = 0; i < data.roleinfo.Count; i++)
            {
                PBMessage.go_login_playerInfo info = data.roleinfo[i];
                if (info == null)
                {
                    data.roleinfo.RemoveAt(i);
                    i--;
                    continue;
                }
                GameObject item = null;
                if (info.camp == 0)
                {
                    item = GameObject.Instantiate(RoomDetailLeftListGrid) as GameObject;
                    item.transform.SetParent(RoomDetailLeftList.transform);
                    RoomDetailLeftList.enabled = true;
                }
                else if (info.camp == 1)
                {
                    item = GameObject.Instantiate(RoomDetailRightListGrid) as GameObject;
                    item.transform.SetParent(RoomDetailRightList.transform);
                    RoomDetailRightList.enabled = true;
                }
                item.transform.localPosition = Vector3.zero;
                item.transform.localScale = Vector3.one;
                item.SetActive(true);
                m_CurrentRoomPlayers.Add(item, info);
                UILabel nameLabel = item.transform.Find("RoleNameLabel").GetComponent<UILabel>();
                if (nameLabel)
                {
                    nameLabel.text = info.name;
                }

                UILabel roleLabel = item.transform.Find("RoleLevelLabel").GetComponent<UILabel>();
                if (roleLabel)
                {
                    roleLabel.text = info.level.ToString();
                }

                Transform roleState = item.transform.Find("RoleState");
                if (roleState)
                {
                    if (info.roleid == data.ownid)
                    {
                        roleState.gameObject.SetActive(true);
                    }
                    else
                    {
                        roleState.gameObject.SetActive(false);
                    }
                }
            }
        }

        /// <summary>
        /// 点击房间列表中的某一个房间
        /// </summary>
        /// <param name="go"></param>
        private void OnClickRoomlistGrid(GameObject go)
        {
            if (ProgressBar)
            {
                ProgressBar.SetActive(false);
            }

            m_CurrentRoomInfos.TryGetValue(go, out m_CurrentSelectedRoomInfo);
            if (m_CurrentSelectedRoomInfo != null)
            {
                PBMessage.go_copy_roominfo_request req = new PBMessage.go_copy_roominfo_request() { copyid = m_CurrentSelectedRoomInfo.copyid, sfbid = m_CurrentSelectedRoomInfo.fbid };
                SendRequest<PBMessage.go_copy_roominfo_request>((int)AccountMessage.GO_COPY_ROOMINFO_REQUEST, req);
            }

        }

        #region Tab listen
        private void OnClickPVPTab(GameObject go)
        {
            if (RoomListView)
            {
                SendRequest<PBMessage.go_copy_list_request>((int)AccountMessage.GO_COPY_LIST_REQUEST, new PBMessage.go_copy_list_request() { battletype = 101 });
                RoomListView.SetActive(true);
            }
            if (CreateRoomView)
            {
                CreateRoomView.SetActive(false);
            }
            if (RoomDetailView)
            {
                RoomDetailView.SetActive(false);
            }
        }
        private void OnClickTeamTab(GameObject go)
        {
            //DisplayToast("暂未开放");
        }
        private void OnClickCreateTab(GameObject go)
        {
            if (ProgressBar)
            {
                ProgressBar.SetActive(false);
            }
            if (CreateRoomView)
            {
                CreateRoomView.SetActive(true);
            }
            if (RoomListView)
            {
                RoomListView.SetActive(false);
            }
        }
        private void OnClickExitTab(GameObject go)
        {
            Invoke("InActiveFragment", 0.2f);
        }
        #endregion

        private void OnRoomCreateViewClickConfirm(GameObject go)
        {
            if (NewRoomName == null)
            {
                //DisplayToast("系统错误");
            }
            else if (string.IsNullOrEmpty(NewRoomName.value))
            {
                //DisplayToast("请输入房间名称");
            }
            else
            {
                PBMessage.go_copy_create_request req = new PBMessage.go_copy_create_request() { copyid = 10001, sign = NewRoomName.value };
                SendRequest<PBMessage.go_copy_create_request>((int)AccountMessage.GO_COPY_CREATE_REQUEST, req);
            }
        }
        private void OnRoomCreateViewClickBack(GameObject go)
        {
            if (CreateRoomView)
            {
                CreateRoomView.SetActive(false);
            }
            if (RoomListView)
            {
                SendRequest<PBMessage.go_copy_list_request>((int)AccountMessage.GO_COPY_LIST_REQUEST, new PBMessage.go_copy_list_request() { battletype = 101 });
                RoomListView.SetActive(true);
            }
        }
        private void OnRoomDetailViewClickConfirm(GameObject go)
        {
            PBMessage.go_copy_login_request req = new PBMessage.go_copy_login_request() { ID = m_CurrentSelectedRoomInfo.fbid, copyid = m_CurrentSelectedRoomInfo.copyid };
            SendRequest<PBMessage.go_copy_login_request>((int)AccountMessage.GO_COPY_LOGIN_REQUEST, req);
        }
        private void OnRoomDetailViewClickBack(GameObject go)
        {
            if (RoomDetailView)
            {
                RoomDetailView.SetActive(false);
            }
            if (RoomListView)
            {
                SendRequest<PBMessage.go_copy_list_request>((int)AccountMessage.GO_COPY_LIST_REQUEST, new PBMessage.go_copy_list_request() { battletype = 101 });
                RoomListView.SetActive(true);
            }
        }
    }
}
