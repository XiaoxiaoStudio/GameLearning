using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Air2000
{
    public class SceneCameraController : MonoBehaviour
    {
        public Transform MainCameraNode;
        public Camera MainCamera;
        public List<Transform> CameraNodes = new List<Transform>();
        private static SceneCameraController m_Instance;
        public static ThirdPersonCameraController ThirdPersonCamera
        {
            get
            {
                return ThirdPersonCameraController.Instance;
            }
        }
        public static SceneCameraController Instance
        {
            get
            {
                return m_Instance;
            }
        }
        void Awake()
        {
            if (m_Instance)
            {
                GameObject.DestroyImmediate(m_Instance);
            }
            m_Instance = this;
        }
    }
}
