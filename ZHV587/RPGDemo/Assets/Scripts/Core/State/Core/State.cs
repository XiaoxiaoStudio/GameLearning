using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Air2000
{
    public abstract class State
    {
        private string m_Name;
        protected StateMachine m_Machine;
        public string Name
        {
            get { return m_Name; }
            set { m_Name = value; }
        }
        public StateMachine Machine
        {
            get { return m_Machine; }
            set { m_Machine = value; }
        }
        public State(string stateName) { m_Name = stateName; }
        public virtual void OnCreated() { }
        public virtual void Begin() { }
        public virtual void Update() { }
        public virtual void End() { }
        public virtual void OnDestroy() { }
    }
}
