using UnityEngine;

namespace Air2000
{
    public class AutoLocalization : MonoBehaviour
    {
        public string LabelKey = string.Empty;

        void Awake()
        {
            if (string.IsNullOrEmpty(LabelKey)==false)
            {
                UIHelper.SetLabelText(transform, Localization.Get(LabelKey));
            }
        }
    }
}
