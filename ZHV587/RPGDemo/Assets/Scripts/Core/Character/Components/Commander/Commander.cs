using System;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using Mono.Xml;
using System.Security;
using System.Collections;

namespace Air2000
{
    public enum CharacterCommand
    {
        None = -1,
        CC_Idle = 1,
        CC_Appear = 2,
        CC_Disappear = 3,
        CC_Die_1 = 4,
        CC_Die_2 = 5,

        CC_Walk = 20,

        CC_Run = 25,

        CC_Attack_1 = 28,
        CC_Attack_2 = 29,
        CC_Attack_3 = 30,
        CC_Attack_4 = 31,
        CC_Attack_5 = 32,

        CC_KnockDown = 35,
        CC_KnockDownStand = 36,

        CC_BeAttack_1 = 37,
        CC_BeAttack_2 = 38,

        CC_Skill_1 = 40,
        CC_Skill_2 = 41,
        CC_Skill_3 = 42,
        CC_Skill_4 = 43,
        CC_Skill_5 = 44,

        CC_Fail = 100,

        CC_Victory = 110,
    }

    public class Commander : CharacterComponent
    {
        [Serializable]
        public class Command
        {
            public delegate void ActiveDelegate(Command currentCommand, Command lastCommand);
            public delegate void BeChangedDelegate(Command currentCommand, Command nextComand);
            public delegate void FinishDelegate(Command command, Motion finishMotion);
            public event ActiveDelegate PostActive;
            public event BeChangedDelegate PostBeChanged;
            public event FinishDelegate PostFinish;

