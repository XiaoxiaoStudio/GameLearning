using System;
using UnityEngine;

namespace Air2000
{
    public class CharacterPoolController : MonoBehaviour
    {
        public static CharacterPoolController Instance;
        //        public Character ShuaiShuai;
        //        public Character Joe;
        //        public Character LittleSister;
        //        public Character Mage;
        //        void Awake()
        //        {
        //            Instance = this;
        //            DontDestroyOnLoad(gameObject);
        //            ResLoader.ListenInitializeFinish += OnResManagerInited;
        //        }
        //        void OnResManagerInited()
        //        {
        //            string shuaishuai_prefab_path;
        //#if ASSETBUNDLE_MODE
        //            shuaishuai_prefab_path="role/shuaishuai";
        //#else
        //            shuaishuai_prefab_path = "Model/Role/Boy_shuai/Shuaishuai(Merged)";
        //#endif
        //            ResLoader.LoadAssetAsync(shuaishuai_prefab_path, "Shuaishuai(Merged)", typeof(GameObject), OnLoadShuaishuai);

        //            string Joe_prefab_path;
        //#if ASSETBUNDLE_MODE
        //            Joe_prefab_path="role/joe";
        //#else
        //            Joe_prefab_path = "Model/Role/Joe/Joe(Merged)";
        //#endif
        //            ResLoader.LoadAssetAsync(Joe_prefab_path, "Joe(Merged)", typeof(GameObject), OnLoadJoe);

        //            string Mage_prefab_path = "role/mage";
        //#if ASSETBUNDLE_MODE
        //            Mage_prefab_path="role/mage";
        //#else
        //            Mage_prefab_path = "Model/Role/Mage/Mage(Merged)";
        //#endif
        //            ResLoader.LoadAssetAsync(Mage_prefab_path, "Mage(Merged)", typeof(GameObject), OnLoadMage);

        //            string LittleSister_prefab_path = "role/littlesister";
        //#if ASSETBUNDLE_MODE
        //            LittleSister_prefab_path= "role/littlesister";
        //#else
        //            LittleSister_prefab_path = "Model/Role/Little_girl/LittleSister(Merged)";
        //#endif
        //            ResLoader.LoadAssetAsync(LittleSister_prefab_path, "LittleSister(Merged)", typeof(GameObject), OnLoadLittleSister);
        //        }
        void Start()
        {
        }
        //void OnLoadShuaishuai(UnityEngine.Object obj, ResLoadParam param)
        //{
        //    if (obj == null) return;
        //    GameObject newObj = GameObject.Instantiate(obj) as GameObject;
        //    if (newObj == null) return;
        //    newObj.name = "Shuaishuai(Merged)";
        //    ShuaiShuai = newObj.GetComponent<Character>();
        //    newObj.SetActive(false);
        //    newObj.transform.SetParent(transform);
        //}
        //void OnLoadJoe(UnityEngine.Object obj, ResLoadParam param)
        //{
        //    if (obj == null) return;
        //    GameObject newObj = GameObject.Instantiate(obj) as GameObject;
        //    if (newObj == null) return;
        //    newObj.name = "Joe(Merged)";
        //    Joe = newObj.GetComponent<Character>();
        //    newObj.SetActive(false);
        //    newObj.transform.SetParent(transform);
        //}
        //void OnLoadMage(UnityEngine.Object obj, ResLoadParam param)
        //{
        //    if (obj == null) return;
        //    GameObject newObj = GameObject.Instantiate(obj) as GameObject;
        //    if (newObj == null) return;
        //    newObj.name = "Mage(Merged)";
        //    Mage = newObj.GetComponent<Character>();
        //    newObj.SetActive(false);
        //    newObj.transform.SetParent(transform);
        //}
        //void OnLoadLittleSister(UnityEngine.Object obj, ResLoadParam param)
        //{
        //    if (obj == null) return;
        //    GameObject newObj = GameObject.Instantiate(obj) as GameObject;
        //    if (newObj == null) return;
        //    newObj.name = "LittleSister(Merged)";
        //    LittleSister = newObj.GetComponent<Character>();
        //    newObj.SetActive(false);
        //    newObj.transform.SetParent(transform);
        //}
        void OnDestroy()
        {

        }

        public static Character Get(string config)
        {
            return null;
            //return GetCharacter("", Color.black);
        }
        public static Character Get(string config, Color color)
        {
            return null;
            //return GetCharacter(NewGameCharacter.StrToProfession(config), color);
        }
        public static Character GetCharacter(Profession prefession, Color color)
        {
            //Character ch = null;
            //switch (prefession)
            //{
            //    case Profession.Boy:
            //        ch = Instance.ShuaiShuai;
            //        break;
            //    case Profession.Girl:
            //        ch = Instance.Joe;
            //        break;
            //    case Profession.Fox:
            //        ch = Instance.LittleSister;
            //        break;
            //    default:
            //        break;
            //}
            //if (ch == null) return null;
            //GameObject newChObj = ObjPoolController.Instantiate(ch.gameObject);
            //if (newChObj == null) return null;
            //ch = newChObj.GetComponent<Character>();
            //if (ch == null) return null;
            //ch.Init(color);
            return null;
        }
        public static bool GonnaQuit = false;
        void OnApplicationQuit()
        {
            GonnaQuit = true;
        }
        public static void Pool(GameObject obj)
        {
            if (GonnaQuit) return;
            if (obj != null)
            {
                Character character = obj.GetComponent<Character>();
                if (character != null)
                {
                }
                ObjPoolController.DestroyImmediate(obj);
            }
        }
        public static void Pool(Character character)
        {
            if (GonnaQuit) return;
            if (character != null)
            {
                ObjPoolController.DestroyImmediate(character.gameObject);
            }
        }
    }
}
