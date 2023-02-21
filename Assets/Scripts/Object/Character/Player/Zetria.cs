

public class Zetria
{
    public int maxHealth;
    public int currentHealth;
    public int maxJumpCount;
    public float standSpeed;
    public float getNPCSpeed;
    public float crouchSpeed;
    public float jumpForce;
    public float jumpGravity;
    public float meleeAttackCD;
    public float pistolAttackCD;
    public float shotGunAttackCD;
    public float emptyAttackCD;
    public float reloadCD;
    public float hurtCD;
    public bool hasToken;
    public bool hasDoorCard;


    public Zetria()
    {
        maxHealth = 10;
        currentHealth = maxHealth;
        maxJumpCount = 1;

        standSpeed = 4f;
        getNPCSpeed = 3.5f;
        crouchSpeed = 2f;
        jumpForce = 12f;
        jumpGravity = 5f;
        meleeAttackCD = 0.4f;
        pistolAttackCD = 0.45f;
        shotGunAttackCD = 0.9f;
        emptyAttackCD = 0.45f;
        reloadCD = 0.8f;
        hurtCD = 1f;

        hasToken = false;
        hasDoorCard = false;

    }
}
