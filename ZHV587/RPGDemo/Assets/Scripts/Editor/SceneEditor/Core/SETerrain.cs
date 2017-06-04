using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using System.Security;
using System.Collections;
using Mono.Xml;
using Air2000;

namespace Air2000
{
    public class SETerrain : SEObject
    {
        [HideInInspector]
        [SerializeField]
        public string PrefabName;
        [HideInInspector]
        [SerializeField]
        public GameObject TerrainObj;
        public override void LoadDependAsset()
        {
            string path = string.Empty;
            if (Application.isPlaying)
            {
                path = "Prefab/Terrain/" + PrefabName;
            }
            else
            {
                path = "Assets/Resources/Prefab/Terrain/" + PrefabName + ".prefab";
            }
            LoadAsset(path, PrefabName);
        }
        //protected override void OnAssetLoaded(UnityEngine.Object obj, ResLoadParam param)
        //{
        //    base.OnAssetLoaded(obj, param);
        //    if (obj == null)
        //    {
        //        CommonUtils.LogError("SETerrain.cs: Load terrain prefab error");
        //        return;
        //    }
        //    else
        //    {
        //        TerrainObj = GameObject.Instantiate(obj) as GameObject;
        //        TerrainObj.transform.SetParent(transform);
        //        CommonUtils.SetLayer(TerrainObj, GameScene.LAYER.Terrain);
        //    }
        //}
    }
}
