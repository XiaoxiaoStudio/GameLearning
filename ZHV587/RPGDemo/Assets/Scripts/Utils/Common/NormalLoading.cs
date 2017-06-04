using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using UnityEngine;

namespace Air2000
{
    public class NormalLoading : MonoBehaviour
    {
        private void OnEnable()
        {
            if (AssetManager.TaskCount == 0)
            {
                gameObject.SetActive(false);
            }
        }
        private void Update()
        {
            if (AssetManager.TaskCount == 0 && CharacterProvider.TaskCount == 0)
            {
                gameObject.SetActive(false);
            }
        }
    }
}
