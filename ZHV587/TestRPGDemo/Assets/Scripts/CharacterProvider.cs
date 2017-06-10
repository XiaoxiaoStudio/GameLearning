using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterProvider : MonoBehaviour
{
    public class fashionInfo
    {
        public string Directory;
        public string Animation;
        public string Head;
        public string Body;
        public string Weapon;
    }

    /// <summary>
    /// Combine character's bone and mesh with animation,body,weapon and etc.
    /// </summary>
    /// <param name="animation"></param>
    /// <param name="head"></param>
    /// <param name="body"></param>
    /// <param name="weapon"></param>
    /// <returns></returns>
    public static GameObject CombineCharacter(GameObject animation, GameObject head, GameObject body, GameObject weapon)
    {
        if (animation == null)
        {
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

        #endregion head bones

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

        #endregion body bones

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

        #endregion weapon bones

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

        #endregion if need combine skinnedmeshrenderer,now we don't do that anain,just for saving time.

        GameObject.DestroyImmediate(weapon);
        GameObject.DestroyImmediate(body);
        GameObject.DestroyImmediate(head);

        return anim_bones_root;
    }
}