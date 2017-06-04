using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Air2000
{
    public class CityScene : Scene
    {
        public enum EventType
        {
            ET_CameraLookAtMenu,
            ET_CameraLookAtRoleSelect,
        }
        public static CityScene Instance { get; set; }
        public CityScene() : base((int)SceneName.City)
        {
            Instance = this;
        }
        public override void Begin()
        {
            base.Begin();
            AssetManager.LoadSceneAsync("City");
            //ContextProvider.StartActivity<MainModule>();
        }
    }
}
