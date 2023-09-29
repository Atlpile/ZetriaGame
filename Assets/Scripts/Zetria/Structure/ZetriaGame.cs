using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FrameCore;

namespace Zetria
{
    public class ZetriaGame : BaseGameStructure<ZetriaGame>
    {
        protected override void RegisterModule()
        {
            AddModel<IZetriaGameModel>(new ZetriaGameModel());
            AddModel<IPlayerModel>(new PlayerModel());
            AddModel<IAmmoModel>(new AmmoModel());
        }
    }
}


