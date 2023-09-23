using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FrameCore;

namespace Zetria
{
    public interface IAmmoModel : IModel
    {
        AmmoInfo Info { get; set; }
    }

    public class AmmoModel : BaseModel, IAmmoModel
    {
        public AmmoInfo Info { get; set; }

        protected override void OnInit()
        {
            Info = new AmmoInfo();
        }


    }
}




