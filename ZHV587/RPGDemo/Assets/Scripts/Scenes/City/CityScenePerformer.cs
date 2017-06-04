using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Air2000
{
    public class CityScenePerformer : Performer
    {
        public Camera SceneCamera;
        public Transform LookAtMenu;
        public Transform LookAtRoleSelect;

        private Transform m_TargetLookAt;

        protected override void Start()
        {
            if (SceneCamera == null)
            {
                SceneCamera = Camera.main;
            }
        }

        protected override void Update()
        {
            base.Update();
            if (m_TargetLookAt && SceneCamera)
            {
                SceneCamera.transform.position = Vector3.Lerp(SceneCamera.transform.position, m_TargetLookAt.position, 1.0f * Time.deltaTime * 2);
                SceneCamera.transform.rotation = Quaternion.Lerp(SceneCamera.transform.rotation, m_TargetLookAt.rotation, 1.0f * Time.deltaTime * 2);
            }
        }
        //protected override void OnSwapActivity(object lastActivity, object currentActivity)
        //{
        //    if (currentActivity != null)
        //    {
        //        if (currentActivity.GetType() == typeof(MenuActivity))
        //        {
        //            m_TargetLookAt = LookAtMenu;
        //        }
        //        else if (currentActivity.GetType() == typeof(RoleActivity))
        //        {
        //            m_TargetLookAt = LookAtRoleSelect;
        //        }
        //    }
        //    base.OnSwapActivity(lastActivity, currentActivity);
        //}
    }
}
