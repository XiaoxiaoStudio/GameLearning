using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Air2000;
using System.Collections;

namespace Air2000
{
    public delegate void PostLoadedCharacterDelegate(CharacterProvider.Request request);
    public class CharacterProvider : MonoBehaviour
    {
        public class FashionInfo
        {
            public string Directory;
            public string Animation;
            public string Head;
            public string Body;
            public string Weapon;
            public string Config;
            public string Key
            {
                get
                {
                    return Animation + "_" + Head + "_" + Body + "_" + Weapon + "_" + Config;
                }
            }
        }
        public class Request
        {
            public Character Character;
            public Task Task;
            public PostLoadedCharacterDelegate Callback;
            public object Param;
            public bool IsDone
            {
                get
                {
                    if (Task != null)
                    {
                        return Task.IsDone;
                    }
                    else
                    {
                        return true;
                    }
                }
            }
            public void Finish(Character character)
            {
                Character = character;
                if (Callback != null)
                {
                    Callback(this);
                }
            }
        }
        public class Task : IEnumerator
        {
            public Player Player;
            public FashionInfo Fashion;
            public List<Request> Requests = new List<Request>();
            public AssetManager.Request LoadAnimationRequset;
            public AssetManager.Request LoadHeadRequest;
            public AssetManager.Request LoadBodyRequest;
            public AssetManager.Request LoadWeaponRequset;
            public AssetManager.Request LoadConfigRequest;
            public bool IsDone
            {
                get
                {
                    if (LoadConfigRequest == null)
                    {
                        return false;
                    }
                    else
                    {
                        return LoadConfigRequest.IsDone;
                    }
                }
            }
            public Task(FashionInfo data)
            {
                Fashion = data;
            }
            public void Execute()
            {
                LoadAnimationRequset = AssetManager.LoadAssetAsync<GameObject>("Role/" + Fashion.Directory + "/", Fashion.Animation);
                LoadHeadRequest = AssetManager.LoadAssetAsync<GameObject>("Role/" + Fashion.Directory + "/Head/", Fashion.Head);
                LoadBodyRequest = AssetManager.LoadAssetAsync<GameObject>("Role/" + Fashion.Directory + "/Body/", Fashion.Body);
                LoadWeaponRequset = AssetManager.LoadAssetAsync<GameObject>("Role/" + Fashion.Directory + "/Weapon/", Fashion.Weapon);
                LoadConfigRequest = AssetManager.LoadAssetAsync<TextAsset>("Config/Role/", Fashion.Config);
            }
            public object Current
            {
                get { return null; }
            }
            public bool MoveNext()
            {
                // It means all character assets is done.
                if (LoadConfigRequest.IsDone)
                {
                    GameObject animation = null;
                    if (LoadAnimationRequset.Asset)
                    {
                        animation = GameObject.Instantiate(LoadAnimationRequset.Asset) as GameObject;
                    }
                    GameObject head = null;
                    if (LoadHeadRequest.Asset)
                    {
                        head = GameObject.Instantiate(LoadHeadRequest.Asset) as GameObject;
                    }
                    GameObject body = null;
                    if (LoadBodyRequest.Asset)
                    {
                        body = GameObject.Instantiate(LoadBodyRequest.Asset) as GameObject;
                    }
                    GameObject weapon = null;
                    if (LoadWeaponRequset.Asset)
                    {
                        weapon = GameObject.Instantiate(LoadWeaponRequset.Asset) as GameObject;
                    }
                    GameObject combinedObj = CombineCharacter(animation, head, body, weapon, Fashion);

                    TextAsset config = LoadConfigRequest.Asset as TextAsset;
                    Finish(combinedObj, config);
                    return false;
                }
                return true;
            }
            public void Reset() { }
            public void Finish(GameObject combinedObj, TextAsset config)
            {
                for (int i = 0; i < Requests.Count; i++)
                {
                    Request req = Requests[i];
                    if (req == null) continue;
                    Character character = null;
                    if (combinedObj)
                    {
                        GameObject avatarObj = GameObject.Instantiate(combinedObj) as GameObject;
                        if (avatarObj)
                        {
                            avatarObj.name = "Avatar";
                            GameObject characterObj = new GameObject();
                            avatarObj.transform.SetParent(characterObj.transform);
                            avatarObj.transform.localPosition = Vector3.zero;
                            avatarObj.transform.localScale = Vector3.one;
                            avatarObj.transform.localRotation = Quaternion.identity;
                            character = characterObj.AddComponent<Character>();
                        }
                    }
                    req.Finish(character);
                }
                if (combinedObj)
                {
                    GameObject.Destroy(combinedObj);
                }
            }
            public void Attach(Request request)
            {
                if (request != null)
                {
                    request.Task = this;
                    Requests.Add(request);
                }
            }
        }

