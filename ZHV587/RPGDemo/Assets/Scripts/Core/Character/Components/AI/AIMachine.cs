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
    public class AIMachine : CharacterComponent
    {
        public enum MethodType
        {
            OnAwake,
            OnStart,
            OnEnable,
            OnDisable,
            OnDestroy,
        }

        public delegate void PostSwapStateDelegate(AIState lastState, AIState currentState);
        public event PostSwapStateDelegate PostSwapState;
        public List<AIState> States = new List<AIState>();
        public List<AITrigger> Triggers = new List<AITrigger>();

        public AIState LastState = null;
        public AIState CurrentState = null;
        public AIState NextState = null;


        #region [Function]

        #region monobehaviour
        protected override void Awake()
        {
            GetCharacter();
            NotifyAllState(MethodType.OnAwake);
        }
        protected override void Start() { NotifyAllState(MethodType.OnStart); }
        protected override void OnEnable() { ExecuteIdleState(); NotifyAllState(MethodType.OnEnable); }
        protected override void OnDisable() { NotifyAllState(MethodType.OnDisable); }
        protected override void Update()
        {
            if (CurrentState != null)
            {
                CurrentState.Update(this);
            }
            if (NextState != null)
            {
                if (CurrentState != null)
                {
                    CurrentState.End(this);
                }
                LastState = CurrentState;
                CurrentState = NextState;
                NextState = null;
                CurrentState.Begin(this);
                if (PostSwapState != null)
                {
                    PostSwapState(LastState, CurrentState);
                }
            }
            UpdateTriggers();
        }
        protected override void OnDestroy()
        {
            LastState = null;
            CurrentState = null;
            NextState = null;
            NotifyAllState(MethodType.OnDestroy);
            if (States != null)
            {
                States.Clear();
            }
        }
        void NotifyAllState(MethodType type)
        {
            if (States != null && States.Count > 0)
            {
                for (int i = 0; i < States.Count; i++)
                {
                    AIState state = States[i];
                    if (state == null) continue;
                    switch (type)
                    {
                        case MethodType.OnAwake:
                            state.OnMachineAwake(this);
                            break;
                        case MethodType.OnStart:
                            state.OnMachineStart(this);
                            break;
                        case MethodType.OnEnable:
                            state.OnMachineEnable(this);
                            break;
                        case MethodType.OnDisable:
                            state.OnMachineDisable(this);
                            break;
                        case MethodType.OnDestroy:
                            state.OnMachineDestroy(this);
                            break;
                        default:
                            break;
                    }
                }
            }
        }
        #endregion

        #region internal
        void UpdateTriggers()
        {
            if (Triggers != null && Triggers.Count > 0)
            {
                for (int i = 0; i < Triggers.Count; i++)
                {
                    AITrigger trigger = Triggers[i];
                    if (trigger == null) continue;
                    if (trigger.IsTrigger(this))
                    {
                        ExecuteState(trigger.ExecuteState);
                        break;
                    }
                }
            }
        }
        public void ExecuteIdleState()
        {
            ExecuteState("AS_Idle");
        }
        public bool ExecuteState(string name)
        {
            AIState state = TryGetState(name);
            if (state == null)
            {
                return false;
            }
            else
            {
                if (CurrentState == null)
                {
                    NextState = state;
                    return true;
                }

                if (state == CurrentState)
                {
                    if (state.ForceStop)
                    {
                        NextState = state;
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    NextState = state;
                    return true;
                }
            }
        }
        #endregion

        #region getting & setting
        public AIState TryGetState(string name)
        {
            if (States == null || States.Count == 0) return null;
            for (int i = 0; i < States.Count; i++)
            {
                AIState state = States[i];
                if (state == null || string.IsNullOrEmpty(state.Name)) continue;
                if (state.Name.Equals(name)) return state;
            }
            return null;
        }
        public void AddState(AIState state)
        {
            if (state == null) return;
            if (TryGetState(state.Name) != null) return;
            if (States == null) States = new List<AIState>();
            States.Add(state);
        }
        public Character GetCharacter()
        {
            if (Character == null)
            {
                Character[] chs = GetComponentsInParent<Character>(true);
                if (chs != null && chs.Length > 0)
                {
                    Character = chs[0];
                }
            }
            return Character;
        }
        #endregion


        public override void ParseXML(SecurityElement element, Character character)
        {
            Character = character;
            if (Character == null)
            {
                CharacterSystemUtils.LogError("AIMachine.cs:Parse xml fail caused by null Character instance");
                return;
            }
            if (element == null)
            {
                CharacterSystemUtils.LogError("AIMachine.cs:Parse xml fail caused by null SecurityElement instance");
                return;
            }
            ArrayList childElements = element.Children;
            if (childElements == null)
            {
                CharacterSystemUtils.LogError("AIMachine.cs:Parse xml fail caused by null child xml nodes");
                return;
            }
            States = new List<AIState>();
            Triggers = new List<AITrigger>();
            for (int i = 0; i < childElements.Count; i++)
            {
                SecurityElement tempElement = childElements[i] as SecurityElement;
                if (tempElement == null) continue;
                if (tempElement.Tag.Equals("States"))
                {
                    // Parse all states.
                    ArrayList stateElements = tempElement.Children;
                    for (int j = 0; j < stateElements.Count; j++)
                    {
                        SecurityElement stateElement = stateElements[j] as SecurityElement;
                        if (stateElement == null) continue;
                        string stateName = stateElement.Attribute("Name");
                        if (string.IsNullOrEmpty(stateName)) continue;
                        if (TryGetState(stateName) != null) continue;
                        Type stateType = Type.GetType(stateName);
                        if (stateType == null) continue;
                        AIState stateObj = stateType.Assembly.CreateInstance(stateType.FullName) as AIState;
                        if (stateObj == null) continue;
                        stateObj.Name = stateName;
                        stateObj.ParseXML(stateElement, this);
                        AddState(stateObj);
                    }
                }
                else if (tempElement.Tag.Equals("Triggers"))
                {
                    // Parse all triggers.
                    ArrayList triggerElements = tempElement.Children;
                    for (int j = 0; j < triggerElements.Count; j++)
                    {
                        SecurityElement triggerElement = triggerElements[j] as SecurityElement;
                        if (triggerElement == null) continue;
                        string executeStateName = triggerElement.Attribute("ExecuteState");
                        if (string.IsNullOrEmpty(executeStateName)) continue;
                        if (TryGetState(executeStateName) == null) continue;
                        AITrigger trigger = new AITrigger(executeStateName);

                        int random = 100;
                        if (triggerElement.Attribute("Random") != null)
                        {
                            int.TryParse(triggerElement.Attribute("Random"), out random);
                        }
                        trigger.Random = random;
                        trigger.ParseXML(triggerElement, this);
                        Triggers.Add(trigger);
                    }
                }
                else
                {
                    continue;
                }
            }
        }
        public override void OnCharacterInitialized(Character character)
        {
            base.OnCharacterInitialized(character);
        }
        #endregion
    }
}
