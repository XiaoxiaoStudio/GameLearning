using System;
using System.Collections;
using System.Collections.Generic;
using System.Security;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;

public class MotionsAnimator : MonoBehaviour
{
    public Animator mAnimator;
    public AnimatorController mAnimatorController;
    public AnimatorControllerLayer mLayer;

    public TextAsset textAsset;

    public List<Motion> Motions = new List<Motion>();

    public Dictionary<RoleMotionType, List<Changeable>> CrossMotionDic = new Dictionary<RoleMotionType, List<Changeable>>();

    public AnimationClip newClip;

    private void Start()
    {
        mAnimator = this.transform.parent.GetChild(0).GetComponent<Animator>();
        mAnimatorController = mAnimator.runtimeAnimatorController as AnimatorController;

        textAsset = Resources.Load<TextAsset>("Config/Warrior");
        ParseXML(textAsset);
        InitAnimator();
    }

    private void Update()
    {
    }

    private void InitAnimator()
    {
        AnimatorStateMachine sm = mAnimatorController.layers[0].stateMachine;
        AnimatorClear(mAnimator);
        CreateSMStateAndMotion(sm, Motions);
        ChildAnimatorState[] AllAStates = sm.states;
        mAnimatorController.AddParameter("PlayAnimationByInt", AnimatorControllerParameterType.Int);
        CreateTransitionFromStateToChangeable(AllAStates, CrossMotionDic);
    }

    private void AnimatorClear(Animator animator)
    {
        AnimatorController mAnimatorController = animator.runtimeAnimatorController as AnimatorController;
        AnimatorStateMachine sm = mAnimatorController.layers[0].stateMachine;
        AnimatorControllerParameter[] acps = mAnimatorController.parameters;
        ChildAnimatorStateMachine[] casms = sm.stateMachines;
        ChildAnimatorState[] cass = sm.states;
        for (int i = 0; i < acps.Length; i++)
        {
            mAnimatorController.RemoveParameter(0);
        }
        for (int i = 0; i < cass.Length; i++)
        {
            if (cass[i].state.name == "Entry" || cass[i].state.name == "Exit" || cass[i].state.name == "Any State")
            {
                break;
            }
            sm.RemoveState(cass[i].state);
        }
        for (int i = 0; i < casms.Length; i++)
        {
            sm.RemoveStateMachine(casms[i].stateMachine);
        }
    }

    private void CreateSMStateAndMotion(AnimatorStateMachine sm, List<Motion> Motions)
    {
        for (int i = 0; i < Motions.Count; i++)
        {
            Debug.Log("Create " + Motions[i].Type.ToString());
            AnimatorState state = sm.AddState(Motions[i].Type.ToString());
            AnimationClip chip = (AnimationClip)AssetDatabase.LoadAssetAtPath("Assets/Defenders/Res/anim/mainRole/warrior/warrior@" + Motions[i].ChipName + ".FBX", typeof(AnimationClip));
            state.motion = chip;
        }
    }

    private void CreateTransitionFromStateToChangeable(ChildAnimatorState[] AllAStates, Dictionary<RoleMotionType, List<Changeable>> oneTomanyDic)
    {
        foreach (ChildAnimatorState item in AllAStates)
        {
            List<Changeable> tempList = null;
            foreach (KeyValuePair<RoleMotionType, List<Changeable>> kvp in oneTomanyDic)
            {
                if (item.state.name == kvp.Key.ToString())
                {
                    tempList = kvp.Value;
                    break;
                }
            }
            if (tempList != null)
            {
                foreach (Changeable changeable in tempList)
                {
                    foreach (var tempState in AllAStates)
                    {
                        if (tempState.state.name == changeable.Type.ToString())
                        {
                            AnimatorStateTransition AST1 = item.state.AddTransition(tempState.state);
                            AST1.AddCondition(AnimatorConditionMode.Equals, changeable.parameters, "PlayAnimationByInt");
                            Motion motion = AnimationCanBreakIn(item.state.name);
                            AST1.hasExitTime = !motion.Break;
                            if (AST1.hasExitTime)
                            {
                                AST1.exitTime = motion.ExitTime;
                            }
                            Debug.Log(item.state.name + "To" + tempState.state + AST1.hasExitTime);
                            break;
                        }
                    }
                }
            }
        }
    }

    private Motion AnimationCanBreakIn(string type)
    {
        foreach (var item in Motions)
        {
            if (item.Type.ToString() == type)
            {
                return item;
            }
        }
        return null;
    }

    #region read XML

