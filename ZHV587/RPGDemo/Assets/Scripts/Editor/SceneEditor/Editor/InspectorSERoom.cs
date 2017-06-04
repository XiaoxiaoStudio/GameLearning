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
    [CustomEditor(typeof(SERoom))]
    public class InspectorSERoom : UnityEditor.Editor
    {
        public SERoom Instance;
        public static string[] ComponentsName = new string[] { "Camera", "Light", "BirthPoint", "MonsterWave" };
        public static int SelectedComIndex = 0;
        void OnEnable()
        {
            Instance = target as SERoom;
        }
        public override void OnInspectorGUI()
        {
            // base.OnInspectorGUI();
            if (Instance == null)
            {
                return;
            }
            EditorUtils.BeginContents();
            GUILayout.Space(4.0f);
            if (GUILayout.Button("Auto Generate"))
            {
                GameObject cameraObj = new GameObject("SECamera");
                cameraObj.transform.SetParent(Instance.transform);
                Instance.Camera = cameraObj.AddComponent<SECamera>();
                Instance.Camera.OnCreate();

                GameObject birthPointObj = new GameObject("SEBirthPoint");
                birthPointObj.transform.SetParent(Instance.transform);
                Instance.BirthPoint = birthPointObj.AddComponent<SEBirthPoint>();
                Instance.BirthPoint.OnCreate();

                GameObject lightObj = new GameObject("SELight");
                lightObj.transform.SetParent(Instance.transform);
                Instance.Light = lightObj.AddComponent<SELight>();
                Instance.Light.OnCreate();

                GameObject terrainObj = new GameObject("SETerrain");
                terrainObj.transform.SetParent(Instance.transform);
                Instance.Terrain = terrainObj.AddComponent<SETerrain>();
                Instance.Terrain.OnCreate();

                GameObject waveObj = new GameObject("SEMonsterWave");
                waveObj.transform.SetParent(Instance.transform);
                Instance.MonsterWave = waveObj.AddComponent<SEMonsterWave>();
                Instance.MonsterWave.OnCreate();
            }
            GUILayout.Space(4.0f);
            GUILayout.BeginHorizontal();
            SelectedComIndex = EditorGUILayout.Popup(SelectedComIndex, ComponentsName);
            if (GUILayout.Button("Create", GUILayout.Height(17f)))
            {
                string name = ComponentsName[SelectedComIndex];
                if (string.IsNullOrEmpty(name))
                {
                    return;
                }
                GameObject obj = new GameObject("SE" + name);
                if (name == "Camera")
                {
                    if (Instance.Camera != null)
                    {
                        GameObject.DestroyImmediate(obj);
                    }
                    else
                    {
                        Instance.Camera = obj.AddComponent<SECamera>();
                        Instance.Camera.OnCreate();
                    }
                }
                else if (name == "Light")
                {
                    if (Instance.Light != null)
                    {
                        GameObject.DestroyImmediate(obj);
                    }
                    else
                    {
                        Instance.Light = obj.AddComponent<SELight>();
                        Instance.Light.OnCreate();
                    }
                }
                else if (name == "BirthPoint")
                {
                    if (Instance.BirthPoint != null)
                    {
                        GameObject.DestroyImmediate(obj);
                    }
                    else
                    {
                        Instance.BirthPoint = obj.AddComponent<SEBirthPoint>();
                        Instance.BirthPoint.OnCreate();
                    }
                }
                else if (name == "MonsterWave")
                {
                    if (Instance.MonsterWave != null)
                    {
                        GameObject.DestroyImmediate(obj);
                    }
                    else
                    {
                        Instance.MonsterWave = obj.AddComponent<SEMonsterWave>();
                        Instance.MonsterWave.OnCreate();
                    }
                }
                if (obj)
                {
                    obj.transform.SetParent(Instance.transform);
                }
            }
            GUILayout.EndHorizontal();

            EditorUtils.EndContents();
        }
    }
}
