using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using System.Security;
using System.Collections;
using Mono.Xml;

namespace Air2000 
{
    [ExecuteInEditMode]
    public class SECamera : SEObject
    {
        [HideInInspector]
        [SerializeField]
        public Camera Camera;
        public float Distance;
        public float Height;

        [HideInInspector]
        public GameObject Target;

        public override void OnCreate()
        {
            base.OnCreate();
            if (GetComponent<Camera>() == null)
            {
                Camera = gameObject.AddComponent<Camera>();
            }
            transform.position = new Vector3(0, 1, -10);
            Camera.clearFlags = CameraClearFlags.Skybox;
            Camera.fieldOfView = 60;
        }

        protected override void Update()
        {
            if (!Target)
            {
                if (Application.isPlaying && PlayerProvider.Hero.Character != null)
                {
                    Target = PlayerProvider.Hero.Character.gameObject;
                }
                else
                {
                    if (SERoom.Instance.BirthPoint)
                    {
                        Target = SERoom.Instance.BirthPoint.gameObject;
                    }
                }
            }
            if (Target)
            {
                Vector3 oriPosition = Target.transform.position;
                oriPosition += new Vector3(0, Height, 0);
                Vector3 direction = transform.forward;
                direction.Normalize();
                transform.position = oriPosition + direction * Distance * (-1.0f);
            }
        }
    }
}
