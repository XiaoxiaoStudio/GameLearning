using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Air2000
{
    public class UI_BattleHeroPanel : Performer
    {
        public UILabel NickName;
        public UILabel Level;
        public UILabel Fight;
        public UILabel HP;
        public UISprite HPSprite;
        public UISprite Head;
        public UISprite HeadFrame;

        public UILabel PlayerHeadText;
        public GameObject PlayerHeadObj;

        public UILabel HurtText;
        public GameObject HurtObj;

        private int m_LastBlood;

        //public override void Visiable(Activity<BattleModule, BattleActivityPerformer> context)
        //{
        //    base.Visiable(context);
        //    if (NickName)
        //    {
        //        NickName.text = PlayerProvider.HeroInfo.name;
        //    }
        //    if (PlayerHeadText)
        //    {
        //        PlayerHeadText.text = PlayerProvider.HeroInfo.name;
        //    }
        //    if (Level)
        //    {
        //        Level.text = PlayerProvider.HeroInfo.level.ToString();
        //    }
        //    if (Fight)
        //    {
        //        Fight.text = PlayerProvider.HeroInfo.fightval.ToString();
        //    }
        //    if (Head)
        //    {
        //        Head.spriteName = ((Profession)PlayerProvider.HeroInfo.profesion).ToString().ToLower();
        //    }
        //    if (HP)
        //    {
        //        HP.text = PlayerProvider.HeroInfo.blood + "/" + PlayerProvider.HeroInfo.maxblood;
        //    }
        //    if (HPSprite)
        //    {
        //        HPSprite.fillAmount = 1;
        //    }
        //    m_LastBlood = PlayerProvider.HeroInfo.maxblood;


        //    PlayerProvider.Hero.PostCreateCharacter += OnCharacterCreated;



        //    NetManager.BindNetworkEvent((int)AccountMessage.GO_COPY_ATTACK_RETURN, OnRecvAttackInfo);
        //}
        private void OnCharacterCreated(Player player,Character character)
        {
            player.PostCreateCharacter -= OnCharacterCreated;
            // Set player position
            if (PlayerProvider.Hero.Data.camp == (int)CampType.Blue)
            {
                if (BattlePerformer.Instance.BlueStandPoint != null)
                {
                    PlayerProvider.Hero.Character.transform.position = BattlePerformer.Instance.BlueStandPoint.position;
                    PlayerProvider.Hero.Character.transform.rotation = BattlePerformer.Instance.BlueStandPoint.rotation;
                }
            }
            else
            {
                if (BattlePerformer.Instance.RedStandPoint != null)
                {
                    PlayerProvider.Hero.Character.transform.position = BattlePerformer.Instance.RedStandPoint.position;
                    PlayerProvider.Hero.Character.transform.rotation = BattlePerformer.Instance.RedStandPoint.rotation;
                }
            }
        }
        //public override void Invisiable(Activity<BattleModule, BattleActivityPerformer> context)
        //{
        //    base.Invisiable(context);
        //    NetManager.UnbindNetworkEvent((int)AccountMessage.GO_COPY_ATTACK_RETURN, OnRecvAttackInfo);
        //}
        private void OnRecvAttackInfo(Evt eventObj)
        {
            PBMessage.go_copy_attack_return pak = NetManager.DeserializeNetPacket<PBMessage.go_copy_attack_return>(eventObj);
            if (pak != null)
            {
                if (pak.attacked.roleid == PlayerProvider.Hero.Data.roleid)
                {
                    if (HP)
                    {
                        HP.text = pak.attacked.blood + "/" + PlayerProvider.Hero.Data.maxblood;
                    }
                    if (HPSprite)
                    {
                        HPSprite.fillAmount = (float)pak.attacked.blood / (float)PlayerProvider.Hero.Data.maxblood;
                    }
                    DisplayHurt(m_LastBlood - pak.attacked.blood);
                    m_LastBlood = pak.attacked.blood;
                }
            }
        }

        private void DisplayHurt(int cutdownBlood)
        {
            if (HurtObj)
            {
                GameObject newHurtObj = GameObject.Instantiate(HurtObj) as GameObject;
                Transform textTrans = newHurtObj.transform.Find("value");
                if (textTrans == null)
                {
                    GameObject.Destroy(newHurtObj);
                    return;
                }
                UILabel valueLabel = textTrans.GetComponent<UILabel>();
                if (valueLabel == null)
                {
                    GameObject.Destroy(newHurtObj);
                    return;
                }
                valueLabel.text = "伤害: " + cutdownBlood;
                Vector3 worldPos = PlayerProvider.Hero.Character.WorldPosition;
                worldPos.y += 2;
                if (Helper.IsInViewPort(SceneCameraController.Instance.MainCamera, worldPos))
                {
                    Vector3 screenPos = SceneCameraController.Instance.MainCamera.WorldToScreenPoint(worldPos);
                    screenPos.z = 0;
                    Vector3 worldPos2 = Constants.UICamera.cachedCamera.ScreenToWorldPoint(screenPos);
                    newHurtObj.transform.SetParent(Constants.UIRoot);
                    newHurtObj.transform.localScale = Vector3.one;
                    newHurtObj.transform.position = worldPos2;

                    TweenPosition positionTweener = newHurtObj.AddComponent<TweenPosition>();
                    Vector3 from = Constants.UIRoot.InverseTransformPoint(worldPos2);
                    positionTweener.from = from;
                    from.y += 50f;
                    positionTweener.to = from;
                    positionTweener.duration = 1f;

                    //TweenAlpha alphaTweener = newHurtObj.AddComponent<TweenAlpha>();
                    //alphaTweener.from = 1.0f;
                    //alphaTweener.to = 0.5f;
                    //alphaTweener.duration = 1f;

                    newHurtObj.AddComponent<AutoDestroy>().Time = 1f;
                    if (newHurtObj.activeSelf == false)
                    {
                        newHurtObj.SetActive(true);
                    }
                }
                else
                {
                    if (newHurtObj.activeSelf == true)
                    {
                        newHurtObj.SetActive(false);
                    }
                }
            }
        }

        protected override void Update()
        {
            base.Update();
            if (PlayerHeadObj)
            {
                if (PlayerProvider.Hero != null && PlayerProvider.Hero.Character && SceneCameraController.Instance.MainCamera != null)
                {
                    Vector3 worldPos = PlayerProvider.Hero.Character.WorldPosition;
                    worldPos.y += 2;
                    if (Helper.IsInViewPort(SceneCameraController.Instance.MainCamera, worldPos))
                    {
                        Vector3 screenPos = SceneCameraController.Instance.MainCamera.WorldToScreenPoint(worldPos);
                        screenPos.z = 0;
                        Vector3 worldPos2 = Constants.UICamera.cachedCamera.ScreenToWorldPoint(screenPos);
                        PlayerHeadObj.transform.position = worldPos2;
                        if (PlayerHeadObj.activeSelf == false)
                        {
                            PlayerHeadObj.SetActive(true);
                        }
                    }
                    else
                    {
                        if (PlayerHeadObj.activeSelf == true)
                        {
                            PlayerHeadObj.SetActive(false);
                        }
                    }
                }
            }
        }
    }
}
