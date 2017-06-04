using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mono.Xml;
using System.Security;
using System.Collections;
using UnityEngine;

namespace Air2000
{
    public class PathFinding : CharacterComponent
    {
        public bool IsMoving = false;
        void OnSwapCommand(Commander.Command lastCommand, Commander.Command currentCommand)
        {
            if (currentCommand != null && currentCommand.Type == CharacterCommand.CC_Walk)
            {
                IsMoving = true;
            }
            else if (currentCommand != null && currentCommand.Type == CharacterCommand.CC_Idle)
            {
                IsMoving = false;
                if (NavAgent.isOnNavMesh)
                {
                    NavAgent.Stop();
                }
            }
        }
        public void SetDestination(Vector3 targetPosition)
        {
            NavAgent.Stop();
            NavAgent.SetDestination(targetPosition);
            NavAgent.Resume();
        }
        public void Stop()
        {
            NavAgent.Stop();
        }
        protected override void Update()
        {
            if (NavAgent && Character.Moveable)
            {
                if (NavAgent.pathStatus == UnityEngine.AI.NavMeshPathStatus.PathComplete)
                {
                    NavAgent.Stop();
                }
                if (IsMoving)
                {
                    Vector3 newTargetPoint = Character.transform.TransformPoint(Vector3.forward * NavAgent.speed * Time.deltaTime);
                    SetDestination(newTargetPoint);
                }
            }
        }
        public override void ParseXML(SecurityElement element, Character character)
        {
            Character = character;
            if (element.Attribute("Radius") != null)
            {
                float val;
                float.TryParse(element.Attribute("Radius"), out val);
                NavAgent.radius = val;
            }
            else
            {
                NavAgent.radius = 0.6f;
            }

            if (element.Attribute("Height") != null)
            {
                float val;
                float.TryParse(element.Attribute("Height"), out val);
                NavAgent.height = val;
            }
            else
            {
                NavAgent.height = 2.0f;
            }

            if (element.Attribute("Speed") != null)
            {
                float val;
                float.TryParse(element.Attribute("Speed"), out val);
                NavAgent.speed = val;
            }
            else
            {
                NavAgent.speed = 7.0f;
            }

            if (element.Attribute("Acceleration") != null)
            {
                float val;
                float.TryParse(element.Attribute("Acceleration"), out val);
                NavAgent.acceleration = val;
            }
            else
            {
                NavAgent.acceleration = 70f;
            }

            if (element.Attribute("AutoBraking") != null)
            {
                bool val;
                bool.TryParse(element.Attribute("AutoBraking"), out val);
                NavAgent.autoBraking = val;
            }
            else
            {
                NavAgent.autoBraking = false;
            }

            if (element.Attribute("AngularSpeed") != null)
            {
                float val;
                float.TryParse(element.Attribute("AngularSpeed"), out val);
                NavAgent.angularSpeed = val;
            }
            else
            {
                NavAgent.angularSpeed = 720f;
            }

            if (element.Attribute("AvoidancePriority") != null)
            {
                int val;
                int.TryParse(element.Attribute("AvoidancePriority"), out val);
                NavAgent.avoidancePriority = val;
            }
            else
            {
                NavAgent.avoidancePriority = 1;
            }

            if (element.Attribute("ObstacleAvoidanceType") != null)
            {
                NavAgent.obstacleAvoidanceType = (UnityEngine.AI.ObstacleAvoidanceType)CharacterSystemUtils.TryParseEnum<UnityEngine.AI.ObstacleAvoidanceType>(element.Attribute("ObstacleAvoidanceType"));
            }
            else
            {
                NavAgent.obstacleAvoidanceType = UnityEngine.AI.ObstacleAvoidanceType.LowQualityObstacleAvoidance;
            }
        }
        public override void OnCharacterInitialized(Character character)
        {
            Commander.PostSwapCommand -= OnSwapCommand;
            Commander.PostSwapCommand += OnSwapCommand;
        }

    }
}
