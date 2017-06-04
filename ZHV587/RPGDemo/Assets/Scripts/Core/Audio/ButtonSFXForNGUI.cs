using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Air2000
{
    public class ButtonSFXForNGUI : MonoBehaviour
    {
        public UISoundEnum DefaultSound = UISoundEnum.Click;
        public UISoundEnum OverrideSound = UISoundEnum.None;
        void OnClick()
        {
            if (OverrideSound != UISoundEnum.None)
            {
                AudioManager.Instance.PlayUISound(OverrideSound);
            }
            else
            {
                AudioManager.Instance.PlayUISound(DefaultSound);
            }
        }
    }
}
