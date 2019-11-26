using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;
using System;

public class Boss_UniqCtrl : Monster
{
    public enum BossState
    {
        IDLE,
        TRACE,
        ATTACK,
        DIE
    };
    public BossState bossState = BossState.IDLE;

    public GameObject FX_Pad;

    private Transform monsterTr;

    private DamageUIPooler damageUIPooler;

    private SkinnedMeshRenderer objectMesh; //  사망하면 Render를 끈다.

    private HpBarScript hpBarScript; // HPBarScript를 이용한다.

    private float angryHp;
    
    void Init()
    {
        this.currHp = this.hp;
        angryHp = base.hp * 0.3f;
        this.isDie = false;
        this.isAttack = false;
        this.isAngry = false;
        bossState = BossState.IDLE;

    }

    private void Awake()
    {
        objectMesh = GetComponentInChildren<SkinnedMeshRenderer>();

        monsterTr = GetComponent<Transform>();
        playerTr = GameObject.FindWithTag("PLAYER").GetComponent<Transform>();
        nvAgent = GetComponent<NavMeshAgent>();
        damageUIPooler = GameObject.FindWithTag("MANAGER").GetComponentInChildren<DamageUIPooler>();

        anim = GetComponentInChildren<Animator>();

        hpBarScript = GetComponent<HpBarScript>();
    }

    void Start()
    {
        Init();
        nvAgent.stoppingDistance = attackDist;

        hpBarScript.Init(this.hp, this.nickname);
        hpBarScript.HideCanvas();

        StartCoroutine(this.CheckMonsterState());
        StartCoroutine(this.MonsterAction());
    }
    
    IEnumerator CheckMonsterState()
    {
        AngryPower = 0;
        while (!isDie)
        {
            yield return new WaitForSeconds(0.2f);
            
            float dist = (playerTr.position - monsterTr.position).sqrMagnitude;
         
            if(dist <= attackDist * attackDist)
            {
                bossState = BossState.ATTACK;
            }
            else if (dist <= traceDist * traceDist)
            {
                bossState = BossState.TRACE;
            }
        }

        Die();
    }

    IEnumerator MonsterAction()
    {
        while (!isDie)
        {
            switch (bossState)
            {
                case BossState.IDLE:
                    nvAgent.isStopped = true;
                    anim.SetBool("WALK", false);
                    break;
                case BossState.TRACE:
                    if (isAttack)
                    {
                        nvAgent.isStopped = true;
                    } else
                    {
                        nvAgent.isStopped = false;
                        RotateTowards(playerTr.position);
                    }
                    nvAgent.destination = playerTr.position;
                    anim.SetBool("WALK", true);
                    break;
                case BossState.ATTACK:
                    if (isAttack)
                    {
                        nvAgent.isStopped = true;
                        break;
                    }
                    RotateTowards(playerTr.position);
                    
                    if (UnityEngine.Random.Range(0, 101) < 10) // 10퍼센트 확률로 극딜 타임 나옴.
                    {
                        anim.SetTrigger("HAPPY");
                    } else
                    {
                        anim.SetTrigger("ATTACK");
                    }
                    break;
            }
            yield return null;
        }
    }
    
    public void RotateTowards(Vector3 targetPos)
    {
        Vector3 direction = (targetPos - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));    // flattens the vector3
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 50f);
    }

    public void Damaged(DamageUIPooler.AttackInfo attackInfo)
    {
        PlayerManager playerManager = GameManager.instance.playerManager;

        // 데미지 빼기
        float damagedHp = this.currHp - attackInfo.damage;
        
        // 데미지 UI 생성
        GameObject damageUI = damageUIPooler.GetPooledObject(attackInfo);
        damageUI.transform.position = monsterTr.position + Vector3.up * scaleFactor.y;
        damageUI.transform.LookAt(playerTr);
        damageUI.SetActive(true);

        // Hit Effect 생성
        GameObject hitEffect = ParticlePooler.instance.GetPooledObject(ParticlePooler.ParticleType.HIT);
        if(hitEffect != null)
        {
            hitEffect.transform.position = monsterTr.position + Vector3.up * scaleFactor.y + monsterTr.transform.forward * 1f;
            hitEffect.GetComponent<ParticleScaler>().ParticleScaleChange(scaleFactor);
            hitEffect.SetActive(true);
        }

        // 피가 0 이하이면 죽음
        if (damagedHp <= 0f)
        {
            damagedHp = 0f;
            isDie = true;
            playerManager.MonsterKillEvent(this);
        }

        // 피해 적용
        this.currHp = damagedHp;
        hpBarScript.SetHpValue(this.currHp);

        if (this.currHp <= angryHp) // 피 50퍼센트 이하이면
        {
            if (isAngry == false)
            {
                BossAnger(); // 보스 화남
            }
        }

        // 죽지 않았다면 데미지 입는 애니메이션
        // anim.SetTrigger("DAMAGE");
        nvAgent.isStopped = true;
    }

    private void BossAnger()
    {
        isAngry = true;
        anim.SetTrigger("HAPPY");
        Invoke("FX_Pad_Invoke", 1.2f);
    }

    private void FX_Pad_Invoke()
    {
        FX_Pad.SetActive(true);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isDie)
        {
            return;
        }

        if (other.CompareTag("PLAYER_ATTACK"))
        {
            DamageUIPooler.AttackInfo Final_AttackInfo = other.gameObject.GetComponent<AttackDamage>().CalculationAttackDamage();

            Damaged(Final_AttackInfo);
        }
    }

    public void Die()
    {
        nvAgent.isStopped = true;
        GetComponent<Collider>().enabled = false;
        anim.SetTrigger("DIE");

        FX_Pad.SetActive(false);

        // objectMesh.enabled = false;
        // GetComponent<HpBarScript>().HideCanvas();
    }
}
