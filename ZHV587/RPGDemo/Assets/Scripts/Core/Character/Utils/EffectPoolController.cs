using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Air2000
{
    public class GameObjProvider : MonoBehaviour
    {
        public static GameObjProvider Instance;
        private static List<GameObject> m_SourceObjs = new List<GameObject>();
        void Awake()
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        void Update()
        {
        }
        void OnDestroy()
        {
            Instance = null;
        }
        public static GameObject Get(string name)
        {
            if (Instance == null) return null;
            Transform effObj = Instance.transform.Find(name);
            if (effObj != null)
                return ObjPoolController.Instantiate(effObj.gameObject);
            return null;
        }
        public static void Pool(GameObject obj)
        {
            if (obj == null)
                return;
            ObjPoolController.Destroy(obj);
        }
    }
}
