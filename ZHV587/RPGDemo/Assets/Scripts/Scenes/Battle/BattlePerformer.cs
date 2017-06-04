using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Air2000
{
    public class BattlePerformer : Performer
    {
        public static BattlePerformer Instance { get; set; }
        public Transform RedStandPoint;
        public Transform BlueStandPoint;
        public Animator BattleBeginCutscene;
        protected override void Awake()
        {
            base.Awake();
            Instance = this;
        }
        protected override void OnEnable()
        {
            base.OnEnable();
        }
        public void PlayBattleBeginCutscene()
        {
            if (BattleBeginCutscene)
            {
                BattleBeginCutscene.enabled = true;
                if (ThirdPersonCameraController.Instance)
                {
                    ThirdPersonCameraController.Instance.enabled = false;
                    Invoke("EnableThirdPersonCamera", 1f);
                }
                BattleBeginCutscene.Play("BattleBeginCutscene");
            }
        }
        private void EnableThirdPersonCamera()
        {
            if (BattleBeginCutscene)
            {
                BattleBeginCutscene.enabled = false;
            }
            if (ThirdPersonCameraController.Instance)
            {
                ThirdPersonCameraController.Instance.enabled = true;
            }
        }
    }
}
