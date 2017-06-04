using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Air2000
{
    public class RanklistActivityPerformer : Performer
    {
        public enum RankType
        {
            Exp,
            Win,
        }

        public GameObject ProgressBar;
        private float m_ProgressBeginTime;
        public UIToggle ExpTapBtn;
        public UIGrid RankGrid;
        public GameObject ItemObj;

        private Dictionary<GameObject, PBMessage.GM_RankInfo> m_CurrentRankInfos = new Dictionary<GameObject, PBMessage.GM_RankInfo>();
        public RankType CurrentRankType = RankType.Exp;

        protected override void Awake()
        {
            base.Awake();

            UIHelper.SetButtonEvent(transform, "Panel/Tab/Exp", OnClickExpRank);
            UIHelper.SetButtonEvent(transform, "Panel/Tab/Win", OnClickWinRank);
            UIHelper.SetButtonEvent(transform, "Panel/Btn_Close", OnClickClose);
        }
        private void ActiveProgress()
        {
            if (ProgressBar)
            {
                ProgressBar.SetActive(true);
                m_ProgressBeginTime = Time.realtimeSinceStartup;
            }
        }
        private void InactiveProgress()
        {
            float deltaTime = Time.realtimeSinceStartup - m_ProgressBeginTime;
            if (deltaTime < 0.5f)
            {
                Invoke("DisableProgress", 0.5f - deltaTime);
            }
            else
            {
                if (ProgressBar)
                {
                    ProgressBar.SetActive(false);
                }
            }
        }
        private void DisableProgress()
        {
            if (ProgressBar)
            {
                ProgressBar.SetActive(false);
            }
        }
        private void OnClickExpRank(GameObject go)
        {
            if (CurrentRankType != RankType.Exp)
            {
                CurrentRankType = RankType.Exp;
                NetManager.SendNetPacket<PBMessage.GM_RankPageRequest>((int)AccountMessage.GM_RANKLISTPAGE_REQUEST, new PBMessage.GM_RankPageRequest() { m_pagenum = 0, m_RankType = (int)CurrentRankType });
                ActiveProgress();
            }
        }
        private void OnClickWinRank(GameObject go)
        {
            if (CurrentRankType != RankType.Win)
            {
                CurrentRankType = RankType.Win;
                NetManager.SendNetPacket<PBMessage.GM_RankPageRequest>((int)AccountMessage.GM_RANKLISTPAGE_REQUEST, new PBMessage.GM_RankPageRequest() { m_pagenum = 0, m_RankType = (int)CurrentRankType });
                ActiveProgress();
            }
        }
        private void OnClickClose(GameObject go)
        {
            //ContextProvider.PauseActivity<RanklistModule>();
        }
        public void Active()
        {
            if (ExpTapBtn)
            {
                ExpTapBtn.value = true;
            }
            ActiveProgress();
            NetManager.SendNetPacket<PBMessage.GM_AllRankListRequest>((int)AccountMessage.GM_ALLRANKLIST_REQUEST, new PBMessage.GM_AllRankListRequest() { });
            NetManager.BindNetworkEvent((int)AccountMessage.GM_ALLRANKLIST_RETURN, OnRecvAllRankListFirstPage);
            NetManager.BindNetworkEvent((int)AccountMessage.GM_RANKLISTPAGE_RETURN, OnRecvRankListPage);
        }
        public void Inactive()
        {
            NetManager.UnbindNetworkEvent((int)AccountMessage.GM_ALLRANKLIST_RETURN, OnRecvAllRankListFirstPage);
            NetManager.UnbindNetworkEvent((int)AccountMessage.GM_RANKLISTPAGE_RETURN, OnRecvRankListPage);
        }
        private void OnRecvAllRankListFirstPage(Evt eventObj)
        {
            if (ProgressBar)
            {
                ProgressBar.SetActive(false);
            }
            PBMessage.GM_AllRankTypeListReturn allInfo = NetManager.DeserializeNetPacket<PBMessage.GM_AllRankTypeListReturn>(eventObj);
            DisplayRanklist(RankType.Exp, allInfo.ranklist[0].m_RankInfo);
        }
        private void OnRecvRankListPage(Evt eventObj)
        {
            InactiveProgress();
            PBMessage.GM_RankListReturn pageInfo = NetManager.DeserializeNetPacket<PBMessage.GM_RankListReturn>(eventObj);
            DisplayRanklist((RankType)pageInfo.m_RankType, pageInfo.m_RankInfo);
        }
        private void DisplayRanklist(RankType type, List<PBMessage.GM_RankInfo> infos)
        {
            if (type != CurrentRankType)
            {
                // 不是当前排行版的类型则不显示
                return;
            }
            // 清空当前的排行版
            if (m_CurrentRankInfos.Count > 0)
            {
                Dictionary<GameObject, PBMessage.GM_RankInfo>.Enumerator it = m_CurrentRankInfos.GetEnumerator();
                for (int i = 0; i < m_CurrentRankInfos.Count; i++)
                {
                    it.MoveNext();
                    KeyValuePair<GameObject, PBMessage.GM_RankInfo> kvp = it.Current;
                    if (kvp.Key)
                    {
                        GameObject.DestroyImmediate(kvp.Key);
                    }
                }
            }
            m_CurrentRankInfos.Clear();

            UISprite m_HeadSprite;
            UILabel m_RoleName;
            UILabel m_Value;
            UISprite m_ValueLE10;
            UISprite m_ValueMR10L;
            UISprite m_ValueMR10R;

            if (RankGrid != null && ItemObj != null
                && infos != null && infos.Count > 0)
            {
                for (int i = infos.Count - 1; i >= 0; i--)
                {
                    PBMessage.GM_RankInfo info = infos[i];
                    if (info == null)
                    {
                        infos.RemoveAt(i);
                        i--;
                        continue;
                    }
                    GameObject item = GameObject.Instantiate(ItemObj) as GameObject;
                    if (item == null) continue;
                    m_HeadSprite = item.transform.Find("Head").GetComponent<UISprite>();
                    m_RoleName = item.transform.Find("Name").GetComponent<UILabel>();

                    m_Value = item.transform.Find("Value").GetComponent<UILabel>();

                    m_ValueLE10 = item.transform.Find("RankNumber(LE10)").GetComponent<UISprite>();
                    m_ValueMR10L = item.transform.Find("RankNumber(MR10)/1").GetComponent<UISprite>();
                    m_ValueMR10R = item.transform.Find("RankNumber(MR10)/2").GetComponent<UISprite>();

                    if (m_HeadSprite != null
                  && m_RoleName != null
               && m_ValueLE10 != null && m_ValueMR10L != null && m_ValueMR10R != null)
                    {
                        item.name = info.m_roleName;
                        item.transform.SetParent(RankGrid.transform);
                        item.transform.localPosition = Vector3.zero;
                        item.transform.localScale = Vector3.one;
                        item.SetActive(true);
                        //m_HeadSprite.spriteName = (Profession)info.p
                        m_RoleName.text = info.m_roleName;

                        switch (type)
                        {
                            case RankType.Exp:
                                m_Value.text = "经验值: " + info.m_value;
                                break;
                            case RankType.Win:
                                m_Value.text = "本周胜局: " + info.m_value;
                                break;
                            default:
                                break;
                        }
                        if (info.m_place <= 3)
                        {
                            //根据 123 换颜色
                            if (info.m_place == 1)
                            {
                                m_ValueLE10.spriteName = "first";
                            }
                            else if (info.m_place == 2)
                            {
                                m_ValueLE10.spriteName = "second";
                            }
                            else if (info.m_place == 3)
                            {
                                m_ValueLE10.spriteName = "third";
                            }
                        }
                        else if (info.m_place <= 9)
                        {
                            m_ValueLE10.spriteName = info.m_place.ToString();
                        }
                        else if (info.m_place > 9 && info.m_place <= 99)
                        {
                            m_ValueMR10L.spriteName = info.m_place.ToString().Substring(0, 1);
                            m_ValueMR10R.spriteName = info.m_place.ToString().Substring(1, 1);
                        }
                        m_CurrentRankInfos.Add(item, info);
                    }

                }
                if (RankGrid.enabled == false)
                {
                    RankGrid.enabled = true;
                }
            }
        }

    }
}
