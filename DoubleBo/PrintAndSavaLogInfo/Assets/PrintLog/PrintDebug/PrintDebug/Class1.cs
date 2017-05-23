using System.Collections;
using UnityEngine;

namespace PrintDebug
{
    public class DebugLog
    {
        /// <summary>
        /// 定义一个标志 用于控制是否输出日志
        /// </summary>
        static public bool EnableLog = true;

        /// <summary>
        /// 打印日志 但是没有传递打印日志对应的组件
        /// </summary>
        /// <param name="message"></param>
        static public void Log(object message)
        {
            Log(message, null);
        }

        /// <summary>
        /// 打印日志 并显示打印日志对应的组件
        /// </summary>
        /// <param name="message"></param>
        /// <param name="context"></param>
        static public void Log(object message, Object context)
        {
            if (EnableLog)
            {
                ////message为打印的信息，context为打印信息对应的组件
                Debug.Log(message, context);
            }
        }

        static public void LogError(object message)
        {
            LogError(message, null);
        }
        static public void LogError(object message, Object context)
        {
            if (EnableLog)
            {
                Debug.LogError(message, context);
            }
        }

        static public void LogWarning(object message)
        {
            LogWarning(message, null);
        }
        static public void LogWarning(object message, Object context)
        {
            if (EnableLog)
            {
                Debug.LogWarning(message, context);
            }
        }
    }
}
