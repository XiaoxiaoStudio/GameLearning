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
    public class SELight : SEObject
    {
        public Light Light;

        public override void OnCreate()
        {
            base.OnCreate();
            if (GetComponent<Light>() == null)
            {
                Light = gameObject.AddComponent<Light>();
                Light.type = LightType.Directional;
                Light.intensity = 1;
                Light.bounceIntensity = 1;
                Light.shadowBias = 0.05f;
                Light.shadowNormalBias = 0.4f;
                Light.shadowNearPlane = 0.2f;
                transform.position = new Vector3(0, 3, 0);
                transform.eulerAngles = new Vector3(50, -30, 0);
            }
        }
    }
}
