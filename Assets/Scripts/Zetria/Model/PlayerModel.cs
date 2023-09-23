using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FrameCore;

namespace Zetria
{
    public interface IPlayerModel : IModel
    {
        float MoveSpeed { get; set; }
    }

    public class PlayerModel : BaseModel, IPlayerModel
    {
        public float MoveSpeed { get; set; }

        protected override void OnInit()
        {

        }
    }
}




