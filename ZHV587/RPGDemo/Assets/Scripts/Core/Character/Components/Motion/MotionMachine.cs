using UnityEngine;
using System.Collections.Generic;
using System;
using Mono.Xml;
using System.Security;
using System.Collections;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Air2000
{
    public class MotionMachine : CharacterComponent
    {
        #region [enum]Method type
        public enum MethodType
        {
            OnAwake,
            OnStart,
            OnEnable,
            OnDisable,
            OnDestroy,
        }
        #endregion

        #region [Fields]

        public delegate void PostSwapMotionDelegate(Motion lastMotion, Motion currentMotion);
        public event PostSwapMotionDelegate PostSwapMotion;


        [HideInInspector]
        [SerializeField]
        public List<Motion> Motions = new List<Motion>();
        [NonSerialized]
        public bool IsPause = false;
        [NonSerialized]
        public float PauseTime;

        [NonSerialized]
        public Motion LastMotion = null;
        [NonSerialized]
        public Motion CurrentMotion = null;
        [NonSerialized]
        public Motion NextMotion = null;

        #endregion

        #region [Function]

        #region monobehaviour
        protected override void Awake()
        {
            base.Awake();
            NotifyAllMotion(MethodType.OnAwake);
        }
        protected override void Start() { base.Start(); NotifyAllMotion(MethodType.OnStart); }
        protected override void OnEnable() { base.OnEnable(); ExecuteMotion(RoleMotionType.RMT_Idle); NotifyAllMotion(MethodType.OnEnable); }
        protected override void OnDisable() { base.OnDisable(); NotifyAllMotion(MethodType.OnDisable); }
        protected override void Update()
        {
            base.Update();
            if (IsPause)
            {
                PauseTime += Time.deltaTime;
                return;
            }
            if (CurrentMotion != null)
            {
                CurrentMotion.OnUpdate(this);
            }
            if (NextMotion != null)
            {
                if (CurrentMotion != null)
                {
                    CurrentMotion.OnEnd(this);
                }
                LastMotion = CurrentMotion;
                CurrentMotion = NextMotion;
                NextMotion = null;
                CurrentMotion.OnBegin(this);
                CurrentMotion.OnUpdate(this);
                if (PostSwapMotion != null)
                {
                    PostSwapMotion(LastMotion, CurrentMotion);
                }
            }
        }
        protected override void OnDestroy()
        {
            base.OnDestroy();
            LastMotion = null;
            CurrentMotion = null;
            NextMotion = null;
            NotifyAllMotion(MethodType.OnDestroy);
            if (Motions != null)
            {
                Motions.Clear();
            }
        }
        void NotifyAllMotion(MethodType type)
        {
            if (Motions != null && Motions.Count > 0)
            {
                for (int i = 0; i < Motions.Count; i++)
                {
                    Motion motion = Motions[i];
                    if (motion == null) continue;
                    switch (type)
                    {
                        case MethodType.OnAwake:
                            motion.OnMachineAwake(this);
                            break;
                        case MethodType.OnStart:
                            motion.OnMachineStart(this);
                            break;
                        case MethodType.OnEnable:
                            motion.OnMachineEnable(this);
                            break;
                        case MethodType.OnDisable:
                            motion.OnMachineDisable(this);
                            break;
                        case MethodType.OnDestroy:
                            motion.OnMachineDestroy(this);
                            break;
                        default:
                            break;
                    }
                }
            }
        }
        #endregion

        #region play control
        public RoleMotionType LastPlayMotion = RoleMotionType.RMT_Attack_1;
        public bool ExecuteMotion(RoleMotionType type)
        {
            Motion nextMotion = GetMotion(type);
            if (nextMotion == null) return false;
            NextMotion = nextMotion;
            return true;
        }
        public bool ExecuteMotionImmediately(RoleMotionType type)
        {
            if (CurrentMotion != null)
            {
                if (CurrentMotion.Type == type) return false;
            }
            Motion nextMotion = GetMotion(type);
            if (nextMotion == null) return false;
            if (CurrentMotion != null) CurrentMotion.OnEnd(this);
            CurrentMotion = nextMotion;
            CurrentMotion.OnBegin(this);
            return true;
        }
        #endregion

        #region getting & setting
        public Motion GetMotion(RoleMotionType type)
        {
            if (Motions == null || Motions.Count == 0) return null;
            for (int i = 0; i < Motions.Count; i++)
            {
                Motion motion = Motions[i];
                if (motion == null) continue;
                if (type.Equals(motion.Type)) return motion;
            }
            return null;
        }
        public MotionPlugin GetSpecificPlugin(RoleMotionType type, string pluginName, Type pluginType)
        {
            Motion motion = GetMotion(type);
            if (motion == null)
            {
                CharacterSystemUtils.LogError("GetSpecificPlugin error caused by null motion named: " + type.ToString());
                return null;
            }
            return motion.GetPlugin(pluginName, pluginType);
        }
        public MotionPlugin GetSpecificPlugin(RoleMotionType type, string pluginName)
        {
            Motion motion = GetMotion(type);
            if (motion == null)
            {
                CharacterSystemUtils.LogError("GetSpecificPlugin error caused by null motion named: " + type.ToString());
                return null;
            }
            return motion.GetPlugin(pluginName);
        }
        #endregion

        #region editor function
#if UNITY_EDITOR
        public void DisplayEditorView()
        {
            GUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("");
            GUILayout.EndHorizontal();
        }
        public void OnPreReplaceAnimation()
        {
            if (Motions != null && Motions.Count > 0)
            {
                for (int i = 0; i < Motions.Count; i++)
                {
                    Motion motion = Motions[i];
                    if (motion == null) continue;
                    motion.OnPreReplaceAnimation();
                }
            }
        }
        public void OnReplacedAnimation()
        {
            if (Motions != null && Motions.Count > 0)
            {
                for (int i = 0; i < Motions.Count; i++)
                {
                    Motion motion = Motions[i];
                    if (motion == null) continue;
                    motion.OnReplacedAnimation();
                }
            }
        }
        public void UpdateDependency()
        {
            if (Motions != null && Motions.Count > 0)
            {
                for (int i = 0; i < Motions.Count; i++)
                {
                    Motion motion = Motions[i];
                    if (motion == null) continue;
                    motion.UpdateDependency();
                }
            }
        }
#endif
        #endregion

        public override void ParseXML(SecurityElement element, Character character)
        {
            base.ParseXML(element, character);
            if (Character == null)
            {
                CharacterSystemUtils.LogError("MotionMachine.cs:Parse xml fail caused by null Character instance");
                return;
            }
            if (element == null)
            {
                CharacterSystemUtils.LogError("MotionMachine.cs:Parse xml fail caused by null SecurityElement instance");
                return;
            }
            ArrayList motionElements = element.Children;
            if (motionElements == null)
            {
                CharacterSystemUtils.LogError("MotionMachine.cs:Parse xml fail caused by null motion nodes");
                return;
            }
            Motions = new List<Motion>();
            for (int i = 0; i < motionElements.Count; i++)
            {
                SecurityElement motionElement = motionElements[i] as SecurityElement;
                if (motionElement == null) continue;
                Motion motion = new Motion();
                motion.Type = (RoleMotionType)CharacterSystemUtils.TryParseEnum<RoleMotionType>(motionElement.Attribute("Type"));
                if (GetMotion(motion.Type) != null)
                {
                    continue;
                }
                motion.ParseXML(motionElement, this);
                Motions.Add(motion);
            }
        }
        public override void OnCharacterInitialized(Character character)
        {
            base.OnCharacterInitialized(character);
        }
        #endregion
    }
}