    private void ParseXML(TextAsset textAsset)
    {
        SecurityElement element = SecurityElement.FromString(textAsset.text);
        SecurityElement MotionsElement = element.SearchForChildByTag("Motions");
        Motions = ParseXMLMotion(MotionsElement);

        #region old Method

        //ArrayList MotionsElements = MotionsElement.Children;
        //for (int i = 0; i < MotionsElements.Count; i++)
        //{
        //    SecurityElement tempElement = MotionsElements[i] as SecurityElement;
        //    if (tempElement == null)
        //        continue;
        //    Motion motion = new Motion();
        //    motion.Type = (RoleMotionType)Enum.Parse(typeof(RoleMotionType), tempElement.Attribute("Type"));
        //    motion.ChipName = tempElement.Attribute("Clip");
        //    motion.Speed = float.Parse(tempElement.Attribute("Speed"));
        //    motion.WrapMode = (WrapMode)Enum.Parse(typeof(WrapMode), tempElement.Attribute("WrapMode"));
        //    string str = tempElement.Attribute("Break");
        //    motion.Break = (str == "true" ? true : false);
        //    Motions.Add(motion);

        //    ArrayList ChangeableElement = tempElement.Children;
        //    List<Changeable> ChangeableList = new List<Changeable>();
        //    for (int j = 0; j < ChangeableElement.Count; j++)
        //    {
        //        SecurityElement tempElementNode = ChangeableElement[j] as SecurityElement;
        //        if (tempElementNode == null)
        //            continue;
        //        Changeable changeable = new Changeable();
        //        changeable.Type = (RoleMotionType)Enum.Parse(typeof(RoleMotionType), tempElementNode.Attribute("Type"));
        //        changeable.parameters = int.Parse(tempElementNode.Attribute("parameters"));
        //        changeable.NextType = (RoleMotionType)Enum.Parse(typeof(RoleMotionType), tempElementNode.Attribute("NextType"));
        //        ChangeableList.Add(changeable);
        //    }
        //    CrossMotionDic.Add(motion.Type, ChangeableList);
        //}

        #endregion old Method
    }

    private List<Motion> ParseXMLMotion(SecurityElement element)
    {
        List<Motion> tempMotionList = new List<Motion>();
        ArrayList MotionsElements = element.Children;
        for (int i = 0; i < MotionsElements.Count; i++)
        {
            SecurityElement tempElement = MotionsElements[i] as SecurityElement;
            if (tempElement == null)
                continue;
            Motion motion = new Motion();
            motion.Type = (RoleMotionType)Enum.Parse(typeof(RoleMotionType), tempElement.Attribute("Type"));
            motion.ChipName = tempElement.Attribute("Clip");
            motion.Speed = float.Parse(tempElement.Attribute("Speed"));
            motion.ExitTime = float.Parse(tempElement.Attribute("ExitTime"));
            motion.NextType = (RoleMotionType)Enum.Parse(typeof(RoleMotionType), tempElement.Attribute("NextType"));
            motion.WrapMode = (WrapMode)Enum.Parse(typeof(WrapMode), tempElement.Attribute("WrapMode"));
            string str = tempElement.Attribute("Break");
            motion.Break = (str == "true" ? true : false);
            tempMotionList.Add(motion);
            CrossMotionDic.Add(motion.Type, ParseXMLChangeable(tempElement));
        }
        return tempMotionList;
    }

    private List<Changeable> ParseXMLChangeable(SecurityElement element)
    {
        List<Changeable> tempChangeableList = new List<Changeable>();
        ArrayList ChangeableElement = element.Children;
        for (int j = 0; j < ChangeableElement.Count; j++)
        {
            SecurityElement tempElementNode = ChangeableElement[j] as SecurityElement;
            if (tempElementNode == null)
                continue;
            Changeable changeable = new Changeable();
            changeable.Type = (RoleMotionType)Enum.Parse(typeof(RoleMotionType), tempElementNode.Attribute("Type"));
            changeable.parameters = int.Parse(tempElementNode.Attribute("parameters"));
            tempChangeableList.Add(changeable);
        }
        return tempChangeableList;
    }

    #endregion read XML

    public void PlayAnimation(RoleMotionType type)
    {
        int parameters = (int)type;
        mAnimator.SetInteger("PlayAnimationByInt", parameters);
    }

    public bool CanExecute(RoleMotionType CurType, RoleMotionType NextType)
    {
        List<Changeable> changeableList;
        CrossMotionDic.TryGetValue(CurType, out changeableList);
        foreach (var item in changeableList)
        {
            if (item.Type == NextType)
            {
                return true;
            }
        }
        return false;
    }
}