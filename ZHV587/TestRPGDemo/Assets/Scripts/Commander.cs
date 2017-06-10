using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Commander : MonoBehaviour
{
    public class Command
    {
        private bool isActive = false;

        public bool GetActive
        {
            get
            {
                return isActive;
            }
        }

        public MotionsAnimator motionsAnimator;
        private RoleMotionType Parameters;

        private Plugin Plugin;

        public RoleMotionType GetParameters
        {
            get
            {
                return Parameters;
            }
        }

        public float mCurTime = 0f;

        private RoleMotionType DefauteState;

        public string GetDefauteState
        {
            get
            {
                return DefauteState.ToString();
            }
        }

        public Command(MotionsAnimator MA, RoleMotionType type, RoleMotionType defauteType, Plugin plugin)
        {
            motionsAnimator = MA;
            Parameters = type;
            DefauteState = defauteType;
            Plugin = plugin;
        }

        public Command(MotionsAnimator MA, RoleMotionType type)
        {
            motionsAnimator = MA;
            Parameters = type;
        }

        public void Execute()
        {
            isActive = true;
            motionsAnimator.PlayAnimation(Parameters);
        }

        public void ExecuteToDefaute()
        {
            motionsAnimator.PlayAnimation(DefauteState);
        }

        public void Update()
        {
            mCurTime += Time.deltaTime;
            if (mCurTime >= motionsAnimator.mAnimator.GetCurrentAnimatorStateInfo(0).length)
            {
                isActive = false;
                mCurTime = 0f;
            }
            if (Plugin != null)
            {
                if (mCurTime >= Plugin.BeginTime)
                {
                    CreateEffect(Plugin.EndTime - Plugin.BeginTime);
                    Plugin = null;
                }
            }
        }

        public void CreateEffect(float destoryTime)
        {
            GameObject effectGo = null;
            if (effectGo == null)
            {
                effectGo = Instantiate(Resources.Load<GameObject>(Plugin.EffectPath + Plugin.EffectName));
                effectGo.transform.parent = motionsAnimator.transform.parent;
                effectGo.transform.localPosition = Plugin.Position;
                effectGo.transform.localRotation = Quaternion.identity;
                effectGo.transform.localScale = Plugin.Scale;
            }
            Destroy(effectGo, destoryTime);
        }
    }

    public Command CurrentCommand = null;
    public Command CacheCommand = null;

    private void Update()
    {
        MoveToNextExecute();
    }

    private void MoveToNextExecute()
    {
        if (CacheCommand != null)
        {
            if (CurrentCommand == null)
            {
                CurrentCommand = CacheCommand;
                CacheCommand = null;
                CurrentCommand.Execute();
                Debug.Log(CurrentCommand.GetParameters.ToString() + ".Execute()");
            }
        }
        if (CurrentCommand != null)
        {
            if (CurrentCommand.GetActive)
            {
                CurrentCommand.Update();
                Debug.Log(CurrentCommand.GetParameters.ToString() + ".Update()");
            }
            else
            {
                if (CacheCommand == null)
                {
                    if (CurrentCommand.GetDefauteState != null)
                    {
                        CurrentCommand.ExecuteToDefaute();
                        Debug.Log("To Defaute Command");
                    }
                }
                CurrentCommand = null;
                Debug.Log("CurrentCommand = null");
            }
        }
    }

    public void Execute(Command command)
    {
        if (CurrentCommand == null)
        {
            CurrentCommand = CacheCommand;
            CacheCommand = command;
            Debug.Log("CurrentCommand = CacheCommand; CacheCommand = command; ");
        }
        else
        {
            if (CurrentCommand.motionsAnimator.CanExecute(CurrentCommand.GetParameters, command.GetParameters))
            {
                CacheCommand = command;
                Debug.Log("CacheCommand == " + command.GetParameters.ToString());
            }
        }
    }
}