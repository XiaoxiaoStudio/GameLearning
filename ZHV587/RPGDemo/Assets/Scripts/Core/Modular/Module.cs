using UnityEngine;
using System.Collections;

namespace Air2000
{
    public class Module
    {
        public EventManager EventProcessor { get; set; }

        public int ID;

        public Module(int id)
        {
            EventProcessor = new EventManager();
        }
    }
}

