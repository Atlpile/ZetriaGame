using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FrameCore
{
    public class Config_Input
    {
        public Dictionary<E_InputTypes, KeyCode> DefaultConfig;

        public Config_Input()
        {
            DefaultConfig = new Dictionary<E_InputTypes, KeyCode>
            {
                {E_InputTypes.SwitchWeapon,  KeyCode.Tab},
                {E_InputTypes.Crouch,        KeyCode.S},
                {E_InputTypes.GunAttack,     KeyCode.K},
                {E_InputTypes.Interacitve,   KeyCode.E},
                {E_InputTypes.Jump,          KeyCode.Space},
                {E_InputTypes.MeleeAttack,   KeyCode.J},
                {E_InputTypes.Pause,         KeyCode.Escape},
                {E_InputTypes.Reload,        KeyCode.R},
                {E_InputTypes.PickUpNPC,     KeyCode.Q},
                {E_InputTypes.PutDownNPC,    KeyCode.F},
            };
        }
    }

}
