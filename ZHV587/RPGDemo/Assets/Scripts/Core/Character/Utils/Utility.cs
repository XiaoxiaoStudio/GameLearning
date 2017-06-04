#define LOG_INFO
#define LOG_WARNING
#define LOG_ERROR

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Air2000
{
    public class CharacterSystemUtils
    {
        public static string GetTypeNameWithoutNamespcae(string varTypeName)
        {
            if (string.IsNullOrEmpty(varTypeName))
            {
                return string.Empty;
            }
            string[] pathArray = varTypeName.Split(new char[] { '.' });
            if (pathArray == null || pathArray.Length == 0)
            {
                return string.Empty;
            }
            return pathArray[pathArray.Length - 1];
        }

        public static string GenerateID()
        {
            byte[] buffer = Guid.NewGuid().ToByteArray();
            return BitConverter.ToString(buffer, 0);
        }

        public static void LogError(string msg)
        {
#if LOG_ERROR
            Debug.LogError("[CharacterSystem] " + msg); return;
#endif
        }
        public static void LogInfo(string msg)
        {
#if LOG_INFO
            Debug.Log("[CharacterSystem] " + msg); return;
#endif
        }
        public static void LogWarning(string msg)
        {
#if LOG_WARNING
            Debug.LogWarning("[CharacterSystem] " + msg); return;
#endif
        }

        public static RoleMotionType CC_RMT(CharacterCommand command)
        {
            if (command == CharacterCommand.None)
            {
                return RoleMotionType.RMT_Idle;
            }
            string[] strArray = command.ToString().Split(new char[] { '_' });
            if (strArray == null || strArray.Length < 2)
            {
                return RoleMotionType.RMT_Idle;
            }
            string rmtStr = "RMT_" + strArray[1];
            try
            {
                RoleMotionType rmt = (RoleMotionType)System.Enum.Parse(typeof(RoleMotionType), rmtStr);
                return rmt;
            }
            catch
            {
                return RoleMotionType.RMT_Idle;
            }
        }
        public static CharacterCommand RMT_CC(RoleMotionType type)
        {
            if (type == RoleMotionType.RMT_Idle)
            {
                return CharacterCommand.CC_Idle;
            }
            else if (type == RoleMotionType.RMT_Run)
            {
                return CharacterCommand.CC_Run;
            }
            //else if (type == RoleMotionType.RMT_Jump)
            //{
            //    return CharacterCommand.CC_JumpToPoint;
            //}
            //else if (type == RoleMotionType.RMT_Fly)
            //{
            //    return CharacterCommand.CC_FlyToPoint;
            //}
            string[] strArray = type.ToString().Split(new char[] { '_' });
            if (strArray == null || strArray.Length < 2)
            {
                return CharacterCommand.CC_Idle;
            }
            string rmtStr = "CC_" + strArray[1];
            try
            {
                CharacterCommand cc = (CharacterCommand)System.Enum.Parse(typeof(CharacterCommand), rmtStr);
                return cc;
            }
            catch
            {
                return CharacterCommand.CC_Idle;
            }
        }
        public static string GenerateRootPath(Transform targetTransform, Transform currentTransform)
        {
            string path = string.Empty;
            path = InternalGenerateRootPath(targetTransform, currentTransform, path);
            return path;
        }
        private static string InternalGenerateRootPath(Transform targetTransform, Transform currentTransform, string path)
        {
            if (targetTransform == null || currentTransform == null)
            {
                return string.Empty;
            }
            if (targetTransform != currentTransform)
            {
                if (string.IsNullOrEmpty(path))
                {
                    path = currentTransform.name;
                }
                else
                {
                    path = currentTransform.name + "/" + path;
                }
                return InternalGenerateRootPath(targetTransform, currentTransform.parent, path);
            }
            else
            {
                return path;
            }
        }
        public static object TryParseEnum<T>(string attrib)
        {
            if (string.IsNullOrEmpty(attrib))
                return -1;
            try
            {
                return Enum.Parse(typeof(T), attrib);
            }
            catch { }
            return -1;
        }
    }
}