        public static CharacterProvider m_Instance;
        private static List<Task> m_LoadTasks = new List<Task>();
        public static int TaskCount
        {
            get { return m_LoadTasks.Count; }
        }
        void Awake()
        {
            m_Instance = this;

            // Start load looper function.
            StartCoroutine(LoadLooper());
        }
        private IEnumerator LoadLooper()
        {
            while (true)
            {
                if (m_LoadTasks != null && m_LoadTasks.Count > 0)
                {
                    Task task = m_LoadTasks[0];
                    task.Execute();
                    yield return StartCoroutine(task);
                    m_LoadTasks.Remove(task);
                }
                yield return null;
            }
        }
        public static CharacterProvider Instance()
        {
            return m_Instance;
        }
        public Task TryGetTask(FashionInfo data)
        {
            if (m_LoadTasks == null || m_LoadTasks.Count == 0)
            {
                return null;
            }
            for (int i = 0; i < m_LoadTasks.Count; i++)
            {
                Task task = m_LoadTasks[i];
                if (task == null) continue;
                if (task.Fashion.Key == data.Key)
                {
                    return task;
                }
            }
            return null;
        }
        public static void Execute(FashionInfo fashionData, PostLoadedCharacterDelegate callback, object param = null)
        {
            if (m_Instance == null)
            {
                CharacterSystemUtils.LogError("CharacterProvider.cs: LoadCharacter fail caused by null CharacterLoader instance.");
                return;
            }
            if (fashionData == null)
            {
                CharacterSystemUtils.LogError("CharacterProvider.cs: LoadCharacter fail caused by null FashionInfo instance.");
                return;
            }
            Task task = m_Instance.TryGetTask(fashionData);
            if (task == null)
            {
                task = new Task(fashionData);
                task.Attach(new Request() { Callback = callback, Param = param });
                m_LoadTasks.Add(task);
            }
            else
            {
                task.Attach(new Request() { Callback = callback, Param = param });
            }
        }

