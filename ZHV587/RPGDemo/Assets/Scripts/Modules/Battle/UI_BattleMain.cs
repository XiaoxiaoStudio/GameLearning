using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


namespace Air2000
{
    public class UI_BattleMain : Performer
    {
        public GameObject VSAnimation;
        protected override void OnEnable()
        {
            base.OnEnable();


            UIHelper.SetButtonEvent(transform, "RightCenter/Btn_Exit", OnClickExitGame);
        }

        private void OnClickExitGame(GameObject go)
        {
            if (Constants.CURRENT_ROOM_INFO != null)
            {
                NetManager.SendNetPacket<PBMessage.go_copy_leave_request>((int)AccountMessage.GO_COPY_LEAVE_REQUEST, new PBMessage.go_copy_leave_request() { copyid = Constants.COPY_ID, fbid = Constants.CURRENT_ROOM_INFO.fbid });
            }
            SceneManager.Goto((int)SceneName.City);
        }
        //public override void Active()
        //{
        //    base.Active();
        //    StrangeLoading.PostDisable += OnLoadingClose;
        //    ActiveFragment<UI_BattleJoyStick>();
        //    ActiveFragment<UI_BattleCharacterDebug>();
        //    ActiveFragment<Frag_Battle_Operation>();
        //    ActiveFragment<UI_BattleHeroPanel>();
        //    ActiveFragment<UI_BattleEnemyPanel>();
        //}
        private void OnLoadingClose(StrangeLoading loading)
        {
            ActiveVS();
        }
        private void ActiveVS()
        {
            if (VSAnimation)
            {
                VSAnimation.SetActive(true);
                Invoke("InactiveVS", 3.0f);
                BattlePerformer.Instance.PlayBattleBeginCutscene();
            }
        }
        private void InactiveVS()
        {
            if (VSAnimation)
            {
                VSAnimation.SetActive(false);
            }
        }
    }
}
