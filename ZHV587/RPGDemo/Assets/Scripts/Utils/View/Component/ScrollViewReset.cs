using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Air2000
{
    public class ScrollViewReset : MonoBehaviour
    {
        private UIScrollView mSv;

        Vector3 mStartPos;

        void Start()
        {
            mSv = GetComponent<UIScrollView>();
            if (mSv != null)
            {
                mStartPos = mSv.panel.cachedTransform.localPosition;
            }
        }

        void OnEnable()
        {
            if (mSv != null)
            {
                //mSv.pShouldMove = false;
                mSv.DisableSpring();

                Vector3 tempVector3 = mStartPos - mSv.panel.cachedTransform.localPosition;
                mSv.MoveRelative(tempVector3);
            }
        }

        public void Reset()
        {
            OnEnable();
        }

        void OnDestroy()
        {
            mSv = null;
        }
    }
}
