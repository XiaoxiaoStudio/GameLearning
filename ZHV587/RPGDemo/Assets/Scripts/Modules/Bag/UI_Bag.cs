using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Air2000
{
    public class SuiteInfo
    {
        public int ID;
        public string ICON;
        public int INDEX;
    }
    public class UI_Bag : Performer
    {
        public enum EquipType
        {
            Current = 9,
            Cloth = 12,
            Head = 11,
            Weapon = 10,
            Wing = 13,
        }

        public GameObject ProgressBar;
        public EquipType CurrentEquipType = EquipType.Cloth;
        public List<UIToggle> RadioGroups = new List<UIToggle>();
        public UIGrid Table;
        public GameObject ItemObj;

        private Dictionary<GameObject, SuiteInfo> m_CurrentSuiteInfos = new Dictionary<GameObject, SuiteInfo>();
        protected override void Awake()
        {
            base.Awake();
            UIHelper.SetButtonEvent(transform, "Containar/Tab/Cloth", OnClickCloth);
            UIHelper.SetButtonEvent(transform, "Containar/Tab/Head", OnClickHead);
            UIHelper.SetButtonEvent(transform, "Containar/Tab/Weapon", OnClickWeapon);
            UIHelper.SetButtonEvent(transform, "Containar/Tab/Wing", OnClickWing);


            UIHelper.SetButtonEvent(transform, "Containar/Btn_Groups/Btn_Close", OnClickClose);
            UIHelper.SetButtonEvent(transform, "Containar/Btn_Groups/Btn_Refresh", OnClickRefresh);
        }
        private void DisplayProgress(bool state)
        {
            if (ProgressBar)
            {
                ProgressBar.SetActive(state);
            }
        }
        protected void OnGUI()
        {
            return;
            if (GUI.Button(new Rect(10, 10, 150, 30), "Put on"))
            {
                PBMessage.go_package_wear_request pak = new PBMessage.go_package_wear_request();
                pak.weartype = 0;
                pak.incid = 1;
                NetManager.SendNetPacket<PBMessage.go_package_wear_request>((int)AccountMessage.GO_PACKAGE_WEAR_REQUEST, pak);
            }
            if (GUI.Button(new Rect(10, 50, 150, 30), "Put off"))
            {
                PBMessage.go_package_wear_request pak = new PBMessage.go_package_wear_request();
                pak.weartype = 1;
                pak.incid = 1;
                NetManager.SendNetPacket<PBMessage.go_package_wear_request>((int)AccountMessage.GO_PACKAGE_WEAR_REQUEST, pak);
            }
            if (GUI.Button(new Rect(10, 90, 150, 30), "delete"))
            {
                PBMessage.go_package_wear_request pak = new PBMessage.go_package_wear_request();
                pak.weartype = 2;
                pak.incid = 1;
                NetManager.SendNetPacket<PBMessage.go_package_wear_request>((int)AccountMessage.GO_PACKAGE_WEAR_REQUEST, pak);
            }
        }
        private void OnTestWear(Evt eventObj)
        {
            PBMessage.go_package_wear_return pak = NetManager.DeserializeNetPacket<PBMessage.go_package_wear_return>(eventObj);
            try
            {
                //DisplayToast("On wear return: wear type is: " + pak.weartype + ", id is: " + pak.incid, 3.0f);
            }
            catch { }
        }
        public void Active()
        {
            if (RadioGroups != null && RadioGroups.Count > 0)
            {
                for (int i = 0; i < RadioGroups.Count; i++)
                {
                    if (i == 0)
                    {
                        RadioGroups[i].value = true;
                    }
                    else
                    {
                        RadioGroups[i].value = false;
                    }
                }
            }
            //激活窗口时请求一条时装的信息
            CurrentEquipType = EquipType.Cloth;
            NetManager.BindNetworkEvent((int)AccountMessage.GO_PACKAGE_GET_RETURN, OnRecvPackageInfo);
            NetManager.BindNetworkEvent((int)AccountMessage.GO_PACKAGE_WEAR_RETURN, OnTestWear);
            RequestEquipInfo(CurrentEquipType);
            DisplayTable();
        }
        public void Inactive()
        {
            NetManager.UnbindNetworkEvent((int)AccountMessage.GO_PACKAGE_GET_RETURN, OnRecvPackageInfo);
            NetManager.UnbindNetworkEvent((int)AccountMessage.GO_PACKAGE_WEAR_RETURN, OnTestWear);
        }
        private void RequestEquipInfo(EquipType type)
        {
            DisplayProgress(true);
            NetManager.SendNetPacket<PBMessage.gopackage_self_request>((int)AccountMessage.GO_PACKAGE_GET_REQUEST, new PBMessage.gopackage_self_request() { bag_type = (int)type });
        }
        private void OnRecvPackageInfo(Evt eventObj)
        {
            DisplayProgress(false);
            PBMessage.gopackage_self_object package = NetManager.DeserializeNetPacket<PBMessage.gopackage_self_object>(eventObj);
            if (package == null)
            {
                //DisplayToast("数据解析错误");
            }
            else
            {
                //DisplayToast("Receive " + package.@object.Count + " objects,and object type is " + ((EquipType)package.bag_type).ToString(), 3.0f);
            }
        }
        private void OnClickCloth(GameObject go)
        {
            if (CurrentEquipType != EquipType.Cloth)
            {
                CurrentEquipType = EquipType.Cloth;
                RequestEquipInfo(CurrentEquipType);
            }
        }
        private void OnClickHead(GameObject go)
        {
            //DisplayToast("暂未开放");
            return;
            if (CurrentEquipType != EquipType.Head)
            {
                CurrentEquipType = EquipType.Head;
                RequestEquipInfo(CurrentEquipType);
            }
        }
        private void OnClickWeapon(GameObject go)
        {
            //DisplayToast("暂未开放");
            return;
            if (CurrentEquipType != EquipType.Weapon)
            {
                CurrentEquipType = EquipType.Weapon;
                RequestEquipInfo(CurrentEquipType);
            }
        }
        private void OnClickWing(GameObject go)
        {
            //DisplayToast("暂未开放");
            return;
            if (CurrentEquipType != EquipType.Wing)
            {
                CurrentEquipType = EquipType.Wing;
                RequestEquipInfo(CurrentEquipType);
            }
        }
        private void OnClickClose(GameObject go)
        {
            //ContextProvider.PauseActivity<BagModule>(Context);
        }
        private void OnClickRefresh(GameObject go)
        {
            //DisplayToast("OnClickRefresh");
        }

        public static List<SuiteInfo> GlobalSuiteInfos = new List<SuiteInfo>()
        {
            new SuiteInfo() {ID=11001,ICON="warrior_suite_1",INDEX=1},
            new SuiteInfo() {ID=11002,ICON="warrior_suite_2",INDEX=2 },
            new SuiteInfo() {ID=11003,ICON="warrior_suite_3",INDEX=3 },
            new SuiteInfo() {ID=11004,ICON="warrior_suite_4",INDEX=4 },
            new SuiteInfo() {ID=11005,ICON="warrior_suite_5",INDEX=5 },
            new SuiteInfo() {ID=11006,ICON="warrior_suite_6",INDEX=6 }
    };
        public static SuiteInfo TryGetSuiteInfo(int id)
        {
            for (int i = 0; i < GlobalSuiteInfos.Count; i++)
            {
                SuiteInfo info = GlobalSuiteInfos[i];
                if (info == null) continue;
                if (info.ID == id)
                {
                    return info;
                }
            }
            return null;
        }
        public static SuiteInfo TryGetSuiteInfo(List<int> ids)
        {
            for (int i = 0; i < ids.Count; i++)
            {
                int id = ids[i];
                SuiteInfo info = TryGetSuiteInfo(id);
                if (info != null)
                {
                    return info;
                }
            }
            return null;
        }
        private void DisplayTable()
        {
            if (m_CurrentSuiteInfos.Count > 0)
            {
                Dictionary<GameObject, SuiteInfo>.Enumerator it = m_CurrentSuiteInfos.GetEnumerator();
                for (int i = 0; i < m_CurrentSuiteInfos.Count; i++)
                {
                    it.MoveNext();
                    KeyValuePair<GameObject, SuiteInfo> kvp = it.Current;
                    if (kvp.Key)
                    {
                        GameObject.DestroyImmediate(kvp.Key);
                    }
                }
            }
            m_CurrentSuiteInfos.Clear();

            UITexture texture;

            if (Table != null && ItemObj != null)
            {
                for (int i = GlobalSuiteInfos.Count - 1; i >= 0; i--)
                {
                    SuiteInfo info = GlobalSuiteInfos[i];
                    GameObject item = GameObject.Instantiate(ItemObj) as GameObject;
                    if (item == null) continue;
                    UIHelper.SetButtonEvent(item.transform, OnClickSuite);
                    texture = item.transform.Find("Texture").GetComponent<UITexture>();

                    if (texture != null)
                    {
                        texture.mainTexture = AssetManager.LoadAsset("Icon/Equip/Suite/", info.ICON) as Texture2D;
                        item.transform.SetParent(Table.transform);
                        item.transform.localPosition = Vector3.zero;
                        item.transform.localScale = Vector3.one;
                        item.SetActive(true);
                        Table.enabled = true;
                        m_CurrentSuiteInfos.Add(item, info);
                    }
                }
            }
        }
        protected override void Update()
        {
            base.Update();
            if (CharacterProvider.TaskCount == 0 && m_ProgressDialog)
            {
                m_ProgressDialog.Close();
                m_ProgressDialog = null;
            }
        }
        private ProgressDialog m_ProgressDialog;
        public void OnClickSuite(GameObject go)
        {
            SuiteInfo suiteInfo = null;
            if (m_CurrentSuiteInfos.TryGetValue(go, out suiteInfo))
            {
                if (PlayerProvider.HeroInfo.equipid.Count > 0)
                {
                    PlayerProvider.HeroInfo.equipid.RemoveAt(0);
                }
                PlayerProvider.HeroInfo.equipid.Insert(0, suiteInfo.ID); CitySceneRoleDisplay.Instance.CreateMainRole();
                //m_ProgressDialog = base.DisplayProgress("正在换装", 30f, false);
            }
            //    PBMessage.go_package_wear_request pak = new PBMessage.go_package_wear_request();
            //pak.weartype = 0;
            //pak.incid = 1;
            //SendNetPacket<PBMessage.go_package_wear_request>((int)AccountMessage.GO_PACKAGE_WEAR_REQUEST, pak);
        }
    }
}
