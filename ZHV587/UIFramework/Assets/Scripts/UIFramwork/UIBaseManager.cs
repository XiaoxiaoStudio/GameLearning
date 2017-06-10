using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIBaseManager : MonoBehaviour
{
    private static UIBaseManager _instance;

    public static UIBaseManager GetInstance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new GameObject("UIBaseManager").AddComponent<UIBaseManager>();
            }
            return _instance;
        }
    }

    /// <summary>
    /// 缓存从Asset中Load的UI窗体
    /// </summary>
    private Dictionary<UIWindowID, GameObject> mCacheUIWindow;

    /// <summary>
    /// 打开过的UI窗体
    /// </summary>
    private Dictionary<UIWindowID, UIBase> mOpenedUIWindow;

    /// <summary>
    /// 当前显示的UI窗体
    /// </summary>
    private Dictionary<UIWindowID, UIBase> mShowUIWindow;

    /// <summary>
    /// 当前打开的UI窗体
    /// </summary>
    private Stack<UIBase> mPopUIWindow;

    private RectTransform Canvas;

    private RectTransform Fixed;

    private RectTransform Normal;

    private RectTransform Pop;

    private List<UIWindowID> deleteUIWindowIDs = new List<UIWindowID>();

    private void Awake()
    {
        DontDestroyOnLoad(this);
        mCacheUIWindow = new Dictionary<UIWindowID, GameObject>();
        mOpenedUIWindow = new Dictionary<UIWindowID, UIBase>();
        mShowUIWindow = new Dictionary<UIWindowID, UIBase>();
        mPopUIWindow = new Stack<UIBase>();
        CacheAllUIWindow();
    }

    public void ShowWindow(UIWindowID uiWindowID)
    {
        if (IsPopInStackPop())
            return;

        if (mShowUIWindow.ContainsKey(uiWindowID))
            return;

        UIBase tempUIBase;
        if (!mOpenedUIWindow.ContainsKey(uiWindowID))
        {
            tempUIBase = LoadUIBaseTomOpenedUIWindow(uiWindowID);
        }
        else
        {
            mOpenedUIWindow.TryGetValue(uiWindowID, out tempUIBase);
        }
        if (tempUIBase == null)
            return;
        WindowType windowType = tempUIBase.UIType._windowType;

        if (windowType == WindowType.Fixed)
        {
            foreach (UIBase item in mShowUIWindow.Values)
            {
                item.Hide();
            }
            mPopUIWindow.Clear();
        }

        LoadUIWindow(uiWindowID);
    }

    /// <summary>
    /// 关闭窗体
    /// </summary>
    /// <param name="uiWindowID"></param>
    public void CloseWindow(UIWindowID uiWindowID)
    {
        if (!mOpenedUIWindow.ContainsKey(uiWindowID))
            return;
        if (!mShowUIWindow.ContainsKey(uiWindowID))
            return;

        UnLoadUIWindow(uiWindowID);
    }

    /// <summary>
    /// 隐藏窗体
    /// </summary>
    /// <param name="uiWindowID"></param>
    private void UnLoadUIWindow(UIWindowID uiWindowID)
    {
        UIBase tempUIBase;
        mShowUIWindow.TryGetValue(uiWindowID, out tempUIBase);
        if (!mPopUIWindow.Contains(tempUIBase))
            return;
        if (mPopUIWindow.Peek() != tempUIBase)
            return;
        mShowUIWindow.Remove(uiWindowID);
        mPopUIWindow.Pop();
        tempUIBase.Hide();
    }

    /// <summary>
    /// 加载UIBase到OpenedUIWindow
    /// </summary>
    /// <param name="uiWindowID"></param>
    /// <returns></returns>
    private UIBase LoadUIBaseTomOpenedUIWindow(UIWindowID uiWindowID)
    {
        GameObject tempGo;
        if (!mCacheUIWindow.ContainsKey(uiWindowID))
        {
            Debug.Log("UIPrefab路径为找到，请确认UIPrefab路径是否手动加载进去");
            return null;
        }
        mCacheUIWindow.TryGetValue(uiWindowID, out tempGo);
        GameObject newGo = Instantiate(tempGo);
        UIBase tempUIBase = newGo.GetComponent<UIBase>();
        mOpenedUIWindow.Add(uiWindowID, tempUIBase);
        if (Fixed == null && Normal == null)
            FindMountNode();
        switch (tempUIBase.UIType._windowType)
        {
            case WindowType.Fixed:
                newGo.transform.SetParent(Fixed);
                newGo.transform.GetComponent<RectTransform>().offsetMax = Vector2.zero;
                newGo.transform.GetComponent<RectTransform>().offsetMin = Vector2.zero;
                break;

            case WindowType.Normal:
                newGo.transform.SetParent(Normal);
                newGo.transform.localPosition = Vector3.zero;
                newGo.transform.GetComponent<RectTransform>().offsetMax = Vector2.zero;
                newGo.transform.GetComponent<RectTransform>().offsetMin = Vector2.zero;
                break;

            case WindowType.Pop:
                newGo.transform.SetParent(Pop);
                newGo.transform.localPosition = Vector3.zero;
                break;

            default:
                break;
        }
        return tempUIBase;
    }

    /// <summary>
    /// 加载FixedUIWindow
    /// </summary>
    /// <param name="uiWindowID"></param>
    private void LoadUIWindow(UIWindowID uiWindowID)
    {
        UIBase tempUIBase;

        mOpenedUIWindow.TryGetValue(uiWindowID, out tempUIBase);
        if (!mShowUIWindow.ContainsKey(uiWindowID))
        {
            mShowUIWindow.Add(uiWindowID, tempUIBase);
        }
        else
        {
            return;
        }
        switch (tempUIBase.UIType._windowType)
        {
            case WindowType.Fixed:
                tempUIBase.Show();
                break;

            case WindowType.Normal:
                foreach (UIBase item in mPopUIWindow.ToArray())
                {
                    item.Hide();
                    foreach (KeyValuePair<UIWindowID, UIBase> kvp in mShowUIWindow)
                    {
                        if (kvp.Value == item)
                        {
                            deleteUIWindowIDs.Add(kvp.Key);
                        }
                    }
                }
                if (deleteUIWindowIDs != null)
                {
                    foreach (UIWindowID item in deleteUIWindowIDs)
                    {
                        mShowUIWindow.Remove(item);
                    }
                }
                deleteUIWindowIDs.Clear();
                mPopUIWindow.Clear();
                mPopUIWindow.Push(tempUIBase);
                tempUIBase.Show();
                break;

            case WindowType.Pop:
                if (!mPopUIWindow.Contains(tempUIBase))
                {
                    mPopUIWindow.Push(tempUIBase);
                }
                tempUIBase.Show();
                break;

            default:
                break;
        }
    }

    /// <summary>
    /// 判断栈顶元素是否为Pop类型，Pop优先级最高，需关闭完才能打开其他窗体
    /// </summary>
    /// <returns></returns>
    private bool IsPopInStackPop()
    {
        if (mPopUIWindow.Count <= 0)
        {
            return false;
        }
        else
        {
            if (mPopUIWindow.Peek().UIType._windowType == WindowType.Pop)
            {
                return true;
            }
            return false;
        }
    }

    /// <summary>
    /// 缓存所有的UIWindow,UIBase
    /// </summary>
    private void CacheAllUIWindow()
    {
        if (UIPrefabsPath.mPrefabsPathDic != null)
        {
            foreach (KeyValuePair<UIWindowID, string> kv in UIPrefabsPath.mPrefabsPathDic)
            {
                GameObject go = Resources.Load(kv.Value) as GameObject;
                if (go != null)
                {
                    mCacheUIWindow.Add(kv.Key, go);
                }
                else
                {
                    Debug.Log("路径加载出错，请检查" + kv.Value + "路径是否正确");
                }
            }
        }
    }

    private void FindMountNode()
    {
        #region 寻找挂载的节点

        Canvas = GameObject.Find("Canvas").transform as RectTransform;
        if (Canvas != null)
        {
            if (Canvas.Find("Fixed"))
            {
                Fixed = Canvas.Find("Fixed") as RectTransform;
            }
            else
            {
                Fixed = new GameObject("Fixed").AddComponent<RectTransform>();
                Fixed.SetParent(Canvas);
                Fixed.anchorMin = new Vector2(0, 0);
                Fixed.anchorMax = new Vector2(1, 1);

                Fixed.offsetMax = Vector2.zero;
                Fixed.offsetMin = Vector2.zero;
            }

            if (Canvas.Find("Normal"))
            {
                Normal = Canvas.Find("Normal") as RectTransform;
            }
            else
            {
                Normal = new GameObject("Normal").AddComponent<RectTransform>();
                Normal.SetParent(Canvas);
                Normal.anchorMin = new Vector2(0, 0);
                Normal.anchorMax = new Vector2(1, 1);
                Normal.localPosition = Vector3.zero;

                Normal.offsetMax = Vector2.zero;
                Normal.offsetMin = Vector2.zero;
            }
            if (Canvas.Find("Pop"))
            {
                Normal = Canvas.Find("Pop") as RectTransform;
            }
            else
            {
                Pop = new GameObject("Pop").AddComponent<RectTransform>();
                Pop.SetParent(Canvas);
                Pop.anchorMin = new Vector2(0, 0);
                Pop.anchorMax = new Vector2(1, 1);
                Pop.localPosition = Vector3.zero;

                Pop.offsetMax = Vector2.zero;
                Pop.offsetMin = Vector2.zero;
            }
        }

        #endregion 寻找挂载的节点
    }
}