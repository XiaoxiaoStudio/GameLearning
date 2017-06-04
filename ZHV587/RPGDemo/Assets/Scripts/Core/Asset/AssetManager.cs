using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Air2000
{
    public delegate void PostFinish(AssetManager.Request request);
    public class AssetManager : MonoBehaviour
    {
        public class Request
        {
            public Task Task;
            public UnityEngine.Object Asset
            {
                get
                {
                    if (Task != null && Task.Operation != null && Task.GetType() == typeof(LoadResTask))
                    {
                        ResourceRequest req = Task.Operation as ResourceRequest;
                        if (req != null)
                        {
                            return req.asset;
                        }
                    }
                    return null;
                }
            }
            public PostFinish Callback;
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
            public void Finish()
            {
                if (Callback != null)
                {
                    Callback(this);
                }
            }
        }
        public class Task : IEnumerator
        {
            public List<Request> Requests = new List<Request>();
            public AsyncOperation Operation;
            public Task()
            {
            }
            public virtual string Key
            {
                get
                {
                    return string.Empty;
                }
            }
            public virtual bool IsDone
            {
                get
                {
                    if (Operation == null)
                    {
                        return false;
                    }
                    else
                    {
                        return Operation.isDone;
                    }
                }
            }
            public object Current
            {
                get { return null; }
            }
            public virtual void Execute()
            {
            }
            public virtual bool MoveNext()
            {
                if (Operation.isDone)
                {
                    Finish();
                    return false;
                }
                return true;
            }
            public virtual void Reset() { }
            public virtual void Finish()
            {
                for (int i = 0; i < Requests.Count; i++)
                {
                    Request req = Requests[i];
                    if (req == null) continue;
                    req.Finish();
                }
            }
            public virtual void Attach(Request req)
            {
                req.Task = this;
                Requests.Add(req);
            }
        }
        public class LoadSceneTask : Task
        {
            public string SceneName;
            public LoadSceneTask(string sceneName)
            {
                SceneName = sceneName;
            }
            public override void Execute()
            {
                base.Execute();
                Operation = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(SceneName);
            }
        }
        public class LoadResTask : Task
        {
            public Type AssetType;
            public string AssetDirectory;
            public string AssetName;
            public LoadResTask(Type assetType, string assetDirectory, string assetName)
            {
                AssetType = assetType;
                AssetDirectory = assetDirectory;
                AssetName = assetName;
            }
            public override string Key
            {
                get
                {
                    return AssetDirectory + AssetName;
                }
            }
            public override bool IsDone
            {
                get
                {
                    if (Operation == null)
                    {
                        return false;
                    }
                    else
                    {
                        return Operation.isDone;
                    }
                }
            }
            public override void Execute()
            {
                Operation = Resources.LoadAsync(AssetDirectory + AssetName, AssetType);
            }
        }
        public AssetManager Instance { get; set; }
        private static AssetManager m_Instance;
        private static List<Task> m_LoadTasks = new List<Task>();
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
        public static int TaskCount
        {
            get
            {
                if (m_LoadTasks == null)
                {
                    return 0;
                }
                return m_LoadTasks.Count;
            }
        }
        public static Task TryGetTask(string key)
        {
            if (m_LoadTasks == null || m_LoadTasks.Count == 0)
            {
                return null;
            }
            for (int i = 0; i < m_LoadTasks.Count; i++)
            {
                Task task = m_LoadTasks[i];
                if (task == null) continue;
                if (task.Key == key)
                {
                    return task;
                }
            }
            return null;
        }
        public static Request LoadAssetAsync<T>(string directory, string name, PostFinish callback = null, object param = null)
        {
            return LoadAssetAsync(typeof(T), directory, name, callback, param);
        }
        public static Request LoadAssetAsync(Type assetType, string directory, string name, PostFinish callback = null, object param = null)
        {
            Task task = TryGetTask(directory + name);
            Request req = new Request() { Callback = callback, Param = param };
            if (task == null)
            {
                task = new LoadResTask(assetType, directory, name);
                task.Attach(req);
                m_LoadTasks.Add(task);
            }
            else
            {
                task.Attach(req);
            }
            return req;
        }
        public static UnityEngine.Object LoadAsset(string directory, string name)
        {
            return Resources.Load(directory + name);
        }
        public static Request LoadSceneAsync(string sceneName, PostFinish callback = null, object param = null)
        {
            Task task = TryGetTask(sceneName);
            if (task == null)
            {
                Request req = new Request() { Callback = callback, Param = param };
                task = new LoadSceneTask(sceneName);
                task.Attach(req);
                m_LoadTasks.Add(task);
                return req;
            }
            return null;
        }
    }
}