        /// <summary>
        /// Combine character's bone and mesh with animation,body,weapon and etc.
        /// </summary>
        /// <param name="animation"></param>
        /// <param name="head"></param>
        /// <param name="body"></param>
        /// <param name="weapon"></param>
        /// <param name="fashion"></param>
        /// <returns></returns>
        public static GameObject CombineCharacter(GameObject animation, GameObject head, GameObject body, GameObject weapon, FashionInfo fashion)
        {
            if (animation == null)
            {
                Helper.LogError("Combine character fail caused by null animation obj,fashion key is: " + fashion.Key);
                return null;
            }

            GameObject anim_bones_root = animation;
            SkinnedMeshRenderer skinnedMeshRenderer = null;
            List<Transform> anim_bones = new List<Transform>();
            Transform[] bones = null;

            #region head bones
            if (head)
            {
                skinnedMeshRenderer = head.transform.GetComponentInChildren<SkinnedMeshRenderer>(true);
                if (skinnedMeshRenderer == null)
                {
                    return null;
                }
                Transform headNode = skinnedMeshRenderer.transform;
                SkinnedMeshRenderer headRenderer = skinnedMeshRenderer;
                Transform[] headBones = headRenderer.bones;
                anim_bones.Add(anim_bones_root.transform);
                anim_bones.AddRange(anim_bones_root.GetComponentsInChildren<Transform>());
                bones = new Transform[headBones.Length];
                for (int i = 0; i < bones.Length; i++)
                {
                    Transform node = null;
                    foreach (Transform bone in anim_bones)
                    {
                        if (headBones[i].name == bone.name)
                        {
                            node = bone;
                            break;
                        }
                    }
                    bones[i] = node;
                }
                headRenderer.bones = bones;
                headNode.SetParent(anim_bones_root.transform);
            }
            #endregion

            #region body bones
            if (body)
            {
                skinnedMeshRenderer = body.transform.GetComponentInChildren<SkinnedMeshRenderer>(true);
                if (skinnedMeshRenderer == null)
                {
                    return null;
                }
                Transform bodyNode = skinnedMeshRenderer.transform;
                SkinnedMeshRenderer bodyRenderer = skinnedMeshRenderer;
                Transform[] bodyBones = bodyRenderer.bones;
                anim_bones.Add(anim_bones_root.transform);
                anim_bones.AddRange(anim_bones_root.GetComponentsInChildren<Transform>());
                bones = new Transform[bodyBones.Length];
                for (int i = 0; i < bones.Length; i++)
                {
                    Transform node = null;
                    foreach (Transform bone in anim_bones)
                    {
                        if (bodyBones[i].name == bone.name)
                        {
                            node = bone;
                            break;
                        }
                    }
                    bones[i] = node;
                }
                bodyRenderer.bones = bones;
                bodyNode.SetParent(anim_bones_root.transform);
            }
            #endregion

            #region weapon bones
            if (weapon)
            {
                skinnedMeshRenderer = weapon.transform.GetComponentInChildren<SkinnedMeshRenderer>(true);
                if (skinnedMeshRenderer == null)
                {
                    return null;
                }
                Transform weaponNode = skinnedMeshRenderer.transform;
                SkinnedMeshRenderer weaponRenderer = skinnedMeshRenderer;
                bones = new Transform[weaponRenderer.bones.Length];
                Transform[] weaponBones = weaponRenderer.bones;
                for (int i = 0; i < bones.Length; i++)
                {
                    Transform node = null;
                    foreach (Transform bone in anim_bones)
                    {
                        if (weaponBones[i].name == bone.name)
                        {
                            node = bone;
                            break;
                        }
                    }
                    bones[i] = node;
                }
                weaponRenderer.bones = bones;
                weaponNode.transform.SetParent(anim_bones_root.transform);
            }
            #endregion

            #region if need combine skinnedmeshrenderer,now we don't do that anain,just for saving time.

            //CombineInstance[] combine_list = {
            //    new CombineInstance(),
            //    new CombineInstance (),
            //    new CombineInstance ()
            //};
            //combine_list[0].mesh = headRenderer.sharedMesh;
            //combine_list[1].mesh = bodyRenderer.sharedMesh;
            //combine_list[2].mesh = weaponRenderer.sharedMesh;


            //List<Color> VertexColors = null;
            //try
            //{
            //    VertexColors = new List<Color>(headRenderer.sharedMesh.vertexCount + bodyRenderer.sharedMesh.vertexCount + weaponRenderer.sharedMesh.vertexCount);
            //}
            //catch
            //{
            //    CommonUtils.LogError("CharacterFashion Error: " + animation.name + "  Line 415!");
            //}

            //Color temcolor = Color.black;
            //for (int i = 0; i < headRenderer.sharedMesh.vertexCount; ++i)
            //{
            //    VertexColors.Add(temcolor);
            //}

            //temcolor = Color.white;
            //for (int i = 0; i < bodyRenderer.sharedMesh.vertexCount; ++i)
            //{
            //    VertexColors.Add(temcolor);
            //}

            //temcolor = Color.white;
            //for (int i = 0; i < weaponRenderer.sharedMesh.vertexCount; ++i)
            //{
            //    VertexColors.Add(temcolor);
            //}


            //GameObject newobject = new GameObject("newBody");
            //newobject.transform.SetParent(anim_bones_root.transform);
            //newobject.transform.localPosition = body.transform.localPosition;
            //Mesh pNewMesh = new Mesh();
            //pNewMesh.name = "newbodymesh";
            //pNewMesh.CombineMeshes(combine_list, true, false);
            //pNewMesh.colors = VertexColors.ToArray();
            ////pNewMesh.uv = newUV.ToArray();
            ////pNewMesh.colors=newcolor.ToArray();

            //List<Transform> NewBones = new List<Transform>();
            //NewBones.AddRange(bodyRenderer.bones);
            //NewBones.AddRange(weaponRenderer.bones);

            //SkinnedMeshRenderer newbodyRenderer = newobject.AddComponent<SkinnedMeshRenderer>() as SkinnedMeshRenderer;
            //newbodyRenderer.sharedMesh = pNewMesh;
            //newbodyRenderer.bones = NewBones.ToArray();
            ////newbodyRenderer.material = new Material(Shader.Find("Air2000/Character/FourTextureDiffuse"));
            //newbodyRenderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;

            //newbodyRenderer.receiveShadows = false;
            //newbodyRenderer.useLightProbes = true;
            //newbodyRenderer.updateWhenOffscreen = true;

            #endregion

            GameObject.DestroyImmediate(weapon);
            GameObject.DestroyImmediate(body);
            GameObject.DestroyImmediate(head);

            return anim_bones_root;

        }
    }
}
