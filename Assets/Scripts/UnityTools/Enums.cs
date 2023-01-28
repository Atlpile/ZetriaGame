public enum E_PlayerStatus { Pistol, ShotGun, NPC }
public enum E_ResourcesPath { Audio, Entity, UI }
public enum E_AudioType { BGM, Effect }
public enum E_InputType
{
    Left, Right, Jump,
    MeleeAttack, FireAttack, ChangeWeapon, Reload,
    Pause

}
public enum E_EventType
{
    PickUpNPC, PickUpShotGun, PickUpToken, PickUpCard,
    InitGamePanelUI,
    UpdatePistolAmmo, UpdateShotGunAmmo,
    PlayerFire, PlayerReload,
    MovePlatform,

}
public enum E_AmmoType { Pistol, ShotGun }
public enum E_BulletMoveType { Upward, Straight, Downward }
public enum E_AIState
{
    Null, Patrol, Chase
}
public enum E_DoorType { Once, Condition, Smart }
public enum E_PlatformButtonType { Null, Up, Down, Left, Right }