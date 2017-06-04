/*----------------------------------------------------------------
            // Copyright © 2015 Air2000
            // 
            // FileName: InspectorCharacter.cs
			// Describle:
			// Created By:  hsu
			// Date&Time:  2016/6/14 20:26:24
            // Modify History:
            //
//----------------------------------------------------------------*/
using System;
using UnityEditor;
using UnityEngine;

namespace Air2000
{
    [CustomEditor(typeof(Character))]
    public class InspectorCharacter : UnityEditor.Editor
    {
        public Character Instance;
        public UnityEngine.Object NewAnimationObj;
        private TextAsset m_LastBaseConfig;
        void OnEnable()
        {
            Instance = target as Character;
        }
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            if (Instance == null) return;
            EditorUtils.BeginContents();
            if (EditorUtils.DrawHeader("Base data"))
            {
                EditorUtils.BeginContents();
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Profession", GUILayout.Width(140f));
                Instance.Profession = (Profession)EditorGUILayout.EnumPopup(Instance.Profession);
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("BaseConfig", GUILayout.Width(140f));
                m_LastBaseConfig = Instance.Config;
                Instance.Config = (TextAsset)EditorGUILayout.ObjectField(Instance.Config, typeof(TextAsset), true);
                if (m_LastBaseConfig != Instance.Config && Instance.Config != null)
                {
                    Instance.ParseXML();
                    Air2000.CharacterSystemUtils.LogInfo("CharacterEditor: Has refreshed character's base config.");
                }
                if (m_LastBaseConfig != null && Instance.Config != null && m_LastBaseConfig.text != Instance.Config.text)
                {
                    Instance.ParseXML();
                    Air2000.CharacterSystemUtils.LogInfo("CharacterEditor: Has refreshed character's base config.");
                }
                if (GUILayout.Button("Null", GUILayout.Height(17)))
                {
                    m_LastBaseConfig = null;
                    Instance.Config = null;
                }
                if (GUILayout.Button("Load", GUILayout.Height(17)))
                {
                    Instance.ParseXML();
                    Air2000.CharacterSystemUtils.LogInfo("CharacterEditor: Has refreshed character's base config.");
                }
                EditorGUILayout.EndHorizontal();


                //EditorGUILayout.BeginHorizontal();
                //EditorGUILayout.LabelField("BodyTexture", GUILayout.Width(140f));
                //Instance.BodyTexture = (BodyTexture)EditorGUILayout.ObjectField(Instance.BodyTexture, typeof(BodyTexture), true);
                //EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();
                //EditorGUILayout.LabelField("Animator", GUILayout.Width(140f));
                //Instance.Animator = (GTAnimator)EditorGUILayout.ObjectField(Instance.Animator, typeof(GTAnimator), true);
                //EditorGUILayout.EndHorizontal();

                EditorUtils.EndContents();
            }
            //GUILayout.Space(1.0f);
            if (EditorUtils.DrawHeader("Motion Config"))
            {
                EditorUtils.BeginContents();
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Machine", GUILayout.Width(140f));
                Instance.MotionMachine = (Air2000.MotionMachine)EditorGUILayout.ObjectField(Instance.MotionMachine, typeof(Air2000.MotionMachine), true);
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Commander", GUILayout.Width(140f));
                Instance.Commander = (Air2000.Commander)EditorGUILayout.ObjectField(Instance.Commander, typeof(Air2000.Commander), true);
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Crossfader", GUILayout.Width(140f));
                Instance.AnimationCrossfader = (Air2000.AnimationCrossfader)EditorGUILayout.ObjectField(Instance.AnimationCrossfader, typeof(Air2000.AnimationCrossfader), true);
                EditorGUILayout.EndHorizontal();
                EditorUtils.EndContents();
            }

