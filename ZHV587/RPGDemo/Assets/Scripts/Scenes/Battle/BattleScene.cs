using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Air2000
{
    public enum CampType
    {
        Blue,
        Red,
        Middle,
    }
    public class BattleScene : Scene
    {
        public List<Player> BlueCamp;
        public List<Player> MiddleCamp;
        public List<Player> RedCamp;
        public Player CurrentEnemy;
        private static BattleScene m_Instance;
        public static BattleScene Instace
        {
            get { return m_Instance; }
        }
        public BattleScene() : base((int)SceneName.Battle)
        {
            m_Instance = this;
            BlueCamp = new List<Player>();
            RedCamp = new List<Player>();
            MiddleCamp = new List<Player>();
        }

        public override void Begin()
        {
            base.Begin();
            if (Constants.IsSinglePlayer)
            {
                PBMessage.go_login_playerInfo data = new PBMessage.go_login_playerInfo();
                data.name = "Hero_Warrior";
                PlayerProvider.Instance.CreateHero(data).PostCreateCharacter += OnHeroCreatedCharacter;
                PlayerProvider.Instance.CreateDebugPlyer(Profession.Warrior).PostCreateCharacter += OnMonsterCreated;
            }
            RegisterNetworkEvent();
            AssetManager.LoadSceneAsync("SingleBattle");
        }

        public void Pause()
        {
            //base.Pause();
            CurrentEnemy = null;
            UnregisterNetworkEvent();
            for (int i = 0; i < BlueCamp.Count; i++)
            {
                Player player = BlueCamp[i];
                if (player == null) continue;
                PlayerProvider.Instance.RemovePlayer(player.Data.roleid);
            }
            for (int i = 0; i < RedCamp.Count; i++)
            {
                Player player = RedCamp[i];
                if (player == null) continue;
                PlayerProvider.Instance.RemovePlayer(player.Data.roleid);
            }
            for (int i = 0; i < MiddleCamp.Count; i++)
            {
                Player player = MiddleCamp[i];
                if (player == null) continue;
                PlayerProvider.Instance.RemovePlayer(player.Data.roleid);
            }
            BlueCamp.Clear();
            RedCamp.Clear();
            MiddleCamp.Clear();
        }
        private void RegisterNetworkEvent()
        {
            NetManager.BindNetworkEvent((int)AccountMessage.GO_COPY_ROLEINFO_RETURN, OnRecvAllRoleInfo);
            NetManager.BindNetworkEvent((int)AccountMessage.GO_POSITION_COMMANF_RETURN, OnSyncPlayerTransform);
            NetManager.BindNetworkEvent((int)AccountMessage.GO_COPY_DEAD_RESULT, OnPlayerDie);
            NetManager.BindNetworkEvent((int)AccountMessage.GO_COPY_RESULT_RETURN, OnRecvSettlement);

        }
        private void UnregisterNetworkEvent()
        {
            NetManager.UnbindNetworkEvent((int)AccountMessage.GO_COPY_ROLEINFO_RETURN, OnRecvAllRoleInfo);
            NetManager.UnbindNetworkEvent((int)AccountMessage.GO_POSITION_COMMANF_RETURN, OnSyncPlayerTransform);
            NetManager.UnbindNetworkEvent((int)AccountMessage.GO_COPY_DEAD_RESULT, OnPlayerDie);
            NetManager.BindNetworkEvent((int)AccountMessage.GO_COPY_RESULT_RETURN, OnRecvSettlement);

        }
        private void OnRecvAllRoleInfo(Evt eventObj)
        {
            PBMessage.go_copy_all_roleinfo pak = NetManager.DeserializeNetPacket<PBMessage.go_copy_all_roleinfo>(eventObj);
            if (pak == null)
            {
                Helper.LogError(this.GetType() + ".cs: OnRecvAllRoleInfo: parse network packet error.");
            }
            else
            {
                for (int i = 0; i < pak.role.Count; i++)
                {
                    PBMessage.go_login_playerInfo playerInfo = pak.role[i];
                    if (playerInfo == null)
                    {
                        continue;
                    }
                    Player player = null;
                    if (playerInfo.roleid == PlayerProvider.HeroInfo.roleid)
                    {
                        player = PlayerProvider.Instance.InitHeroBattleData(playerInfo);
                    }
                    else
                    {
                        player = PlayerProvider.Instance.CreatePlayer(playerInfo);
                    }
                    if (player != null)
                    {
                        if (player.CampType == CampType.Blue)
                        {
                            BlueCamp.Add(player);
                        }
                        else if (player.CampType == CampType.Red)
                        {
                            RedCamp.Add(player);
                        }
                        else if (player.CampType == CampType.Middle)
                        {
                            MiddleCamp.Add(player);
                        }
                        Helper.Log(playerInfo.name + " enter battle,camp is " + playerInfo.camp);
                    }
                }
            }

            CurrentEnemy = GetEnemy(PlayerProvider.Hero);

            //ContextProvider.StartActivity<BattleModule>();
        }
        private void OnSyncPlayerTransform(Evt evt)
        {
            PBMessage.go_charactercommand_return info = NetManager.DeserializeNetPacket<PBMessage.go_charactercommand_return>(evt);
            if (info != null)
            {
                Player player = PlayerProvider.Instance.GetOnlinePlayer(info.roleid);
                if (player != null && player.Character != null)
                {
                    player.Character.SyncTransform(info);
                }
            }
        }
        private void OnPlayerDie(Evt evt)
        {
            PBMessage.go_copy_dead pak = NetManager.DeserializeNetPacket<PBMessage.go_copy_dead>(evt);
            if (pak != null)
            {
                Player player = PlayerProvider.Instance.GetOnlinePlayer(pak.deadid);
                if (player != null)
                {
                    //Performer.DisplayToast("玩家 " + player.Data.name + " 已经阵亡");
                    if (player.Character)
                    {
                        player.Character.ExecuteCommand(CharacterCommand.CC_Die_1);
                    }
                }
                else
                {
                    Helper.LogError(this.GetType() + ".cs: Received a player is dead,but can not find this player,id is: " + pak.deadid);
                }
            }
        }
        private void OnRecvSettlement(Evt evt)
        {
            PBMessage.go_copy_result_return pak = NetManager.DeserializeNetPacket<PBMessage.go_copy_result_return>(evt);
            if (evt != null)
            {
                if (PlayerProvider.HeroInfo.camp != pak.wincamp)
                {
                    //Performer.DisplayToast("你赢了", 2.0f);
                }
                else
                {
                    //Performer.DisplayToast("你输了", 2.0f);
                }
                if (Constants.CURRENT_ROOM_INFO != null)
                {
                    NetManager.SendNetPacket<PBMessage.go_copy_leave_request>((int)AccountMessage.GO_COPY_LEAVE_REQUEST, new PBMessage.go_copy_leave_request() { copyid = Constants.COPY_ID, fbid = Constants.CURRENT_ROOM_INFO.fbid });
                }
                SceneManager.Goto((int)SceneName.City);
            }
        }
        private void OnHeroCreatedCharacter(Player player, Character character)
        {
            player.PostCreateCharacter -= OnHeroCreatedCharacter;
            if (character)
            {
                player.CampType = CampType.Blue;
                player.EnableAI = false;
                AddPlayer(player);
            }
            else
            {
                Helper.LogError("BattleScene.cs: Enter battle fail");
            }
        }
        private void OnMonsterCreated(Player player, Character character)
        {
            player.CampType = CampType.Red;
            player.EnableAI = false;
            AddPlayer(player);
        }
        public void AddPlayer(Player player)
        {
            if (player == null)
                return;
            if (player.CampType == CampType.Blue)
            {
                BlueCamp.Add(player);
            }
            else if (player.CampType == CampType.Red)
            {
                RedCamp.Add(player);
            }
            else if (player.CampType == CampType.Middle)
            {
                MiddleCamp.Add(player);
            }
        }
        public Player GetEnemy(Player player)
        {
            if (player == null) return null;
            List<Player> tempPlayers = new List<Player>();
            if (player.CampType == CampType.Blue)
            {
                tempPlayers.AddRange(RedCamp);
            }
            else
            {
                tempPlayers.AddRange(BlueCamp);
            }
            if (tempPlayers.Count > 0)
            {
                return tempPlayers[0];
            }
            return null;
        }
        public List<Player> GetAllEnemy(Player player)
        {
            if (player == null) return null;
            List<Player> tempPlayers = new List<Player>();
            if (player.CampType == CampType.Blue)
            {
                tempPlayers.AddRange(RedCamp);
            }
            else
            {
                tempPlayers.AddRange(BlueCamp);
            }
            return tempPlayers;
        }
        public void GetNearestEnemy(Vector3 position, Player player, out Player outPlayer)
        {
            float nearest = Mathf.Infinity;
            Player nearestPlayer = null;
            List<Player> tempPlayers = new List<Player>();
            if (player.CampType == CampType.Blue)
            {
                tempPlayers = RedCamp;
            }
            else if (player.CampType == CampType.Red)
            {
                tempPlayers = BlueCamp;
            }
            if (tempPlayers == null || tempPlayers.Count == 0)
            {
                outPlayer = null;
                return;
            }
            for (int i = 0; i < tempPlayers.Count; ++i)
            {
                if (tempPlayers[i] == null || tempPlayers[i].IsDie || tempPlayers[i].Character == null || tempPlayers[i].Character.gameObject.activeSelf == false)
                {
                    continue;
                }
                float curentdis = (tempPlayers[i].Character.WorldPosition - position).magnitude;
                if (curentdis < nearest)
                {
                    nearest = curentdis;
                    nearestPlayer = tempPlayers[i];
                }
            }
            outPlayer = nearestPlayer;
            return;
        }
    }
}
