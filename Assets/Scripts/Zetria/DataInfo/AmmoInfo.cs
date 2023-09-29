using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Zetria
{
    public class AmmoInfo
    {
        //包子弹数量
        public int packageCount;
        //弹夹最大容弹数量
        public int ammoLimit;
        //当前弹夹容弹数量
        public int currentCount;
        //可填充子弹数量
        public int maxCount;


        public AmmoInfo(int packageCount, int ammoLimit)
        {
            this.packageCount = packageCount;
            this.ammoLimit = ammoLimit;
        }

        public void ClearAmmo()
        {
            currentCount = 0;
            maxCount = 0;
        }
    }
}