            //if (Air2000.ARPG.EditorUtility.DrawHeader("Global Effect"))
            //{
            //    Air2000.ARPG.EditorUtility.BeginContents();
            //    EditorGUILayout.BeginHorizontal();
            //    EditorGUILayout.LabelField("FootShadow", GUILayout.Width(140f));
            //    Instance.FootShadow = (Air2000.ARPG.EffectRoot)EditorGUILayout.ObjectField(Instance.FootShadow, typeof(Air2000.ARPG.EffectRoot), true);
            //    EditorGUILayout.EndHorizontal();

            //    EditorGUILayout.BeginHorizontal();
            //    EditorGUILayout.LabelField("SelfFlow", GUILayout.Width(140f));
            //    Instance.SelfFlow = (Air2000.ARPG.EffectRoot)EditorGUILayout.ObjectField(Instance.SelfFlow, typeof(Air2000.ARPG.EffectRoot), true);
            //    EditorGUILayout.EndHorizontal();

            //    EditorGUILayout.BeginHorizontal();
            //    EditorGUILayout.LabelField("Overlap", GUILayout.Width(140f));
            //    Instance.Overlap = (Air2000.ARPG.EffectRoot)EditorGUILayout.ObjectField(Instance.Overlap, typeof(Air2000.ARPG.EffectRoot), true);
            //    EditorGUILayout.EndHorizontal();
            //    Air2000.ARPG.EditorUtility.EndContents();
            //}

            //if (Air2000.ARPG.EditorUtility.DrawHeader("Controllers"))
            //{
            //    Air2000.ARPG.EditorUtility.BeginContents();
            //    //EditorGUILayout.BeginHorizontal();
            //    //EditorGUILayout.LabelField("Grid Collison", GUILayout.Width(140f));
            //    //Instance.GridController = (GridCollisionController)EditorGUILayout.ObjectField(Instance.GridController, typeof(GridCollisionController), true);
            //    //EditorGUILayout.EndHorizontal();

            //    //EditorGUILayout.BeginHorizontal();
            //    //EditorGUILayout.LabelField("Global Effect", GUILayout.Width(140f));
            //    //Instance.EffectController = (GlobalEffectController)EditorGUILayout.ObjectField(Instance.EffectController, typeof(GlobalEffectController), true);
            //    //EditorGUILayout.EndHorizontal();

            //    //EditorGUILayout.BeginHorizontal();
            //    //EditorGUILayout.LabelField("Face To Screen", GUILayout.Width(140f));
            //    //Instance.FaceToScreenController = (FaceToScreenController)EditorGUILayout.ObjectField(Instance.FaceToScreenController, typeof(FaceToScreenController), true);
            //    //EditorGUILayout.EndHorizontal();
            //    Air2000.ARPG.EditorUtility.EndContents();
            //}

            if (EditorUtils.DrawHeader("Animation Operation"))
            {
                EditorUtils.BeginContents();

                GUILayout.Space(2.0f);

                GUILayout.BeginHorizontal();
                NewAnimationObj = (UnityEngine.Object)EditorGUILayout.ObjectField(NewAnimationObj, typeof(GameObject), false);

                if (GUILayout.Button("Replace Animation", GUILayout.Height(15)))
                {
                    if (NewAnimationObj != null)
                    {
                        ReplaceAnimation();
                    }
                }
                GUILayout.EndHorizontal();


                //EditorGUILayout.BeginHorizontal();
                //EditorGUILayout.LabelField("Global Effect", GUILayout.Width(140f));
                //Instance.EffectController = (GlobalEffectController)EditorGUILayout.ObjectField(Instance.EffectController, typeof(GlobalEffectController), true);
                //EditorGUILayout.EndHorizontal();

                //EditorGUILayout.BeginHorizontal();
                //EditorGUILayout.LabelField("Face To Screen", GUILayout.Width(140f));
                //Instance.FaceToScreenController = (FaceToScreenController)EditorGUILayout.ObjectField(Instance.FaceToScreenController, typeof(FaceToScreenController), true);
                //EditorGUILayout.EndHorizontal();

                EditorUtils.EndContents();
            }

            //if (GTools.Character.EditorUtility.DrawHeader("Utility"))
            //{
            //    GTools.Character.EditorUtility.BeginContents();

