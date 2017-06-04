using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Air2000
{
    public class RandomPlayAnimation : MonoBehaviour
    {
        public Animation Animation;
        public AnimationClip IdleClip;
        public AnimationClip[] ClipList;
        public float MinIdleTime = 3;
        public float MaxIdleTime = 6;
        public float LeftIdleTime;
        private bool m_IsIdle;
        void Awake()
        {

        }

        void Start()
        {
            if (Animation == null)
            {
                Animation = GetComponent<Animation>();
            }
            if (Animation == null || ClipList == null || ClipList.Length == 0 || IdleClip == null)
            {
                enabled = false;
                return;
            }
            PlayIdle();
        }
        void PlayIdle()
        {
            Animation.clip = IdleClip;
            Animation.wrapMode = WrapMode.Loop;
            Animation.Play();
            LeftIdleTime = UnityEngine.Random.Range(MinIdleTime, MaxIdleTime);
            m_IsIdle = true;
        }
        void PlayOther(AnimationClip clip)
        {
            Animation.clip = clip;
            Animation.wrapMode = WrapMode.Clamp;
            Animation.Play();
            m_IsIdle = false;
        }
        void Update()
        {
            if (Animation.isPlaying == false)
            {
                PlayIdle();
            }
            else
            {
                bool randomPlay = false;
                if (m_IsIdle)
                {
                    LeftIdleTime -= Time.deltaTime;
                    if (LeftIdleTime <= 0)
                    {
                        randomPlay = true;
                    }
                }
                if (randomPlay == true)
                {
                    int tempIndex = UnityEngine.Random.Range(0, ClipList.Length - 1);
                    AnimationClip tempClip = ClipList[tempIndex];
                    if (tempClip == null)
                    {
                        PlayIdle();
                    }
                    else
                    {
                        PlayOther(tempClip);
                    }
                }
            }
        }
    }
}
