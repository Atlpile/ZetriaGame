using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FrameCore;

namespace Zetria
{
    public interface IPlayerModel : IModel
    {
        bool IsInLevel { get; set; }
        float CurrentHealth { get; set; }
        float MaxHealth { get; set; }
    }

    public class PlayerModel : BaseModel, IPlayerModel
    {
        public bool IsInLevel { get; set; }
        public float CurrentHealth { get; set; }
        public float MaxHealth { get; set; }
        public bool HasShortGun { get; set; }

        protected override void OnInit()
        {
            IsInLevel = false;
        }


    }
}