            //    //EditorGUILayout.BeginHorizontal();
            //    //EditorGUILayout.LabelField("Grid Collison", GUILayout.Width(140f));
            //    //Instance.GridController = (GridCollisionController)EditorGUILayout.ObjectField(Instance.GridController, typeof(GridCollisionController), true);
            //    //EditorGUILayout.EndHorizontal();
            //    //if (GUILayout.Button("Setup Body(Animation)"))
            //    //{
            //    //    Component[] coms = Instance.GetComponentsInChildren<Component>(true);
            //    //    if (coms != null && coms.Length > 0)
            //    //    {
            //    //        for (int i = 0; i < coms.Length; i++)
            //    //        {
            //    //            Component com = coms[i];
            //    //            if (com == null) continue;

            //    //        }
            //    //    }
            //    //}
            //    GUILayout.Space(2.0f);
            //    if (GUILayout.Button("Update Dependency"))
            //    {

            //    }

            //    //EditorGUILayout.BeginHorizontal();
            //    //EditorGUILayout.LabelField("Global Effect", GUILayout.Width(140f));
            //    //Instance.EffectController = (GlobalEffectController)EditorGUILayout.ObjectField(Instance.EffectController, typeof(GlobalEffectController), true);
            //    //EditorGUILayout.EndHorizontal();

            //    //EditorGUILayout.BeginHorizontal();
            //    //EditorGUILayout.LabelField("Face To Screen", GUILayout.Width(140f));
            //    //Instance.FaceToScreenController = (FaceToScreenController)EditorGUILayout.ObjectField(Instance.FaceToScreenController, typeof(FaceToScreenController), true);
            //    //EditorGUILayout.EndHorizontal();

            //    GTools.Character.EditorUtility.EndContents();
            //}

