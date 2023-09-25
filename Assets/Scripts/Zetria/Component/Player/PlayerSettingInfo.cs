using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Zetria
{
    public class PlayerSettingInfo
    {
        public int maxHealth;
        public PlayerCDInfo cdInfo;
        public PlayerControlInfo controlInfo;
        public PlayerOffsetInfo offsetInfo;

        public PlayerSettingInfo()
        {
            maxHealth = 2;

            cdInfo = new PlayerCDInfo();
            controlInfo = new PlayerControlInfo();
            offsetInfo = new PlayerOffsetInfo();
        }
    }

    public class PlayerControlInfo
    {
        public int maxJumpCount;
        public float standMoveSpeed;
        public float getNPCMoveSpeed;
        public float crouchMoveSpeed;
        public float jumpForce;
        public float jumpGravity;
        public float drag;

        public PlayerControlInfo()
        {
            maxJumpCount = 1;
            standMoveSpeed = 4f;
            getNPCMoveSpeed = 3.5f;
            crouchMoveSpeed = 2f;
            jumpForce = 15f;
            drag = 3f;
            jumpGravity = 5f;
        }
    }

    public class PlayerCDInfo
    {
        public float meleeAttackCD;
        public float pistolAttackCD;
        public float shotGunAttackCD;
        public float emptyAttackCD;
        public float reloadCD;
        public float hurtCD;

        public PlayerCDInfo()
        {
            meleeAttackCD = 0.4f;
            pistolAttackCD = 0.45f;
            shotGunAttackCD = 0.9f;
            emptyAttackCD = 0.45f;
            reloadCD = 0.8f;
            hurtCD = 0.5f;
        }
    }

    public class PlayerOffsetInfo
    {
        public Vector3 pistolBulletLeftOffset, pistolBulletRightOffset;
        public Vector3 shortGunBulletLeftOffset, shortGunBulletRightOffset;
        public Vector3 bulletOffsetWithCrouch;
        public Vector3 kickFXRightOffset, kickFXLeftOffset;
        public Vector3 jumpFXOffset;
        public Vector2 crouchSize, crouchOffset;
        public Vector2 standSize, standOffset;

        public PlayerOffsetInfo()
        {
            pistolBulletLeftOffset = new Vector2(-1f, 1.15f);
            pistolBulletRightOffset = new Vector2(1f, 1.15f);
            shortGunBulletLeftOffset = new Vector2(-0.5f, 0.75f);
            shortGunBulletRightOffset = new Vector2(0.5f, 0.75f);
            bulletOffsetWithCrouch = new Vector2(0, -0.5f);
            kickFXRightOffset = new Vector2(0.75f, 1f);
            kickFXLeftOffset = new Vector2(-0.75f, 1f);
        }
    }

    [System.Serializable]
    public class PlayerStateInfo
    {
        public bool isRight;
        public bool isGround;
        public bool isMeleeAttack;
        public bool isPistolAttack;
        public bool isShotGunAttack;
        public bool isEmptyAttack;
        public bool isCrouch;
        public bool isReload;
        public bool isHurt;
        public bool isDead;
        public bool canStand;
        public bool canFireAttack;
        public bool canReload;
        public bool hasToken;
        public bool hasDoorCard;
        public bool hasShortGun;
    }

    [System.Serializable]
    public class PlayerDynamicInfo
    {
        public PlayerStateInfo stateInfo;
        public E_PlayerStatus status;
        public float currentMoveSpeed;
        public int currentJumpCount;
        public int currentHealth;
        public int horizontalMove;

        public PlayerDynamicInfo()
        {
            stateInfo = new PlayerStateInfo();
            status = E_PlayerStatus.Pistol;
        }
    }
}

