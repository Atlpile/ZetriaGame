using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FrameCore
{
    public struct TestEventArgs
    {
        public string name;
        public int age;
        public float atk;
        public bool sex;

        public TestEventArgs(string name, int age, float atk, bool sex)
        {
            this.name = name;
            this.age = age;
            this.atk = atk;
            this.sex = sex;
        }
    }
}