            [HideInInspector]
            [SerializeField]
            public string DisplayName;
            public CharacterCommand Type;
            public CharacterCommand AutoNext;
            public List<RoleMotionType> SequentialMotions = new List<RoleMotionType>();
            public List<Changeable> ChangeableCommands = new List<Changeable>();
            [NonSerialized]
            public List<Motion> Motions = new List<Motion>();
            [NonSerialized]
            public Commander Commander;
            [NonSerialized]
            public bool ActiveStatus = false;
            private float m_PlayTime;
            private int m_CurrentMotionIndex;
            private CharacterCommand m_CachedCommand = CharacterCommand.None;
            private float m_PlayCachedCommandBeginTime;
            public void OnCharacterInitialized(Character character)
            {
                if (Commander.MotionMachine.Motions != null && Commander.MotionMachine.Motions.Count > 0 && SequentialMotions != null && SequentialMotions.Count > 0)
                {
                    for (int i = 0; i < SequentialMotions.Count; i++)
                    {
                        RoleMotionType motionType = SequentialMotions[i];
                        Motion motion = Commander.MotionMachine.GetMotion(motionType);
                        if (motion == null) continue;
                        if (Motions == null) Motions = new List<Motion>();
                        Motions.Add(motion);
                    }
                }
            }
            private void OnLastMotionForceStop(Motion motion)
            {
                if (motion != null)
                {
                    motion.PostForceStop -= OnLastMotionForceStop;
                }
                Motion nextMotion = null;
                if (Commander.MotionMachine == null || Commander == null) return;
                if (Motions == null || Motions.Count == 0) return;
                for (int i = 0; i < Motions.Count; i++)
                {
                    Motion tempMotion = Motions[i];
                    if (tempMotion == null) continue;
                    if (tempMotion == motion)
                    {
                        if (i < Motions.Count - 1)
                        {
                            nextMotion = Motions[i + 1];
                            break;
                        }
                    }
                }
                if (nextMotion == null)
                {
                    StopCommand(motion);
                }
                else
                {
                    nextMotion.PostForceStop += OnLastMotionForceStop;
                    Commander.MotionMachine.ExecuteMotion(nextMotion.Type);
                }
            }
            public void OnAwake(MotionMachine machine, Commander commander)
            {
                Commander = commander;
            }
            public void OnDestroy(MotionMachine machine, Commander commander)
            {
                if (Motions != null)
                {
                    Motions.Clear();
                }
                Commander = null;
            }
            public void Begin(Commander commander)
            {
                if (commander == null || commander.MotionMachine == null) return;
                if (Motions == null || Motions.Count == 0) return;
                Motion motion = Motions[0];
                if (motion == null) return;
                m_CachedCommand = CharacterCommand.None;
                m_CurrentMotionIndex = 0;
                m_PlayTime = 0.0f;
                ActiveStatus = true;
                motion.PostForceStop += OnLastMotionForceStop;
                if (PostActive != null)
                {
                    PostActive(this, commander.LastCommand);
                }
                commander.MotionMachine.ExecuteMotion(motion.Type);
            }
            public void Update(Commander commander)
            {
                m_PlayTime += Time.deltaTime;
                if (m_CachedCommand != CharacterCommand.None && m_PlayTime >= m_PlayCachedCommandBeginTime)
                {
                    if (Commander != null)
                    {
                        Commander.ExecuteCommand(m_CachedCommand, true);
                    }
                }
            }
            public void Finish(Commander commander, Command nextCommand)
            {
                m_CachedCommand = CharacterCommand.None;
                m_CurrentMotionIndex = 0;
                m_PlayTime = 0.0f;
                ActiveStatus = false;
                if (PostBeChanged != null)
                {
                    PostBeChanged(this, nextCommand);
                }
                if (Motions != null && Motions.Count > 0)
                {
                    for (int i = 0; i < Motions.Count; i++)
                    {
                        Motion tempMotion = Motions[i];
                        if (tempMotion == null) continue;
                        tempMotion.PostForceStop -= OnLastMotionForceStop;
                    }
                }
            }
            private void StopCommand(Motion lastMotion)
            {
                m_CachedCommand = CharacterCommand.None;
                m_CurrentMotionIndex = 0;
                m_PlayTime = 0.0f;
                ActiveStatus = false;
                if (Commander) Commander.ExecuteCommand(AutoNext);
                if (PostFinish != null)
                {
                    PostFinish(this, lastMotion);
                }
            }
            public bool DoCache(CharacterCommand type)
            {
                Changeable obj = TryGetChangeable(type);
                if (obj == null)
                {
                    return false;
                }
                else
                {
                    if (obj.EnableCache == false)
                    {
                        return false;
                    }
                    else
                    {
                        if (obj.CacheL <= m_PlayTime && m_PlayTime < obj.CacheR)
                        {
                            m_CachedCommand = type;
                            m_PlayCachedCommandBeginTime = obj.CacheR;
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                }
            }
            public Motion TryGetMotion(RoleMotionType type)
            {
                if (Motions == null || Motions.Count == 0) return null;
                for (int i = 0; i < Motions.Count; i++)
                {
                    Motion tempMotion = Motions[i];
                    if (tempMotion == null) continue;
                    if (tempMotion.Type == type)
                    {
                        return tempMotion;
                    }
                }
                return null;
            }
            public void AddSequentialMotion(RoleMotionType type)
            {
                if (SequentialMotions.Contains(type) == false)
                {
                    SequentialMotions.Add(type);
                }
            }
            public Changeable TryGetChangeable(CharacterCommand command)
            {
                if (ChangeableCommands == null || ChangeableCommands.Count == 0) return null;
                for (int i = 0; i < ChangeableCommands.Count; i++)
                {
                    Changeable obj = ChangeableCommands[i];
                    if (obj == null) continue;
                    if (obj.Type == command) return obj;
                }
                return null;
            }
            public void AddChangeable(Changeable obj)
            {
                ChangeableCommands.Add(obj);
            }



        }

        [Serializable]
        public class Changeable
        {
            [HideInInspector]
            [SerializeField]
            public string DisplayName;
            public CharacterCommand Type;
            public bool EnableCache = false;
            public float CacheL;
            public float CacheR;
        }


        public delegate void PostEnableDelegate(Commander commander);
        public delegate void PostDelegate(Commander commander);
        public delegate void PostSwapCommandDelegate(Command lastCommand, Command currentCommand);
        public event PostEnableDelegate PostEnable;
        public event PostDelegate PostDisable;
        public event PostSwapCommandDelegate PostSwapCommand;

        public Command LastCommand;
        public Command CurrentCommand;
        public List<Command> Commands = new List<Command>();

        #region [Functions]
        public bool IsProcessing(CharacterCommand type)
        {
            if (CurrentCommand == null)
                return false;
            return CurrentCommand.Type == type;
        }
        public bool ExecuteCommand(CharacterCommand type, bool isCachedCommand = false)
        {
            if (type == CharacterCommand.None) return false;

            Command wannaPlayCommand = TryGet(type);
            if (wannaPlayCommand == null) return false;

            if (CurrentCommand == null)
            {
                LastCommand = CurrentCommand;
                CurrentCommand = wannaPlayCommand;
                if (LastCommand != null && LastCommand.ActiveStatus == true)
                {
                    LastCommand.Finish(this, wannaPlayCommand);
                }
                CurrentCommand.Begin(this);
                if (PostSwapCommand != null)
                {
                    //if(CurrentCommand.Type != CharacterCommand.CC_Walk)
                    //{
                    //    Character.PathFinding.Stop();
                    //}
                    PostSwapCommand(LastCommand, CurrentCommand);
                }
                return true;
            }
            else
            {
                Changeable changeable = CurrentCommand.TryGetChangeable(type);
                if (changeable == null)
                {
                    // Can not break current command
                    return false;
                }
                else
                {
                    if (isCachedCommand == false && changeable.EnableCache)
                    {
                        bool cached = CurrentCommand.DoCache(type);
                        return cached;
                    }
                    else
                    {
                        LastCommand = CurrentCommand;
                        CurrentCommand = wannaPlayCommand;
                        if (LastCommand != null && LastCommand.ActiveStatus == true)
                        {
                            LastCommand.Finish(this, wannaPlayCommand);
                        }
                        CurrentCommand.Begin(this);
                        if (PostSwapCommand != null)
                        {
                            PostSwapCommand(LastCommand, CurrentCommand);
                        }
                        return true;
                    }
                }
            }
        }
        public Command TryGet(CharacterCommand command)
        {
            if (Commands == null || Commands.Count == 0) return null;
            for (int i = 0; i < Commands.Count; i++)
            {
                Command cmd = Commands[i];
                if (cmd == null) continue;
                if (cmd.Type == command) return cmd;
            }
            return null;
        }
        public bool Add(Command cmd)
        {
            if (cmd == null) return false;
            if (TryGet(cmd.Type) != null) return false;
            if (Commands == null) Commands = new List<Command>();
            Commands.Add(cmd); return true;
        }

        #region monobehaviour
        protected override void Awake()
        {
            base.Awake();
            if (Commands != null && Commands.Count > 0)
            {
                for (int i = 0; i < Commands.Count; i++)
                {
                    Command cmd = Commands[i];
                    if (cmd == null) continue;
                    cmd.OnAwake(MotionMachine, this);
                }
            }
        }
        protected override void OnEnable()
        {
            base.OnEnable();
            if (PostEnable != null) PostEnable(this);
        }
        protected override void Update()
        {
            base.Update();
            if (CurrentCommand != null)
            {
                CurrentCommand.Update(this);
            }
        }
        protected override void OnDisable()
        {
            base.OnDisable();
            if (PostDisable != null) PostDisable(this);
        }
        protected override void OnDestroy()
        {
            base.OnDestroy();
            if (Commands != null && Commands.Count > 0)
            {
                for (int i = 0; i < Commands.Count; i++)
                {
                    Command cmd = Commands[i];
                    if (cmd == null) continue;
                    cmd.OnDestroy(MotionMachine, this);
                }
            }
            PostSwapCommand = null;
        }
        #endregion

        public override void ParseXML(SecurityElement element, Character character)
        {
            Character = character;

            ArrayList commandsElement = element.Children;
            if (commandsElement != null && commandsElement.Count > 0)
            {

                Commands = new List<Command>();
                for (int i = 0; i < commandsElement.Count; i++)
                {
                    SecurityElement commandElement = commandsElement[i] as SecurityElement;
                    if (commandElement == null) continue;
                    string commandName = commandElement.Attribute("Type");
                    Command command = new Command();
                    command.Commander = this;
                    command.Type = (CharacterCommand)CharacterSystemUtils.TryParseEnum<CharacterCommand>(commandName);
                    if (TryGet(command.Type) != null) continue;
                    command.DisplayName = commandName;
                    if (string.IsNullOrEmpty(commandElement.Attribute("AutoNext")))
                    {
                        command.AutoNext = CharacterCommand.CC_Idle;
                    }
                    else
                    {
                        command.AutoNext = (CharacterCommand)CharacterSystemUtils.TryParseEnum<CharacterCommand>(commandElement.Attribute("AutoNext"));
                    }
                    string motionsStr = commandElement.Attribute("SequentialMotions");
                    if (string.IsNullOrEmpty(motionsStr)) continue;
                    string[] motionsArray = motionsStr.Split(new char[] { ',' });
                    if (motionsArray == null || motionsArray.Length == 0) continue;
                    command.SequentialMotions = new List<RoleMotionType>();
                    for (int j = 0; j < motionsArray.Length; j++)
                    {
                        string rmtStr = motionsArray[j];
                        if (string.IsNullOrEmpty(rmtStr)) continue;
                        RoleMotionType rmt = (RoleMotionType)CharacterSystemUtils.TryParseEnum<RoleMotionType>(rmtStr);
                        command.AddSequentialMotion(rmt);
                    }
                    ArrayList changeableElements = commandElement.Children;
                    if (changeableElements != null && changeableElements.Count > 0)
                    {
                        for (int j = 0; j < changeableElements.Count; j++)
                        {
                            SecurityElement changeableElement = changeableElements[j] as SecurityElement;
                            if (changeableElement == null) continue;
                            string changeableName = changeableElement.Attribute("Type");
                            if (string.IsNullOrEmpty(changeableName)) continue;
                            Changeable changeable = new Changeable();
                            changeable.Type = (CharacterCommand)CharacterSystemUtils.TryParseEnum<CharacterCommand>(changeableName);
                            if (command.TryGetChangeable(changeable.Type) != null) continue;
                            changeable.DisplayName = changeableName;
                            bool.TryParse(changeableElement.Attribute("EnableCache"), out changeable.EnableCache);
                            float.TryParse(changeableElement.Attribute("CacheL"), out changeable.CacheL);
                            float.TryParse(changeableElement.Attribute("CacheR"), out changeable.CacheR);
                            command.AddChangeable(changeable);
                        }
                    }
                    command.Commander = this;
                    Commands.Add(command);
                }
            }
        }
        public override void OnCharacterInitialized(Character character)
        {
            base.OnCharacterInitialized(character);
            if (Commands != null && Commands.Count > 0)
            {
                for (int i = 0; i < Commands.Count; i++)
                {
                    Command command = Commands[i];
                    if (command == null) continue;
                    command.OnCharacterInitialized(character);
                }
            }
        }

        #endregion
    }
}
