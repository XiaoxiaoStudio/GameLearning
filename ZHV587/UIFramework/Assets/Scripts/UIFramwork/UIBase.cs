using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WindowType
{
    Fixed,
    Normal,
    Pop,
}

public enum ShowType
{
    DoNothing,
    HideOther,
}

public class UIType
{
    public WindowType _windowType;
    public ShowType _showType;
    public UIWindowID _uiWindowID;

    public UIType(WindowType windowType, ShowType showType, UIWindowID uiWindowID)
    {
        _windowType = windowType;
        _showType = showType;
        _uiWindowID = uiWindowID;
    }
}

public abstract class UIBase : MonoBehaviour
{
    private UIType _UIType;

    public UIType UIType
    {
        get
        {
            return _UIType;
        }

        set
        {
            _UIType = value;
        }
    }

    private Transform _CacheTransform;

    public Transform CacheTransform
    {
        get
        {
            if (null == _CacheTransform)
            {
                _CacheTransform = this.transform;
            }
            return _CacheTransform;
        }
    }

    private GameObject _UIGameObject;

    public GameObject UIGameObject
    {
        get
        {
            if (null == _UIGameObject)
            {
                _UIGameObject = this.gameObject;
            }
            return _UIGameObject;
        }
    }

    public abstract void InitUIBase();

    private void Awake()
    {
        OnAwake();
    }

    private void Start()
    {
        OnStart();
    }

    private void Update()
    {
        OnUpdate();
    }

    public virtual void OnAwake()
    {
        InitUIBase();
    }

    public virtual void OnStart()
    { }

    public virtual void OnUpdate()
    { }

    public virtual void Show()
    {
        UIGameObject.SetActive(true);
    }

    public virtual void Hide()
    {
        UIGameObject.SetActive(false);
    }

    public virtual void DestoryUI()
    {
        Destroy(UIGameObject);
    }
}

/// <summary>
/// 枚举名字与prefabs名字相同且与类名相同
/// </summary>
public enum UIWindowID
{
    test1,
    test2,
    MainForm,
    kanpsack,
    hero,
    arena,
    shop,
    UsePop,
}

public class UIPrefabsPath
{
    private const string UIPath = "UIPrefabs/";

    public static Dictionary<UIWindowID, string> mPrefabsPathDic = new Dictionary<UIWindowID, string>()
    {
        {UIWindowID.test1, UIPath+UIWindowID.test1.ToString()},
        {UIWindowID.test2, UIPath+UIWindowID.test2.ToString()},
        {UIWindowID.MainForm, UIPath+UIWindowID.MainForm.ToString()},
        {UIWindowID.shop, UIPath+UIWindowID.shop.ToString()},
        {UIWindowID.kanpsack, UIPath+UIWindowID.kanpsack.ToString()},
        {UIWindowID.arena, UIPath+UIWindowID.arena.ToString()},
        {UIWindowID.hero, UIPath+UIWindowID.hero.ToString()},
        {UIWindowID.UsePop, UIPath+UIWindowID.UsePop.ToString()}
    };
}