using UnityEngine;

namespace Air2000
{
    public abstract class Performer : MonoBehaviour
    {
        public delegate void PostLifeDelegate(Performer performer);

        public event PostLifeDelegate PostOnAwake;

        public event PostLifeDelegate PostOnEnable;

        public event PostLifeDelegate PostOnStart;

        public event PostLifeDelegate PostOnUpdate;

        public event PostLifeDelegate PostOnLateUpdate;

        public event PostLifeDelegate PostOnDisable;

        public event PostLifeDelegate PostOnDestroy;

        protected virtual void Awake()
        {
            if (PostOnAwake != null)
            {
                PostOnAwake(this);
            }
        }

        protected virtual void OnEnable()
        {
            if (PostOnEnable != null)
            {
                PostOnEnable(this);
            }
        }

        protected virtual void Start()
        {
            if (PostOnStart != null)
            {
                PostOnStart(this);
            }
        }

        protected virtual void OnDisable()
        {
            if (PostOnDisable != null)
            {
                PostOnDisable(this);
            }
        }

        protected virtual void Update()
        {
            if (PostOnUpdate != null)
            {
                PostOnUpdate(this);
            }
        }

        protected virtual void LateUpdate()
        {
            if (PostOnLateUpdate != null)
            {
                PostOnLateUpdate(this);
            }
        }

        protected virtual void OnDestroy()
        {
            if (PostOnDestroy != null)
            {
                PostOnDestroy(this);
            }
        }
    }
}
