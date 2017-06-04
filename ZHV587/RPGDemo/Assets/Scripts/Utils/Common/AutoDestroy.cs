using UnityEngine;

namespace Air2000
{
    public class AutoDestroy : MonoBehaviour
    {
        public float Time = 1;
        void Start()
        {
            Invoke("Destroy", Time);
        }
        void Destroy()
        {
            GameObject.Destroy(gameObject);
        }
    }
}
