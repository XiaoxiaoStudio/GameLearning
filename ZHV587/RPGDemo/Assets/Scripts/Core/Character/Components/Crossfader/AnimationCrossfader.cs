using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using Mono.Xml;
using System.Security;
using System.Collections;

namespace Air2000
{
    #region [class]InspecAnimationCrossfader
#if UNITY_EDITOR
    [CustomEditor(typeof(AnimationCrossfader))]
    public class InspecAnimationCrossfader : Editor
    {
        public AnimationCrossfader Instance;
        void OnEnable()
        {
            Instance = target as AnimationCrossfader;
        }
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            //if (Instance == null) return;
            //if (GUILayout.Button("Auto Generate"))
            //{
            //    Instance.GetMotionMachine();
            //    if (Instance.Machine == null) return;
            //    if (Instance.Machine.Motions == null || Instance.Machine.Motions.Count == 0) return;
            //    if (Instance.Trees == null) Instance.Trees = new List<AnimationCrossfader.Tree>();

            //    for (int i = 0; i < Instance.Machine.Motions.Count; i++)
            //    {
            //        Motion motion = Instance.Machine.Motions[i];
            //        if (motion == null) continue;
            //        AnimationCrossfader.Tree root = Instance.TryGetTree(motion.Type);
            //        if (root == null)
            //        {
            //            root = new AnimationCrossfader.Tree();
            //            root.Name = motion.Type;
            //            root.DisplayName = motion.Type.ToString();
            //            Instance.AddTree(root);
            //        }
            //    }

            //    for (int i = 0; i < Instance.Trees.Count; i++)
            //    {
            //        AnimationCrossfader.Tree root = Instance.Trees[i];
            //        if (root == null) continue;
            //        for (int j = 0; j < Instance.Trees.Count; j++)
            //        {
            //            AnimationCrossfader.Tree target = Instance.Trees[j];
            //            if (target == null) continue;
            //            if (root.HasNode(target.Name) == false)
            //            {
            //                if (root.Nodes == null) root.Nodes = new List<AnimationCrossfader.Node>();
            //                if (root.Name == target.Name)
            //                {
            //                    root.Nodes.Add(new AnimationCrossfader.Node() { Name = target.Name, DisplayName = target.Name.ToString(), Value = 0f });
            //                }
            //                else
            //                {
            //                    root.Nodes.Add(new AnimationCrossfader.Node() { Name = target.Name, DisplayName = target.Name.ToString(), Value = 0.2f });
            //                }
            //            }
            //        }
            //    }
            //}
        }
    }
