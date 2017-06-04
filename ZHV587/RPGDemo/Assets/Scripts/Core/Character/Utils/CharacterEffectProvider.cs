using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Air2000
{
    public class CharacterEffectProvider
    {
        private void RegisterCommonEffect()
        {
            Register("Effect/SkillEffect/Common/", "archerHit");
            Register("Effect/SkillEffect/Common/", "warriorHit");
            Register("Effect/SkillEffect/Common/", "mageHit");
        }
        private static CharacterEffectProvider m_Instance;
        public static CharacterEffectProvider Instance
        {
            get
            {
                if (m_Instance == null)
                {
                    m_Instance = new CharacterEffectProvider();
                    m_Instance.RegisterCommonEffect();
                }
                return m_Instance;
            }
        }
        private static Transform m_EffectRecorderNode;
        public static Transform EffectRecorderNode
        {
            get
            {
                if (m_EffectRecorderNode == null)
                {
                    m_EffectRecorderNode = new GameObject("EffectsNode").transform;
                    GameObject.DontDestroyOnLoad(m_EffectRecorderNode);
                }
                return m_EffectRecorderNode;
            }
        }
        public static Dictionary<string, GameObject> m_EffectRecorder = new Dictionary<string, GameObject>();
        public static void Register(string directory, string name)
        {
            if (Instance == null)
            {

            }
            string key = directory + name;
            if (string.IsNullOrEmpty(key))
            {
                return;
            }
            GameObject obj = null;
            if (m_EffectRecorder.TryGetValue(key, out obj))
            {
                return;
            }
            AssetManager.LoadAssetAsync<GameObject>(directory, name, OnLoadedEffectObj, key);
            m_EffectRecorder.Add(key, null);
        }
        public static void OnLoadedEffectObj(AssetManager.Request req)
        {
            if (req.Asset == null)
            {
                Helper.LogError(Instance.GetType() + ".cs: Load effect error,asset path is: " + req.Param);
                return;
            }
            GameObject obj = null;

            obj = GameObject.Instantiate(req.Asset) as GameObject;
            obj.name = req.Asset.name;
            Helper.SetLayer(obj, LAYER.PlayerEffect.ToString());
            obj.transform.SetParent(EffectRecorderNode);
            obj.SetActive(false);
            m_EffectRecorder.Remove((string)req.Param);
            m_EffectRecorder.Add((string)req.Param, obj);
        }
        public static GameObject Get(string directory, string name)
        {
            string key = directory + name;
            if (string.IsNullOrEmpty(key))
            {
                return null;
            }
            GameObject originObj = null;
            if (m_EffectRecorder.TryGetValue(key, out originObj))
            {
                if (originObj)
                {
                    return ObjectPoolController.Instantiate(originObj);
                }
            }
            return null;
        }
        public static void Pool(GameObject obj)
        {
            if (obj)
            {
                ObjectPoolController.Destroy(obj);
            }
        }
    }
}
