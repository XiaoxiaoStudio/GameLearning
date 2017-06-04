using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mono.Xml;
using System.Security;
using System.Collections;
using UnityEngine;

namespace Air2000
{
    public abstract class CharacterComponent : MonoBehaviour
    {
        public Character Character;
        public Player Player
        {
            get
            {
                return Character == null ? null : Character.Player;
            }
        }
        public UnityEngine.AI.NavMeshAgent NavAgent
        {
            get
            {
                return Character == null ? null : Character.NavAgent;
            }
        }
        public Animation Animation
        {
            get
            {
                return Character == null ? null : Character.Animation;
            }
        }
        public MotionMachine MotionMachine
        {
            get
            {
                return Character == null ? null : Character.MotionMachine;
            }
        }
        public AnimationCrossfader Crossfader
        {
            get
            {
                return Character == null ? null : Character.AnimationCrossfader;
            }
        }
        public Commander Commander
        {
            get
            {
                return Character == null ? null : Character.Commander;
            }
        }
        public SkillController SkillController
        {
            get
            {
                return Character == null ? null : Character.SkillController;
            }
        }
        public FlyerController FlyerController
        {
            get
            {
                return Character == null ? null : Character.FlyerController;
            }
        }
        protected virtual void Awake() { }
        protected virtual void OnEnable() { }
        protected virtual void Start() { }
        protected virtual void Update() { }
        protected virtual void LateUpdate() { }
        protected virtual void OnDisable() { }
        protected virtual void OnDestroy() { }
        public virtual void ParseXML(SecurityElement element, Character character)
        {
            Character = character;
        }
        public virtual void OnCharacterInitialized(Character character)
        {
            Character = character;
        }
    }
}
