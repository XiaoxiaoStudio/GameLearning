using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Air2000
{
    public class LoginScene : Scene
    {
        public enum EventType
        {
            ET_CameraLookAtMenu,
            ET_CameraLookAtRoleSelect,
        }

        public LoginScene() : base((int)SceneName.Login) { }

        public override void Begin()
        {
            base.Begin();
            AssetManager.LoadSceneAsync("NewLogin");
            //ContextProvider.StartActivity<LoginModule>();
        }
    }
}
