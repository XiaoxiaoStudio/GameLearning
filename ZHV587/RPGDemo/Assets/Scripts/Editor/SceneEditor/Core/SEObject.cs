using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using System.Security;
using System.Collections;
using Mono.Xml;

namespace Air2000
{
    public class SEObject : MonoBehaviour
    {
        public delegate void PostAssetLoadedDelegate(SEObject obj);
        public event PostAssetLoadedDelegate PostAssetLoaded;
        protected virtual void Awake() { }
        protected virtual void OnEnable() { }
        protected virtual void Start() { }
        protected virtual void OnDisable() { }
        protected virtual void OnDestroy() { }
        protected virtual void Update() { }
        protected virtual void LateUpdate() { }

        public virtual void OnCreate() { }
        public virtual void LoadDependAsset() { }
        protected void LoadAsset(string filepath, string filename)
        {
            if (Application.isPlaying)
            {
                //ResLoader.LoadAssetAsync(filepath, filename, typeof(GameObject), OnAssetLoaded);
            }
            else
            {
#if UNITY_EDITOR
               // OnAssetLoaded(UnityEditor.AssetDatabase.LoadAssetAtPath<GameObject>(filepath), null);
#endif
            }

        }
        //protected virtual void OnAssetLoaded(UnityEngine.Object obj, ResLoadParam param)
        //{
        //    if (PostAssetLoaded != null)
        //    {
        //        PostAssetLoaded(this);
        //    }
        //}
        public virtual void ParseXML(SecurityElement element)
        {
            transform.position = Helper.StrToVec3(element.Attribute("Position"));
            transform.eulerAngles = Helper.StrToVec3(element.Attribute("Rotation"));
            transform.localScale = Helper.StrToVec3(element.Attribute("Scale"));
        }
        public virtual SecurityElement GenerateXmlElement(SecurityElement element)
        {
            element.AddAttribute("Position", transform.position.ToString());
            element.AddAttribute("Rotation", transform.eulerAngles.ToString());
            element.AddAttribute("Scale", transform.localScale.ToString());
            return element;
        }
    }
}
