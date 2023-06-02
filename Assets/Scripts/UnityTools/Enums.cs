public enum E_PlayerStatus { Pistol, ShotGun, NPC }
public enum E_ResourcesPath { Audio, Entity, UI, FX }
public enum E_AudioType { BGM, Effect }
public enum E_InputType
{
    Crouch, Jump, MeleeAttack, GunAttack, SwitchWeapon, Reload,
    Interacitve, PutDownNPC, PickUpNPC,
    Pause
}


public enum E_EventType
{
    PickUpNPC, PickUpShotGun, PickUpToken, PickUpDoorCard,
    InitGamePanelUI,
    PlayerFire, PlayerReload,
    MovePlatform,
    PressKey, PressKeyDown, PressKeyUp

}
public enum E_AmmoType { Pistol, ShotGun }
public enum E_BulletMoveType { Upward, Straight, Downward }
public enum E_AIState { Null, Patrol, Chase }
public enum E_DoorType { Once, Condition, Smart }
public enum E_PlatformButtonType { Null, Up, Down, Left, Right }