using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FrameCore
{
    public class Config_Input
    {
        public Dictionary<E_InputType, KeyCode> DefaultConfig;

        public Config_Input()
        {
            DefaultConfig = new Dictionary<E_InputType, KeyCode>
            {
                {E_InputType.SwitchWeapon,  KeyCode.Tab},
                {E_InputType.Crouch,        KeyCode.S},
                {E_InputType.GunAttack,     KeyCode.K},
                {E_InputType.Interacitve,   KeyCode.E},
                {E_InputType.Jump,          KeyCode.Space},
                {E_InputType.MeleeAttack,   KeyCode.J},
                {E_InputType.Pause,         KeyCode.Escape},
                {E_InputType.Reload,        KeyCode.R},
                {E_InputType.PickUpNPC,     KeyCode.Q},
                {E_InputType.PutDownNPC,    KeyCode.F},
            };
        }
    }

}
