using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class PlayerManager : MonoBehaviour
{
    public enum AnimationType
    {
        IDLE,
        RUN,
        ATTACK1,
        DAMAGE,
        JUMP,
        DEATH,
        VICTORY
    }
    public AnimationType animationType;
    public Animator anim;

    public Slider hpSlider;
    public Slider mpSlider;
    public Slider expSlider;
    public TextMeshProUGUI levelText;

    private Vector3 OriginPos;
    private Quaternion OriginRot;

    public PlayerInfo playerInfo; // 플레이어 기본 정보
    public GameObject QuestComponentSave; // 퀘스트 저장 오브젝트

    // 스킬
    public GameObject skill1_Projectile;
    public UI_Cooldown skill1_UI_Cooldown;
    private float cooldown_skill1 = 5f;
    private int skill1_mp = 30;

    public UI_Cooldown skill2_UI_Cooldown;
    private float cooldown_skill2 = 60f;

    public UI_Cooldown skill3_UI_Cooldown;
    private float cooldown_skill3 = 60f;

    public bool isAttack = false;
    public bool isJump = false;
    public bool isDie = false;

    [HideInInspector] public List<Quest> questList = new List<Quest>();

    public Action<Quest> AddQuestEvent;
    public Action<Quest> RemoveQuestEvent;
    public Action<Quest> CompleteQuestEvent;
    public Action<Monster> MonsterKillEvent;
    public Action<PlayerInfo> LevelUpEvent;

    private void Awake()
    {
        // 플레이어 정보 초기화
        playerInfo = new PlayerInfo();
        playerInfo.nickname = "푸쿠이";

        playerInfo.currHp = playerInfo.hp;
        hpSlider.maxValue = playerInfo.hp;
        hpSlider.value = hpSlider.maxValue;

        playerInfo.currMp = playerInfo.mp;
        mpSlider.maxValue = playerInfo.mp;
        mpSlider.value = mpSlider.maxValue;

        playerInfo.currExp = 0;
        expSlider.maxValue = playerInfo.exp;
        expSlider.value = 0;

        playerInfo.level = 1;
    }

    void Start()
    {
        // 초기화도 하면서 UI 세팅
        playerInfo.attackPowerStatic = 30; // 원래 30
        playerInfo.attackPowerAdd = 0; // UI랑 연동되어있음.
        playerInfo.attackPowerMul = 1;

        playerInfo.attackSpeedStatic = 1f;
        playerInfo.attackSpeedMul = 1;

        playerInfo.moveSpeedStatic = 8f; // 원래 8
        playerInfo.moveSpeedAdd = 0;
        playerInfo.moveSpeedMul = 1;

        playerInfo.criticalHitStatic = 15;
        playerInfo.criticalHitAdd = 0;
        playerInfo.criticalHitMul = 1;

        OriginPos = this.transform.position;
        OriginRot = this.transform.rotation;

        // 이벤트 추가
        MonsterKillEvent += GetExp; // 몬스터 죽였을 때
        CompleteQuestEvent += GetExp; // 퀘스트 완료했을 때

        StartCoroutine(HPMP_AutoHeal()); // 체젠, 마젠
    }

    IEnumerator HPMP_AutoHeal()
    {
        while (!isDie)
        {
            // 체젠, 마젠
            if (playerInfo.currHp < playerInfo.hp)
            {
                int virtualHp = playerInfo.currHp + 2;
                if (virtualHp > playerInfo.hp)
                    virtualHp = playerInfo.hp;
                playerInfo.currHp = virtualHp;
            }
            if (playerInfo.currMp < playerInfo.mp)
            {
                int virtualMp = playerInfo.currMp + 1;
                if (virtualMp > playerInfo.mp)
                    virtualMp = playerInfo.mp;
                playerInfo.currMp = virtualMp;
            }
            yield return new WaitForSeconds(2f);
        }
        yield break;
    }

    private void Update()
    {
        if (isDie)
        {
            // 다시 살아나기
            if (Input.GetKeyDown(KeyCode.G))
            {
                isDie = false;
                GameUiManager.instance.HidePlayerDie();
                GameUiManager.instance.ShowFadeIn();

                StartCoroutine(HPMP_AutoHeal()); // 체젠, 마젠
                playerInfo.currHp = playerInfo.hp;
                playerInfo.currMp = playerInfo.mp;

                transform.position = OriginPos;
                transform.rotation = OriginRot;

                playerInfo.currExp -= Mathf.RoundToInt(playerInfo.currExp * 0.3f); // 현재 경험치의 30퍼 떨굼
            }
        }

        if (playerInfo.currHp != hpSlider.value)
        {
            hpSlider.value = Mathf.Lerp(hpSlider.value, playerInfo.currHp, 0.2f);
        }
        if (playerInfo.currMp != mpSlider.value)
        {
            mpSlider.value = Mathf.Lerp(mpSlider.value, playerInfo.currMp, 0.2f);
        }
        if (playerInfo.currExp != expSlider.value)
        {
            expSlider.value = Mathf.Lerp(expSlider.value, playerInfo.currExp, 0.2f);
        }
    }

    #region 퀘스트 관련
    public void AddQuest(Quest quest)
    {
        questList.Add(quest);

        GameUiManager.instance.Notice("퀘스트를 수락하셨습니다.", false);
        SetQuestToOneNPC(quest);
        // 이벤트 발생 (UI 적용)
        AddQuestEvent(quest);
    }
    public void RemoveQuest(Quest quest)
    {
        for (int i=0; i<questList.Count; i++)
        {
            if (questList[i].number == quest.number)
            {
                Quest tmp = questList[i];
                tmp.enabled = false;

                // 이벤트 발생 (UI 적용)
                RemoveQuestEvent(quest);
            }
        }
    }
    public void SetQuestToOneNPC(Quest quest)
    {
        GameObject[] NPCArr = GameObject.FindGameObjectsWithTag("NPC");

        if (NPCArr == null)
            return;
        
        for (int i = 0; i < NPCArr.Length; i++)
        {
            if (quest.npcName == NPCArr[i].GetComponent<NPCCtrl>().npcName)
            {
                NPCArr[i].GetComponent<NPCCtrl>().SetQuest_WhenPlayerAddQuest(quest);
            }
        }
    }

    public void SetQuestToAllNPC()
    {
        GameObject[] NPCArr = GameObject.FindGameObjectsWithTag("NPC");

        if (NPCArr == null)
            return;
        
        for (int i=0; i<questList.Count; i++)
        {
            for(int j=0; j<NPCArr.Length; j++)
            {
                // Debug.Log("퀘스트 비교 중, quest의 NpcName:" + questList[i].npcName + ", NPC의 이름:" + NPCArr[j].GetComponent<NPCCtrl>().npcName);

                if (questList[i].npcName == NPCArr[j].GetComponent<NPCCtrl>().npcName)
                {
                    NPCArr[j].GetComponent<NPCCtrl>().SetQuest_WhenChangeScene(questList[i]);
                }
            }
        }
    }
    #endregion

    #region 공격하기, 피해 입기

    // 공격하기
    public DamageUIPooler.AttackInfo Attack()
    {
        // 치명타 등등
        DamageUIPooler.AttackInfo playerAttack;

        int random = UnityEngine.Random.Range(1, 101);
        if (random >= playerInfo.criticalHit) // 기본 공격
        {
            playerAttack.damage = UnityEngine.Random.Range(playerInfo.attackPower - 5, playerInfo.attackPower + 5);
            playerAttack.damageType = DamageUIPooler.DamageType.BASIC;
        } else // 크리티컬
        {
            playerAttack.damage = UnityEngine.Random.Range(playerInfo.attackPower - 5, playerInfo.attackPower + 5) * 2;
            playerAttack.damageType = DamageUIPooler.DamageType.CRITICAL;
        }

        // 타격 시 쿨타임 감소
        skill2_UI_Cooldown.CurrCooldownPlus(1f);
        skill3_UI_Cooldown.CurrCooldownPlus(1f);

        return playerAttack;
    }

    // 피해 입기
    public void Damaged(DamageUIPooler.AttackInfo attackInfo)
    {
        Debug.Log("플레이어가 피해를 입음 : " + attackInfo.damage);

        if (isDie)
        {
            return;
        }

        Camera.main.GetComponent<CameraCtrl>().PlayerDamaged(0.5f);

        int virtualHp = playerInfo.currHp - attackInfo.damage;

        if (virtualHp < 0)
        {
            virtualHp = 0;

            // 사망 판정
            isDie = true;
            setAnimTrigger(AnimationType.DEATH);
            GameUiManager.instance.ShowPlayerDie();
        } else
        {
            setAnimTrigger(AnimationType.DAMAGE);
        }

        playerInfo.currHp = virtualHp;
    }

    #endregion

    #region UI 관련

    public bool CanUseMp(int mp)
    {
        int virtualMp = playerInfo.currMp - mp;
        if (virtualMp < 0)
        {
            return false;
        }
        return true;
    }
    public void UseMP(int mp)
    {
        playerInfo.currMp -= mp;
    }

    public void ChangeHpSlider()
    {
        hpSlider.transform.Find("Text").GetComponent<TextMeshProUGUI>().text = playerInfo.currHp + " / " + playerInfo.hp;
    }

    public void ChangeMpSlider()
    {
        mpSlider.transform.Find("Text").GetComponent<TextMeshProUGUI>().text = playerInfo.currMp + " / " + playerInfo.mp;
    }

    public void ChangeExpSlider()
    {
        expSlider.transform.Find("Text").GetComponent<TextMeshProUGUI>().text = playerInfo.currExp + " / " + playerInfo.exp;
    }

    #endregion

    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("ENEMY_ATTACK"))
        {
            DamageUIPooler.AttackInfo Final_AttackInfo = other.gameObject.GetComponent<MonsterDamage>().CalculationAttackDamage();

            Damaged(Final_AttackInfo);
        }
    }
    

    #region 버프, 스킬

    public void AddBuff(Buff buff)
    {
        GetComponent<BuffManager>().AddBuff(buff);
    }

    // Skill 1 사용
    public void Skill1()
    {
        if (CanUseMp(skill1_mp)) // 마나 있으면
        {
            if (skill1_UI_Cooldown.canUse)
            {
                skill1_Projectile.GetComponent<Projectile>().Show();
                skill1_Projectile.GetComponent<Projectile>().SkillRealActive += useSkill1;
            }
            else
            {
                GameUiManager.instance.Notice("쿨타임입니다.", false);
            }
        }
        else // 마나 없으면
        {
            GameUiManager.instance.Notice("마나가 없습니다.", false);
        }
    }

    // Skill 1 실제 사용
    public void useSkill1()
    {
        UseMP(skill1_mp);
        
        skill1_UI_Cooldown.cooldown = cooldown_skill1;
        skill1_UI_Cooldown.StartCoolDown();
    }

    // Skill 2 사용
    public void Skill2()
    {
        if (skill2_UI_Cooldown.canUse)
        {
            skill2_UI_Cooldown.cooldown = cooldown_skill2;
            skill2_UI_Cooldown.StartCoolDown();

            playerInfo.currHp = playerInfo.hp;
        }
        else
        {
            GameUiManager.instance.Notice("쿨타임입니다.", false);
        }
    }
    // Skill 3 사용
    public void Skill3()
    {
        if (skill3_UI_Cooldown.canUse)
        {
            skill3_UI_Cooldown.cooldown = cooldown_skill3;
            skill3_UI_Cooldown.StartCoolDown();

            playerInfo.currMp = playerInfo.mp;
        }
        else
        {
            GameUiManager.instance.Notice("쿨타임입니다.", false);
        }
    }

    #endregion


    #region 경험치 및 레벨업
    // 경험치 얻기
    public void GetExp(Monster monsterCtrl)
    {
        int virtualExp = playerInfo.currExp + monsterCtrl.exp;

        //Debug.Log("현재 경험치 : " + virtualExp + ", 레벨 최대 경험치 : " + playerInfo.exp);

        while (virtualExp >= playerInfo.exp) // 렙업 여러번 할 수도 있으니까
        {
            virtualExp -= playerInfo.exp; // 레벨업 경험치를 초과하면, 레벨업 후 초과한 경험치 올려줌

            LevelUp();
        }
        
        playerInfo.currExp = virtualExp;
    }
    public void GetExp(Quest quest)
    {
        int virtualExp = playerInfo.currExp + quest.reward;

        //Debug.Log("현재 경험치 : " + virtualExp + ", 레벨 최대 경험치 : " + playerInfo.exp);

        while (virtualExp >= playerInfo.exp) // 렙업 여러번 할 수도 있으니까
        {
            virtualExp -= playerInfo.exp; // 레벨업 경험치를 초과하면, 레벨업 후 초과한 경험치 올려줌

            LevelUp();
        }

        playerInfo.currExp = virtualExp;
    }

    public void LevelUp()
    {
        playerInfo.level += 1;
        hpSlider.maxValue = playerInfo.hp;
        playerInfo.currHp = playerInfo.hp;
        mpSlider.maxValue = playerInfo.mp;
        playerInfo.currMp = playerInfo.mp;

        GameUiManager.instance.Notice("레벨 업!!", true);
        GameUiManager.instance.SetUIs(playerInfo); // 레벨업한 능력치 Ui 적용

        GameObject levelupEffect = ParticlePooler.instance.GetPooledObject(ParticlePooler.ParticleType.LEVELUP);
        if (levelupEffect != null)
        {
            levelupEffect.transform.position = this.transform.position + Vector3.up * 0.05f;
            levelupEffect.transform.SetParent(this.transform);
            levelupEffect.SetActive(true);
        }

        levelText.text = "Lv " + playerInfo.level;
        
        expSlider.maxValue = playerInfo.exp; // 레벨에 따른 최대 경험치 바꾸기

        // 레벨 업 이벤트 발생
        if (LevelUpEvent != null)
        {
            LevelUpEvent(this.playerInfo);
        }
    }

    #endregion


    #region 애니메이션 제어 함수

    public void setAnimBool(AnimationType type, bool result)
    {
        string strType = type.ToString();

        if (anim.GetBool(strType) == result) // 같으면 return;
        {
            return;
        }
        anim.SetBool(strType, result);
    }

    public void setAnimTrigger(AnimationType type)
    {
        string strType = type.ToString();

        anim.SetTrigger(strType);
    }

    #endregion
    
}