            EditorUtils.EndContents();
        }
        private void UpdateDependency()
        {
            Instance.Animation = null;
            //Instance.BodyTexture = null;
            //Instance.EffectController = null;
            //Instance.GridController = null;
            //Instance.Animator = null;
            //Instance.FaceToScreenController = null;

            Instance.GetBodyTransform();
            //Instance.GetBodyTexture();
            //Instance.GetAnimator();
            Instance.GetMotionMachine();
            Instance.GetCommander();
            Instance.GetAnimationCrossfader();
            //Instance.GetGridController();
            //Instance.GetEffectController();
            //Instance.GetFaceToScreenController();

            Transform bodyTran = Instance.GetBodyTransform();
            if (bodyTran != null)
            {
                //effect root
                Transform effectBundleTran = bodyTran.Find("EffectBundle");
                if (effectBundleTran != null)
                {
                    //Transform footShadow = effectBundleTran.Find("FootShadow");
                    //if (footShadow != null)
                    //{
                    //    Instance.FootShadow = footShadow.GetComponent<Air2000.ARPG.EffectRoot>();
                    //}
                    //Transform selfFlow = effectBundleTran.Find("SelfFlow");
                    //if (selfFlow != null)
                    //{
                    //    Instance.SelfFlow = selfFlow.GetComponent<Air2000.ARPG.EffectRoot>();
                    //}
                    //Transform overlap = effectBundleTran.Find("Overlap");
                    //if (overlap != null)
                    //{
                    //    Instance.Overlap = overlap.GetComponent<Air2000.ARPG.EffectRoot>();
                    //}
                }
            }
        }
        private void ReplaceAnimation()
        {
            if (Instance.GetMotionMachine() != null)
            {
                Instance.GetMotionMachine().OnPreReplaceAnimation();
            }
            if (NewAnimationObj == null)
            {
                Debug.LogError("InspecFXQCharacter::ReplaceAnimation: please select a animation prefab"); return;
            }
            Transform oldBody = Instance.GetBodyTransform();
            if (oldBody == null)
            {
                Debug.LogError("InspecFXQCharacter::ReplaceAnimation: error caused by null old body transform"); return;
            }
            GameObject newBody = GameObject.Instantiate(NewAnimationObj) as GameObject;
            if (newBody == null)
            {
                Debug.LogError("InspecFXQCharacter::ReplaceAnimation: error caused by null new body transform"); return;
            }
            if (newBody.GetComponent<Animation>() == null)
            {
                Debug.LogError("InspecFXQCharacter::ReplaceAnimation: error caused by null Animation component attach to newBody' gameobject "); return;
            }
            newBody.name = oldBody.name;
            newBody.transform.position = oldBody.transform.position;
            newBody.transform.localScale = oldBody.transform.localScale;
            newBody.transform.eulerAngles = oldBody.transform.eulerAngles;

            Transform effectBundleTransform = oldBody.Find("EffectBundle");
            if (effectBundleTransform != null)
            {
                Transform newEffectBundleTransform = GameObject.Instantiate(effectBundleTransform.gameObject).transform;
                newEffectBundleTransform.name = effectBundleTransform.name;
                newEffectBundleTransform.SetParent(newBody.transform);
                newEffectBundleTransform.localPosition = Vector3.zero;
                newEffectBundleTransform.localScale = Vector3.one;
                newEffectBundleTransform.localRotation = Quaternion.identity;
            }

            Transform controllerBundleTransform = oldBody.Find("ControllerBundle");
            if (controllerBundleTransform != null)
            {
                Transform newControllerBundleTransform = GameObject.Instantiate(controllerBundleTransform.gameObject).transform;
                newControllerBundleTransform.name = controllerBundleTransform.name;
                newControllerBundleTransform.SetParent(newBody.transform);
                newControllerBundleTransform.localPosition = Vector3.zero;
                newControllerBundleTransform.localScale = Vector3.one;
                newControllerBundleTransform.localRotation = Quaternion.identity;
            }

            //BodyTexture oldBodyTexture = Instance.GetBodyTexture();
            //if (oldBodyTexture != null)
            //{
            //    string rootPath = GTools.Character.Utility.GenerateRootPath(oldBody, oldBodyTexture.transform);
            //    Transform newBodyTextureTransform = newBody.transform.Find(rootPath);
            //    if (newBodyTextureTransform != null)
            //    {
            //        BodyTexture newBodyTexture = newBodyTextureTransform.gameObject.AddComponent<BodyTexture>();
            //        newBodyTexture.Raw = oldBodyTexture.Raw;
            //        newBodyTexture.Red = oldBodyTexture.Red;
            //        newBodyTexture.Green = oldBodyTexture.Green;
            //        newBodyTexture.Blue = oldBodyTexture.Blue;
            //        newBodyTexture.Yellow = oldBodyTexture.Yellow;
            //        newBodyTexture.UpdateDependency();
            //    }
            //}

            //NormalFace oldNormalFace = Instance.GetComponentInChildren<NormalFace>();
            //if (oldNormalFace != null)
            //{
            //    string rootPath = GTools.Character.Utility.GenerateRootPath(oldBody, oldNormalFace.transform);
            //    Transform newNormalFaceTransform = newBody.transform.Find(rootPath);
            //    if (newNormalFaceTransform != null)
            //    {
            //        NormalFace newNormalFace = newNormalFaceTransform.gameObject.AddComponent<NormalFace>();
            //        newNormalFace.MinInterval = oldNormalFace.MinInterval;
            //        newNormalFace.MaxInterval = oldNormalFace.MaxInterval;
            //        newNormalFace.TextureInterval = oldNormalFace.TextureInterval;
            //        newNormalFace.Texs = new List<Texture2D>(oldNormalFace.Texs);
            //    }
            //}

            if (Instance.GetMotionMachine() != null)
            {
                Instance.GetMotionMachine().OnPreReplaceAnimation();
            }

            oldBody.SetParent(null);
            Helper.SetLayer(newBody, LAYER.Player.ToString());
            newBody.transform.SetParent(Instance.transform);
            UpdateDependency();
            if (Instance.GetMotionMachine() != null)
            {
                Instance.GetMotionMachine().OnReplacedAnimation();
                Instance.GetMotionMachine().UpdateDependency();
            }
        }
    }
}
