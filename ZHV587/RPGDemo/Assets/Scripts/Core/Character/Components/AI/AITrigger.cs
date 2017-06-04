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
    [Serializable]
    public class AITrigger
    {
        public string ExecuteState;
        public int Random;
        [NonSerialized]
        public AIMachine Machine;
        public List<AICondition> Conditions = new List<AICondition>();
        public AITrigger(string executeState) { ExecuteState = executeState; }
        public bool IsTrigger(AIMachine machine)
        {
            if (Conditions == null || Conditions.Count == 0)
            {
                return false;
            }
            else
            {
                for (int i = 0; i < Conditions.Count; i++)
                {
                    AICondition condition = Conditions[i];
                    if (condition == null) continue;
                    if (condition.IsSatisfied(machine, this) == false)
                        return false;
                }
                int random = UnityEngine.Random.Range(0, 100);
                if (random < Random)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        public void ParseXML(SecurityElement element, AIMachine machine)
        {
            Machine = machine;
            ArrayList conditionElements = element.Children;
            if (conditionElements == null || conditionElements.Count == 0)
            {
                return;
            }
            if (Conditions == null)
                Conditions = new List<AICondition>();
            Conditions.Clear();

            for (int i = 0; i < conditionElements.Count; i++)
            {
                SecurityElement conditionElement = conditionElements[i] as SecurityElement;
                if (conditionElement == null) continue;
                string conditionName = conditionElement.Attribute("Name");
                if (string.IsNullOrEmpty(conditionName)) continue;
                Type conditionType = Type.GetType(conditionName);
                if (conditionType == null) continue;
                AICondition conditionObj = conditionType.Assembly.CreateInstance(conditionType.FullName) as AICondition;
                if (conditionObj == null) continue;
                conditionObj.Name = conditionName;
                conditionObj.ParseXML(conditionElement, this);
                Conditions.Add(conditionObj);
            }
        }
    }
}
