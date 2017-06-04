using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using System.Security;
using System.Collections;
using Mono.Xml;
using UnityEditor;

namespace Air2000
{
    [CustomEditor(typeof(SETerrain))]
    public class InspectorSETerrain : UnityEditor.Editor
    {
        public SETerrain Instance;
        void OnEnable()
        {
            Instance = target as SETerrain;
        }
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            if (Instance == null)
                return;
            GUILayout.BeginHorizontal();
            Instance.PrefabName = EditorGUILayout.TextField("PrefabName", Instance.PrefabName);
            if (GUILayout.Button("Load"))
            {
                Instance.LoadDependAsset();
            }
            GUILayout.EndHorizontal();
        }
    }
}