#endif
    #endregion

    #region [class]AnimationCrossfader
    public class AnimationCrossfader : CharacterComponent
    {
        #region [class]Tree
        [Serializable]
        public class Tree
        {
            public string Name;
            public bool Replay;
            public List<Node> Nodes = new List<Node>();
            public bool HasNode(string name)
            {
                if (Nodes == null || Nodes.Count == 0) return false;
                for (int i = 0; i < Nodes.Count; i++)
                {
                    Node target = Nodes[i];
                    if (target.Name == name) return true;
                }
                return false;
            }
            public Node GetNode(string name)
            {
                if (Nodes == null || Nodes.Count == 0) return null;
                for (int i = 0; i < Nodes.Count; i++)
                {
                    Node target = Nodes[i];
                    if (target == null) continue;
                    if (target.Name == name) return target;
                }
                return null;
            }
        }
        #endregion

        #region [class]Node
        [Serializable]
        public class Node
        {
            public string Name;
            public float Value;
        }
        #endregion

        public delegate void PostSwapAnimationDelegate(string lastClip, string currentClip);
        public event PostSwapAnimationDelegate PostSwapAnimation;
        public List<Tree> Trees = new List<Tree>();
        private string m_CurrentAnimationClip;
        private string m_LastAnimationClip;
        private bool m_ForcePlayAnimation;// To prevent animation doesn't work.

        #region [Functions]

        #region  monobehaviour
        protected override void Awake() { base.Awake(); }
        protected override void OnEnable() { base.OnEnable(); m_ForcePlayAnimation = true; }
        #endregion

        #region set & get
        public void AddTree(Tree tree)
        {
            if (tree == null) return;
            if (Trees == null) Trees = new List<Tree>();
            for (int i = 0; i < Trees.Count; i++)
            {
                Tree tempTree = Trees[i];
                if (tempTree == null) continue;
                if (tempTree.Name == tree.Name) return;
            }
            Trees.Add(tree);
        }
        public Tree TryGetTree(string treeName)
        {
            if (Trees == null || Trees.Count == 0) return null;
            for (int i = 0; i < Trees.Count; i++)
            {
                Tree tempTree = Trees[i];
                if (tempTree == null) continue;
                if (tempTree.Name == treeName) return tempTree;
            }
            return null;
        }
        public Node GetNode(string treeName, string nodeName)
        {
            Tree tree = TryGetTree(treeName);
            if (tree == null) return null;
            return tree.GetNode(nodeName);
        }
        #endregion

        #region play control
        public void PlayIdle()
        {
            if (Animation == null || MotionMachine == null) return;
            Motion idleMotion = MotionMachine.GetMotion(RoleMotionType.RMT_Idle);
            if (idleMotion == null) return;
            if (Animation != null)
            {
                Animation.Play(idleMotion.ClipName);
            }
        }
        public void Play(Motion motion)
        {
            if (motion == null) return;
            if (MotionMachine == null || Animation == null) return;

            if (m_CurrentAnimationClip == null)
            {
                Animation.Play(motion.ClipName);
            }
            else if (m_ForcePlayAnimation)
            {
                m_ForcePlayAnimation = false;
                Animation.Play(motion.ClipName);
            }
            else
            {
                Tree tree = TryGetTree(m_CurrentAnimationClip);
                if (motion.ClipName == m_CurrentAnimationClip)
                {
                    if (tree != null && tree.Replay)
                    {
                        AnimationClip clip = Animation.GetClip(motion.ClipName);
                        Animation.Stop();
                        Animation.Play(motion.ClipName);
                    }
                }
                else
                {
                    Node node = GetNode(m_CurrentAnimationClip, motion.ClipName);
                    float value = 0f;
                    if (node != null)
                    {
                        value = node.Value;
                    }
                    if (value <= 0)
                    {
                        Animation.Stop();
                        Animation.Play(motion.ClipName);
                    }
                    else
                    {
                        Animation.CrossFade(motion.ClipName, value);
                    }
                }
            }
            m_LastAnimationClip = m_CurrentAnimationClip;
            m_CurrentAnimationClip = motion.ClipName;
            if (PostSwapAnimation != null)
            {
                PostSwapAnimation(m_LastAnimationClip, m_CurrentAnimationClip);
            }
        }
        #endregion

        public override void ParseXML(SecurityElement element, Character character)
        {
            base.ParseXML(element, character);
            ArrayList treesElement = element.Children;
            if (treesElement != null && treesElement.Count > 0)
            {
                Trees = new List<Tree>();
                for (int i = 0; i < treesElement.Count; i++)
                {
                    SecurityElement treeElement = treesElement[i] as SecurityElement;
                    if (treeElement == null) continue;
                    string treeName = treeElement.Attribute("Clip");
                    if (string.IsNullOrEmpty(treeName)) continue;
                    if (TryGetTree(treeName) != null) continue;
                    Tree tree = new Tree();
                    tree.Name = treeName;
                    if (string.IsNullOrEmpty(treeElement.Attribute("Replay")))
                    {
                        tree.Replay = true;
                    }
                    else
                    {
                        bool.TryParse(treeElement.Attribute("Replay"), out tree.Replay);
                    }

                    ArrayList nodesElements = element.Children;
                    if (nodesElements != null && nodesElements.Count > 0)
                    {
                        for (int j = 0; j < treesElement.Count; j++)
                        {
                            SecurityElement nodeElement = nodesElements[j] as SecurityElement;
                            if (nodeElement == null) continue;
                            string nodeName = nodeElement.Attribute("Clip");
                            if (string.IsNullOrEmpty(nodeName)) continue;
                            if (tree.HasNode(nodeName)) continue;
                            Node node = new Node();
                            node.Name = nodeName;
                            float.TryParse(nodeElement.Attribute("Value"), out node.Value);
                            tree.Nodes.Add(node);
                        }
                    }
                    AddTree(tree);
                }
            }
            PlayIdle();
        }

        public override void OnCharacterInitialized(Character character)
        {
            base.OnCharacterInitialized(character);
        }
        #endregion
    }
    #endregion
}

