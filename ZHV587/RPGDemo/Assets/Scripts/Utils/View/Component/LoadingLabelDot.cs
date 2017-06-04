using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Air2000
{
    public class LoadingLabelDot : MonoBehaviour
    {
        public UILabel UILabel;
        public string DotStr = ".";
        public float Interval = 0.2f;
        public int MaxDotCount = 5;
        private int m_CurrentDotCount = 0;
        private string m_OriginalText;
        private float m_LastRecordTime = -1;
        void OnEnable()
        {
            if (UILabel == null)
            {
                UILabel = GetComponent<UILabel>();
            }
            if (UILabel == null)
            {
                enabled = false;
                return;
            }
            m_OriginalText = "Loading";
        }

        void Update()
        {
            if (m_LastRecordTime == -1f)
            {
                m_LastRecordTime = Time.realtimeSinceStartup;
            }
            if ((Time.realtimeSinceStartup - m_LastRecordTime) >= Interval)
            {
                m_LastRecordTime = Time.realtimeSinceStartup;
                if (UILabel != null && string.IsNullOrEmpty(m_OriginalText) == false)
                {
                    m_CurrentDotCount++;
                    if (m_CurrentDotCount > MaxDotCount)
                    {
                        m_CurrentDotCount = 1;
                    }
                    string tempStr = m_OriginalText;
                    for (int i = 0; i < m_CurrentDotCount; i++)
                    {
                        tempStr += DotStr;
                    }
                    UILabel.text = tempStr;
                }
            }
        }
    }
}
