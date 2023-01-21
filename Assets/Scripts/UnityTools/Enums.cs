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
    PickUpNPC,
    InitGamePanelUI,
    UpdatePistolAmmo, UpdateShotGunAmmo,
    PlayerFire, PlayerReload

}
public enum E_AmmoType { Pistol, ShotGun }