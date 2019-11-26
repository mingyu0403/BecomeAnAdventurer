using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;
using System;

public class MonsterCtrl : Monster
{
    public enum MonsterState
    {
        IDLE,
        TRACE,
        ATTACK,
        HOME,
        DIE
    };
    public MonsterState monsterState = MonsterState.IDLE;

    private Transform monsterTr;
    public Vector3 OringinPos; // 젠 위치
    
    private DamageUIPooler damageUIPooler;

    private SkinnedMeshRenderer objectMesh; //  사망하면 Render를 끈다.
    
    private HpBarScript hpBarScript; // HPBarScript를 이용한다.
    
    void Init()
    {
        //this.nickname = "BOMB";
        base.currHp = base.hp;
        //this.traceDist = 10f;
        //this.attackDist = 3f;
        this.isDie = false;
        this.isAttack = false;
        this.isAngry = false;
        this.AngryPower = 0;
        //this.exp = 60;

        this.transform.position = OringinPos;

        monsterState = MonsterState.IDLE;
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
        float GoHome = 0;
        AngryPower = 0;
        while (!isDie)
        {
            yield return new WaitForSeconds(0.2f);
            
            float dist = (playerTr.position - monsterTr.position).sqrMagnitude;
         
            if(dist <= attackDist * attackDist)
            {
                monsterState = MonsterState.ATTACK;
            }
            else if (dist <= traceDist * traceDist || isAngry)
            {
                monsterState = MonsterState.TRACE;

                GoHome = 0; // 참을성 초기화
                if (AngryPower > 3) // 3초동안 아무 일 없으면, 플레이어를 쫒아오지 않음.
                {
                    isAngry = false;
                }
                else
                {
                    AngryPower += 0.2f;
                }
            }
            else
            {
                float d = (OringinPos - monsterTr.position).sqrMagnitude;

                if (d <= 9f) // 원래 위치랑 가까우면 그냥 있음.
                {
                    monsterState = MonsterState.IDLE;
                    GoHome = 0;
                }
                else // 멀면 카운트 세다가 3초가 되면, 홈으로 돌아감
                {

                    if (GoHome > 3) // 3초동안 아무 일 없으면, 원래 위치로 돌아감.
                    {
                        monsterState = MonsterState.HOME;
                    }
                    else
                    {
                        monsterState = MonsterState.IDLE;
                        GoHome += 0.2f;
                    }
                }
            }
        }

        Die();
    }

    IEnumerator MonsterAction()
    {
        while (!isDie)
        {
            switch (monsterState)
            {
                case MonsterState.IDLE:
                    nvAgent.isStopped = true;
                    anim.SetBool("WALK", false);
                    break;
                case MonsterState.HOME:
                    nvAgent.isStopped = false;
                    nvAgent.destination = OringinPos;
                    anim.SetBool("WALK", true);
                    RotateTowards(OringinPos);
                    break;
                case MonsterState.TRACE:
                    if (isAttack)
                    {
                        nvAgent.isStopped = true;
                    } else
                    {
                        nvAgent.isStopped = false;
                    }
                    nvAgent.destination = playerTr.position;
                    anim.SetBool("WALK", true);
                    RotateTowards(playerTr.position);
                    break;
                case MonsterState.ATTACK:
                    if (isAttack)
                    {
                        break;
                    }
                    anim.SetTrigger("ATTACK");
                    break;
            }
            yield return null;
        }
    }
    
    private void RotateTowards(Vector3 targetPos)
    {
        Vector3 direction = (targetPos - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));    // flattens the vector3
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 50f);
    }

    public void Damaged(DamageUIPooler.AttackInfo attackInfo)
    {
        PlayerManager playerManager = GameManager.instance.playerManager;

        isAngry = true;
        AngryPower = 0;

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

        // 죽지 않았다면 데미지 입는 애니메이션
        anim.SetTrigger("DAMAGE");
        nvAgent.isStopped = true;
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
        objectMesh.enabled = false;
        GetComponent<HpBarScript>().HideCanvas();

        StartCoroutine(Respon());
    }

    IEnumerator Respon()
    {
        yield return new WaitForSeconds(15f);
        
        Init();
        hpBarScript.Init(this.hp, this.nickname);

        nvAgent.isStopped = false;
        GetComponent<Collider>().enabled = true;
        objectMesh.enabled = true;

        StartCoroutine(this.CheckMonsterState());
        StartCoroutine(this.MonsterAction());
    }
}
