using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Air2000
{
    public class Player
    {
        public delegate void PostCreateCharacterDelegate(Player player, Character character);
        public event PostCreateCharacterDelegate PostCreateCharacter;

        protected Character m_Character;
        protected List<Character> m_Pets;
        protected GameObject mCharacterHold;
        protected EventManager m_EventProcessor;
        protected PlayerType m_PlayerType;
        protected PlayerState m_PlayerState;
        protected CampType m_CampType;

        private Player m_Enemy;
        public Player Enemy
        {
            get
            {
                return m_Enemy;
            }
            set
            {
                if (value == null)
                {
                    return;
                }
                if (value.Character == null)
                {
                    m_Enemy = null;
                    Character.Enemy = null;
                }
                else
                {
                    m_Enemy = value;
                    Character.Enemy = m_Enemy.Character;
                }
            }
        }
        private bool m_IsDie;
        public bool IsDie
        {
            get
            {
                return m_IsDie;
            }
            set
            {
                m_IsDie = value;
            }
        }
        public bool EnableAI
        {
            get
            {
                return Character.AIMachine.enabled;
            }
            set
            {
                Character.AIMachine.enabled = value;
            }
        }
        public EventManager EventProcessor
        {
            get { return m_EventProcessor; }
            set { m_EventProcessor = value; }
        }
        public Character Character
        {
            get { return m_Character; }
            set { m_Character = value; }
        }
        public List<Character> Pets
        {
            get { return m_Pets; }
            set { m_Pets = value; }
        }
        public PlayerType PlayerType
        {
            get { return m_PlayerType; }
        }
        public CampType CampType
        {
            get
            {
                if (Data == null)
                {
                    return CampType.Middle;
                }
                else
                {
                    return (CampType)(Data.camp);
                }
            }
            set
            {
                m_CampType = value;
            }
        }
        public Player(PlayerType type)
        {
            m_PlayerType = type;
            m_EventProcessor = new EventManager();
        }

        private PBMessage.go_login_playerInfo m_Data;
        public PBMessage.go_login_playerInfo Data
        {
            get { return m_Data; }
            set { m_Data = value; }
        }
        public bool Initialize(PBMessage.go_login_playerInfo data)
        {
            Data = data;
            return true;
        }
        public bool UpdateData(PBMessage.go_login_playerInfo data)
        {
            return true;
        }
        public void CreateCharacter()
        {
            CharacterProvider.FashionInfo fashion = new CharacterProvider.FashionInfo();
            fashion.Directory = ((Profession)Data.profesion).ToString();
            fashion.Animation = ((Profession)Data.profesion).ToString();

            SuiteInfo info = UI_Bag.TryGetSuiteInfo(Data.equipid);
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

            fashion.Config = ((Profession)Data.profesion).ToString();
            CharacterProvider.Execute(fashion, OnCreatedCharacter);
        }
        private void OnCreatedCharacter(CharacterProvider.Request request)
        {
            if (Character != null)
            {
                GameObject.DestroyImmediate(Character.gameObject);
            }
            if (request.Character == null)
            {
                CharacterSystemUtils.LogError("Player.cs: CreateCharacter fail,fashion key is " + request.Task.Fashion.Key);
            }
            Character = request.Character;
            Character.Player = this;
            Character.Initialize(this, request.Task.LoadConfigRequest.Asset as TextAsset);
            if (PlayerType == PlayerType.PT_Hero || PlayerType == PlayerType.PT_Player)
            {
                // Disable AIMachine
                EnableAI = false;
            }
            Character.SetParent(Constants.PlayersNode);
            if (PostCreateCharacter != null)
            {
                PostCreateCharacter(this, Character);
            }
        }
        public void Attack(Player player)
        {
            if (player != null)
            {
                FaceToEnemy(player);
                if (PlayerType == PlayerType.PT_Hero)
                {
                    PBMessage.go_copy_attack_request pak = new PBMessage.go_copy_attack_request();
                    pak.attackid = Data.roleid;
                    pak.attackedid = player.Data.roleid;
                    NetManager.SendNetPacket<PBMessage.go_copy_attack_request>((int)AccountMessage.GO_COPY_ATTACK_REQUEST, pak);
                }
                player.Beattack(this);
            }
        }
        public void Beattack(Player player)
        {
            if (player != null && player.Character)
            {
                Enemy = player;
            }
            if (Character)
            {
                FaceToEnemy(Enemy);
                Character.ExecuteCommand(CharacterCommand.CC_BeAttack_1, true);
            }
        }
        public void FaceToEnemy(Player enemy)
        {
            if (enemy != null && enemy.Character)
            {
                Enemy = enemy;
                Vector3 direction = enemy.Character.WorldPosition - Character.WorldPosition;
                direction.Normalize();
                Character.WorldRotation = Quaternion.LookRotation(direction).eulerAngles;
            }
        }
    }
}
