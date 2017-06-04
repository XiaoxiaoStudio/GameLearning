using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Air2000
{
    public abstract class Scene
    {
        public int ID { get; set; }

        public EventManager EventProcessor { get; set; }

        public Performer Performer { get; set; }

        public Scene(int id)
        {
            ID = id;
            EventProcessor = new EventManager();
        }

        public virtual void Begin()
        {
            Game.EventProcessor.Notify(new EvtEx<Scene>((int)CrossContextEventType.GE_SceneBegin, this));
        }

        public virtual void Update() {  }

        public virtual void End()
        {
            Resources.UnloadUnusedAssets();
            Game.EventProcessor.Notify(new EvtEx<Scene>((int)CrossContextEventType.GE_SceneEnd, this));
        }
    }
}
