using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Air2000
{
    public enum PlayerType
    {
        PT_Hero = 0,
        PT_Player = 1,
        PT_Robot = 3,
        PT_Monster = 4,
    }

    public enum PlayerState
    {
        PS_Active,
        PS_InActive,
        PS_InVisible,
        PS_Die,
    }

    public class PlayerProvider
    {
        private static Player m_Hero;
        public static Player Hero
        {
            get
            {
                return m_Hero;
            }
        }
        public static PBMessage.go_login_playerInfo HeroInfo
        {
            get
            {
                if (m_Hero != null)
                {
                    return m_Hero.Data;
                }
                return null;
            }
        }

        private Dictionary<int, Player> m_OnlinePlayers = new Dictionary<int, Player>();
        private Dictionary<int, Player> m_BattlePlayers = new Dictionary<int, Player>();

        private static PlayerProvider m_Instance;
        public static PlayerProvider Instance
        {
            get
            {
                if (m_Instance == null)
                {
                    m_Instance = new PlayerProvider();

                }
                return m_Instance;
            }
        }
        public PlayerProvider() : base()
        {
            NetManager.BindNetworkEvent((int)AccountMessage.GO_COPY_NEW_MAN_RETURN, OnPlayerEnter);
            NetManager.BindNetworkEvent((int)AccountMessage.GO_COPY_MAN_LEAVE_RETURN, OnPlayerLeave);
            NetManager.BindNetworkEvent((int)AccountMessage.GO_POSITION_COMMANF_RETURN, OnSyncPlayerTransform);
        }
        public void Logout()
        {
            if (m_Hero != null)
            {
                RemovePlayer(HeroInfo.roleid);
            }
            m_Hero = null;
        }
        private void OnPlayerEnter(Evt evt)
        {
            //PBMessage.go_copy_allrole_return infos = ServiceProvider.DeserializeNetPacket<PBMessage.go_copy_allrole_return>(evt);
            //if (infos.roledata != null && infos.roledata.Count > 0)
            //{
            //    for (int i = 0; i < infos.roledata.Count; i++)
            //    {
            //        CreatePlayer(infos.roledata[i]);
            //    }
            //}
            //if (ContextProvider.CurrentScene != BattleScene.Instace)
            //{
            //    ContextProvider.StartScene<BattleScene>();
            //}
        }
        private void OnPlayerLeave(Evt evt)
        {
            PBMessage.go_copy_roleleave_return info = NetManager.DeserializeNetPacket<PBMessage.go_copy_roleleave_return>(evt);
            if (info != null)
            {
                RemovePlayer(info.roleid);
            }
        }
        private void OnSyncPlayerTransform(Evt evt)
        {
            PBMessage.go_charactercommand_return info = NetManager.DeserializeNetPacket<PBMessage.go_charactercommand_return>(evt);
            if (info != null)
            {
                Player player = GetOnlinePlayer(info.roleid);
                if (player != null && player.Character != null)
                {
                    player.Character.SyncTransform(info);
                }
                //MultiPlayerService.Hero.Character.SyncTransform(info);
            }
        }

        public Player CreateMonster(PBMessage.go_login_playerInfo playerInfo, CharacterProvider.FashionInfo fashion)
        {
            Player player = new Player(PlayerType.PT_Monster);
            player.Initialize(playerInfo);
            return player;
        }
        public Player InitHeroBattleData(PBMessage.go_login_playerInfo data)
        {
            Hero.Data = data;
            Hero.CreateCharacter();
            return Hero;
        }
        public Player CreateHero()
        {
            return CreateHero(HeroInfo);
        }
        public Player CreateHero(PBMessage.go_login_playerInfo playerInfo)
        {
            Player tempPlayer = null;
            if (m_OnlinePlayers.TryGetValue(playerInfo.roleid, out tempPlayer))
            {
                m_Hero = tempPlayer;
                return tempPlayer;
            }
            if (m_Hero == null)
            {
                m_Hero = new Player(PlayerType.PT_Hero);
            }
            m_Hero.Initialize(playerInfo);

            CharacterProvider.FashionInfo fashion = new CharacterProvider.FashionInfo();
            fashion.Directory = ((Profession)HeroInfo.profesion).ToString();
            fashion.Animation = ((Profession)HeroInfo.profesion).ToString();


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


            fashion.Config = ((Profession)HeroInfo.profesion).ToString();


            m_OnlinePlayers.Add(playerInfo.roleid, m_Hero);
            return m_Hero;
        }
        public Player CreatePlayer(PBMessage.go_login_playerInfo data)
        {
            Player player = null;
            if (m_OnlinePlayers.TryGetValue(data.roleid, out player))
            {
                return player;
            }
            player = new Player(PlayerType.PT_Player);

            player.Initialize(data);
            player.CreateCharacter();

            m_OnlinePlayers.Add(data.roleid, player);
            return player;
        }
        public Player CreateDebugPlyer(Profession profession)
        {
            Player player = new Player(PlayerType.PT_Player);
            player.Data = new PBMessage.go_login_playerInfo();
            player.Data.name = "test01";
            CharacterProvider.FashionInfo fashion = new CharacterProvider.FashionInfo();
            fashion.Directory = profession.ToString();
            fashion.Animation = profession.ToString();
            fashion.Head = "Head01";
            fashion.Body = "Body01";
            fashion.Weapon = "Weapon01";
            fashion.Config = profession.ToString();
            player.Initialize(player.Data);
            return player;
        }
        public void RemovePlayer(int playerID)
        {

            Player player = null;
            if (m_OnlinePlayers.TryGetValue(playerID, out player))
            {
                if (player.Character)
                {
                    UnityEngine.GameObject.Destroy(player.Character.gameObject);
                }
                if (playerID != HeroInfo.roleid)//Do not remove hero.
                {
                    m_OnlinePlayers.Remove(playerID);
                }
            }
        }
        public Player GetOnlinePlayer(int playerID)
        {
            Player player = null;
            if (m_OnlinePlayers != null)
            {
                m_OnlinePlayers.TryGetValue(playerID, out player);
            }
            return player;
        }
        public void GetOnlinePlayers(out List<Player> players)
        {
            players = new List<Player>();
            if (m_OnlinePlayers != null && m_OnlinePlayers.Count > 0)
            {
                Dictionary<int, Player>.Enumerator it = m_OnlinePlayers.GetEnumerator();
                for (int i = 0; i < m_OnlinePlayers.Count; i++)
                {
                    it.MoveNext();
                    players.Add(it.Current.Value);
                }
            }
        }
    }
}
