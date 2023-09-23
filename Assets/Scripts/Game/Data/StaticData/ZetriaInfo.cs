using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//TODO:静态数据和动态数据要分离

[System.Serializable]
public class ZetriaInfo
{
    [Header("Settings")]
    public int maxHealth;
    public int currentHealth;
    public int maxJumpCount;
    public int currentJumpCount;
    public float standMoveSpeed;
    public float getNPCMoveSpeed;
    public float crouchMoveSpeed;
    public float jumpForce;
    public float jumpGravity;
    public float drag;
    public float meleeAttackCD;
    public float pistolAttackCD;
    public float shotGunAttackCD;
    public float emptyAttackCD;
    public float reloadCD;
    public float hurtCD;

    [Header("BulletOffset")]
    public Vector3 pistolBulletLeftOffset, pistolBulletRightOffset;
    public Vector3 shortGunBulletLeftOffset, shortGunBulletRightOffset;
    public Vector3 bulletOffsetWithCrouch;

    [Header("FXOffset")]
    public Vector3 kickFXRightOffset, kickFXLeftOffset;
    public Vector3 jumpFXOffset;

    [Header("Player Size & Offset")]
    public Vector2 crouchSize, crouchOffset;
    public Vector2 standSize, standOffset;

    [Header("Status")]
    public bool isMeleeAttack;
    public bool isPistolAttack;
    public bool isShotGunAttack;
    public bool isEmptyAttack;
    public bool isCrouch;
    public bool isReload;
    public bool isHurt;
    public bool isDead;
    public bool canStand;
    public bool hasToken;
    public bool hasDoorCard;
    public bool hasShortGun;


    public ZetriaInfo()
    {
        // maxHealth = 10;
        maxHealth = 2;
        currentHealth = maxHealth;

        maxJumpCount = 1;
        standMoveSpeed = 4f;
        getNPCMoveSpeed = 3.5f;
        crouchMoveSpeed = 2f;
        jumpForce = 15f;
        drag = 3f;
        jumpGravity = 5f;

        meleeAttackCD = 0.4f;
        pistolAttackCD = 0.45f;
        shotGunAttackCD = 0.9f;
        emptyAttackCD = 0.45f;
        reloadCD = 0.8f;
        hurtCD = 0.5f;

        hasToken = false;
        hasDoorCard = false;
        hasShortGun = false;

        pistolBulletLeftOffset = new Vector2(-1f, 1.15f);
        pistolBulletRightOffset = new Vector2(1f, 1.15f);
        shortGunBulletLeftOffset = new Vector2(-0.5f, 0.75f);
        shortGunBulletRightOffset = new Vector2(0.5f, 0.75f);
        bulletOffsetWithCrouch = new Vector2(0, -0.5f);
        kickFXRightOffset = new Vector2(0.75f, 1f);
        kickFXLeftOffset = new Vector2(-0.75f, 1f);

    }
}
