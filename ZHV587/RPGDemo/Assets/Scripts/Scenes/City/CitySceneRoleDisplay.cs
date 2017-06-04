#define PRINT_LOG
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Air2000
{
    public class CitySceneRoleDisplay : MonoBehaviour
    {
        public Vector3 Scale;
        private bool IsPlaySpecIdle = false;
        private BoxCollider TouchBoxCollider;
        public static CitySceneRoleDisplay Instance;
        public Character Character;
        void Awake()
        {
            if (TouchBoxCollider == null)
            {
                TouchBoxCollider = GetComponent<BoxCollider>();
            }
            Instance = this;
        }
        void Start()
        {
            CreateMainRole();
        }
        void OnDisable()
        {
        }
        void OnDestroy()
        {
            Instance = null;
            if (Character != null)
            {
                TBTwistToRotate tbUtil = Character.GetComponent<TBTwistToRotate>();
                if (tbUtil != null)
                {
                    UnityEngine.Object.DestroyImmediate(tbUtil);
                }
                TwistRecognizer reco = Character.GetComponent<TwistRecognizer>();
                if (reco != null)
                {
                    UnityEngine.Object.DestroyImmediate(reco);
                }
                Character = null;
            }
        }
        public void CreateMainRole()
        {
            if (Character)
            {
                GameObject.DestroyImmediate(Character.gameObject);
                Character = null;
            }
            CharacterProvider.FashionInfo fashion = new CharacterProvider.FashionInfo();
            fashion.Directory = ((Profession)PlayerProvider.HeroInfo.profesion).ToString();
            fashion.Animation = ((Profession)PlayerProvider.HeroInfo.profesion).ToString();

            SuiteInfo info = UI_Bag.TryGetSuiteInfo(PlayerProvider.HeroInfo.equipid);
            if (info == null)
            {
                fashion.Head = "Head1";
                fashion.Body = "Body1";
                fashion.Weapon = "Weapon1";
            }
            else
            {
                fashion.Head = "Head" + info.INDEX;
                fashion.Body = "Body" + info.INDEX;
                fashion.Weapon = "Weapon" + info.INDEX;
            }
            fashion.Config = ((Profession)PlayerProvider.HeroInfo.profesion).ToString();
            CharacterProvider.Execute(fashion, OnCreateMainRole);
        }
        public void OnCreateMainRole(CharacterProvider.Request req)
        {
            if (req.Character != null)
            {
                Character = req.Character;
                Player player = new Player(PlayerType.PT_Robot);
                player.Data = new PBMessage.go_login_playerInfo();
                player.Data.name = "CitySceneRole";
                Helper.SetLayer(req.Character.gameObject, LAYER.Player.ToString());
                req.Character.Player = player;
                player.Character = req.Character;
                req.Character.SetChActive(true);
                Character = req.Character;
                Character.Initialize(player, req.Task.LoadConfigRequest.Asset as TextAsset);
                SetupRoleModel(Character);
            }
        }
        public void SetupRoleModel(Character character)
        {
            if (character == null)
            {
                PrintLog("SetupRoleModel error caused by null FXQCharacter instance"); return;
            }
            Character = character;
            GameObject.DontDestroyOnLoad(Character.gameObject);
            Character.gameObject.SetActive(true);
            Character.transform.SetParent(transform);
            Character.SetLocalPosition(new Vector3(0, 0.18f, 0));
            Character.SetLocalScale(Scale);
            Character.SetRotation(Quaternion.identity);
            Character.gameObject.AddComponent<TBTwistToRotate>();
            Character.gameObject.AddComponent<TwistRecognizer>();
            Character.ExecuteCommand(CharacterCommand.CC_Idle);
        }

        public void OnDrag(DragGesture gesture)
        {
            if (gesture == null) return;
            if (gesture.Selection != gameObject) return;
            Vector3 euler = transform.eulerAngles;
            euler = new Vector3(euler.x, euler.y - gesture.DeltaMove.x, euler.z);
            transform.eulerAngles = euler;
        }
        public void OnTap(TapGesture gesture)
        {
            //if (gesture == null) return;
            //if (gesture.Selection != gameObject) return;
            //if (IsPlaySpecIdle == true) { return; }
            //if (mCharacterInfo != null && mCharacterInfo.Characher != null && mCharacterInfo.Characher.pMotionMachine != null)
            //{
            //    NCSpeedLight.Motion motion = mCharacterInfo.Characher.pMotionMachine.GetStateByName(RoleMotionType.RMT_SpacialIdle.ToString()) as NCSpeedLight.Motion;
            //    if (motion == null) { return; }
            //    motion.AddStateListener(this);
            //    mCharacterInfo.Characher.pMotionMachine.SetNextState(RoleMotionType.RMT_SpacialIdle.ToString());
            //    IsPlaySpecIdle = true;
            //}
        }
        public static void PrintLog(string msg)
        {
#if PRINT_LOG
            Debug.Log("CityRoleDisplay: " + msg);
#endif
        }
    }
}
