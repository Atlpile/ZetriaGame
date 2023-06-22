public enum E_PlayerStatus { Pistol, ShotGun, NPC }
public enum E_ResourcesPath { Audio, Entity, UI, FX }
public enum E_AudioType { BGM, Effect }
public enum E_AudioSetttingType { Stop, Pause, Resume }
public enum E_InputType
{
    Crouch, Jump, MeleeAttack, GunAttack, SwitchWeapon, Reload,
    Interacitve, PutDownNPC, PickUpNPC,
    Pause
}


public enum E_EventType
{
    PickUpNPC, PickUpShortGun, PickUpToken, PickUpDoorCard, PickUpPistolAmmo, PickUpShortGunAmmo,
    InitGamePanelUI,
    PlayerFire, PlayerReload, PlayerAddHP, PlayerTeleport,
    MovePlatform,
    PressKey, PressKeyDown, PressKeyUp,
    UpdateBGMVolume, UpdateEffectVolume,

}
public enum E_AmmoType { Pistol, ShotGun }
public enum E_BulletMoveType { Upward, Straight, Downward }
public enum E_AIState { Null, Patrol, Chase, Idle, Dead, Attack }
public enum E_DoorType { Once, Condition, Smart }
public enum E_MonsterType { Ground, Fly, Static }
public enum E_CheckType { Rect, Sphere }