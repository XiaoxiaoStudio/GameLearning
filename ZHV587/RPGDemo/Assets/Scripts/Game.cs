using UnityEngine;

namespace Air2000
{
    public class ObjectTableItem
    {
        public int id;
        public string object_name;
        public string object_icon;
        public string object_model;
        public int object_type;
        public int profession;
        public int buygold;
        public int sellgold;
        public string describe;
    }
    public class Game : MonoBehaviour
    {
        public static Game Instance { get; set; }

        public static EventManager EventProcessor = new EventManager();

        public GUISkin Skin;
        void Awake()
        {
            InitializeGame();
            RegisterScenes();
            RegisterModules();
            SceneManager.Goto((int)SceneName.Login);
        }
        void OnGUI()
        {
            if (NetManager.Instance != null)
            {
                if (Skin)
                {
                    GUI.skin = Skin;
                }
                if (NetManager.CurrentLatency > 0)
                {
                    GUI.Label(new Rect(10, Screen.height - 80, 400, 50), "Ping: " + NetManager.CurrentLatency + " ms");
                }
                else
                {
                    GUI.Label(new Rect(10, Screen.height - 80, 400, 50), "Not connect to server yet");
                }
                if (SceneManager.Current.GetType() == typeof(LoginScene))
                {
                    GUILayout.BeginHorizontal();
                    GUI.Label(new Rect(10, Screen.height - 40, 100, 50), "IP: ");
                    Constants.SERVER_ADDRESS = GUI.TextField(new Rect(60, Screen.height - 40, 200, 50), Constants.SERVER_ADDRESS);
                    GUILayout.EndHorizontal();
                }
                else
                {
                    GUILayout.BeginHorizontal();
                    GUI.Label(new Rect(10, Screen.height - 40, 100, 50), "IP: ");
                    GUI.Label(new Rect(60, Screen.height - 40, 200, 50), Constants.SERVER_ADDRESS);
                    GUILayout.EndHorizontal();
                }
            }
            //if (GUI.Button(new Rect(10, 10, 300, 40), "Get data from sqlite."))
            //{
            //    LocalItem tmpItem = null;
            //    string sql = "select * from object";
            //    bool b = SQLiteProvider.Instance.GetLocalItem(sql, out tmpItem);

            //    if (tmpItem != null && b == true)
            //    {
            //        while (tmpItem.Read())
            //        {
            //            ObjectTableItem item = new ObjectTableItem();

            //            item.id = tmpItem.GetValueByColumnName("id", 0);
            //            item.object_name = tmpItem.GetValueByColumnName("object_name", "0");
            //            item.object_icon = tmpItem.GetValueByColumnName("object_icon", "0");
            //            item.object_model = tmpItem.GetValueByColumnName("object_model", "0");
            //            item.object_type = tmpItem.GetValueByColumnName("object_type", 0);
            //            item.profession = tmpItem.GetValueByColumnName("profession", 0);
            //            item.buygold = tmpItem.GetValueByColumnName("buygold", 0);
            //            item.sellgold = tmpItem.GetValueByColumnName("sellgold", 0);
            //            item.object_model = tmpItem.GetValueByColumnName("describe", "0");
            //        }
            //    }
            //    tmpItem.Close();

            //    int a = 1;
            //}

            //if (GUI.Button(new Rect(10, 10, 300, 40), "Test Enter Room"))
            //{
            //    //ServiceProvider.SendNetPacket<PBMessage.Login_first_request>((int)AccountMessage.)
            //    NetManager.Instance.CreateConnection(NetManager.ServerType.Logic, "127.0.0.1", 6000, null);
            //}
            //if (GUI.Button(new Rect(10, 50, 300, 40), "Send a test msg"))
            //{
            //    Connection toLoginServer = NetManager.Instance.GetConnection(NetManager.ServerType.Logic);
            //    if (toLoginServer != null)
            //    {
            //        PBMessage.go_login_request req = new PBMessage.go_login_request();
            //        req.logintype = 1;
            //        req.account = "hsu2016";
            //        req.pwd = "123456";

            //        for (int i = 0; i < 6; i++)
            //        {
            //            toLoginServer.SendMessageProtoBuf<PBMessage.go_login_request>((int)AccountMessage.GO_ACCOUNT_LOGIN_REQUEST, req);
            //        }
            //    }
            //}
        }
        private void InitializeGame()
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            Application.targetFrameRate = 30; // Disable vsync and set target frame rate to 30fps
            Screen.sleepTimeout = SleepTimeout.NeverSleep; // Keep the screen always display.
            SQLiteHelper.Instance.Initialize();
            if (Constants.LoadingView)
            {
                Constants.LoadingView.Initialize();
            }
        }
        private void RegisterScenes()
        {
            SceneManager.Register(new LoginScene());
            SceneManager.Register(new CityScene());
            SceneManager.Register(new BattleScene());
        }
        private void RegisterModules()
        {
            ModuleManager.Register(new LoginModule());
            ModuleManager.Register(new MainModule());
            ModuleManager.Register(new RanklistModule());
            ModuleManager.Register(new BagModule());
            ModuleManager.Register(new BattleModule());
            ModuleManager.Register(new MatchModule());
        }
        private int ClickBackBtnCount = 0;
        private float m_LastClickBackBtnTime;
        private void Update()
        {
            if (Application.platform == RuntimePlatform.Android && (Input.GetKeyDown(KeyCode.Escape)))
            {
                if ((Time.realtimeSinceStartup - m_LastClickBackBtnTime) < 2000)
                {
                    KillProgress();
                }
                else
                {
                    m_LastClickBackBtnTime = Time.realtimeSinceStartup;
                }
            }
        }
        private void KillProgress()
        {
            Application.Quit();
        }
        private void OnDestroy()
        {
            NetManager.DisconnectAll();
        }
    }
}
