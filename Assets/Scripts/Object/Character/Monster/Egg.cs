using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Egg : BaseMonster
{
    [SerializeField] private bool canCreateMonster;
    [SerializeField] private E_SpawnMonsterType spawnType;


    protected override void InitCharacter()
    {
        monsterInfo.monsterType = E_MonsterType.Static;
        monsterInfo.groundSpeed = 0;
        monsterInfo.checkRadius = 3;

        currentHealth = monsterInfo.maxHealth;
        currentMoveSpeed = monsterInfo.groundSpeed;
        destroyTime = 0.3f;
        fsm.ChangeState(E_AIState.Idle);
    }

    public override void Attack()
    {
        //若可以生成怪物，则根据生成类型生成怪物
        if (canCreateMonster == true && isDead == false)
            StartCoroutine(IE_SpawnMonster());
    }

    public override void Dead()
    {
        StartCoroutine(IE_Dead());
    }


    private void SpawnMonster()
    {
        switch (spawnType)
        {
            case E_SpawnMonsterType.Null:
                Debug.Log("Egg不生成怪物");
                break;
            case E_SpawnMonsterType.Mon1:
                Debug.Log("Egg生成Horizontal");
                GameObject monster = GameManager.Instance.ResourcesLoader.Load<GameObject>(E_ResourcesPath.Object, "Wolfman");
                monster.transform.position = this.transform.position;
                break;
            case E_SpawnMonsterType.Mon2:
                Debug.Log("Egg生成Vertical");
                break;
        }
    }

    private IEnumerator IE_Dead()
    {
        anim.Play("Dead");
        rb2D.bodyType = RigidbodyType2D.Kinematic;
        col2D.enabled = false;
        GameManager.Instance.AudioManager.AudioPlay(E_AudioType.Effect, "enemy_death_01");


        yield return new WaitForSeconds(destroyTime);
        Destroy(this.gameObject);
    }

    private IEnumerator IE_SpawnMonster()
    {
        anim.Play("Dead");
        SpawnMonster();
        GameManager.Instance.AudioManager.AudioPlay(E_AudioType.Effect, "enemy_death_01");
        isDead = true;

        yield return new WaitForSeconds(destroyTime);
        Destroy(this.gameObject);
    }


    private void OnTriggerStay2D(Collider2D other)
    {
        IDamageable damageable = other.gameObject.GetComponent<IDamageable>();
        if (damageable != null && other.gameObject.name == "Player")
        {
            damageable.Damage(this.transform.position);
        }
    }

    protected override void OnDrawGizmos()
    {
        if (canCreateMonster)
            base.OnDrawGizmos();
    }
}
