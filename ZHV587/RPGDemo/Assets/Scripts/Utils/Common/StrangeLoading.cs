using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Air2000
{
    public class StrangeLoading : MonoBehaviour
    {
        public delegate void PostDisableDelegate(StrangeLoading loading);
        public static event PostDisableDelegate PostDisable;
        public Character Character;
        void OnEnable()
        {
            if (Character == null)
            {
                gameObject.SetActive(false);
                return;
            }
            if (AssetManager.TaskCount == 0)
            {
                gameObject.SetActive(false);
                return;
            }
        }
        void OnDisable()
        {
            if (PostDisable != null)
            {
                PostDisable(this);
            }
        }
        void OnCreateCallback(CharacterProvider.Request req)
        {
            if (req.Character != null)
            {
                Player player = new Player(PlayerType.PT_Robot);
                player.Data = new PBMessage.go_login_playerInfo();
                player.Data.name = "LoadingCharacter";
                req.Character.transform.SetParent(transform);
                req.Character.SetLocalScale(new Vector3(150, 150, 150));
                req.Character.SetLocalRotation(Quaternion.Euler(new Vector3(0, 120, 0)));
                req.Character.SetLocalPosition(new Vector3(-27.32f, -99, 0));
                req.Character.Player = player;
                player.Character = req.Character;
                req.Character.SetChActive(true);
                Character = req.Character;
                Character.Initialize(player, req.Task.LoadConfigRequest.Asset as TextAsset);
                player.EnableAI = false;

                Character.NavAgent.enabled = false;

                Character.Moveable = false;
                Helper.SetLayer(req.Character.gameObject, LAYER.UI.ToString());
            }
        }
        void CreateCharacter()
        {
            Player player = new Player(PlayerType.PT_Hero);
            player.Data = new PBMessage.go_login_playerInfo();
            player.Data.name = "LoadingCharacter";
            CharacterProvider.FashionInfo fashion = new CharacterProvider.FashionInfo();
            fashion.Directory = "Archer";
            fashion.Animation = "Archer";
            fashion.Head = "Head01";
            fashion.Body = "Body01";
            fashion.Weapon = "Weapon01";
            fashion.Config = "Archer";

            CharacterProvider.Execute(fashion, OnCreateCallback);
        }
        void Update()
        {
            if (Character)
            {
                if (Character.Commander.CurrentCommand != null)
                {
                    if (Character.Commander.CurrentCommand.Type != CharacterCommand.CC_Run)
                    {
                        Character.ExecuteCommand(CharacterCommand.CC_Run, true);
                    }
                }
                else
                {
                    Character.ExecuteCommand(CharacterCommand.CC_Run, true);
                }
            }
            if (AssetManager.TaskCount == 0 && CharacterProvider.TaskCount == 0)
            {
                gameObject.SetActive(false);
            }
        }
        public void Initialize()
        {
            CreateCharacter();
        }
        public void Active()
        {
            if (Character)
            {
                gameObject.SetActive(true);
            }
            else
            {
                gameObject.SetActive(false);
            }
        }
    }
}
