using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FrameCore;

namespace Zetria
{
    public class ContinueGameCommand : BaseCommand
    {
        protected override void OnExecute()
        {
            Debug.Log("继续游戏");
        }
    }
}



