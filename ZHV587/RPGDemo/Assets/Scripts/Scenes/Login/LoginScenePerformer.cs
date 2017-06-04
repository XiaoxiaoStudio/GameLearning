using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Air2000
{
    public class LoginScenePerformer : Performer
    {
        public Camera SceneCamera;

        public Transform CameraLookAtMenu;
        public Transform CameraLookAtMainRole;
        public Transform CameraLookAtWarrior;
        public Transform CameraLookAtArcher;

        public Transform MainRoleStand;
        public Transform WarriorStand;
        public Transform ArcherStand;

        private Transform m_TargetLookAt;
        public static LoginScenePerformer Instance;

        public Character Warrior;
        public Character Archer;

        public Character CurrentCharacter;

        protected override void Awake()
        {
            Instance = this;
            base.Awake();
        }
        protected override void Start()
        {
            base.Start();
            if (SceneCamera == null)
            {
                SceneCamera = Camera.main;
            }
            //if (Archer == null)
            //{
            //    Player player = new Player(PlayerType.PT_Hero);
            //    player.PlayerData = new PBMessage.go_login_playerInfo();
            //    player.PlayerData.name = "RoleCreate_Archer";
            //    CharacterProvider.FashionInfo fashion = new CharacterProvider.FashionInfo();
            //    fashion.Directory = "Archer";
            //    fashion.Animation = "Archer";
            //    fashion.Head = "Head01";
            //    fashion.Body = "Body01";
            //    fashion.Weapon = "Weapon01";
            //    fashion.Config = "Archer";

            //    CharacterProvider.Execute(player, fashion, OnLoadArcherCallback);
            //}
            //else
            //{
            //    Archer.transform.position = LoginScenePerformer.Instance.ArcherStand.position;
            //    Archer.transform.rotation = LoginScenePerformer.Instance.ArcherStand.rotation;
            //}

            //if (Warrior == null)
            //{
            //    Player player = new Player(PlayerType.PT_Hero);
            //    player.PlayerData = new PBMessage.go_login_playerInfo();
            //    player.PlayerData.name = "RoleCreate_Warrior";
            //    CharacterProvider.FashionInfo fashion = new CharacterProvider.FashionInfo();
            //    fashion.Directory = "Warrior";
            //    fashion.Animation = "Warrior";
            //    fashion.Head = "Head01";
            //    fashion.Body = "Body01";
            //    fashion.Weapon = "Weapon01";
            //    fashion.Config = "Warrior";

            //    CharacterProvider.Execute(player, fashion, OnLoadWarriorCallback);
            //}
            //else
            //{
            //    Warrior.transform.position = LoginScenePerformer.Instance.WarriorStand.position;
            //    Warrior.transform.rotation = LoginScenePerformer.Instance.WarriorStand.rotation;
            //}
        }
        private void OnLoadArcherCallback(CharacterProvider.Request req)
        {
            Archer = req.Character;
            if (Archer)
            {
                //req.Player.Character = req.Character;
                //req.Player.EnableAI = false;

                //Archer.transform.position = LoginScenePerformer.Instance.ArcherStand.position;
                //Archer.transform.rotation = LoginScenePerformer.Instance.ArcherStand.rotation;
            }

        }
        private void OnLoadWarriorCallback(CharacterProvider.Request req)
        {
            Warrior = req.Character;
            if (Warrior)
            {
                //req.Player.Character = req.Character;
                //req.Player.EnableAI = false;

                //Warrior.transform.position = LoginScenePerformer.Instance.WarriorStand.position;
                //Warrior.transform.rotation = LoginScenePerformer.Instance.WarriorStand.rotation;
            }
        }
        protected override void OnDestroy()
        {
            Instance = null;
            base.OnDestroy();
        }

        protected override void Update()
        {
            base.Update();
            if (m_TargetLookAt && SceneCamera)
            {
                SceneCamera.transform.position = Vector3.Lerp(SceneCamera.transform.position, m_TargetLookAt.position, 1.0f * Time.deltaTime * 2);
                SceneCamera.transform.rotation = Quaternion.Lerp(SceneCamera.transform.rotation, m_TargetLookAt.rotation, 1.0f * Time.deltaTime * 2);
            }
        }
        public void SetLookAt(Transform target)
        {
            m_TargetLookAt = target;
        }
    }
}
