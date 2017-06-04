using UnityEngine;

namespace Air2000
{
    public class ProgressDialog : MonoBehaviour
    {

#if ASSETBUNDLE_MODE
		public static string ResName = "Progress";
#else
        public static string ResName = "UI/Dialog/Progress";
#endif
        private string Text = "";
        private float m_Timeout = 10f;
        private float m_StartTime = 0.0f;
        private bool m_Cancelable = false;
        void Start()
        {
            Initialize();
        }
        void Initialize()
        {
            m_StartTime = Time.realtimeSinceStartup;
            Helper.SetLabelText(transform, "Parent/Label", Text);
        }
        public void Close()
        {
            if ((Time.realtimeSinceStartup - m_StartTime) < 0.5f)
            {
                Invoke("DestroyDialog", 0.3f);
            }
            else
            {
                GameObject.Destroy(gameObject);

            }
        }
        void DestroyDialog()
        {
            GameObject.Destroy(gameObject);
        }
        void Update()
        {
            if (Time.realtimeSinceStartup - m_StartTime > m_Timeout)
            {
                Close();
            }
        }
        public void OnClick()
        {
            if (m_Cancelable)
            {
                Close();
            }
        }
        public void SetParam(string text, float time, bool cancelable)
        {
            Text = text;
            m_Timeout = time;
            m_Cancelable = cancelable;
        }
    }
}
