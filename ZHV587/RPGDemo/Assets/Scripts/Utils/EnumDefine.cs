using UnityEngine;
using System.Collections;

namespace Air2000
{
    public enum LAYER
    {
        Default = 0,
        UI = 5,
        Dialog = 9,
        Terrain = 10,
        SceneEffect = 11,
        Player = 12,
        PlayerEffect = 13,
    }

    public enum SceneName
    {
        Login = 0,
        City = 1,
        Battle = 2,
    }

    public enum ModuleName
    {
        Bag = 1,
        Battle = 2,
        Login = 3,
        Main = 4,
        Match = 5,
        Ranklist = 6,
        Room = 7,
    }

    public enum ServerType
    {
        Login, // login(account) server
        Logic, // game logic server
    }

    public enum CrossContextEventType
    {
        GE_HeroChange,//角色发生切换
        GE_SceneBegin, //场景开始
        GE_LevelWasLoaded, //关卡加载完成
        GE_SceneWasLoaded, //场景加载完成
        GE_SceneEnd, //场景结束

        /// <summary>
        /// 主界面蒙板打开/关闭
        /// </summary>
        GE_SetMainMask,

        GE_ClientCommond,

        GE_NetWorkState,

        GE_EditorCreateCharacter,
    }


    public enum NetWorkEventType
    {
        NE_LoginSuccess,
        NE_LoginFailed,
        NE_NotifyLoginInfo,
        NE_NotifySelfInfo,
        NE_NotifyBeanAndDiamond,
        NE_EnterRoomSuccess,
        NE_EnterRoomFailed,
        NE_EnterRoomLocked,
        NE_LeaveRoom,
        NE_SitDownInfo,
        NE_UserEnter,
        NE_UserExit,
        NE_HandsUp,
        NE_GameMessage,
        NE_GameRanking,
        NE_BuyBean,
        NE_SendBean,
        NE_NotifyOffLine,
        NE_NoticeMessage,
        NE_NoticeAwardInfo,
        NE_PublicAnnoument,
        NE_NotifyComplete,
        NE_NotifyProgress,
        NE_NotifyNetworkChange,
        NE_NotifyMailNum,
        NE_NotifyMailInfo,
        NE_NotifyMailOperate,
        NE_NotifyIsGivenDonner,
        NE_NotifyMailChange,
        NE_NotifyMailFriendInfo,
        /// 通知网络连接状态.
        NE_NotifyNetConnect,
        NE_NotifyMailFriendAdjunct,
        NE_NotifyMailAllFriendAdjunct,
        NE_NotigyDimongdBugProp,
        NE_NotifyPlayerInfo,
        NE_NotifyMsgChange,
        NE_NotifySystemAdjunct,
        NE_NotifyOnlineNumber,
        NE_NotifyAvatarInfo,
        NE_NotifyChangeAvatar,
        NE_NotifyClientEXECommond,
        NE_NotifyRedChangePoint,

        NE_NotifyAvatarCultivate,
        NE_NotifyAvatarSkill,

        NE_NotifyUpdateVersion,
        NE_NotifyBeacon,

        NE_NotifySignatureBook,
        NE_NotifySignatureMission,


        NE_NotifyAllMissionInfo,
        NE_NotifyMissionAward,
        NE_NotifyNetworkStateInfo,
        NE_NotiftConfigChange,

        NE_NotifyChatInfo,
        NE_NotifyPhysicalNumber,
    }

    public enum SceneEventType
    {
        SE_None,
        SE_SceneLoadingEnd,
        SE_SceneLoadingBegin,
        /// <summary>
        /// 战斗界面初始化完成.
        /// </summary>
        SE_BattleUIStartEnd,
        /// <summary>
        /// 游戏开始.
        /// </summary>
        SE_StartBattleGame,
        /// <summary>
        /// 游戏相机更改
        /// </summary>
        SE_GameCameraChange,

        /// <summary>
        /// 通知飞机动画结算.
        /// </summary>
        SE_NotifyPlaneAnimaionEnd,

        /// <summary>
        /// 显示点数.
        /// </summary>
        SE_ShowDice,

        /// <summary>
        /// 通知谁掷骰子.
        /// </summary>
        SE_Notify_BattleUI_WhoDice,

        /// <summary>
        /// 飞机抵达位置.
        /// </summary>
        SE_PlaneArrive,

        /// <summary>
        /// 单个玩家飞机模型创建完成.
        /// </summary>
        SE_CharacterEnd,

        /// <summary>
        /// 游戏重连.
        /// </summary>
        SE_GameReconnection,

        /// <summary>
        /// 再玩一次.
        /// </summary>
        SE_PlayAgain,

        /// <summary>
        /// 托管状态.
        /// </summary>
        SE_TrusteeshipState,

        SE_InitedChessboard,

        ///通知TSDK登录成功;
        SE_NotifyTSDKLoginSucceed,
        SE_NotifyTSDKLoginFail,
        SE_NotifyStartTSDKLogin,
        SE_NotifyVersionUpdate,
        SE_NotifyConfigCheck,

        SE_GameOver,

        /// <summary>
        /// 加载完成.
        /// </summary>
        SE_CompletedLoading,
    }


}

