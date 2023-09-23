using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FrameCore;


namespace Zetria
{
    public interface IZetriaGameModel : IModel
    {
        float BGMVolume { get; set; }
        float EffectVolume { get; set; }
    }

    public class ZetriaGameModel : BaseModel, IZetriaGameModel
    {
        public float BGMVolume { get; set; }
        public float EffectVolume { get; set; }

        protected override void OnInit()
        {

        }
    }

}



