
[System.Serializable]
public class ZetriaInfo
{
    public int maxHealth;
    public int currentHealth;
    public int maxJumpCount;
    public float standSpeed;
    public float getNPCSpeed;
    public float crouchSpeed;
    public float jumpForce;
    public float jumpGravity;
    public float drag;
    public float meleeAttackCD;
    public float pistolAttackCD;
    public float shotGunAttackCD;
    public float emptyAttackCD;
    public float reloadCD;
    public float hurtCD;
    public bool hasToken;
    public bool hasDoorCard;
    public bool hasShortGun;


    public ZetriaInfo()
    {
        // maxHealth = 10;
        maxHealth = 2;
        currentHealth = maxHealth;
        maxJumpCount = 1;

        standSpeed = 4f;
        getNPCSpeed = 3.5f;
        crouchSpeed = 2f;
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

    }
}
