using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;

public class CreateAnimator : MonoBehaviour
{
    private Animator mAnimator;
    private AnimatorController mAnimatorController;
    private AnimatorControllerLayer mLayer;

    // Use this for initialization
    private void Start()
    {
        InitAnimator();
    }

    // Update is called once per frame
    private void Update()
    {
    }

    private void InitAnimator()
    {
        mAnimator = GetComponent<Animator>();
        mAnimatorController = mAnimator.runtimeAnimatorController as AnimatorController;
        AnimatorStateMachine sm = mAnimatorController.layers[0].stateMachine;

        // Add parameters
        //mAnimatorController.AddParameter("TransitionNow", AnimatorControllerParameterType.Trigger);
        //mAnimatorController.AddParameter("Reset", AnimatorControllerParameterType.Trigger);
        //mAnimatorController.AddParameter("GotoB1", AnimatorControllerParameterType.Trigger);
        //mAnimatorController.AddParameter("GotoC", AnimatorControllerParameterType.Trigger);

        //AnimatorState stateA = sm.AddState("A");
        //AnimatorState stateB = sm.AddState("B");
        //Add Transitions
        //AnimatorStateTransition Transition = stateA.AddTransition(stateB);

        //ChildAnimatorState[] mStates = sm.states;
        //Debug.Log(mStates.Length);
        //for (int i = 0; i < mStates.Length; i++)
        //{
        //    Debug.Log(mStates[i].state.name);
        //}
        //AnimatorStateMachine ASM = sm.AddStateMachine("ASMA");
        //AnimatorState stateA = sm.AddState("A");
    }

    private static void AddStateTransition(string path, AnimatorControllerLayer layer)
    {
        AnimatorStateMachine sm = layer.stateMachine;
        //根据动画文件读取它的AnimationClip对象
        AnimationClip newClip = Resources.Load(path, typeof(AnimationClip)) as AnimationClip;

        ////取出动画名子 添加到state里面
        AnimatorState state = sm.AddState(newClip.name);
        //5.0改变
        state.motion = newClip;
        Debug.Log(state.motion);
        //把state添加在layer里面
        AnimatorStateTransition trans = sm.AddAnyStateTransition(state);
    }
}