using System.Collections.Generic;

namespace Air2000
{
    public delegate void PostSwapSceneDelegate(Scene last, Scene current);

    public static class SceneManager
    {
        public static Dictionary<int, Scene> Scenes = new Dictionary<int, Scene>();

        public static Scene Last { get; set; }

        public static Scene Current { get; set; }

        public static Scene Next { get; set; }

        public static event PostSwapSceneDelegate PostSwapScene;

        public static void Register(Scene scene)
        {
            if (scene == null) return;
            if (Scenes.ContainsKey(scene.ID) == false)
            {
                Scenes.Add(scene.ID, scene);
            }
        }

        public static void Goto(int id)
        {
            Scene scene = null;
            if (Scenes.TryGetValue(id, out scene))
            {
                if (Current == scene)
                {
                    return;
                }
                Next = scene;
            }
        }

        public static void Update()
        {
            if (Current != null)
            {
                Current.Update();
            }
            if (Next != null)
            {
                if (Current != null)
                {
                    Current.End();
                }
                Last = Current;
                Current = Next;
                Next = null;
                if (PostSwapScene!= null)
                {
                    PostSwapScene(Last, Current);
                }
                Current.Begin();
            }
        }

    }
}

