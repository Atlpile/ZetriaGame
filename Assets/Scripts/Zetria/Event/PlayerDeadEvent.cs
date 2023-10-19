using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FrameCore
{
    public struct PlayerDeadEvent
    {
        public int testEventInt;
        public string testEventString;

        public PlayerDeadEvent(int testEventInt, string testEventString)
        {
            this.testEventInt = testEventInt;
            this.testEventString = testEventString;
        }
    }
}


